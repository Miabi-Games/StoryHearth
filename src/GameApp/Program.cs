using StoryHearth.Platform.RaylibBackend;

namespace GameApp;

public static class Program
{
    public static int Main(string[] args)
    {
        var appDef = new MyGameApplicationDefinition();

        return
            RaylibApplicationRunner.ConvertToExitCode(
            RaylibApplicationRunner.BuildAndRunApplication(appDef, args));
    }
}
