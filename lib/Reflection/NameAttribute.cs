using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class NameAttribute : Attribute
    {
        public string Name { get; init; }
        public NameAttribute() : this(null) { }
        public NameAttribute(string value) => Name = value;
        public virtual string GetName(Type type) => Name ?? type.Name;
        public virtual string GetName(MemberInfo info) => Name ?? info.Name;
        public static string GetMemberName(MemberInfo info) => info.GetCustomAttribute<NameAttribute>()?.GetName(info);
        public static IEnumerable<string> GetMemberNames(MemberInfo info) => info.GetCustomAttributes<NameAttribute>().Select(_ => _.GetName(info));
        public static string GetTypeName(Type type) => type.GetCustomAttribute<NameAttribute>()?.GetName(type);
        public static IEnumerable<string> GetTypeNames(Type type) => type.GetCustomAttributes<NameAttribute>().Select(_ => _.GetName(type));
    }
}
