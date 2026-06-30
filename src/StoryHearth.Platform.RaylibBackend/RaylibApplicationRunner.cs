using Raylib_cs;

using StoryHearth.Engine;

namespace StoryHearth.Platform.RaylibBackend;

public static class RaylibApplicationRunner
{
    public static ApplicationRunResult BuildAndRunApplication(
        ApplicationDefinition appDefinition, string[] args)
    {
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
        ApplicationBuilder builder = new ApplicationBuilder();
        return builder.Build(appDefinition, args);
    }

    private static void Run(Application app)
    {
        int2 window_size = app.Settings.CanvasTargetSize;
        int2 window_min_size = app.Settings.WindowMinSize;
        string windowTitle = app.Settings.WindowTitle;

        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(window_size.x, window_size.y, windowTitle);
        Raylib.SetTargetFPS(60);
        Raylib.SetWindowMinSize(window_min_size.x, window_min_size.y);

        while(!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText(windowTitle, 40, 40, 40, Color.RayWhite);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
