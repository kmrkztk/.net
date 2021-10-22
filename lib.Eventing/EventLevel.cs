using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Runtime.Versioning;
using System.Collections;

namespace Lib.Eventing
{
    [SupportedOSPlatform("windows")]
    public class EventLevel : IEnumerable<EventLevel>
    {
        public EventLogEntryType Level { get; init; } 
        public int Value { get; init; }
        public bool Has(EventLogEntryType level) => (Level & level) > 0;
        public bool HasFailed => Has(EventLogEntryType.FailureAudit);
        public bool HasError  => Has(EventLogEntryType.Error       );
        public bool HasWarn   => Has(EventLogEntryType.Warning     );
        public bool HasInfo   => Has(EventLogEntryType.Information );
        public bool HasTrace  => Has(EventLogEntryType.SuccessAudit);
        EventLevel(int value) : this(EventLogEntryType.Information, value) { }
        EventLevel(EventLogEntryType level) : this(level, 0) { }
        EventLevel(EventLogEntryType level, int value)
        {
            Level = level;
            Value = value;
        }
        public override string ToString() => Level switch
        {
            EventLogEntryType.FailureAudit  => "Failed" ,
            EventLogEntryType.Error         => "Error"  ,
            EventLogEntryType.Warning       => "Warn"   ,
            EventLogEntryType.Information   => "Info"   ,
            EventLogEntryType.SuccessAudit  => "Trace"  ,
            _ => string.Join(" | ", this.Select(_ => _.ToString())),
        };
        public override bool Equals(object obj) => obj is EventLevel e && e.Level == Level;
        public override int GetHashCode() => Level.GetHashCode();
        public static EventLevel Failed { get; private set; } = new(EventLogEntryType.FailureAudit  , 1);
        public static EventLevel Error  { get; private set; } = new(EventLogEntryType.Error         , 2);
        public static EventLevel Warn   { get; private set; } = new(EventLogEntryType.Warning       , 3);
        public static EventLevel Info   { get; private set; } = new(EventLogEntryType.Information   , 4);
        public static EventLevel Trace  { get; private set; } = new(EventLogEntryType.SuccessAudit  , 5);
        public static EventLevel ErrorOver => Failed    | Error;
        public static EventLevel WarnOver  => ErrorOver | Warn;
        public static EventLevel InfoOver  => WarnOver  | Info;
        public static EventLevel All => InfoOver  | Trace;
        public static EventLevel None { get; private set; } = new(0, 0);
        public static EventLevel[] Levels => new[] { Failed, Error, Warn, Info, Trace, };
        public static EventLevel Of(EventLogEntryType level) => level switch
        {
            EventLogEntryType.FailureAudit  => Failed   ,
            EventLogEntryType.Error         => Error    ,
            EventLogEntryType.Warning       => Warn     ,
            EventLogEntryType.Information   => Info     ,
            EventLogEntryType.SuccessAudit  => Trace    ,
            _ => new(level),
        };
        public static EventLevel Of(int value) => Levels.FirstOrDefault(_ => _.Value == value) ?? new(value);
        public IEnumerator<EventLevel> GetEnumerator()
        {
            if (HasFailed) yield return Failed;
            if (HasError ) yield return Error;
            if (HasWarn  ) yield return Warn;
            if (HasInfo  ) yield return Info;
            if (HasTrace ) yield return Trace;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static explicit operator EventLevel(EventLogEntryType level) => Of(level);
        public static explicit operator EventLevel(byte? value) => value == null ? Info : Of(value.Value);
        public static EventLevel operator |(EventLevel a, EventLevel b) => new(a.Level | b.Level);
        public static EventLevel operator &(EventLevel a, EventLevel b) => new(a.Level & b.Level);
        public static EventLevel operator ^(EventLevel a, EventLevel b) => new(a.Level ^ b.Level);
        public static EventLevel operator ~(EventLevel a) => new(~a.Level);
    }
}
