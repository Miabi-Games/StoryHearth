using System;

namespace StoryHearth.Platform;

public class Platform : IDisposable
{
    public Platform() { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }
}
