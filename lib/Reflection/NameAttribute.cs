using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib
{
    [AttributeUsage(AttributeTargets.All)]
    public class NameAttribute : Attribute
    {
        public string Value { get; init; }
        public NameAttribute() : this(null) { }
        public NameAttribute(string value) => Value = value;
        public virtual string GetValue(MemberInfo info) => Value ?? info?.Name;
        public static string GetName(MemberInfo info) => info?.GetCustomAttribute<NameAttribute>()?.GetValue(info);
    }
}
