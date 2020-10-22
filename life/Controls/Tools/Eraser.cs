using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;

namespace life.Controls
{
    public partial class Eraser : Pen
    {
        public Eraser() : this(null) { }
        public Eraser(int width) : this(null) => Width = width;
        public Eraser(IContainer container) : base(container) { }
        [DefaultValue(5)] public override int Width { get => Map.Map.Width; set => Map.Map = Maps.EmptyBlock(value); }
    }
}
