using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Configuration;
using Lib.Jsons;
using Lib.Reflection;
using Lib.Logs.DefaultLoggers;

namespace Lib.Logs
{
    public class LogListener : List<ILogger>
    {
        static readonly Dictionary<string, Type> _loggers = typeof(ILogger)
                .GetEnumerableOfType()
                .Where(_ => !_.IsAbstract)
                .Where(_ => !_.IsInterface)
                .SelectMany(type => NameAttribute.GetTypeNames(type)
                .Concat(new[] { type.Name, })
                .Select(n => n.ToLower())
                .Distinct()
                .Select(name => (name, type)))
                .ToDictionary(_ => _.name, _ => _.type);
        List<ILogger> _listener;
        public void Reload(Stream stream)
        {
            _listener.Do(_ =>
            {
                _.Dispose();
                this.Remove(_);
            });
            _listener = Try
                .Of(() => Json.Load(stream)
                    .AsArray()
                    .Select(_ => (settings: _, key: _["type"]?.Value?.ToLower() ?? "default"))
                    .Where(_ => _loggers.ContainsKey(_.key))
                    .Select(_ => _.settings.Cast(_loggers[_.key]))
                    .Cast<ILogger>()
                    .ToList())
                .Catch(() => new() { new ConsoleLogger(), new TraceLogger(), })
                .Invoke();
            _listener.Do(_ => this.Add(_));
        }
        public void Refresh() => this
            .Where(_ => 
                Try.Of(() =>
                {
                    _.Refresh();
                    return false;
                })
                .Catch(() => true)
                .Invoke())
            .ToList()
            .Do(_ => Remove(_));
    }
}
