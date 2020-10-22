using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public abstract partial class DiffAlgorithm<T>
    {
        public IEnumerable<Diff<T>> Diff(IEnumerable<T> left, IEnumerable<T> right) => Diff(left, right, DefaultComparison);
        public IEnumerable<Diff<T>> Diff(IEnumerable<T> left, IEnumerable<T> right, DiffComparison<T> comparison)
        {
            var left_ = left.ToArray();
            var right_ = right.ToArray();
            var ses = Calc(left_, right_, comparison);
            return ses.GetDiffs(left_, right_);
        }
        public EditScripts Calc(IEnumerable<T> left, IEnumerable<T> right) => Calc(left, right, DefaultComparison);
        public EditScripts Calc(IEnumerable<T> left, IEnumerable<T> right, DiffComparison<T> comparison) => Calc(left.ToArray(), right.ToArray(), comparison);
        protected abstract EditScripts Calc(T[] left, T[] right, DiffComparison<T> comparison);
        public static DiffAlgorithm<T> DP => new DpAlgorithm();
        public static DiffAlgorithm<T> OND => new OndAlgorithm();
        public static DiffAlgorithm<T> ONP => new OnpAlgorithm();
        public static DiffAlgorithm<T> Default => ONP;
        public static DiffComparison<T> DefaultComparison => (l, r) => l?.Equals(r) ?? r == null;
    }
    public delegate bool DiffComparison<T>(T left, T right);
}
