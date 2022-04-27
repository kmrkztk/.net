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
        public long? Index { get; set; }
        public long? ID { get; set; }
        public string Log { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public EventLevel Level { get; set; }
        public DateTime? DateTime { get; set; }
        public override string ToString() => ToString(false);
        public string ToString(bool message) => ToString("[{log}] #{index} :{id} [{level}] {datetime} {source} " + (message ? "\r\n{message}" : ""));
        public string ToString(string format) => format?.ReplaceKeywords(
            new[] { "log", "index", "id", "level", "datetime", "source", "category", "user", "machine", "message", },
            new[] { Log, Index?.ToString(), ID?.ToString(), Level.ToString(), DateTime?.ToString(), Source, Category, UserName, MachineName, Message, });
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
            Message = Try.Of(() => record.FormatDescription().TrimEnd("\r\n")).Invoke(),
            Level = (EventLevel)record.Level,
            DateTime = record.TimeCreated,
        };
        public static IEnumerable<Event> Of(string query, string path) => Of(new EventLogQuery(path, PathType.LogName, query));
        public static IEnumerable<Event> Of(string query, params string[] path) => Of(path.Select(_ => new EventLogQuery(_, PathType.LogName, query)));
        public static IEnumerable<Event> Of(EventQuery query, IEnumerable<string> path) => Of(query, path.ToArray());
        public static IEnumerable<Event> Of(EventQuery query, params string[] path) => Of(query.ToString(), path);
        public static IEnumerable<Event> Of(EventLogQuery query) => Of(new EventLogReader(query));
        public static IEnumerable<Event> Of(IEnumerable<EventLogQuery> query) => Of(query.Select(_ => new EventLogReader(_)));
        public static IEnumerable<Event> Of(EventLogReader reader) => reader.ReadAll().AsEvent();
        public static IEnumerable<Event> Of(IEnumerable<EventLogReader> reader) => reader
            .Select(_ => Of(_))
            .SoftOrder((a, b) =>
            {
                var a_ = a.DateTime?.Ticks ?? 0;
                var b_ = b.DateTime?.Ticks ?? 0;
                return a_ < b_ ? -1 : a_ > b_ ? 1 : 0;
            })
            ;
    }
}
