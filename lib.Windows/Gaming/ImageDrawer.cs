using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Windows.Gaming
{
    public class ImageDrawer : Drawer
    {
        public Point Location { get; set; }
        public Image Image { get; set; }
        protected override void Draw(Graphics g) => g.DrawImageUnscaled(Image, Location);
        public ImageDrawer() : this(null) { }
        public ImageDrawer(IContainer container) => container?.Add(this);
    }
}
