using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using Lib.Reflection;

namespace Lib.Configuration
{
    public class Ini : IEnumerable<KeyValuePair<string, string>>
    {
        readonly char _comment = '#';
        readonly char _separator = '=';
        readonly string _pattern = "^(?<key>[^{0}]+){1}(?<value>[^{0}]+)({0}.+)?$";
        Dictionary<string, string> _table = new();
        public string this[string key]
        {
            get => _table.ContainsKey(key) ? _table[key] : null;
            set { if (_table.ContainsKey(key)) if (value == null) _table.Remove(key); else _table[key] = value; else _table.Add(key, value); }
        }
        public Ini() { }
        public Ini(string filename) : this() => Reset(filename);
        public Ini(Stream stream) : this() => Reset(stream);
        public Ini(TextReader reader) : this() => Reset(reader);
        public void Reset() => Reset(DefaultFileName);
        public void Reset(string filename) { using var sr = new StreamReader(filename); Reset(sr); }
        public void Reset(Stream stream) { using var sr = new StreamReader(stream); Reset(sr); }
        public void Reset(TextReader reader)
        {
            var lines = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "").Split('\n');
            var comment = new Regex("^" + _comment);
            var regex = new Regex(string.Format(_pattern, _comment, _separator));
            _table.Clear();
            _table = lines
                .Where(_ => !comment.IsMatch(_))
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .Select(_ => regex.Match(_))
                .Select(_ => (
                    key: _.Groups["key"].Value.Trim(), 
                    value: _.Groups["value"].Value.Trim()))
                .Where(_ => !string.IsNullOrEmpty(_.key))
                .Distinct((x, y) => string.Compare(x.key, y.key))
                .ToDictionary(_ => _.key, _ => _.value);
        }
        public T Cast<T>() => (T)Cast(typeof(T));
        public object Cast(Type type)
        {
            var instance = type.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>());
            var map = PropertyMap.Of(instance);
            map .Where(_ => _table.ContainsKey(_.Key))
                .Do(_ => Try.Of(() => _.Value.SetValue(this[_.Key])));
            return instance;
        }
        public override string ToString() => string.Join("\r\n", _table.Select(_ => string.Format("{0}{1}{2}", _.Key, _separator, _.Value)));
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _table.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public static string DefaultFileName => Process.GetCurrentProcess().MainModule.FileName + ".ini";
        public static T Load<T>() => Load<T>(new Ini(DefaultFileName));
        public static T Load<T>(string filename) => Load<T>(new Ini(filename));
        public static T Load<T>(Stream stream) => Load<T>(new Ini(stream));
        public static T Load<T>(TextReader reader) => Load<T>(new Ini(reader));
        public static T Load<T>(Ini ini) => ini.Cast<T>();
    }
}
