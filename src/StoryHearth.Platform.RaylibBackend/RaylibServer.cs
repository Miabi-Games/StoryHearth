using Raylib_cs;

using StoryHearth.Engine;

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace StoryHearth.Platform.RaylibBackend;

// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
public sealed class RaylibServer : IRenderServer, IDisposable
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    public static ApplicationRunResult BuildAndRunApplication(
        ApplicationDefinition appDefinition, string[] args)
    {
        try
        {
            EnsureOneRun();

            using var server = new RaylibServer();
            server.Initialize(appDefinition);

            using var container = BuildApplication(appDefinition, args);
            server.Run(container.Application);

            return ApplicationRunResult.Success;
        }
        catch (Exception ex)
        {
            var currentStack = new StackTrace(skipFrames: 1, fNeedFileInfo: true);

            StringBuilder details = new();

            details.Append("---\n");
            details
                .Append(ex.GetType().FullName).Append(": ")
                .Append(ex.Message).Append('\n');
            details.Append(ex.StackTrace).Append('\n');
            details.Append(currentStack);
            details.Append("---").Append('\n');
            details.Append('\n');

            details.Replace("\r\n", "\n");

            _platformLog.WriteError("An unhandled exception has occurred", details.ToString());

            if (!Debugger.IsAttached)
            {
                _platformLog.WriteInfo("The application is exiting with error code 1.");
                Environment.Exit(1); // exit immediately
            }
            throw; // let the debugger handle it (it may change the exit code)
        }
    }

    public static int ConvertToExitCode(ApplicationRunResult result)
    {
        return result.IsSuccess ? 0 : 1;
    }

    private static ApplicationContainer BuildApplication(
        ApplicationDefinition appDefinition, string[] args)
    {
        var builder = new ApplicationBuilder();
        return builder.Build(appDefinition, args);
    }

    private static void EnsureOneRun()
    {
        if (Interlocked.CompareExchange(ref _runOnce, true, false))
        {
            throw new InvalidOperationException(
                "The raylib backend only supports running a single " +
                "application during its process lifecycle.");

            // This was a design decision. With proper cleanup after running an
            // application, the raylib backend could conceivably run a second
            // time within the same process lifecycle, but doing so introduces
            // new potential errors and incompatibilities with potential future
            // platforms, with no significant benefits. A separate platform will
            // be made for running an application within another application
            // (e.g., a game within an editor). This wouldn't have enabled that
            // anyway; it requires a wrapper platform rendering to a viewport
            // within another application.
        }
    }

    private static bool _runOnce = false;

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    public int2 CanvasSize { get; private set; }
    public int2 ScreenSize { get; private set; }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    static RaylibServer()
    {
        _loggers = [new ConsoleLogger()];

        _raylibLog = new SimpleLog("raylib", _loggers);
        _platformLog = new SimpleLog(ILog.PlatformLogTag, _loggers);
        _engineLog = new SimpleLog(ILog.EngineLogTag, _loggers);
        _gameLog = new SimpleLog(ILog.GameLogTag, _loggers);

        unsafe { Raylib.SetTraceLogCallback(&LogCallback); }
        Raylib.SetTraceLogLevel(TraceLogLevel.Info);
    }

    private RaylibServer() { }

    ~RaylibServer() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    // ─────────────────────────────────────────────────────────────────────────
    private void Initialize(ApplicationDefinition appDefinition)
    {
        _platformLog.WriteMilestone("Initializing raylib backend");

        // ─────────────────────────────────────────────────────────────────────
        #region raylib configuration

        _windowTitle = appDefinition.WindowTitle ?? "";
        _canvasTargetSize = appDefinition.CanvasTargetSize;

        if (_canvasTargetSize.x <= 0 || _canvasTargetSize.y <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(appDefinition),
                "The canvas target size must be positive in both dimensions");
        }

        _screenMinSize = appDefinition.ScreenMinSize;

        if (_screenMinSize.x <= 0 || _screenMinSize.y <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(appDefinition),
                "The minimum window size must be postiive in both dimensions");
        }

        int2 screen_size = new()
        {
            x = Math.Max(_screenMinSize.x, _canvasTargetSize.x),
            y = Math.Max(_screenMinSize.y, _canvasTargetSize.y),
        };

        // ─────────────────────────────────────────────────────────────────────

        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

        Raylib.InitWindow(screen_size.x, screen_size.y, _windowTitle);

        Raylib.SetTargetFPS(60);
        Raylib.SetWindowMinSize(_screenMinSize.x, _screenMinSize.y);

        #endregion raylib configuration
        // ─────────────────────────────────────────────────────────────────────

        // ─────────────────────────────────────────────────────────────────────
        #region first frame

        // ─────────────────────────────────────────────────────────────────────
        // The first frame does not tick the application (the application
        // doesn't exist yet). It finishes preparing server for use.
        // ─────────────────────────────────────────────────────────────────────

        if (!Raylib.WindowShouldClose())
        {
            // ─────────────────────────────────────────────────────────────────

            int current_monitor = Raylib.GetCurrentMonitor();
            int monitor_width = Raylib.GetMonitorWidth(current_monitor);
            int monitor_height = Raylib.GetMonitorHeight(current_monitor);

            // Note that the max size here is only the max initial size. The
            // user can still resize the window beyond this limit.

            int max_width = monitor_width * 4 / 5;
            int max_height = monitor_height * 4 / 5;

            if (_screenMinSize.x > max_width || _screenMinSize.y > max_height)
            {
                throw new Exception(
                    "For simplicity, the current version of the application does " +
                    "not allow a minimum window size gerater than 80% of the " +
                    "monitor resolution. The handling of smaller monitors may " +
                    "be improved in future versions. But for now it's better to " +
                    "simply set the minimum window size to something small " +
                    "enough that this exception won't occur in normal client " +
                    "environments.");
            }

            screen_size = GetScreenSize();

            if (screen_size.x > max_width || screen_size.y > max_height)
            {
                screen_size.x = Math.Min(screen_size.x, max_width);
                screen_size.y = Math.Min(screen_size.y, max_height);

                Raylib.SetWindowPosition(monitor_width / 10, monitor_height / 10);
                Raylib.SetWindowSize(screen_size.x, screen_size.y);
            }

            ResizeCanvas();

            // ─────────────────────────────────────────────────────────────────

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Raylib.EndDrawing();

            // ─────────────────────────────────────────────────────────────────
        }

        #endregion first frame
        // ─────────────────────────────────────────────────────────────────────

        _platformLog.WriteMilestone("Finished initializing raylib backend");
    }
    // ─────────────────────────────────────────────────────────────────────────

    // ─────────────────────────────────────────────────────────────────────────
    private void Run(Application app)
    {
        while (!Raylib.WindowShouldClose())
        {
            if (Raylib.IsWindowResized()) ResizeCanvas();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText(_windowTitle, 40, 40, 40, Color.RayWhite);

            Raylib.EndDrawing();
        }
    }
    // ─────────────────────────────────────────────────────────────────────────

    // ─────────────────────────────────────────────────────────────────────────
    private static void Dispose(bool disposing)
    {
        if (disposing)
        {
            _platformLog.WriteMilestone("Disposing of raylib backend");
        }

        if (Raylib.IsWindowReady()) Raylib.CloseWindow();
    }
    // ─────────────────────────────────────────────────────────────────────────

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    private static int2 GetScreenSize()
    {
        return new()
        {
            x = Raylib.GetScreenWidth(),
            y = Raylib.GetScreenHeight(),
        };
    }

    private void ResizeCanvas()
    {
        int2 screen_size = GetScreenSize();

        double screen_w = screen_size.x;
        double screen_h = screen_size.y;

        if (screen_w < 1 || screen_h < 1)
        {
            CanvasSize = _canvasTargetSize;
            ScreenSize = (2, 2);
        }

        double canvas_w = _canvasTargetSize.x;
        double canvas_h = _canvasTargetSize.y;

        if (screen_w / canvas_w >= screen_h / canvas_h)
        {
            canvas_w = canvas_h * screen_w / screen_h;
        }
        else
        {
            canvas_h = canvas_w * screen_h / screen_w;
        }

        CanvasSize = ((int)canvas_w, (int)canvas_h);
        ScreenSize = screen_size;

        _platformLog.WriteInfo($"Screen size set to {ScreenSize.x}×{ScreenSize.y}");
        _platformLog.WriteInfo($"Canvas size set to {CanvasSize.x}×{CanvasSize.y}");
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void LogCallback(int logLevel, sbyte* text, sbyte* args)
    {
        var message = Logging.GetLogMessage(new(text), new(args));

        switch ((TraceLogLevel)logLevel)
        {
        case TraceLogLevel.Trace: _raylibLog.WriteVerbose(message); break;
        case TraceLogLevel.Debug: _raylibLog.DebugWriteVerbose(message); break;
        case TraceLogLevel.Info: _raylibLog.WriteInfo(message); break;
        case TraceLogLevel.Warning: _raylibLog.WriteWarning(message); break;
        case TraceLogLevel.Error: _raylibLog.WriteError(message); break;
        case TraceLogLevel.Fatal: _raylibLog.WriteError(message); break;

        default: _raylibLog.DebugWriteInfo(message); break;
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

    private int2 _canvasTargetSize;
    private int2 _screenMinSize;
    private string _windowTitle = "";

    private readonly static ILog _raylibLog;
    private readonly static ILog _platformLog;
    private readonly static ILog _engineLog;
    private readonly static ILog _gameLog;

    private readonly static ImmutableArray<ILogger> _loggers;
}
// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
