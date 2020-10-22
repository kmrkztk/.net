using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Bit
{
    public struct Bit32 : IEnumerable<int>
    {
        static Bit32()
        {
            BitTable = new Bit32[Size];
            var b = 1u;
            for (var i = 0; i < Size; i++) 
            {
                BitTable[i] = new Bit32(b);
                b <<= 1;
            }
        }
        public static Bit32 Zero = new Bit32(0);
        public static Bit32 Max = new Bit32(-1);
        public static Bit32[] BitTable;
        public static Bit32 ByLength(int length) => ByLength(0, length);
        public static Bit32 ByLength(int offset, int length) => Max >> (Size - length) << offset;
        public const int Size = 32;
        public uint Data { get; private set; }
        public bool this[int index]
        {
            get => ((Data >> index) & 0x00000001) != 0;
            set => Data = value ? Data | (uint)(1 << index) : Data & ~(uint)(1 << index);
        }
        public Bit32(long data) { unchecked { Data = (uint)data; } }
        public Bit32(int data) { unchecked { Data = (uint)data; } }
        public Bit32(uint data) => Data = data;
        public Bit32 Reverse()
        {
            var v = Data;
            unchecked
            {
                if (Data == 0u) return Zero;
                v = (v & 0x55555555) << 1 | (v >> 1 & 0x55555555);
                v = (v & 0x33333333) << 2 | (v >> 2 & 0x33333333);
                v = (v & 0x0f0f0f0f) << 4 | (v >> 4 & 0x0f0f0f0f);
                v = (v & 0x00ff00ff) << 8 | (v >> 8 & 0x00ff00ff);
                v = (v & 0x0000ffff) << 16 | (v >> 16 & 0x0000ffff);
            }
            return new Bit32() { Data = v, };
        }
        public IEnumerable<int> SignificantPositions()
        {
            for (var i = 0; i < Size; i++) if (this & BitTable[i]) yield return i;
        }
        public int Count
        {
            get
            {
                if (Data == 0u) return 0;
                var b = Data;
                b = (b & 0x55555555) + (b >> 1 & 0x55555555);
                b = (b & 0x33333333) + (b >> 2 & 0x33333333);
                b = (b & 0x0f0f0f0f) + (b >> 4 & 0x0f0f0f0f);
                b = (b & 0x00ff00ff) + (b >> 8 & 0x00ff00ff);
                b = (b & 0x0000ffff) + (b >> 16 & 0x0000ffff);
                return (int)b;
            }
        }
        public static Bit32 FromArray(int[] array, int offset, int length)
        {
            unchecked
            {
                var b = Zero;
                for (var i = offset; i < offset + length; i++)
                {
                    b <<= 1;
                    if (array[i] != 0) b |= 1;
                }
                return b;
            }
        }
        public int[] ToArray(int offset, int length)
        {
            var buf = new int[length];
            var d = Data;
            if (d == 0u) return buf;
            var len = offset + length;
            d >>= offset;
            for (var i = offset; i < len; i++)
            {
                buf[i] = (int)~(d & 1) + 1;
                d >>= 1;
            }
            return buf;
        }
        public IEnumerator<int> GetEnumerator() => ToArray(0, Size).Cast<int>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string ToString() 
        {
            var s = new string('0', Size) + Convert.ToString(Data, 2);
            return s.Substring(s.Length - Size);
        }
        public override bool Equals(object obj)
        {
            if (obj is Bit32 b) return this == b;
            return false;
        }
        public override int GetHashCode() => base.GetHashCode();

        public static explicit operator uint(Bit32 value) => value.Data;
        public static explicit operator int(Bit32 value) { unchecked { return (int)value.Data; } }
        public static Bit32 operator &(Bit32 a, Bit32 b) => a & b.Data;
        public static Bit32 operator |(Bit32 a, Bit32 b) => a | b.Data;
        public static Bit32 operator ^(Bit32 a, Bit32 b) => a ^ b.Data;
        public static Bit32 operator ~(Bit32 a) => new Bit32(~a.Data);
        public static Bit32 operator &(Bit32 a, int b) { unchecked { return new Bit32(a.Data & (uint)b); } }
        public static Bit32 operator |(Bit32 a, int b) { unchecked { return new Bit32(a.Data | (uint)b); } }
        public static Bit32 operator ^(Bit32 a, int b) { unchecked { return new Bit32(a.Data ^ (uint)b); } }
        public static Bit32 operator &(int a, Bit32 b) { unchecked { return new Bit32((uint)a & b.Data); } }
        public static Bit32 operator |(int a, Bit32 b) { unchecked { return new Bit32((uint)a | b.Data); } }
        public static Bit32 operator ^(int a, Bit32 b) { unchecked { return new Bit32((uint)a ^ b.Data); } }
        public static Bit32 operator &(Bit32 a, uint b) => new Bit32(a.Data & b);
        public static Bit32 operator |(Bit32 a, uint b) => new Bit32(a.Data | b);
        public static Bit32 operator ^(Bit32 a, uint b) => new Bit32(a.Data ^ b);
        public static Bit32 operator &(uint a, Bit32 b) => new Bit32(a & b.Data);
        public static Bit32 operator |(uint a, Bit32 b) => new Bit32(a | b.Data);
        public static Bit32 operator ^(uint a, Bit32 b) => new Bit32(a ^ b.Data);
        public static Bit32 operator <<(Bit32 a, int count) => new Bit32(a.Data << count);
        public static Bit32 operator >>(Bit32 a, int count) => new Bit32(a.Data >> count);
        public static bool operator !(Bit32 a) => a.Data == 0;
        public static bool operator true(Bit32 a) => a.Data != 0;
        public static bool operator false(Bit32 a) => a.Data == 0;
        public static Bit32 operator +(Bit32 a, Bit32 b) => a + b.Data;
        public static Bit32 operator -(Bit32 a, Bit32 b) => a - b.Data;
        public static Bit32 operator *(Bit32 a, Bit32 b) => a * b.Data;
        public static Bit32 operator /(Bit32 a, Bit32 b) => a / b.Data;
        public static Bit32 operator +(Bit32 a, uint b) => new Bit32(a.Data + b);
        public static Bit32 operator -(Bit32 a, uint b) => new Bit32(a.Data - b);
        public static Bit32 operator *(Bit32 a, uint b) => new Bit32(a.Data * b);
        public static Bit32 operator /(Bit32 a, uint b) => new Bit32(a.Data / b);
        public static Bit32 operator +(Bit32 a, int b) => new Bit32(a.Data + b);
        public static Bit32 operator -(Bit32 a, int b) => new Bit32(a.Data - b);
        public static Bit32 operator *(Bit32 a, int b) => new Bit32(a.Data * b);
        public static Bit32 operator /(Bit32 a, int b) => new Bit32(a.Data / b);
        public static bool operator ==(Bit32 a, Bit32 b) => a.Data == b.Data;
        public static bool operator !=(Bit32 a, Bit32 b) => a.Data != b.Data;
    }
}
