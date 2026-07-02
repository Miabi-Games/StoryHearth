using StoryHearth.Platform.RaylibBackend;

namespace GameApp;

public static class Program
{
    public static int Main(string[] args)
    {
        var appDef = new MyGameApplicationDefinition();

        return
            RaylibServer.ConvertToExitCode(
            RaylibServer.BuildAndRunApplication(appDef, args));
    }
}
