using System;

namespace StoryHearth.Engine;

public interface ILogger
{
    void Write(
        DateTime dateTime,
        string logName,
        LogEventType type,
        string summary,
        string? details = null);
}
