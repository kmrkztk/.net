﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lib.Json
{
    public enum JsonValueType
    {
        None, String, Numeric, Boolean, Null
    }
    public abstract partial class Json
    {
        public const char ObjectChar = '{';
        public const char ObjectCharEnd = '}';
        public const char ArrayChar = '[';
        public const char ArrayCharEnd = ']';
        public const char BlockChar = '"';
        public const char ValueSeparator = ':';
        public const char Separator = ',';
        public const char EscapeChar = '\\';
        public const char Tab = '\t';
        public const char CR = '\r';
        public const char LF = '\n';
        public const string CRLF = "\r\n";
        readonly static Dictionary<string, string> _esc = new Dictionary<string, string>()
        {
            { "\"", "\\\"" },
            { "\\", @"\\" },
            { "/" , @"\/" },
            { "\b", @"\b" },
            { "\f", @"\f" },
            { "\n", @"\n" },
            { "\r", @"\r" },
            { "\t", @"\\t" },
        };
        public static string Escape(string value)
        {
            var v = value;
            foreach (var k in _esc.Keys) v = v.Replace(k, _esc[k]);
            return v.UnicodeEscape(); ;
        }
        public static string Unescape(string value)
        {
            var v = value;
            foreach (var k in _esc.Keys) v = v.Replace(_esc[k], k);
            return v.UnicodeUnescape();
        }
        public Json Parent { get; protected set; }
        public void SetParent(Json json) => Parent = json;
        public static T Load<T>(string value) => Load<T>(new StringReader(value));
        public static T Load<T>(Stream stream) => Load<T>(new StreamReader(stream));
        public static T Load<T>(TextReader reader) => Load<T>(new JsonReader(reader));
        public static T Load<T>(JsonReader reader) => Load(reader).Cast<T>();
        public static JsonObject Load(string value) => Load(new StringReader(value));
        public static JsonObject Load(Stream stream) => Load(new StreamReader(stream));
        public static JsonObject Load(TextReader reader) => (JsonObject)Load(new JsonReader(reader));
        public static Json Load(JsonReader reader)
        {
            switch ((char)reader.Peek())
            {
                case ObjectChar:
                    return new JsonObject(reader);
                case ArrayChar:
                    return new JsonArray(reader);
                default:
                    return new JsonValue(reader);
            }
        }
        public static JsonValueType GetJsonValueType(string value)
        {
            switch (value)
            {
                case "null": return JsonValueType.Null;
                case "true": case "false": return JsonValueType.Boolean;
                default:
                    if (value[0] == BlockChar) return JsonValueType.String;
                    else if (decimal.TryParse(value, out _)) return JsonValueType.Numeric;
                    else return JsonValueType.None;
            }
        }
        public string StartChar => this is JsonObject ? ObjectChar.ToString() : this is JsonArray ? ArrayChar.ToString() : "";
        public string EndChar => this is JsonObject ? ObjectCharEnd.ToString() : this is JsonArray ? ArrayCharEnd.ToString() : "";
        public Json() { }
        public Json(Stream stream) : this(new StreamReader(stream)) { }
        public Json(TextReader reader) : this(new JsonReader(reader)) { }
        public Json(JsonReader reader) => Import(reader);
        public void Import(Stream stream) => Import(new StreamReader(stream));
        public void Import(TextReader reader) => Import(new JsonReader(reader));
        public abstract void Import(JsonReader reader);
        public string Format() => Format(JsonFormatSettings.Default);
        public abstract string Format(JsonFormatSettings setting);
        public T Cast<T>() => (T)Cast(typeof(T));
        public abstract object Cast(Type type);
        protected string TrimBlock(string value) => value.Trim(BlockChar);
        public override string ToString() => Format();
    }
}
