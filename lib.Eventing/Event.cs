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
        public long? Index          { get; set; }
        public long? ID             { get; set; }
        public string Log           { get; set; }
        public string Source        { get; set; }
        public string Category      { get; set; }
        public string UserName      { get; set; }
        public string MachineName   { get; set; }
        public string Message       { get; set; }
        public EventLevel Level     { get; set; }
        public DateTime? DateTime   { get; set; }
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
        public static Event Of(EventLogEntry entry) => new()
        {
            //Log = entry.Container,
            ID = entry.InstanceId,
            Index = entry.Index,
            Source = entry.Source,
            Category = entry.Category,
            UserName = entry.UserName,
            MachineName = entry.MachineName,
            Message = entry.Message.TrimEnd("\r\n"),
            Level = (EventLevel)entry.EntryType,
            DateTime = entry.TimeGenerated,
        };
        public static Event Of(EventRecord record) => new()
        {
            Log = record.LogName,
            ID = record.Id,
            Index = record.RecordId,
            Source = record.ProviderName,
            //Category = record.Category,
            UserName = record.UserId?.Value,
            MachineName = record.MachineName,
            Message = record.FormatDescription().TrimEnd("\r\n"),
            Level = (EventLevel)record.Level,
            DateTime = record.TimeCreated,
        };
        public static IEnumerable<Event> Of(EventQuery query, IEnumerable<string> path) => Of(query, path.ToArray());
        public static IEnumerable<Event> Of(EventQuery query, params string[] path) => path
            .Select(_ => new EventLogQuery(_, PathType.LogName, query.ToString()))
            .Select(_ => new EventLogReader(_))
            .Select(_ => _.ReadAll())
            .Select(_ => _.AsEvent())
            .SoftOrder((a, b) => 
            {
                var a_ = a.DateTime?.Ticks ?? 0;
                var b_ = b.DateTime?.Ticks ?? 0;
                return a_ < b_ ? -1 : a_ > b_ ? 1 : 0;
            })
            ;
    }
}
