using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FormatAttribute : Attribute
    {
        public interface IFormatter
        {
            string ToString(object obj);
        }
        class InternalFormatter : IFormatter
        {
            public string Format { get; init; }
            public IFormatProvider Provider { get; init; }
            public string ToString(object obj) => obj is IFormattable f ? f.ToString(Format, Provider) : obj?.ToString();
        }
        class DefaultFormatter : IFormatter
        {
            public string ToString(object obj) => obj?.ToString();
        }
        public IFormatter Formatter { get; init; }
        public FormatAttribute(string format) : this(format, null) { }
        public FormatAttribute(string format, IFormatProvider provider) : this(new InternalFormatter() { Format = format, Provider = provider }) { }
        public FormatAttribute(IFormatter formatter) => Formatter = formatter;
        public FormatAttribute(Type type) : this(
            type.GetInterfaces().Any(_ => _ == typeof(IFormatter)) ?
            (IFormatter)type.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<object>()) : null) { }
        public static IFormatter GetFormatter(MemberInfo info) => info.GetCustomAttribute<FormatAttribute>()?.Formatter ?? new DefaultFormatter();
    }
}
