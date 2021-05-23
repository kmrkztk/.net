using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace Lib.Logs
{
    public class LogParameters
    {
        public delegate object Generator(Log log, string message);
        readonly Dictionary<string, Generator> _dictionary = new();
        public LogParameters() { }
        public LogParameters((string key, Generator generator)[] values) : this() 
        {
            foreach (var (key, generator) in values) _dictionary.Add(key, generator);
        }
        public Generator this[string key]
        {
            get => _dictionary[key];
            set { if (_dictionary.ContainsKey(key)) _dictionary[key] = value; else _dictionary.Add(key, value); }
        }
        public void AddRange(IEnumerable<(string keyword, Generator generator)> generators) => generators.Foreach(_ => this[_.keyword] = _.generator);
        public string[] Keys => _dictionary.Keys.ToArray();
        public object[] Generate(Log log, string message) => _dictionary.Values.Select(_ => _.Invoke(log, message)).ToArray();
    }
}
