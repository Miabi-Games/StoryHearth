using StoryHearth.Engine;

namespace StoryHearth.Platform.Raylib;

public class RaylibPlatform : Platform
{
    private RaylibPlatform() { }

    public static ApplicationRunResult BuildAndRunApplication(
        EngineApplicationDefinition appDefinition)
    {
        return ApplicationRunResult.Unknown;
    }

    public static int ConvertToExitCode(ApplicationRunResult result)
    {
        return result.IsSuccess ? 0 : 1;
    }
}
