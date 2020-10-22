using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff
{
    public class DiffType
    {
        public const char N = 'N';
        public const char I = 'I';
        public const char D = 'D';
        public static readonly DiffType None = new DiffType(N);
        public static readonly DiffType Insert = new DiffType(I);
        public static readonly DiffType Delete = new DiffType(D);
        public bool IsNone => this.Value == N;
        public bool IsInsert => this.Value == I;
        public bool IsDelete => this.Value == D;
        public char Value { get; }
        protected DiffType(char value) => Value = value;
        public override string ToString() => Value.ToString();
        public override bool Equals(object obj) => obj is DiffType diff && this.Value == diff.Value;
        public override int GetHashCode() => base.GetHashCode();
        public static bool operator ==(DiffType a, DiffType b) => a?.Value == b?.Value;
        public static bool operator !=(DiffType a, DiffType b) => !(a == b);
    }
}
