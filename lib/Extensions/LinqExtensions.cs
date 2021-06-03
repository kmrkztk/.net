using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class LinqExtensions
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
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enums, Comparison<T> comparison) => enums.Distinct(new ComparisonComparer<T>(comparison));
        class ComparisonComparer<T> : IEqualityComparer<T>
        {
            readonly Comparison<T> _comparison;
            public ComparisonComparer(Comparison<T> comparison) => _comparison = comparison;
            public bool Equals(T x, T y) => _comparison(x, y) == 0;
            public int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
        }
    }
}
