using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;

namespace Lib.Windows.Gaming
{
    public partial class Display : UserControl
    {
        public Display()
        {
            InitializeComponent();
            this.Paint += (sender, e) => _bg.Render();
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Layer Layer => _bg.Layer;
        [Browsable(false)] public Drawer BaseDrawer => _bg;
        [Browsable(false)] public Graphics Graphics => _g.Graphics;
        public void Render() => _g.Render();
    }
}
