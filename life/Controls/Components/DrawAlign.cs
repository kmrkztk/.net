using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Windows.Gaming;

namespace life.Controls
{
    public partial class DrawAlign : Component
    {
        public DrawAlign() : this(null) { }
        public DrawAlign(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        public void Align(ContentAlignment align)
        {
            if (Owner.Drawer == null) return;
            if (Owner.Map == null) return;
            var a = (int)align;
            var map = Owner.MapRectangle;
            var client = Owner.ClientSize;
            if ((a & 0x1111) > 0) map.X = 0;
            if ((a & 0x2222) > 0) map.X = (client.Width - map.Width) / 2;
            if ((a & 0x4444) > 0) map.X = client.Width - map.Width;
            if ((a & 0x000F) > 0) map.Y = 0;
            if ((a & 0x00F0) > 0) map.Y = (client.Height - map.Height) / 2;
            if ((a & 0x0F00) > 0) map.Y = client.Height - map.Height;
            Owner.Drawer.Location = map.Location;
        }
    }
}
