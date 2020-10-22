using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Text
{
    public class Base64Encoder
    {
        public static string Encode(string value) => Encode(value, Encoding.UTF8);
        public static string Encode(string value, Encoding encode) => Encode(encode.GetBytes(value), 0, encode.GetByteCount(value));
        public static string Encode(TextReader reader) => Encode(reader.ReadToEnd().Replace("\r", "").Replace("\n", ""));
        public static string Encode(byte[] buffer) => Encode(buffer, 0, buffer.Length);
        public static string Encode(byte[] buffer, int offset, int length) => Convert.ToBase64String(buffer, offset, length);
        public static string Encode(Stream stream)
        {
            var sb = new StringBuilder();
            foreach (var s in _Encode(stream)) sb.Append(s);
            return sb.ToString();
        }
        public static void Encode(Stream stream1, Stream stream2) => Encode(stream1, new StreamWriter(stream2, Encoding.UTF8));
        public static void Encode(Stream stream, TextWriter writer)
        {
            foreach (var b in _Encode(stream)) writer.Write(b);
        }
        static IEnumerable<string> _Encode(Stream stream)
        {
            var buf = new byte[12288];
            int len;
            while ((len = stream.Read(buf, 0, buf.Length)) > 0) yield return Encode(buf, 0, len);
        }

        public static string Decode(string value, Encoding encode) => encode.GetString(Decode(value));
        public static byte[] Decode(string value) => Convert.FromBase64String(value);
        public static void Decode(string value, Stream output) => Decode(new StringReader(value), output);
        public static void Decode(Stream stream, Stream output) => Decode(new StreamReader(stream), output);
        public static void Decode(TextReader reader, Stream output)
        {
            var buf = Decode(System.Text.RegularExpressions.Regex.Unescape(reader.ReadToEnd().Replace("\r", "").Replace("\n", "")));
            output.Write(buf, 0, buf.Length);
        }
    }
}
