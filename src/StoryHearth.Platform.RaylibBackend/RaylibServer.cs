using Raylib_cs;

using System;

namespace StoryHearth.Platform.RaylibBackend;

public class RaylibServer : IRenderServer, IDisposable
{
    public bool IsDisposed { get; private set; } = false;

    public RaylibServer()
    {
    }

    ~RaylibServer() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed) return;

        IsDisposed = true;
    }

    public static int2 GetScreenSize()
    {
        return new()
        {
            x = Raylib.GetScreenWidth(),
            y = Raylib.GetScreenHeight(),
        };
    }
    int2 IRenderServer.GetScreenSize() => GetScreenSize();
}
