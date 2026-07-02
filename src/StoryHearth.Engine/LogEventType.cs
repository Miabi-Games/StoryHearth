namespace StoryHearth.Engine;

public enum LogEventType : int
{
    Verbose = -1,
    Info,
    Warning,
    Error,
    Milestone,
}

public static class LogEventTypeExtensions
{
    extension(LogEventType type)
    {
        public bool IsVerbose => type == LogEventType.Verbose;
        public bool IsInfo => type == LogEventType.Info;
        public bool IsWarning => type == LogEventType.Warning;
        public bool IsError => type == LogEventType.Error;
        public bool IsMilestone => type == LogEventType.Milestone;

        public bool IsWarningOrError => type.IsWarning || type.IsError;

        // may later add extensions to help with log filtering
    }
}
