using StoryHearth.Engine;
using StoryHearth.Platform.Raylib;

namespace GameApp;

public class MyGameApplication : GameApplication
{
    public static int Main(string[] args)
    {
        var appDef = new MyGameApplicationDefinition();

        return
            RaylibPlatform.ConvertToExitCode(
            RaylibPlatform.BuildAndRunApplication(appDef));
    }
}
