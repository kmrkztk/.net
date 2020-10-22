using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace life.Controls
{
    public class FlameDrawer : Lib.Windows.Gaming.Drawer
    {
        Color _back = Color.LightGray;
        System.Drawing.Pen _pen = new System.Drawing.Pen(Color.LightGray)
        {
            DashStyle = System.Drawing.Drawing2D.DashStyle.Dash,
        };
        [DefaultValue(typeof(Color), "LightGray")]
        public Color BackColor
        {
            get => _back;
            set
            {
                _back = value;
                _pen?.Dispose();
                _pen = new System.Drawing.Pen(value)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dash,
                };
            }
        }
        [Browsable(false)] public Rectangle Rectangle { get; set; }
        public FlameDrawer() : base() { }
        public FlameDrawer(IContainer container) : base(container) { }
        protected override void Draw(Graphics g)
        {
            g.DrawRectangle(_pen, Rectangle);
        }
    }
}
