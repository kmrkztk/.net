using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib.Reflection
{
    public static class ReflectionExtentions
    {
        public static bool HasAttribute<T>(this MemberInfo element) where T : Attribute => element.GetCustomAttributes<T>().Any();
        public static bool HasAttribute<T>(this Type element) where T : Attribute => element.GetCustomAttributes<T>().Any();

    }
}
