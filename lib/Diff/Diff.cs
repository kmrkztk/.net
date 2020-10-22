using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff
{
    public class Diff<T>
    {
        public virtual DiffType Type { get; private set; }
        public virtual T Value { get; private set; }
        public virtual int Index { get; private set; }
        Diff(DiffType type, T value, int index)
        {
            Value = value;
            Type = type;
            Index = index;
        }
        public static Diff<T> None(T value, int index) => new Diff<T>(DiffType.None, value, index);
        public static Diff<T> Insert(T value, int index) => new Diff<T>(DiffType.Insert, value, index);
        public static Diff<T> Delete(T value, int index) => new Diff<T>(DiffType.Delete, value, index);
        public override string ToString() => string.Format("[{0}]{1}", Type, Value);
    }
}
