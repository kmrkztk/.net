using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Entity
{
    [TypeConverter(typeof(IDTypeConverter))]
    public struct ID : IComparable<ID>, IEquatable<ID>, IConvertible
    {
        public static ID Null => new("") { Value = null };
        public static ID Max => new(decimal.MaxValue);
        public decimal? Value { get; private set; }
        public decimal ValueOrZero => Value ?? decimal.Zero;
        public ID(decimal? value)
        {
            Value = value;
            if (ValueOrZero != decimal.Truncate(ValueOrZero)) throw new ArgumentException(nameof(value));
        }
        public ID(string value) : this(string.IsNullOrEmpty(value) ? null : decimal.Parse(value)) { }
        public override string ToString() => Value?.ToString() ?? string.Empty;
        public int CompareTo(ID other)
        {
            var value_ = other.Value;
            if (Value == null && value_ == null) return 0;
            if (Value == null) return -1;
            if (value_ == null) return  1;
            return Value < value_ ? -1 : Value > value_ ? 1 : 0;
        }
        public bool Equals(ID other) => CompareTo(other) == 0;
        public override bool Equals(object obj) => obj is ID id && Equals(id);
        public override int GetHashCode() => base.GetHashCode();

        public TypeCode GetTypeCode() => TypeCode.Object;
        public bool ToBoolean(IFormatProvider provider) => ValueOrZero > 0;
        public byte ToByte(IFormatProvider provider) => decimal.ToByte(ValueOrZero);
        public char ToChar(IFormatProvider provider) => throw new NotImplementedException();
        public DateTime ToDateTime(IFormatProvider provider) => throw new NotImplementedException();
        public decimal ToDecimal(IFormatProvider provider) => ValueOrZero;
        public double ToDouble(IFormatProvider provider) => decimal.ToDouble(ValueOrZero);
        public short ToInt16(IFormatProvider provider) => decimal.ToInt16(ValueOrZero);
        public int ToInt32(IFormatProvider provider) => decimal.ToInt32(ValueOrZero);
        public long ToInt64(IFormatProvider provider) => decimal.ToInt64(ValueOrZero);
        public sbyte ToSByte(IFormatProvider provider) => decimal.ToSByte(ValueOrZero);
        public float ToSingle(IFormatProvider provider) => decimal.ToSingle(ValueOrZero);
        public string ToString(IFormatProvider provider) => Value?.ToString(provider);
        public object ToType(Type conversionType, IFormatProvider provider) => TypeDescriptor.GetConverter(typeof(ID)).ConvertTo(this, conversionType);
        public ushort ToUInt16(IFormatProvider provider) => decimal.ToUInt16(ValueOrZero);
        public uint ToUInt32(IFormatProvider provider) => decimal.ToUInt32(ValueOrZero);
        public ulong ToUInt64(IFormatProvider provider) => decimal.ToUInt64(ValueOrZero);

        public static bool operator ==(ID x, ID y) => x.Equals(y);
        public static bool operator !=(ID x, ID y) => !x.Equals(y);
        public static bool operator <(ID x, ID y) => x.CompareTo(y) < 0;
        public static bool operator >(ID x, ID y) => x.CompareTo(y) > 0;
        public static bool operator <=(ID x, ID y) => x.CompareTo(y) <= 0;
        public static bool operator >=(ID x, ID y) => x.CompareTo(y) >= 0;
        public static ID operator +(ID src, int value) => src.Value == null ? Null : new(src.Value + value);
        public static ID operator ++(ID src) => new(src.Value + 1);
        public static ID operator -(ID src, int value) => new(src.Value - value);
        public static ID operator --(ID src) => new(src.Value - 1);
        public static implicit operator ID(string value) => new(value);
        public static implicit operator ID(decimal? value) => new(value);
        public static implicit operator ID(decimal value) => new(value);
        public static implicit operator ID(long value) => new(value);
        public static implicit operator ID(int value) => new(value);
        public static implicit operator ID(short value) => new(value);
        public static implicit operator ID(byte value) => new(value);
        public static implicit operator ID(ulong value) => new(value);
        public static implicit operator ID(uint value) => new(value);
        public static implicit operator ID(ushort value) => new(value);
        public static implicit operator ID(sbyte value) => new(value);
        public static implicit operator string(ID value) => value.ToString();
        public static implicit operator decimal?(ID value) => value.Value;
        public static implicit operator decimal(ID value) => value.ValueOrZero;
        public static implicit operator long(ID value) => decimal.ToInt64(value.ValueOrZero);
        public static implicit operator int(ID value) => decimal.ToInt32(value.ValueOrZero);
        public static implicit operator short(ID value) => decimal.ToInt16(value.ValueOrZero);
        public static implicit operator byte(ID value) => decimal.ToByte(value.ValueOrZero);
        public static implicit operator ulong(ID value) => decimal.ToUInt64(value.ValueOrZero);
        public static implicit operator uint(ID value) => decimal.ToUInt32(value.ValueOrZero);
        public static implicit operator ushort(ID value) => decimal.ToUInt16(value.ValueOrZero);
        public static implicit operator sbyte(ID value) => decimal.ToSByte(value.ValueOrZero);
    }
}
