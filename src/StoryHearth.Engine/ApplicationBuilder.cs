namespace StoryHearth.Engine;

public class ApplicationBuilder
{
    public ApplicationBuilder() { }

    public Application Build(ApplicationDefinition appDefinition, string[] args)
    {
        return new Application(appDefinition);
    }
}
