namespace StoryHearth.Engine;

public class ApplicationBuilder
{
    public ApplicationBuilder() { }

    public ApplicationContainer Build(
        ApplicationDefinition appDefinition,
        string[] args)
    {
        var app = new Application();
        return new(app);
    }
}
