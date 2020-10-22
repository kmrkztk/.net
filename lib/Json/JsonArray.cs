using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Json
{
    public class JsonArray : Json, IList<Json>
    {
        readonly List<Json> _value = new List<Json>();
        public JsonArray() : base() { }
        public JsonArray(Stream stream) : base(stream) { }
        public JsonArray(TextReader reader) : base(reader) { }
        public JsonArray(JsonReader reader) : base(reader) { }
        public override void Import(JsonReader reader)
        {
            Clear();
            var s = reader.ReadBlock();
            var comma = Separator.ToString();
            if (s != StartChar) throw reader.FormatException();
            while (!reader.EndOfText)
            {
                if (((char)reader.Peek()).ToString() == EndChar)
                {
                    reader.Read();
                    break;
                }
                var val = Json.Load(reader);
                this.Add(val);
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
                s.Append(string.Join(Separator + setting.Separator, _value.Select(_ => string.Format("{0}{1}",
                   setting.InnerIndents,
                   _.Format(setting.InnerSetting)))));
                s.Append(setting.Separator);
                s.Append(setting.Indents);
            }
            s.Append(EndChar);
            return s.ToString();
        }
        public override object Cast(Type type)
        {
            if (type == this.GetType()) return this;
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>)) throw new ArgumentException();
            var generic = type.GetGenericArguments()[0];
            var instance = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as IList;
            foreach (var o in this.Select(_ => _.Cast(generic))) instance.Add(o);
            return instance;
        }
        #region for IList
        public Json this[int index] { get => ((IList<Json>)_value)[index]; set => ((IList<Json>)_value)[index] = value; }
        public int Count => ((IList<Json>)_value).Count;
        public bool IsReadOnly => ((IList<Json>)_value).IsReadOnly;
        public void Add(Json item)
        {
            ((IList<Json>)_value).Add(item);
            item.SetParent(this);
        }
        public void Clear() => ((IList<Json>)_value).Clear();
        public bool Contains(Json item) => ((IList<Json>)_value).Contains(item);
        public void CopyTo(Json[] array, int arrayIndex) => ((IList<Json>)_value).CopyTo(array, arrayIndex);
        public IEnumerator<Json> GetEnumerator() => ((IList<Json>)_value).GetEnumerator();
        public int IndexOf(Json item) => ((IList<Json>)_value).IndexOf(item);
        public void Insert(int index, Json item) => ((IList<Json>)_value).Insert(index, item);
        public bool Remove(Json item) => ((IList<Json>)_value).Remove(item);
        public void RemoveAt(int index) => ((IList<Json>)_value).RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => ((IList<Json>)_value).GetEnumerator();
        #endregion
    }
}
