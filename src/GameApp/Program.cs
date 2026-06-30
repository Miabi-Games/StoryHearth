using StoryHearth.Platform.Raylib;

namespace GameApp;

public static class Program
{
    public static int Main(string[] args)
    {
        var appDef = new MyGameApplicationDefinition();

        return
            RaylibPlatform.ConvertToExitCode(
            RaylibPlatform.BuildAndRunApplication(appDef, args));
    }
}
