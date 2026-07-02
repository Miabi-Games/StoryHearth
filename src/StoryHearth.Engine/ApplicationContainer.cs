using System;
using System.Threading;

namespace StoryHearth.Engine;

/// <summary>
///     Contains an instance of the Application class and all of its
///     dependencies. Used by platforms to manage the application's lifetime.
/// </summary>
public sealed class ApplicationContainer : IDisposable
{
    public Application Application { get; }

    public ApplicationContainer(
        Application application,
        IDisposable? dependencyContainer = null)
    {
        Application = application;
        _dependencyContainer = dependencyContainer;
    }

    public void Dispose()
    {
        var container = Interlocked.Exchange(ref _dependencyContainer, null);
        container?.Dispose();
        GC.SuppressFinalize(this);
    }

    private IDisposable? _dependencyContainer;
}
