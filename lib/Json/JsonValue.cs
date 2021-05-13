using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Jsons
{
    public class JsonValue : Json
    {
        string _value;
        public JsonValueType ValueType { get; private set; } = JsonValueType.None;
        public JsonValue() : base() { }
        public JsonValue(Stream stream) : base(stream) { }
        public JsonValue(TextReader reader) : base(reader) { }
        public JsonValue(JsonReader reader) : base(reader) { }
        public override string Value { get => _value; set => SetValue(value); }
        public void SetValue(string value)
        {
            _value = value;
            ValueType = value == null ? JsonValueType.Null : JsonValueType.String;
        }
        public void SetValue(bool value)
        {
            _value = value ? "true" : "false";
            ValueType = JsonValueType.Boolean;
        }
        public void SetValue(int value) => SetValue((decimal)value);
        public void SetValue(decimal value)
        {
            _value = value.ToString();
            ValueType = JsonValueType.Numeric;
        }
        public override void Import(JsonReader reader)
        {
            var s = reader.ReadBlock() ?? throw reader.FormatException();
            if (s.Length == 0) throw reader.FormatException();
            ValueType = GetJsonValueType(s);
            if (ValueType == JsonValueType.None) throw reader.FormatException();
            if (ValueType == JsonValueType.String) s = TrimBlock(s);
            _value = Unescape(s);
        }
        public override string Format(JsonFormatSettings setting) =>
            string.Format("{0}{1}{0}", 
                ValueType == JsonValueType.String ? BlockChar.ToString() : "", setting.Escape ? Escape(_value) : _value
            );
        public override dynamic Cast(Type type) => Type.GetTypeCode(type) == TypeCode.Object ? type.Cast(_value) : Convert.ChangeType(_value, type);
        public override IEnumerable<Json> Find(params string[] keys) 
        {
            if (keys.Length == 0) yield return this;
            yield break; 
        }
        public override string ToString() => _value;
    }
}