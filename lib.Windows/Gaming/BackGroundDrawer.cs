using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Windows.Gaming
{
    public class BackGroundDrawer : Lib.Windows.Gaming.Drawer
    {
        Color _back = Color.Gray;
        Brush _brush = new SolidBrush(Color.Gray);
        [DefaultValue(typeof(Color), "Gray")]
        public Color BackColor
        {
            get => _back;
            set
            {
                _back = value;
                _brush.Dispose();
                _brush = new SolidBrush(value);
            }
        }
        public BackGroundDrawer() : base() { }
        public BackGroundDrawer(IContainer container) : base(container) { }

        protected override void Draw(Graphics g)
        {
            g.FillRectangle(_brush, g.ClipBounds);
        }
    }
}
