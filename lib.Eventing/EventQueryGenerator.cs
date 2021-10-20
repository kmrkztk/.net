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
        public string Query => GenerateQuery().ToString();
        public abstract EventQuery GenerateQuery();
        public string ToXml(params string[] path)
        {
            var q = Query;
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
            xw.WriteStartElement("Query");
            xw.WriteAttributeString("Id", "0");
            xw.WriteAttributeString("Path", path.FirstOrDefault());
            foreach (var p in path)
            {
                xw.WriteStartElement("Select");
                xw.WriteAttributeString("Path", p);
                xw.WriteString(q);
                xw.WriteEndElement();
            }
            xw.WriteEndElement();
            xw.WriteEndElement();
            xw.Flush();
            return sb.ToString();
        }

        public static SystemQueryGenerator System => new();

        public class SystemQueryGenerator : EventQueryGenerator
        {
            readonly List<EventQueryGenerator> _gens = new()
            {
                new ProviderQueryGenerator(),
                new LevelQueryGenerator(),
                new EventIdQueryGenerator(),
                new TimeCreatedQueryGenerator(),
            };
            public ProviderQueryGenerator Provider => (ProviderQueryGenerator)_gens[0];
            public LevelQueryGenerator Level => (LevelQueryGenerator)_gens[1];
            public EventIdQueryGenerator EventID => (EventIdQueryGenerator)_gens[2];
            public TimeCreatedQueryGenerator TimeCreated => (TimeCreatedQueryGenerator)_gens[3];
            public override EventQuery GenerateQuery() => _gens.Any() ? EventQuery.All.Backet(EventQuery.Of().System(_gens.Select(_ => _.GenerateQuery()))) : EventQuery.All;
        }
        public abstract class AnyValuesQueryGenerator<T> : EventQueryGenerator
        {
            readonly List<T> _values = new();
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
        public class LevelQueryGenerator : AnyValuesQueryGenerator<EventLogEntryType>
        {
            public override EventQuery GenerateQuery(List<EventLogEntryType> values) => EventQuery.Of().LevelIs(values);
        }
        public class TimeCreatedQueryGenerator : EventQueryGenerator
        {
            public DateTime? From { get; set; }
            public DateTime? To { get; set; }
            public TimeSpan? Diff { get; set; }
            public void DiffHours(int value) => Diff = TimeSpan.FromHours(value);
            public void DiffDays(int value) => Diff = TimeSpan.FromDays(value);
            public override EventQuery GenerateQuery() =>
                Diff != null ? EventQuery.Of().TimeCreated(Diff.Value) :
                From != null && To != null ? EventQuery.Of().TimeCreated(From.Value, To.Value) : 
                From != null ? EventQuery.Of().TimeCreatedFrom(From.Value) :
                To   != null ? EventQuery.Of().TimeCreatedTo  (To  .Value) :
                EventQuery.Empty;
        }
    }
}
