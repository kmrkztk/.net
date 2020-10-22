using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using life.IO;

namespace life.Controls
{
    public partial class MapPreview : Form
    {
        public event EventHandler Updated;
        protected virtual void OnUpdated() => Updated?.Invoke(this, EventArgs.Empty);
        public Map Map { get => _view.Map.Map; set => _view.Map.Map = value ?? Maps.Empty; }
        public void Set(Map map)
        {
            Map = map;
            _view.Drawer.Scale = 1;
            _align.Align(ContentAlignment.MiddleCenter);
        }
        public void Set(MapFileInfo fi)
        {
            this.Text = fi?.FullPath ?? fi?.ResouceName;
            Set(fi?.Load());
        }
        public MapPreview(MapFileInfo fi) : this() => Set(fi);
        public MapPreview(Map map) : this() => Set(map);
        public MapPreview()
        {
            InitializeComponent();
            _view.Map.Updated += (sender, e) => OnUpdated();
        }
    }
}
