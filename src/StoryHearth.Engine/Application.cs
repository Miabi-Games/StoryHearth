using System;

namespace StoryHearth.Engine;

public class Application : IDisposable
{
    public string WindowTitle => "A StoryHearth Game"; // will eventually come from data file

    public Application(ApplicationDefinition appDefinition)
    {
        _appDefinition = appDefinition;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    private ApplicationDefinition _appDefinition;
}
