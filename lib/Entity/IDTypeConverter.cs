using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Lib.Entity
{
    public class IDTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(ID) ||
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
            sourceType == typeof(ID) ||
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
                () =>
                {
                    if (value == null) return true;
                    var type = value.GetType();
                    type = type.GetGenericTypeDefinition() == typeof(Nullable<>) ? type.GetGenericArguments()[0] : type;
                    return 
                        (value is string s && decimal.TryParse(s, out var d)) ||
                        type == typeof(decimal) ||
                        type == typeof(long)    ||
                        type == typeof(int)     ||
                        type == typeof(short)   ||
                        type == typeof(byte)    ||
                        type == typeof(ulong)   ||
                        type == typeof(uint)    ||
                        type == typeof(ushort)
                    ;
                },
                ex => base.IsValid(context, value)
            )
            .Invoke();
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) => new ID(decimal.Parse(value?.ToString()));
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
            value is ID id ? id == ID.Null ? null : Convert.ChangeType(id, destinationType) : 
            base.ConvertTo(context, culture, value, destinationType);
        
    }
}
