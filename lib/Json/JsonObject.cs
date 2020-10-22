using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Json
{
    public class JsonObject : Json, IDictionary<string, Json>
    {
        readonly Dictionary<string, Json> _value = new Dictionary<string, Json>();
        public JsonObject() : base() { }
        public JsonObject(Stream stream) : base(stream) { }
        public JsonObject(TextReader reader) : base(reader) { }
        public JsonObject(JsonReader reader) : base(reader) { }
        public override void Import(JsonReader reader)
        {
            Clear();
            var s = reader.ReadBlock() ?? throw reader.FormatException();
            var colon = ValueSeparator.ToString();
            var comma = Separator.ToString();
            if (s != StartChar) throw reader.FormatException();
            while (!reader.EndOfText)
            {
                if (((char)reader.Peek()).ToString() == EndChar)
                {
                    reader.Read();
                    break;
                }
                s = reader.ReadBlock();
                if (GetJsonValueType(s) != JsonValueType.String) throw reader.FormatException();
                var key = TrimBlock(s);
                s = reader.ReadBlock();
                if (s != colon) throw reader.FormatException();
                var val = Json.Load(reader);
                this.Add(key, val);
                s = reader.ReadBlock();
                if (s == EndChar) break;
                if (s == comma) continue;
                throw reader.FormatException();
            }
        }
        public override string Format(JsonFormatSettings setting)
        {
            var s = new StringBuilder();
            s.Append(StartChar);
            if (_value.Count > 0)
            {
                s.Append(setting.Separator);
                s.Append(string.Join(Separator + setting.Separator, _value.Keys.Select(_ => string.Format("{0}{1}{3}{1} {2} {4}",
                    setting.InnerIndents,
                    BlockChar,
                    ValueSeparator,
                    _,
                    _value[_].Format(setting.InnerSetting)))));
                s.Append(setting.Separator);
                s.Append(setting.Indents);
            }
            s.Append(EndChar);
            return s.ToString();
        }
        public override object Cast(Type type)
        {
            if (type == this.GetType()) return this;
            var instance = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var map = new PropertyMap(instance);
            foreach (var k in map.Keys) map.SetValue(k, this[k].Cast(map[k].Property.PropertyType));
            return instance;
        }
        public IEnumerable<Json> Find(params string[] keys)
        {
            foreach (var k in this.Keys)
            {
                if (keys.Contains(k)) yield return this[k];
                if (this[k] is JsonObject o1) foreach (var j in o1.Find(keys)) yield return j;
                if (this[k] is JsonArray a) foreach (var i in a) if (i is JsonObject o2) foreach (var j in o2.Find(keys)) yield return j;
            }
        }

        #region for IDictionary
        public Json this[string key] { get => ((IDictionary<string, Json>)_value)[key]; set => ((IDictionary<string, Json>)_value)[key] = value; }
        public ICollection<string> Keys => ((IDictionary<string, Json>)_value).Keys;
        public ICollection<Json> Values => ((IDictionary<string, Json>)_value).Values;
        public int Count => ((IDictionary<string, Json>)_value).Count;
        public bool IsReadOnly => ((IDictionary<string, Json>)_value).IsReadOnly;
        public bool ContainsKey(string key) => ((IDictionary<string, Json>)_value).ContainsKey(key);
        public void Add(string key, Json value)
        {
            ((IDictionary<string, Json>)_value).Add(key, value);
            value.SetParent(this);
        }
        public bool Remove(string key) => ((IDictionary<string, Json>)_value).Remove(key);
        public bool TryGetValue(string key, out Json value) => ((IDictionary<string, Json>)_value).TryGetValue(key, out value);
        public void Add(KeyValuePair<string, Json> item)
        {
            ((IDictionary<string, Json>)_value).Add(item);
            item.Value.SetParent(this);
        }
        public void Clear() => ((IDictionary<string, Json>)_value).Clear();
        public bool Contains(KeyValuePair<string, Json> item) => ((IDictionary<string, Json>)_value).Contains(item);
        public void CopyTo(KeyValuePair<string, Json>[] array, int arrayIndex) => ((IDictionary<string, Json>)_value).CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<string, Json> item) => ((IDictionary<string, Json>)_value).Remove(item);
        public IEnumerator<KeyValuePair<string, Json>> GetEnumerator() => ((IDictionary<string, Json>)_value).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IDictionary<string, Json>)_value).GetEnumerator();
        #endregion
    }
}
