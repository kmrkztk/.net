using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Logs;

namespace Lib
{
    public static class LinqExtensions
    {
        public static void Do<T>(this IEnumerable<T> enums, Action<T> action) => enums.Each(action).Do();
        public static void Do<T>(this IEnumerable<T> enums, Action<T, int> action) => enums.Each(action).Do();
        public static void Do<T>(this IEnumerable<T> enums) { foreach (var _ in enums) { } }
        public static TResult Do<T, TResult>(this IEnumerable<T> enums, Func<T, TResult, TResult> selector) => enums.Do(default, selector);
        public static TResult Do<T, TResult>(this IEnumerable<T> enums, Func<T, TResult, int, TResult> selector) => enums.Do(default, selector);
        public static TResult Do<T, TResult>(this IEnumerable<T> enums, TResult initial, Func<T, TResult, TResult> selector) => enums.Do(initial, (_, res, i) => selector(_, res));
        public static TResult Do<T, TResult>(this IEnumerable<T> enums, TResult initial, Func<T, TResult, int, TResult> selector)
        {
            var result = initial;
            enums.Each((_, i) => result = selector(_, result, i));
            return result;
        }
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enums, Action<T> action) => enums.Each((_, i) => action(_));
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enums, Action<T, int> action) => enums.Select((_, i) =>
        {
            action(_, i);
            return _;
        });
        public static IEnumerable<T> AsEnumerable<T>(this Array array) => array.AsEnumerable().Cast<T>();
        public static IEnumerable AsEnumerable(this Array array)
        {
            foreach (var _ in array) yield return _;
        }

        public static IEnumerable<T> Tail<T>(this IEnumerable<T> enums, int count)
        {
            var _ = enums.ToList();
            return _.Skip(_.Count - count);
        }
        public static IEnumerable<T> TailIf<T>(this IEnumerable<T> enums, int count) => count < 0 ? enums : enums.Tail(count);
        public static IEnumerable<T> TakeIf<T>(this IEnumerable<T> enums, int count) => count < 0 ? enums : enums.Take(count);
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enums, Comparison<T> comparison) => enums.Distinct(new ComparisonComparer<T>(comparison));
        public static IEnumerable<T> SoftOrder<T>(this IEnumerable<IEnumerable<T>> enums, Comparison<T> comparison) => enums.Skip(1).Do(enums.FirstOrDefault(), (_0, _1) => _1.SoftOrder(_0, comparison));
        public static IEnumerable<T> SoftOrder<T>(this IEnumerable<T> enums1, IEnumerable<T> enums2, Comparison<T> comparison)
        {
            using var e1 = enums1.GetEnumerator();
            using var e2 = enums2.GetEnumerator();
            var b1 = e1.MoveNext();
            var b2 = e2.MoveNext();
            while (b1 && b2)
            {
                var c = comparison(e1.Current, e2.Current);
                if (c <= 0)
                {
                    yield return e1.Current;
                    b1 = e1.MoveNext();
                }
                if (c >= 0)
                {
                    yield return e2.Current;
                    b2 = e2.MoveNext();
                }
            }
            while (e1.MoveNext()) yield return e1.Current;
            while (e2.MoveNext()) yield return e2.Current;
        }
        class ComparisonComparer<T> : IEqualityComparer<T>
        {
            readonly Comparison<T> _comparison;
            public ComparisonComparer(Comparison<T> comparison) => _comparison = comparison;
            public bool Equals(T x, T y) => _comparison(x, y) == 0;
            public int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
        }
        #region branch
        public static IEnumerable<(T1, T2)> Branch<T1, T2>(this IEnumerable<T1> enums1, Func<T1, IEnumerable<T2>> function) => enums1.Branch(function, (_1, _2) => (_1, _2));
        public static IEnumerable<(T1, T2)> Invert<T1, T2>(this IEnumerable<T1> enums1, IEnumerable<T2> enums2) => enums1.Invert(enums2, (_1, _2) => (_1, _2));
        public static IEnumerable<(T1, T2)> Matrix<T1, T2>(this IEnumerable<T1> enums1, IEnumerable<T2> enums2) => enums1.Matrix(enums2, (_1, _2) => (_1, _2));
        public static IEnumerable<T3> Branch<T1, T2, T3>(this IEnumerable<T1> enums1, Func<T1, IEnumerable<T2>> function, Func<T1, T2, T3> selector) => enums1.SelectMany(_1 => function(_1).Select(_2 => selector(_1, _2)));
        public static IEnumerable<T3> Invert<T1, T2, T3>(this IEnumerable<T1> enums1, IEnumerable<T2> enums2, Func<T1, T2, T3> selector) => enums2.Matrix(enums1, (_1, _2) => selector(_2, _1));
        public static IEnumerable<T3> Matrix<T1, T2, T3>(this IEnumerable<T1> enums1, IEnumerable<T2> enums2, Func<T1, T2, T3> selector)
        {
            var enums2_ = enums2.ToList();
            return enums1.Branch(_ => enums2_, selector);
        }
        #endregion
        #region log
        public static IEnumerable<T> Log<T>(this IEnumerable<T> enums, Level level) => enums.Log(level, _ =>_?.ToString());
        public static IEnumerable<T> Log<T>(this IEnumerable<T> enums, Level level, string message) => enums.Log(level, _ => string.Format(message, _));
        public static IEnumerable<T> Log<T>(this IEnumerable<T> enums, Level level, Func<T, string> action) => enums.Each(_ => Logs.Log.Of(level).Out(action(_)));
        public static IEnumerable<T> Trace<T>(this IEnumerable<T> enums) => enums.Log(Level.Trace);
        public static IEnumerable<T> Trace<T>(this IEnumerable<T> enums, string message) => enums.Log(Level.Trace, message);
        public static IEnumerable<T> Trace<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Log(Level.Trace, action);
        public static IEnumerable<T> Debug<T>(this IEnumerable<T> enums) => enums.Log(Level.Debug);
        public static IEnumerable<T> Debug<T>(this IEnumerable<T> enums, string message) => enums.Log(Level.Debug, message);
        public static IEnumerable<T> Debug<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Log(Level.Debug, action);
        public static IEnumerable<T> Info<T>(this IEnumerable<T> enums) => enums.Log(Level.Info);
        public static IEnumerable<T> Info<T>(this IEnumerable<T> enums, string message) => enums.Log(Level.Info, message);
        public static IEnumerable<T> Info<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Log(Level.Info, action);
        public static IEnumerable<T> Warn<T>(this IEnumerable<T> enums) => enums.Log(Level.Warn);
        public static IEnumerable<T> Warn<T>(this IEnumerable<T> enums, string message) => enums.Log(Level.Warn, message);
        public static IEnumerable<T> Warn<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Log(Level.Warn, action);
        public static IEnumerable<T> Error<T>(this IEnumerable<T> enums) => enums.Log(Level.Error);
        public static IEnumerable<T> Error<T>(this IEnumerable<T> enums, string message) => enums.Log(Level.Error, message);
        public static IEnumerable<T> Error<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Log(Level.Error, action);
        public static IEnumerable<T> Fatal<T>(this IEnumerable<T> enums) => enums.Log(Level.Fatal);
        public static IEnumerable<T> Fatal<T>(this IEnumerable<T> enums, string message) => enums.Log(Level.Fatal, message);
        public static IEnumerable<T> Fatal<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Log(Level.Fatal, action);
        public static IEnumerable<T> Console<T>(this IEnumerable<T> enums) => enums.Console(_ => _?.ToString());
        public static IEnumerable<T> Console<T>(this IEnumerable<T> enums, string message) => enums.Console(_ => string.Format(message, _));
        public static IEnumerable<T> Console<T>(this IEnumerable<T> enums, Func<T, string> action) => enums.Each(_ => System.Console.WriteLine(action(_)));
        #endregion
        #region exception
        public static IEnumerable<T> Catch<T>(this IEnumerable<T> enums) => enums.Catch(null);
        public static IEnumerable<T> Catch<T>(this IEnumerable<T> enums, Catched<Exception> catched) => enums.Catch<T, Exception>(catched);
        public static IEnumerable<T> Catch<T, TException>(this IEnumerable<T> enums, Catched<TException> catched) where TException : Exception
        {
            using var e = enums.GetEnumerator();
            while (true)
            {
                try
                {
                    if (!e.MoveNext()) break;
                }
                catch (Exception ex)
                {
                    if (ex is TException ex_)
                    {
                        catched?.Invoke(ex_);
                        continue;
                    }
                    throw;
                }
                yield return e.Current;
            }
        }
        #endregion
        #region dispose
        public static void Dispose<T>(this IEnumerable<T> enums) where T : IDisposable => enums.Do(_ => _.Dispose());
        public static IEnumerable<T> Using<T>(this IEnumerable<T> enums) where T : IDisposable
        {
            foreach (var _ in enums) using (_) yield return _;
        }
        #endregion
    }
}
