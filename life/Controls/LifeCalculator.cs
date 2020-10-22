using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace life.Controls
{
    public partial class LifeCalculator : Lib.Windows.Gaming.Calculator
    {
        public MapData Map { get; set; }
        public LifeCalculator() : base() { }
        public LifeCalculator(IContainer container) : base(container) { }
        protected override void Calc() => Map?.Next();
    }
}
