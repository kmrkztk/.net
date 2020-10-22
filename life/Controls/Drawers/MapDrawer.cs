using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;
using Lib.Windows.Drawing;

namespace life.Controls
{
    public class MapDrawer : Lib.Windows.Gaming.Drawer
    {
        readonly Drawer _drawer = new Drawer();
        MapData _map;
        DrawArea _area = DrawArea.Empty;
        EventHandler _updated;
        public event EventHandler<DrawAreaChangedEventArgs> DrawAreaChanged;
        protected virtual void OnDrawAreaChanged(DrawAreaChangedEventArgs e) => Render(() => DrawAreaChanged?.Invoke(this, e));
        [DefaultValue(typeof(Color), "Lime")] public Color AliveColor { get => _drawer.AliveColor; set => Render(() => _drawer.AliveColor = value); }
        [DefaultValue(typeof(Color), "Black")] public Color DeathColor { get => _drawer.DeathColor; set => Render(() => _drawer.DeathColor = value); }
        [Browsable(false)]
        public DrawArea Area
        {
            get => _area;
            set
            {
                if (_area == value) return;
                var old = _area;
                _area = value;
                OnDrawAreaChanged(new DrawAreaChangedEventArgs(old, _area));
            }
        }
        [DefaultValue(typeof(Point), "Empty")] public Point Location { get => Area.Location; set => Area = new DrawArea(value, Area.Scale); }
        [DefaultValue(1)] public int Scale { get => Area.Scale; set => Area = new DrawArea(Area.Location, value); }
        [DefaultValue(null)]
        public virtual MapData Map
        {
            get => _map;
            set
            {
                if (_updated == null) _updated = (sender, e) => Render();
                if (_map != null) _map.Updated -= _updated;
                _map = value;
                if (_map != null) _map.Updated += _updated;
            }
        }
        public MapDrawer() : base() { }
        public MapDrawer(IContainer container) : base(container) { }
        protected override void Draw(Graphics g)
        {
            if (Map?.Map == null) return;
            var rect = _area.GetRenderingRectangle(_map.Size, Display.ClientSize);
            if (rect.Size.IsEmpty) return;
            using (var bmp = new Bitmap(rect.Width, rect.Height))
            {
                Paint(bmp, _area.GetDrawLocation());
                g.DrawImage(bmp, rect.Location); 
            }
        }
        protected virtual void Paint(Bitmap bmp, Point location) => _drawer.Draw(bmp, Map?.Map, location, _area.Scale);
        protected void Render(Action action)
        {
            action.Invoke();
            Render();
        }
    }
    public class DrawAreaChangedEventArgs : EventArgs
    {
        public DrawArea OldValue { get; }
        public DrawArea NewValue { get; }
        public bool IsZoomed => OldValue.Scale != NewValue.Scale;
        public bool IsOffseted => OldValue.Location != NewValue.Location;
        public DrawAreaChangedEventArgs(DrawArea @old, DrawArea @new)
        {
            OldValue = old;
            NewValue = @new;
        }
    }
}
