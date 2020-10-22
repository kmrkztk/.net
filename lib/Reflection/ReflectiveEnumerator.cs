using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Reflection
{
    public static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<Type> GetEnumerableOfType<T>() => GetEnumerableOfType(typeof(T));
        public static IEnumerable<Type> GetEnumerableOfType<T>(Assembly assembly) => GetEnumerableOfType(typeof(T), assembly);
        public static IEnumerable<Type> GetEnumerableOfType<T>(Type[] types) => GetEnumerableOfType(typeof(T), types);
        public static IEnumerable<Type> GetEnumerableOfType(this Type type) => GetEnumerableOfType(type, type.Assembly);
        public static IEnumerable<Type> GetEnumerableOfType(this Type type, Assembly assembly) => GetEnumerableOfType(type, assembly?.GetTypes() ?? new Type[] { });
        public static IEnumerable<Type> GetEnumerableOfType(this Type type, Type[] types) => type.IsInterface ?
                types.Where(_ => _.GetInterfaces().Any(t => t == type)) :
                types.Where(_ => _.IsSubclassOf(type) && !_.IsAbstract);
        public static IEnumerable<T> GetEnumerableInstanceOfType<T>(params object[] constructorArgs) => GetEnumerableOfType<T>().Select(_ => (T)Activator.CreateInstance(_, constructorArgs));
    }
}
