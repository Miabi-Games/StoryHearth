using System;

namespace StoryHearth.Engine;

public interface ILogger
{
    // Note: dateTime is expected to be the local time
    void Write(
        DateTime dateTime, string logTag, LogEventType type,
        string summary, string? details = null);
}
