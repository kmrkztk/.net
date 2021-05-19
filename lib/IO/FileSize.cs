using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lib.IO
{
    [TypeConverter(typeof(FileSizeConverter))]
    public struct FileSize : IComparable<FileSize>
    {
        readonly decimal _value;
        public decimal Value => _value;
        public FileSize(decimal size) => _value = size;
        public FileSize(FileInfo file) : this(file.Length) { }
        public FileSize(DirectoryInfo directory) : this(GetSize(directory)) { }
        static long GetSize(DirectoryInfo directory) 
            => directory.GetFiles().Sum(_ => _.Length) + directory.GetDirectories().Sum(_ => GetSize(_));
        public override string ToString()
        {
            if (_value == 0) return "0byte";
            var size = _value;
            var index = 0;
            var format = new[] { 
                "{0:0}bytes", 
                "{0:0.##}kb", 
                "{0:0.##}MB", 
                "{0:0.##}GB", 
                "{0:0.##}TB", 
            };
            while(size > 512 && index < format.Length - 1)
            {
                size /= 1024;
                index++;
            }
            return string.Format(format[index], size);
        }
        public int CompareTo(FileSize other)
        {
            var d = _value - other._value;
            return d == 0 ? 0 : d < 0 ? -1 : 1;
        }
        public static Match Match(string value) => Regex.Match(value.ToLower(), @"^(\d+(\.\d+)?)([tgmk]b?|bytes?)?$");
        public static FileSize Parse(string value)
        {
            if (TryParse(value, out var result)) return result;
            throw new FormatException();
        }
        public static bool TryParse(string value, out FileSize result)
        {
            result = default;
            var match = Match(value);
            if (!match.Success) return false;
            var i = decimal.Parse(match.Groups[1].Captures[0].Value);
            var u = match.Groups.Count == 4 ? match.Groups[3].Captures.Count == 1 ? match.Groups[3].Captures[0].Value : "" : "";
            switch (u)
            {
                case "": case "byte": case "bytes": break;
                case "k": case "kb": i *= 1024; break;
                case "m": case "mb": i *= 1024 * 1024; break;
                case "g": case "gb": i *= 1024 * 1024 * 1024; break;
                case "t": case "tb": i *= 1024m * 1024m * 1024m * 1024; break;
            }
            result = new(i);
            return true;
        }
    }
    public class FileSizeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) ||
            sourceType == typeof(decimal) ||
            sourceType == typeof(long) ||
            sourceType == typeof(int) ||
            sourceType == typeof(short) ||
            sourceType == typeof(byte) ||
            sourceType == typeof(ulong) ||
            sourceType == typeof(uint) ||
            sourceType == typeof(ushort);
        public override bool CanConvertTo(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) ||
            sourceType == typeof(decimal) ||
            sourceType == typeof(long) ||
            sourceType == typeof(int) ||
            sourceType == typeof(short) ||
            sourceType == typeof(byte) ||
            sourceType == typeof(ulong) ||
            sourceType == typeof(uint) ||
            sourceType == typeof(ushort);
        public override bool IsValid(ITypeDescriptorContext context, object value) =>
            Try.Of(
                () => value != null &&
                    (value is string s && FileSize.Match(s).Success) ||
                    value is decimal ||
                    value is long || value is int || value is short || value is byte ||
                    value is ulong || value is uint || value is ushort || value is sbyte
                ,
                ex => base.IsValid(context, value)
            )
            .Invoke();
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => value is string v ? FileSize.Parse(v) :
               value is decimal m ? new FileSize(m) :
               value is long l ? new FileSize(l) :
               value is int i ? new FileSize(i) :
               value is short s ? new FileSize(s) :
               value is byte b ? new FileSize(b) :
               value is ulong ul ? new FileSize(ul) :
               value is uint ui ? new FileSize(ui) :
               value is ushort us ? new FileSize(us) :
               value is double d ? new FileSize((decimal)d) :
               value is float f ? new FileSize((decimal)f) :
               null;
    }
}
