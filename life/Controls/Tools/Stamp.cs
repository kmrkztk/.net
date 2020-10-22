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
    public partial class Stamp : Tool
    {
        public Stamp() : this(null) { }
        public Stamp(IContainer container) : base(container)
        {
            Map.Map = Maps.Block(20);
        }
        public override void Drag(DragDropEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Location = e.Location;
            OnEditing();
        }
        public override void Drop(DragDropEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Location = e.Location;
            Edit.WriteOverlap();
        }
        public override void Cancel()
        {
            OnEdited();
        }
    }
}
