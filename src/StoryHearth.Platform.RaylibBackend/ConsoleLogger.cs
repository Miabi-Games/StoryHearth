using StoryHearth.Engine;

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace StoryHearth.Platform.RaylibBackend;

public partial class ConsoleLogger : ILogger
{
    static ConsoleLogger()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
    }

    public void Write(
        DateTime dateTime, string logTag, LogEventType type,
        string summary, string? details = null)
    {
        lock (_lock)
        {
            _message.Clear();
            _message.Append(dateTime.ToString("yyyy-MM-dd HH:mm:ss'T'zzz"));
            _message.Append(' ').Append(type.GetLogTag());
            _message.Append(": [").Append(logTag).Append("] ");
            _message.AppendLine(summary);

            if (!string.IsNullOrWhiteSpace(details))
            {
                string formatted = HBarMarker().Replace(details, _hbar);

                _message.AppendLine();
                _message.Append(formatted);

                if (!formatted.EndsWith('\n')) _message.AppendLine();
            }

            Console.Write(_message);
        }
    }

    private StringBuilder _message = new();

    private readonly Lock _lock = new();
    private readonly string _hbar = new string('─', Console.WindowWidth) + "\n";

    [GeneratedRegex(@"^---+(?:\r\n?|\n)", RegexOptions.Multiline)]
    private static partial Regex HBarMarker();
}
