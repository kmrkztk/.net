using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace life
{
    public struct DrawArea
    {
        Point _location;
        int _scale;
        public Point Location 
        {
            get => _location; 
            set => _location = value; 
        }
        public int Scale 
        {
            get => _scale;
            set => _scale = value > 1 ? value : 1;
        }
        public DrawArea(Point location, int scale)
        {
            _location = location;
            _scale = scale > 1 ? scale : 1;
        }
        public Rectangle GetRenderingRectangle(Size mapSize, Size clientSize)
        {
            var rect = new Rectangle(_location.X, _location.Y, mapSize.Width * _scale, mapSize.Height * _scale);
            rect.Intersect(new Rectangle(Point.Empty, clientSize));
            return rect;
        }
        public Point GetDrawLocation()
        {
            var x = _location.X;
            var y = _location.Y;
            if (x > 0) x = 0;
            if (y > 0) y = 0;
            return new Point(x, y);
        }
        public Point GetLocationOnMap(Point location)
        {
            var p = Point.Subtract(location, new Size(_location));
            return new Point(p.X / _scale, p.Y / _scale);
        }
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => string.Format("{0} x{1}", _location, _scale);
        public static DrawArea Empty => new DrawArea(Point.Empty, 1);
        public static bool operator ==(DrawArea a, DrawArea b) => a._location == b._location && a._scale == b._scale;
        public static bool operator !=(DrawArea a, DrawArea b) => a._location != b._location || a._scale != b._scale;
    }
}
