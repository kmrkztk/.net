using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;

namespace life.Controls
{
    public partial class Pen : Tool
    {
        public Pen() : this(null) { }
        public Pen(int width) : this(null) => Width = width;
        public Pen(IContainer container) : base(container) => Width = 5;
        [DefaultValue(5)] public virtual int Width { get => Map.Map.Width; set => Map.Map = Maps.Block(value); }
        public override void Move(MouseEventArgs e)
        {
            Location = e.Location;
            OnEditing();
        }
        public override void Drag(DragDropEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Location = e.Location;
            Edit.WriteOverlap();
        }
    }
}
