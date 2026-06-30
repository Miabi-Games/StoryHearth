using Raylib_cs;

using StoryHearth.Engine;

namespace StoryHearth.Platform.RaylibBackend;

public static class RaylibApplicationRunner
{
    const int SCREEN_WIDTH = 1920;
    const int SCREEN_HEIGHT = 1080;

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
        Raylib.InitWindow(SCREEN_WIDTH, SCREEN_HEIGHT, app.WindowTitle);
        Raylib.SetTargetFPS(60);

        while(!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText(app.WindowTitle, 40, 40, 40, Color.RayWhite);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
