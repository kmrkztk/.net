using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Lib.Windows.Drawing
{
    public class BitmapBuffer : IDisposable
    {
        readonly static PixelFormat _format = PixelFormat.Format32bppArgb;
        readonly BitmapData _bd;
        readonly GCHandle _hndl;
        readonly Bitmap _bmp;
        readonly int[] _buf;
        public int Width => _bd.Width;
        public int Height => _bd.Height;
        public int Stride => _bd.Stride;
        public Size Size => new Size(Width, Height);
        public int[] Buffer => _buf;
        public BitmapBuffer(Bitmap bmp)
        {
            _bmp = bmp ?? throw new ArgumentNullException();
            _bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, _format);
            _buf = new int[bmp.Height * _bd.Width];
            _hndl = GCHandle.Alloc(_buf, GCHandleType.Pinned);
            Marshal.Copy(_bd.Scan0, _buf, 0, _buf.Length);
        }
        public void Dispose()
        {
            Marshal.Copy(_buf, 0, _bd.Scan0, _buf.Length);
            _hndl.Free();
            _bmp.UnlockBits(_bd);
        }
        public int this[int index] { get => _buf[index]; set => _buf[index] = value; }
        public int this[int x, int y] { get => this[x + y * _bd.Width]; set => this[x + y * _bd.Width] = value; }
        public int this[Point p] { get => this[p.X, p.Y]; set => this[p.X, p.Y] = value; }
        public void FillRectangle(Point point, Size size, Color color) => FillRectangle(point, size, color.ToArgb());
        public void FillRectangle(Point point, Size size, int color) => FillRectangle(point.X, point.Y, size.Width, size.Height, color);
        public void FillRectangle(int x, int y, int width, int height, Color color) => FillRectangle(x, y, width, height, color.ToArgb());
        public void FillRectangle(int x, int y, int width, int height, int color)
        {
            var buf = Enumerable.Range(0, width).Select(_ => color).ToArray();
            var i0 = x + y * Width;
            var i1 = x + (y + height) * Width;
            for (var i = i0; i < i1; i += Width) Array.Copy(buf, 0, _buf, i, width);
        }
        public void FillSquare(Point point, int size, Color color) => FillRectangle(point, new Size(size, size), color);
        public void DrawPoint(Point point, Color color) => DrawPoint(point, color.ToArgb());
        public void DrawPoint(Point point, int color) => DrawPoint(point.X, point.Y, color);
        public void DrawPoint(int x, int y, Color color) => DrawPoint(x, y, color.ToArgb()); 
        public void DrawPoint(int x, int y, int color) => this[x, y] = color;
        public static int[] Extract(Bitmap bmp)
        {
            using (var bd = new BitmapBuffer(bmp)) return bd._buf;
        }
        public static Bitmap Create(int width, int height, int[] data)
        {
            var bmp = new Bitmap(width, height, _format);
            using (var bd = new BitmapBuffer(bmp)) Marshal.Copy(data, 0, bd._bd.Scan0, bd._buf.Length);
            return bmp;
        }
    }
}
