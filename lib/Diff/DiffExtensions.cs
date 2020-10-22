using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Diff;
using Lib.Diff.Algorithm;

namespace Lib
{
    public static class DiffExtensions
    {
        public static IEnumerable<Diff<T>> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right) => Diff(left, right, DiffAlgorithm<T>.DefaultComparison);
        public static IEnumerable<Diff<T>> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right, DiffComparison<T> comparison) => Diff(left, right, comparison, DiffAlgorithm<T>.Default);
        public static IEnumerable<Diff<T>> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right, DiffAlgorithm<T> algorithm) => Diff(left, right, DiffAlgorithm<T>.DefaultComparison, algorithm);
        public static IEnumerable<Diff<T>> Diff<T>(this IEnumerable<T> left, IEnumerable<T> right, DiffComparison<T> comparison, DiffAlgorithm<T> algorithm) => algorithm.Diff(left, right, comparison);
    }
}
