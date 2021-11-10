using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Versioning;
using System.Xml;
using System.Xml.Serialization;

namespace Lib.Eventing
{
    [SupportedOSPlatform("windows")]
    public abstract class EventQueryGenerator
    {
        public string QueryString => GenerateQuery().ToString();
        public abstract EventQuery GenerateQuery();
        public string GenerateXPath(string path0, params string[] path) => GenerateXPath(path.Prepend(path0));
        public string GenerateXPath(IEnumerable<string> path)
        {
            var q = QueryString;
            var sb = new StringBuilder();
            var setting = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine,
            };
            using var xw = XmlWriter.Create(sb, setting);

            xw.WriteStartElement("QueryList");
            path.Do((_, i) =>
            {
                xw.WriteStartElement("Query");
                xw.WriteAttributeString("Id", i.ToString());
                xw.WriteAttributeString("Path", _);
                xw.WriteStartElement("Select");
                xw.WriteAttributeString("Path", _);
                xw.WriteString(q);
                xw.WriteEndElement();
                xw.WriteEndElement();
            });
            xw.WriteEndElement();
            xw.Flush();
            return sb.ToString();
        }
        public EventLogQuery Generate(string path0) => new(path0, PathType.LogName, QueryString);
        public static SystemQueryGenerator System => new();
        public static AnyQueryGenerator Any(EventQuery query) => new(query);

        public class AnyQueryGenerator : EventQueryGenerator
        {
            public EventQuery Query { get; set; }
            public override EventQuery GenerateQuery() => Query;
            public AnyQueryGenerator(EventQuery query) => Query = query;
        }
        public class SystemQueryGenerator : EventQueryGenerator
        {
            readonly List<EventQueryGenerator> _gens = new()
            {
                new ProviderQueryGenerator(),
                new LevelQueryGenerator(),
                new EventIdQueryGenerator(),
                new IndexQueryGenerator(),
                new TimeCreatedQueryGenerator(),
            };
            public ProviderQueryGenerator    Provider    => (ProviderQueryGenerator   )_gens[0];
            public LevelQueryGenerator       Level       => (LevelQueryGenerator      )_gens[1];
            public EventIdQueryGenerator     EventID     => (EventIdQueryGenerator    )_gens[2];
            public IndexQueryGenerator       Index       => (IndexQueryGenerator      )_gens[3];
            public TimeCreatedQueryGenerator TimeCreated => (TimeCreatedQueryGenerator)_gens[4];
            public override EventQuery GenerateQuery() => _gens.Any() ? EventQuery.All.Backet(EventQuery.Of().System(_gens.Select(_ => _.GenerateQuery()))) : EventQuery.All;
        }
        public abstract class AnyValuesQueryGenerator<T> : EventQueryGenerator
        {
            protected readonly List<T> _values = new();
            public abstract EventQuery GenerateQuery(List<T> values);
            public override EventQuery GenerateQuery() => _values.Any() ? GenerateQuery(_values) : EventQuery.Empty;
            public void Add(params T[] value) => _values.AddRange(value);
            public void Add(IEnumerable<T> value) => Add(value.ToArray());
            public void Clear() => _values.Clear();
        }
        public class ProviderQueryGenerator : AnyValuesQueryGenerator<string>
        {
            public override EventQuery GenerateQuery(List<string> values) => EventQuery.Of().Provider(values);
        }
        public class EventIdQueryGenerator : AnyValuesQueryGenerator<int>
        {
            public override EventQuery GenerateQuery(List<int> values) => EventQuery.Of().EventIs(values);
        }
        public class IndexQueryGenerator : AnyValuesQueryGenerator<int>
        {
            public override EventQuery GenerateQuery(List<int> values) => EventQuery.Of().IndexIs(values);
        }
        public class LevelQueryGenerator : AnyValuesQueryGenerator<EventLevel>
        {
            public override EventQuery GenerateQuery(List<EventLevel> values) => EventQuery.Of().LevelIs(values.SelectMany(_ => _.Select(__ => __)).Distinct());
        }
        public class TimeCreatedQueryGenerator : EventQueryGenerator
        {
            public DateTime? From { get; set; }
            public DateTime? To { get; set; }
            public TimeSpan? Diff { get; set; }
            public void DiffHours(int value) => Diff = TimeSpan.FromHours(value);
            public void DiffDays(int value) => Diff = TimeSpan.FromDays(value);
            public override EventQuery GenerateQuery() =>
                Diff != null ? EventQuery.Of().TimeCreated(EventQuery.Of().TimeDiff(Diff.Value).And().TimeDiff().GreaterEqual().Value(0)) :
                From != null && To != null ? EventQuery.Of().TimeCreated(From.Value, To.Value) : 
                From != null ? EventQuery.Of().TimeCreatedFrom(From.Value) :
                To   != null ? EventQuery.Of().TimeCreatedTo  (To  .Value) :
                EventQuery.Empty;
        }
    }
}
