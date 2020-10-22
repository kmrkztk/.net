using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace life.Controls
{
    public class MapDrawerFull : MapDrawer
    {
        Bitmap _bmp;
        EventHandler _updated;
        public MapDrawerFull() : base() { }
        public MapDrawerFull(IContainer container) : base(container) { }
        protected override void Dispose(bool disposing) 
        {
            base.Dispose(disposing);
            _bmp?.Dispose();
        }
        public override MapData Map
        {
            get => base.Map;
            set
            {
                if (_updated == null) _updated = (sender, e) => Refresh();
                if (base.Map != null) base.Map.Updated -= _updated;
                base.Map = value;
                if (base.Map != null) base.Map.Updated += _updated;
                Refresh();
            }
        }
        protected override void Draw(Graphics g)
        {
            if (_bmp == null) return;
            if (_bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.DontCare) return;
            g.DrawImage(_bmp, Location);
        }
        protected override void OnDrawAreaChanged(DrawAreaChangedEventArgs e)
        {
            if (e.IsZoomed) Refresh();
            base.OnDrawAreaChanged(e);
        }
        public void Refresh()
        {
            _bmp?.Dispose();
            if (Map == null) return;
            var size = new Size(this.Map.Width * Scale, this.Map.Height * Scale);
            if (size.IsEmpty) return;
            _bmp = new Bitmap(size.Width, size.Height);
            Paint(_bmp, Point.Empty);
        }
    }
}
