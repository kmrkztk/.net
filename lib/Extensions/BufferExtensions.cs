using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class BufferExtensions
    {
        public static IEnumerable<byte[]> Split(this byte[] value, int count)
        {
            if (count <= 0)
            {
                yield return value;
                yield break;
            }
            var length = value.Length;
            var offset = 0;
            while (length > 0)
            {
                var len = length > count ? count : length;
                var buf = new byte[len];
                Buffer.BlockCopy(value, offset, buf, 0, len);
                yield return buf;
                length -= count;
                offset += count;
            }
        }
        public static string ToHex(this byte[] value, int length) => BitConverter.ToString(value, 0, length).Replace("-", string.Empty);
        public static string ToHex(this byte[] value) => ToHex(value, value.Length);
        public static byte[] ToBinary(this string value) => value.Replace("\r", "").Replace("\n", "").Split(2).Select(_ => Convert.ToByte(_, 16)).ToArray();
        public static byte[] Part(this byte[] value, int index) => value.Part(index, value.Length - index);
        public static byte[] Part(this byte[] value, int index, int length)
        {
            var buf = new byte[length];
            Buffer.BlockCopy(value, index, buf, 0, length);
            return buf;
        }
    }
}
