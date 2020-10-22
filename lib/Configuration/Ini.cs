using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lib.Configuration
{
    public class Ini : IEnumerable<KeyValuePair<string, string>>
    {
        readonly char _comment = '#';
        readonly char _separator = '=';
        readonly string _pattern = "^(?<key>[^{0}]+){1}(?<value>[^{0}]+)({0}.+)?$";
        readonly Dictionary<string, string> _table = new Dictionary<string, string>();
        public string this[string key] => _table.ContainsKey(key) ? _table[key] : null;
        public Ini() => Reset();
        public Ini(string filename) => Reset(filename);
        public Ini(TextReader reader) => Reset(reader);
        public void Reset() => Reset(Process.GetCurrentProcess().MainModule.FileName + ".ini");
        public void Reset(string filename) { using (var sr = new StreamReader(filename)) Reset(sr); }
        public void Reset(TextReader reader)
        {
            var lines = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "").Split('\n');
            var comment = new Regex("^" + _comment);
            var regex = new Regex(string.Format(_pattern, _comment, _separator));
            _table.Clear();
            foreach (var m in lines
                .Where(_ => !comment.IsMatch(_))
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .Select(_ => regex.Match(_))
                .Select(_ => new[]
                {
                    _.Groups["key"].Value.Trim(),
                    _.Groups["value"].Value.Trim(),
                })
                .Where(_ => !string.IsNullOrEmpty(_[0]))
                ) if (!_table.ContainsKey(m[0])) _table.Add(m[0], m[1]);
        }
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _table.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public static T Load<T>() => Load<T>(new Ini());
        public static T Load<T>(string filename) => Load<T>(new Ini(filename));
        public static T Load<T>(TextReader reader) => Load<T>(new Ini(reader));
        public static T Load<T>(Ini ini)
        {
            var type = typeof(T);
            var instance = (T)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var map = new PropertyMap(instance);
            foreach (var m in map) try { map.SetValue(m.Key, ini[m.Key]); } catch { }
            return instance;
        }
    }
}
