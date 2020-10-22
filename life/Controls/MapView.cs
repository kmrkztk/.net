using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Gaming;
using Lib.Windows.Controls;

namespace life.Controls
{
    public partial class MapView : Display
    {
        public MapView()
        {
            InitializeComponent();
            Components = new Components(this);
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] public Components Components { get; private set; } 
        [Browsable(false)] public MapData Map => _map;
        [Browsable(false)] public MapDrawer Drawer => _drawer;
        [Browsable(false)] public DragDropTrace Mouse => _mouse;
        [Browsable(false)] public Rectangle MapRectangle => new Rectangle(_drawer.Location, new Size(_map.Width * _drawer.Scale, _map.Height * _drawer.Scale));
        public void Superimpose(Point location, Map map) => _map.Superimpose(Drawer.Area.GetLocationOnMap(location), map);
        public void Overlap(Point location, Map map) => _map.Overlap(Drawer.Area.GetLocationOnMap(location), map);
    }
}
