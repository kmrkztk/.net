using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Versioning;

namespace Lib.Eventing
{
    [SupportedOSPlatform("windows")]
    public class Event
    {
        public int Index { get; init; }
        public long ID { get; init; }
        public string Log { get; init; }
        public string Source { get; init; }
        public string Category { get; init; }
        public string UserName { get; init; }
        public string MachineName { get; init; }
        public string Message { get; init; }
        public EventLogEntryType Level { get; init; }
        public DateTime DateTime { get; init; }
        public override string ToString() => ToString(false);
        public string ToString(bool message) =>
            $"[{Log}] " +
            $"#{Index} " +
            $":{ID} " +
            $"[{Level}] " +
            $"{DateTime} " +
            $"{Source} " +
            (message ? $"\r\n{Message}" : "") +
            "";
        public static IEnumerable<Event> Of(EventLog log) => log
            .Entries
            .Cast<EventLogEntry>()
            .Select(_ => Of(log, _))
            .Reverse()
            ;
        public static Event Of(EventLog log, EventLogEntry entry) => new()
        {
            Log = log.Log,
            ID = entry.InstanceId,
            Index = entry.Index,
            Source = entry.Source,
            Category = entry.Category,
            UserName = entry.UserName,
            MachineName = entry.MachineName,
            Message = entry.Message,
            Level = entry.EntryType != 0 ? entry.EntryType : EventLogEntryType.Information,
            DateTime = entry.TimeGenerated,
        };

    }
}
