namespace StoryHearth.Engine;

public class ApplicationBuilder
{
    public ApplicationBuilder() { }

    public Application Build(ApplicationDefinition appDef, string[] args)
    {
        return new Application();
    }
}
