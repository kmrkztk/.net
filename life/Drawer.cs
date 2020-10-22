using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Lib;
using Lib.Bit;
using Lib.Windows.Drawing;

namespace life
{
    public class Drawer
    {
        int _alive = Color.Lime.ToArgb();
        int _death = Color.Black.ToArgb();
        public Color AliveColor { get => Color.FromArgb(_alive); set => _alive = value.ToArgb(); }
        public Color DeathColor { get => Color.FromArgb(_death); set => _death = value.ToArgb(); }
        public void Draw(Bitmap bmp, Map map) { using (var bd = new BitmapBuffer(bmp)) Draw(bd, map); }
        public void Draw(BitmapBuffer buffer, Map map)
        {
            var hei = map.Height;
            var stride = map.Stride;
            var size = Map.DataSize;
            var j = 0;
            var jx = 0;
            var offset = 0;
            unchecked
            {
                for (var i = stride + 1; i < stride * (hei + 1); i++)
                {
                    if (i % stride == 0)
                    {
                        j = 0;
                        jx = 0;
                        continue;
                    }
                    jx += size;
                    if (jx > buffer.Width) jx = buffer.Width;
                    var dat = (uint)map.Data.Data[i];
                    if (dat == 0u)
                    {
                        for (; j < jx; j++) buffer[offset++] = _death;
                    }
                    else
                    {
                        for (; j < jx; j++)
                        {
                            var b = (int)(~(dat & 1u) + 1);
                            buffer[offset++] = (b & _alive) | (~b & _death);
                            dat >>= 1;
                        }
                    }
                }
            }
        }
        public void Draw(Bitmap bmp, Map map, int scale) { using (var bd = new BitmapBuffer(bmp)) Draw(bd, map, scale); }
        public void Draw(BitmapBuffer buffer, Map map, int scale)
        {
            if (scale == 1 && buffer.Size == map.Size)
            {
                Draw(buffer, map);
                return;
            }
            var hei = map.Height;
            var stride = map.Stride;
            var size = Map.DataSize;
            var buf = new int[buffer.Width];
            var j = 0;
            var jx = 0;
            var offset = 0;
            unchecked
            {
                for (var i = stride + 1; i < stride * (hei + 1); i++)
                {
                    jx += size * scale;
                    if (jx > buffer.Width) jx = buffer.Width;
                    var dat = (uint)map.Data.Data[i];
                    if (dat == 0u)
                    {
                        for (;j < jx;) for(var k = 0; k < scale; k++) buf[j++] = _death;
                    }
                    else
                    {
                        for (; j < jx;)
                        {
                            var b = (int)(~(dat & 1u) + 1);
                            b = (b & _alive) | (~b & _death);
                            dat >>= 1;
                            for (var k = 0; k < scale; k++) buf[j++] = b;
                        }
                    }
                    if (i % stride == stride - 1)
                    {
                        for (var k = 0; k < scale; k++)
                        {
                            Array.Copy(buf, 0, buffer.Buffer, offset, buffer.Width);
                            offset += buffer.Width;
                        }
                        j = 0;
                        jx = 0;
                        i++;
                    }
                }
            }
        }
        public void Draw(Bitmap bmp, Map map, Point offset, int scale) => Draw(bmp, map, offset.X, offset.Y, scale);
        public void Draw(Bitmap bmp, Map map, int x, int y, int scale) { using (var bd = new BitmapBuffer(bmp)) Draw(bd, map, x, y, scale); }
        public void Draw(BitmapBuffer buffer, Map map, Point offset, int scale) => Draw(buffer, map, offset.X, offset.Y, scale);
        public void Draw(BitmapBuffer buffer, Map map, int x, int y, int scale)
        {
            if (x == 0 && y == 0 &&
                buffer.Width == map.Width * scale &&
                buffer.Height == map.Height * scale)
            {
                Draw(buffer, map, scale);
                return;
            }
            unchecked
            {
                var r = new Rectangle(x, y, map.Width * scale, map.Height * scale);
                r.Intersect(new Rectangle(0, 0, buffer.Width, buffer.Height));
                var offset = r.X + r.Y * buffer.Width;
                var x0 = (r.X - x) / scale;
                var wid = r.Width / scale;
                wid += r.Left % scale ==0 ? 0 : 1;
                wid += r.Right % scale == 0 ? 0 : 1;
                for (var y_ = r.Top; y_ < r.Bottom;)
                {
                    var buf = new int[r.Width];
                    {
                        var i = 0;
                        var y0 = (y_ - y) / scale;
                        var dat = map.ExtractLine(x0, y0, wid);
                        var j = 0;
                        for (var x_ = r.Left; x_ < r.Right;)
                        {
                            var b = dat[j++];
                            b = (b & _alive) | (~b & _death);
                            var len = scale - x_ % scale;
                            len = x_ + len > r.Right ? r.Right - x_ : len;
                            for (var k = 0; k < len; k++) buf[i++] = b;
                            x_ += len;
                        }
                    }
                    {
                        var len = scale - y_ % scale;
                        len = y_ + len > r.Bottom ? r.Bottom - y_ : len;
                        for (var i = 0; i < len; i++)
                        {
                            Array.Copy(buf, 0, buffer.Buffer, offset, buf.Length);
                            offset += buffer.Width;
                        }
                        y_ += len;
                    }
                }
            }
        }
    }
}
