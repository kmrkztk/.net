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
    public static class EventingExtentions
    {
        public static IEnumerable<Event> AsEvent(this IEnumerable<EventLogEntry> entries) => entries.Using().Select(_ => Event.Of(_));
        public static IEnumerable<Event> AsEvent(this IEnumerable<EventRecord> records) => records.Using().Select(_ => Event.Of(_));
        public static IEnumerable<Event> AsEvent(this EventLog @event) => @event
            .Entries
            .Cast<EventLogEntry>()
            .AsEvent()
            .Each(_ => _.Log = @event.Log)
            .Reverse()
            ;
        public static IEnumerable<EventRecord> ReadAll(this EventLogReader reader)
        {
            using var r = reader;
            EventRecord rec;
            while ((rec = r.ReadEvent()) != null) yield return rec;
        }
        
    }
}
