﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Lib;
using Lib.Configuration;
using Lib.Eventing;
using Lib.Logs;

namespace evvw
{
    class Program
    {
        static void Main()
        {
            var a = Arguments<Option>.Load();
            var sep = "--------------------------------------------------";
            var query = a.Options.GetQueryGenerator();
            var events = Event.Of(query.GenerateQuery(), a.Distinct())
                    .TakeIf(a.Options.Count)
                    .TailIf(a.Options.Tail)
                    ;
            do
            {
                events
                    .Console(_ => a.Options.Separator, _ => sep)
                    .Console(_ => _.ToString(a.Options.Format))
                    .Each(_ => query.TimeCreated.From = _.DateTime ?? query.TimeCreated.From)
                    .Do()
                    ;
                if (a.Options.Follow)
                {
                    System.Threading.Thread.Sleep(500);
                    query.TimeCreated.To = null;
                    query.TimeCreated.Diff = null;
                    events = Event.Of(query.GenerateQuery(), a.Distinct());
                }

            } while (a.Options.Follow);
        }
        [Detail("")]
        class Option
        {
            [Command] [Command("s")] [CommandValue(Separator = ',')] public List<string> Source { get; set; } = new();
            [Command] [Command("i")] [CommandValue(Separator = ',')] public List<int>    Index  { get; set; } = new();
            [Command] [CommandValue(Separator = ',')] public List<int> Level { get; set; } = new();
            [Command] [CommandValue(Separator = ',')] public List<int> ID { get; set; } = new();
            [Command] [CommandValue] public DateTime? From { get; set; } = null;
            [Command] [CommandValue] public DateTime? To   { get; set; } = null;
            [Command] [CommandValue] public int DiffTime   { get; set; } = 3600000;
            [Command] [CommandValue] public TimeSpan DiffTimeSpan { get => TimeSpan.FromMilliseconds(DiffTime); set => DiffTime = (int)value.TotalMilliseconds; }
            [Command] [Command("h")] [CommandValue] public int DiffHour { get => (int)DiffTimeSpan.TotalHours; set => DiffTimeSpan = TimeSpan.FromHours(value); }
            [Command] [Command("d")] [CommandValue] public int DiffDay  { get => (int)DiffTimeSpan.TotalDays ; set => DiffTimeSpan = TimeSpan.FromDays(value) ; }
            [Command] [Command("n")] [CommandValue] public int Count { get; set; } = -1;
            [Command] [Command("t")] [CommandValue] public int Tail  { get; set; } = -1;
            [Command] [Command("q")] [CommandValue] public string Query { get; set; }
            [Command] [Command("f")] [CommandValue] public string Format { get; set; } = "{datetime} [{level}] {message}";
            [Command] public bool Separator { get; set; }
            [Command] public bool Follow { get; set; }
            [Command] public bool Failed { get => Level.Contains(EventLevel.Failed.Value); set => Level.Add(EventLevel.Failed    .Value); }
            [Command] public bool Error  { get => Level.Contains(EventLevel.Error .Value); set => Level.Add(EventLevel.ErrorOver .Value); }
            [Command] public bool Warn   { get => Level.Contains(EventLevel.Warn  .Value); set => Level.Add(EventLevel.WarnOver  .Value); }
            [Command] public bool Info   { get => Level.Contains(EventLevel.Info  .Value); set => Level.Add(EventLevel.InfoOver  .Value); }
            public EventLevel GetLevel()
            {
                if (!Level.Any()) return EventLevel.All;
                return Level.Select(_ => EventLevel.Of(_)).Do(EventLevel.None, (_0, _1) => _1 | _0);
            }
            public EventQueryGenerator.SystemQueryGenerator GetQueryGenerator()
            {
                var gen = EventQueryGenerator.System;
                gen.Provider.Add(Source);
                gen.Level.Add(GetLevel());
                gen.Index.Add(Index);
                gen.EventID.Add(ID);
                gen.TimeCreated.From = From;
                gen.TimeCreated.To = To;
                gen.TimeCreated.Diff = DiffTime == 0 ? null : DiffTimeSpan;
                return gen;
            }
            public EventQuery GetQuery()
            {
                if (!string.IsNullOrEmpty(Query)) return EventQuery.Of(Query);
                return GetQueryGenerator().GenerateQuery();
            }
        }
    }
}
