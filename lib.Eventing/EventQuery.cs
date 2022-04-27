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
    public class EventQuery
    {
        readonly string _value;
        public EventQuery Prev { get; private set; }
        public EventQuery First => Chains.FirstOrDefault();
        public bool IsEmpty => string.IsNullOrEmpty(_value);
        public List<EventQuery> Chains
        {
            get
            {
                IEnumerable<EventQuery> prevs()
                {
                    var p = this;
                    while (p != null)
                    {
                        yield return p;
                        p = p.Prev;
                    }
                }
                return prevs().Reverse().ToList();
            }
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            Write(sb);
            Trace.WriteLine(sb);
            return sb.ToString();
        }
        protected virtual void Write(StringBuilder sb)
        {
            Prev?.Write(sb);
            sb.Append(_value);
        }
        public EventQuery() : this("") { }
        public EventQuery(string value) => _value = value;
        public EventQuery Next(string value) => new(value) { Prev = this };
        public EventQuery Next(EventQuery query)
        {
            query.First.Prev = this;
            return query;
        }
        public EventQuery And() => Next(" and ");
        public EventQuery And(IEnumerable<EventQuery> query) => And(query.ToArray());
        public EventQuery And(params EventQuery[] query) => Join(_ => _.And(), query);
        public EventQuery Or() => Next(" or ");
        public EventQuery Or(IEnumerable<EventQuery> query) => Or(query.ToArray());
        public EventQuery Or(params EventQuery[] query) => Join(_ => _.Or(), query);
        public EventQuery Join(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => Join(separate, query.ToArray());
        public EventQuery Join(Func<EventQuery, EventQuery> separate, params EventQuery[] query)
        {
            query = query.Where(_ => !_.IsEmpty).ToArray();
            var q = this;
            for (var i = 0; i < query.Length; i++)
            {
                q = q.Next(query[i]);
                if (i < query.Length - 1) q = separate(q);
            }
            return q;
        }
        public EventQuery Section(string pre, EventQuery query, string end) => Next(pre).Next(query).Next(end);
        public EventQuery Section(Func<EventQuery, EventQuery, EventQuery> section, Func<EventQuery, EventQuery> separate, params EventQuery[] query)
        {
            var q = this;
            if (query.Any()) q = section(q, Of().Join(separate, query));
            return q;
        }
        public EventQuery Parenthese(EventQuery query) => Section("(", query, ")");
        public EventQuery Parenthese(Func<EventQuery, EventQuery> separate, params EventQuery[] query) => Section((_1, _2) => _1.Parenthese(_2), separate, query);
        public EventQuery Parenthese(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => Parenthese(separate, query.ToArray());
        public EventQuery Parenthese(params EventQuery[] query) => Parenthese(_ => _.Or(), query);
        public EventQuery Parenthese(IEnumerable<EventQuery> query) => Parenthese(query.ToArray());
        public EventQuery Backet(EventQuery query) => Section("[", query, "]");
        public EventQuery Backet(Func<EventQuery, EventQuery> separate, params EventQuery[] query) => Section((_1, _2) => _1.Backet(_2), separate, query);
        public EventQuery Backet(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => Backet(separate, query.ToArray());
        public EventQuery Backet(params EventQuery[] query) => Backet(_ => _.Or(), query);
        public EventQuery Backet(IEnumerable<EventQuery> query) => Backet(query.ToArray());
        public EventQuery Brace(EventQuery query) => Section("{", query, "}");
        public EventQuery Brace(Func<EventQuery, EventQuery> separate, params EventQuery[] query) => Section((_1, _2) => _1.Brace(_2), separate, query);
        public EventQuery Brace(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => Brace(separate, query.ToArray());
        public EventQuery Brace(params EventQuery[] query) => Brace(_ => _.Or(), query);
        public EventQuery Brace(IEnumerable<EventQuery> query) => Brace(query.ToArray());
        public EventQuery System(EventQuery query) => Next(new EventQuery("System").Backet(query));
        public EventQuery System(Func<EventQuery, EventQuery> separate, params EventQuery[] query) => Section((_1, _2) => _1.System(_2), separate, query);
        public EventQuery System(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => System(separate, query.ToArray());
        public EventQuery System(params EventQuery[] query) => System(_ => _.And(), query);
        public EventQuery System(IEnumerable<EventQuery> query) => System(query.ToArray());
        public EventQuery Provider(EventQuery query) => Next(new EventQuery("Provider").Backet(query));
        public EventQuery Provider(Func<EventQuery, EventQuery> separate, params EventQuery[] query) => Section((_1, _2) => _1.Provider(_2), separate, query);
        public EventQuery Provider(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => Provider(separate, query.ToArray());
        public EventQuery Provider(params EventQuery[] query) => Provider(_ => _.Or(), query);
        public EventQuery Provider(IEnumerable<EventQuery> query) => Provider(query.ToArray());
        public EventQuery Provider(params string[] names) => Provider(names.Select(_ => Of().Name().Equal().Value(_)));
        public EventQuery Provider(IEnumerable<string> names) => Provider(names.ToArray());
        public EventQuery TimeCreated(EventQuery query) => Next(new EventQuery("TimeCreated").Backet(query));
        public EventQuery TimeCreated(Func<EventQuery, EventQuery> separate, params EventQuery[] query) => Section((_1, _2) => _1.TimeCreated(_2), separate, query);
        public EventQuery TimeCreated(Func<EventQuery, EventQuery> separate, IEnumerable<EventQuery> query) => TimeCreated(separate, query.ToArray());
        public EventQuery TimeCreated(params EventQuery[] query) => TimeCreated(_ => _.And(), query);
        public EventQuery TimeCreated(IEnumerable<EventQuery> query) => TimeCreated(query.ToArray());
        public EventQuery TimeCreatedFrom(DateTime value) => TimeCreated(Of().SystemTimeFrom(value));
        public EventQuery TimeCreatedTo  (DateTime value) => TimeCreated(Of().SystemTimeTo  (value));
        public EventQuery TimeCreated(DateTime from, DateTime to) => TimeCreated(Of().SystemTimeFrom(from), Of().SystemTimeTo(to));
        public EventQuery TimeCreated(int value) => TimeCreated(Of().TimeDiff(value));
        public EventQuery TimeCreated(TimeSpan value) => TimeCreated(Of().TimeDiff(value));
        public EventQuery TimeCreatedDiffHour(int value) => TimeCreated(Of().TimeDiffHour(value));
        public EventQuery TimeCreatedDiffDay(int value) => TimeCreated(Of().TimeDiffDay(value));
        public EventQuery Equal() => Next(" = ");
        public EventQuery GreaterThan() => Next(" > ");
        public EventQuery GreaterEqual() => Next(" >= ");
        public EventQuery LessThan() => Next(" < ");
        public EventQuery LessEqual() => Next(" <= ");
        public EventQuery Value(string value) => Next($"'{value}'");
        public EventQuery Value(int value) => Next($"{value}");
        public EventQuery Value(DateTime value) => Value(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        public EventQuery Value(EventLevel value) => Value(value.Value);
        public EventQuery Value(EventLogEntryType level) => Value((EventLevel)level);
        public EventQuery Key(string key) => Next($"@{key}");
        public EventQuery Name() => Key("Name");
        public EventQuery SystemTime() => Key("SystemTime");
        public EventQuery SystemTimeFrom(DateTime value) => SystemTime().GreaterEqual().Value(value);
        public EventQuery SystemTimeTo  (DateTime value) => SystemTime().LessEqual().Value(value);
        public EventQuery TimeDiff() => Next("timediff").Parenthese(Of().SystemTime());
        public EventQuery TimeDiff(int value) => TimeDiff().LessEqual().Value(value);
        public EventQuery TimeDiff(TimeSpan value) => TimeDiff((int)value.TotalMilliseconds);
        public EventQuery TimeDiffHour(int value) => TimeDiff(TimeSpan.FromHours(value));
        public EventQuery TimeDiffDay(int value) => TimeDiff(TimeSpan.FromDays(value));
        public EventQuery Level() => Next("Level");
        public EventQuery LevelIs(params EventLevel[] levels) => Parenthese(levels.Select(_ => Of().Level().Equal().Value(_)));
        public EventQuery LevelIs(IEnumerable<EventLevel> levels) => LevelIs(levels.ToArray());
        public EventQuery LevelIs(params EventLogEntryType[] levels) => LevelIs(levels.Select(_ => (EventLevel)_));
        public EventQuery LevelIs(IEnumerable<EventLogEntryType> levels) => LevelIs(levels.ToArray());
        public EventQuery Event() => Next("EventID");
        public EventQuery EventIs(params int[] id) => Parenthese(id.Select(_ => Of().Event().Equal().Value(_)));
        public EventQuery EventIs(IEnumerable<int> id) => EventIs(id.ToArray());
        public EventQuery Index() => Next("EventRecordID");
        public EventQuery IndexIs(params int[] id) => Parenthese(id.Select(_ => Of().Index().Equal().Value(_)));
        public EventQuery IndexIs(IEnumerable<int> id) => IndexIs(id.ToArray());
        public static EventQuery Of(string value = "") => new(value);
        public static EventQuery Empty => Of();
        public static EventQuery All => Of("*");
    }
}
