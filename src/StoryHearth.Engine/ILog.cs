using System.Diagnostics;

namespace StoryHearth.Engine;

public interface ILog
{
    const string PlatformLogTag = "platform";
    const string EngineLogTag = "engine";
    const string GameLogTag = "game";

    void WriteVerbose(string summary, string? details = null);
    void WriteInfo(string summary, string? details = null);
    void WriteWarning(string summary, string? details = null);
    void WriteError(string summary, string? details = null);
    void WriteMilestone(string summary, string? details = null);

#pragma warning disable IDE1006 // Naming Styles

    // These are just tags used to specify which log to provide or inject

    public interface Platform : ILog { }
    public interface Engine : ILog { }
    public interface Game : ILog { }

#pragma warning restore IDE1006 // Naming Styles
}

public static class ILogExtensions
{
    extension(ILog log)
    {
        [Conditional("DEBUG")] public void DebugWriteVerbose  (string summary, string? details = null) => log.WriteVerbose  (summary, details);
        [Conditional("DEBUG")] public void DebugWriteInfo     (string summary, string? details = null) => log.WriteInfo     (summary, details);
        [Conditional("DEBUG")] public void DebugWriteWarning  (string summary, string? details = null) => log.WriteWarning  (summary, details);
        [Conditional("DEBUG")] public void DebugWriteError    (string summary, string? details = null) => log.WriteError    (summary, details);
        [Conditional("DEBUG")] public void DebugWriteMilestone(string summary, string? details = null) => log.WriteMilestone(summary, details);
    }
}
