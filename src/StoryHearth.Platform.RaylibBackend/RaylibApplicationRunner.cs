using Raylib_cs;

using StoryHearth.Engine;

using System;
using System.Threading;

namespace StoryHearth.Platform.RaylibBackend;

public static class RaylibApplicationRunner
{
    public static ApplicationRunResult BuildAndRunApplication(
        ApplicationDefinition appDefinition, string[] args)
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

        using (Application app = Build(appDefinition, args)) Run(app);
        return ApplicationRunResult.Success;
    }

    public static int ConvertToExitCode(ApplicationRunResult result)
    {
        return result.IsSuccess ? 0 : 1;
    }

    private static Application Build(
        ApplicationDefinition appDefinition, string[] args)
    {
        var builder = new ApplicationBuilder();
        return builder.Build(appDefinition, args);
    }

    private static void Run(Application app)
    {
        InitializeWindow(app);
        RunMainLoop(app);
        CloseWindow();
    }

    private static void InitializeWindow(Application app)
    {
        int2 canvas_target_size = app.Settings.CanvasTargetSize;
        int2 window_min_size = app.Settings.WindowMinSize;
        string window_title = app.Settings.WindowTitle ?? "";

        if (canvas_target_size.x <= 0 || canvas_target_size.y <= 0)
        {
            throw new System.Exception(
                "The canvas target size must be positive");
        }

        if (window_min_size.x <= 0 || window_min_size.y <= 0)
        {
            throw new System.Exception(
                "The minimum window size must be postiive");
        }

        int2 window_size = new()
        {
            x = Math.Max(window_min_size.x, canvas_target_size.x),
            y = Math.Max(window_min_size.y, canvas_target_size.y),
        };

        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);

        Raylib.InitWindow(window_size.x, window_size.y, window_title);

        Raylib.SetTargetFPS(60);
        Raylib.SetWindowMinSize(window_min_size.x, window_min_size.y);
    }

    private static void CloseWindow()
    {
        Raylib.CloseWindow();
    }

    private static void RunMainLoop(Application app)
    {
        string window_title = app.Settings.WindowTitle ?? "";

        using var server = new RaylibServer();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText(window_title, 40, 40, 40, Color.RayWhite);

            Raylib.EndDrawing();
        }
    }

    private static bool _runOnce = false;
}
