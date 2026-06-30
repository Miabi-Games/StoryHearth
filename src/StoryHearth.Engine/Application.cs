using System;

namespace StoryHearth.Engine;

public class Application : IDisposable
{
    public Application()
    {
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}
