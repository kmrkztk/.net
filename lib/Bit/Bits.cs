using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Bit
{
    public class Bits : ICloneable, IEnumerable<int>
    {
        public const int DataSize = Bit32.Size;
        public Bit32[] Data { get; private set; }
        public int Length { get; }
        public int Count => Data.Sum(_ => _.Count);
        public bool this[int index]
        {
            get
            {
                if (index >= Length) throw new IndexOutOfRangeException();
                return Data[index / DataSize][index % DataSize];
            }
            set
            {
                if (index >= Length) throw new IndexOutOfRangeException();
                Data[index / DataSize][index % DataSize] = value;
            }
        }
        public Bits(int length) : this(new Bit32[(length - 1) / DataSize + 1], length) { }
        public Bits(IEnumerable<int> bits) : this(bits.ToArray()) { }
        public Bits(IEnumerable<Bit32> bits) : this(bits.ToArray()) { }
        public Bits(IEnumerable<int> bits, int length) : this(bits.ToArray(), length) { }
        public Bits(IEnumerable<Bit32> bits, int length) : this(bits.ToArray(), length) { }
        public Bits(int[] bits) : this(bits, bits.Length * DataSize) { }
        public Bits(Bit32[] bits) : this(bits, bits.Length * DataSize) { }
        public Bits(int[] bits, int length) : this(bits.Select(_ => new Bit32(_)).ToArray(), length) { }
        public Bits(Bit32[] bits, int length)
        {
            Data = bits;
            Length = length;
        }
        public void Clear() => Data = new Bit32[Data.Length];
        public object Clone() => new Bits(Length) { Data = (Bit32[])Data.Clone(), };
        public override string ToString() => string.Join(",", Data
            .Select((_, i) => {
                var b = _.Reverse().ToString();
                return i * DataSize < Length ? b : b.Substring(0, Length - i * DataSize);
                }));
        public IEnumerator<int> GetEnumerator() => Data.Select(_ => (int)_).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public static Bits operator &(Bits a, Bits b) => a.Length < b.Length ? b & a : new Bits(a.Data.Select((_, i) =>  _ & (i < b.Data.Length ? b.Data[i] : Bit32.Zero)), a.Length);
        public static Bits operator |(Bits a, Bits b) => a.Length < b.Length ? b | a : new Bits(a.Data.Select((_, i) =>  _ | (i < b.Data.Length ? b.Data[i] : Bit32.Zero)), a.Length);
        public static Bits operator ^(Bits a, Bits b) => a.Length < b.Length ? b ^ a : new Bits(a.Data.Select((_, i) =>  _ ^ (i < b.Data.Length ? b.Data[i] : Bit32.Zero)), a.Length);
        public static Bits operator ~(Bits a) => new Bits(a.Data.Select(_ => ~_), a.Length);
    }
}
