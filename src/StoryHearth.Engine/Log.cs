namespace StoryHearth.Engine;

public abstract class Log : ILog
{
    protected abstract void Write(LogEventType type, string summary, string? details);

    void ILog.WriteVerbose(string summary, string? details)   => Write(LogEventType.Verbose, summary, details);
    void ILog.WriteInfo(string summary, string? details)      => Write(LogEventType.Info, summary, details);
    void ILog.WriteWarning(string summary, string? details)   => Write(LogEventType.Warning, summary, details);
    void ILog.WriteError(string summary, string? details)     => Write(LogEventType.Error, summary, details);
    void ILog.WriteMilestone(string summary, string? details) => Write(LogEventType.Milestone, summary, details);
}
