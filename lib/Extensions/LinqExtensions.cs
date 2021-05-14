using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
public static    class LinqExtensions
    {
        public static void Foreach<T>(this IEnumerable<T> enums, Action<T> action) => Foreach(enums, (_, i) => action(_));
        public static void Foreach<T>(this IEnumerable<T> enums, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in enums) action(e, i++);
        }
        public static IEnumerable<T> AsEnumerable<T>(this Array array) => array.AsEnumerable().Cast<T>();
        public static IEnumerable AsEnumerable(this Array array)
        {
            foreach (var _ in array) yield return _;
        }
    }
}
