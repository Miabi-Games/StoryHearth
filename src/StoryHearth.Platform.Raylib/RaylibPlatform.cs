using StoryHearth.Engine;

namespace StoryHearth.Platform.Raylib;

public class RaylibPlatform : Platform
{
    private RaylibPlatform() { }

    public static ApplicationRunResult BuildAndRunApplication(
        ApplicationDefinition appDefinition, string[] args)
    {
        using Application app = Build(appDefinition, args);
        return Run(app);
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

    private static ApplicationRunResult Run(Application app)
    {
        return ApplicationRunResult.Success;
    }
}
