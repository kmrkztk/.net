using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public class DiffType : Lib.Diff.DiffType
    {
        public const char R = 'R';
        public static new readonly DiffType None = new DiffType(N);
        public static new readonly DiffType Insert = new DiffType(I);
        public static new readonly DiffType Delete = new DiffType(D);
        public static readonly DiffType Replace = new DiffType(R);
        public bool IsReplace => this.Value == R;
        public int Distance { get; }
        public int DX { get; }
        public int DY { get; }
        protected DiffType(char value) : base(value)
        {
            switch (value)
            {
                case N:
                    Distance = 0;
                    DX = 1;
                    DY = 1;
                    break;
                case I:
                    Distance = 1;
                    DX = 0;
                    DY = 1;
                    break;
                case D:
                    Distance = 1;
                    DX = 1;
                    DY = 0;
                    break;
                case R:
                    Distance = 2;
                    DX = 1;
                    DY = 1;
                    break;
                default: break;
            }
        }
    }
}
