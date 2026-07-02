using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace StoryHearth.Engine;

public class SimpleLog : Log
{
    public string Name { get; }
    public ImmutableArray<ILogger> Loggers { get; }

    public SimpleLog(string name, IEnumerable<ILogger> loggers)
    {
        Name = name ?? "";
        Loggers = loggers.ToImmutableArray();
    }

    protected override void Write(LogEventType type, string summary, string? details)
    {
        var now = DateTime.Now;
        foreach (var logger in Loggers) logger.Write(now, Name, type, summary, details);
    }
}
