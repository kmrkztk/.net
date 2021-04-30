using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
public static    class LinqExtensions
    {
        public static void Foreach<T>(this IEnumerable<T> enums, Action<T> action)
        {
            foreach (var e in enums) action(e);
        }
        public static void Foreach<T>(this IEnumerable<T> enums, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in enums) action(e, i++);
        }
    }
}
