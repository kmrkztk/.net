using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Bit;

namespace life
{
    public class Map : IEnumerable<int>
    {
        Bits _dat = new Bits(Bits.DataSize * 2 * 2);
        int _wid = 0;
        int _hei = 0;
        int _stride = 0;
        public const int DataSize = Bits.DataSize;
        public int Width
        {
            get => _wid;
            set => Resize(value, _hei);
        }
        public int Height
        {
            get => _hei;
            set => Resize(_wid, value);
        }
        public Size Size
        {
            get => new Size(_wid, _hei);
            set => Resize(value);
        }
        public int Stride => _stride;
        public bool this[int x, int y] { get => this[Index(x, y)]; set => this[Index(x, y)] = value; }
        public bool this[int index] { get => _dat[index]; set => _dat[index] = value; }
        public Bits Data => _dat;
        public int Length => _dat.Length; 
        public int Count => _dat.Count;
        public IEnumerable<Point> Points
        {
            get
            {
                for (var y = 0; y < _hei; y++)
                {
                    var i = (y + 1) * _stride + 1;
                    for (var x = 0; x < _stride - 1; x++)
                        foreach (var p in _dat.Data[i + x].SignificantPositions())
                            yield return new Point(x * Bits.DataSize + p, y);
                }
            }
        }
        public Map(Size size) => Resize(size);
        public Map(int width, int height) => Resize(width, height);
        public Map(int[,] dat) : this(dat.GetLength(0), dat.GetLength(1), dat.Cast<int>().ToArray()) { }
        public Map(int width, int height, int[] dat) : this(width, height)
        {
            var offset = 0;
            for (var i = _stride + 1; i < (_hei + 1) * _stride; i++)
            {
                if (i % _stride == 0)
                {
                    offset = 0;
                    continue;
                }
                var len = (offset + DataSize) > _wid ? _wid % DataSize : DataSize;
                _dat.Data[i] = Bit32.FromArray(dat, offset, len);
                offset += len;
            }
        }
        Map(int width, int height, Bits dat) : this(width, height) => _dat = dat;
        public void Clear() => _dat = new Bits(_dat.Length);
        public void Resize(Size size) => Resize(size.Width, size.Height);
        public void Resize(int width, int height)
        {
            var stride = (width - 1) / Bits.DataSize + 2;
            var dat = new Bits(new Bit32[stride * (height + 2) + 1]);
            var my = (_hei < height ? _hei : height) + 1;
            var mx = _stride < stride ? _stride : stride;
            for (var y = 1; y < my; y++) for (var x = 1; x < mx; x++) dat.Data[x + y * stride] = _dat.Data[x + y * _stride];
            _dat = dat;
            _wid = width;
            _hei = height;
            _stride = stride;
        }
        public Map Clone() => new Map(_wid, _hei, (Bits)_dat.Clone());
        public void Superimpose(Point location, Map map)
        {
            var r = new Rectangle(location, map.Size);
            r.Intersect(new Rectangle(new Point(0, 0), this.Size));
            for (var iy = 0; iy < r.Height; iy++) for (var ix = 0; ix < r.Width; ix++) this[ix + r.X, iy + r.Y] = map[ix, iy] || this[ix + r.X, iy + r.Y];
        }
        public void Overlap(Point location, Map map)
        {
            var r = new Rectangle(location, map.Size);
            r.Intersect(new Rectangle(new Point(0, 0), this.Size));
            for (var iy = 0; iy < r.Height; iy++) for (var ix = 0; ix < r.Width; ix++) this[ix + r.X, iy + r.Y] = map[ix, iy];
        }
        public Map Rotate(int direction)
        {
            switch (direction % 4)
            {
                case 0: return Clone();
                case 1: return Rotate();
                case 2: return Reverse().UpsideDown();
                case 3: return Reverse().UpsideDown().Rotate();
                default: return null;
            }
        }
        public Map Rotate()
        {
            var map = new Map(_hei, _wid);
            var size = DataSize;
            var j = 0;
            var remain = 0;
            var h = size - (_hei - 1) % size - 2;
            for (var i = _stride + 0; i < (_hei + 1) * _stride; i++)
            {
                if (i % _stride == 0)
                {
                    remain = _wid;
                    j = map._stride * 2 - ((i / _stride + h) / size) - 1;
                    continue;
                }
                var len = remain < size ? remain : size;
                var d = _dat.Data[i];
                for (var _ = 0; _ < len; _++)
                {
                    map._dat.Data[j] <<= 1;
                    map._dat.Data[j] |= d & 1u;
                    j += map._stride;
                    d >>= 1;
                }
                remain -= len;
            }
            return map;
        }
        public Map Reverse()
        {
            var map = new Map(_wid, _hei);
            var remain = _wid % DataSize;
            for (var i = _stride + 1; i < (_hei + 1) * _stride; i++)
            {
                if (i % _stride == 0) continue;
                var j = i / _stride * _stride + _stride - i % _stride;
                var b = _dat.Data[i];
                if (b != Bit32.Zero)
                {
                    b = b.Reverse();
                    map._dat.Data[j] |= b >> (DataSize - remain);
                    map._dat.Data[j - 1] = b << remain;
                }
            }
            return map;
        }
        public Map UpsideDown()
        {
            var map = new Map(_wid, _hei);
            var i0 = _stride + 1;
            var i1 = (_hei + 1) * _stride;
            var i2 = i1 + 2;
            for (var i = i0; i < i1; i += _stride) Array.Copy(_dat.Data, i, map._dat.Data, i2 - i, _stride - 1);
            return map;
        }
        public int Index(int x, int y) => _stride * Bits.DataSize * (y + 1) + x + Bits.DataSize;
        public (int X, int Y) Point(int index) => (index % (_stride * Bits.DataSize) - Bits.DataSize, index / (_stride * Bits.DataSize) - 1);
        public int[] ExtractLine(int x, int y, int width)
        {
            unchecked
            {
                var buf = new int[width];
                var j = 0;
                var xx = x + width;
                var i0 = (y + 1) * _stride + 1;
                for (var i = x; i < xx;)
                {
                    var dat = Data.Data[i0 + i / DataSize];
                    var len = DataSize - i % DataSize;
                    len = i + len > xx ? xx - i : len;
                    var jx = j + len;
                    dat >>= (i % DataSize);
                    if (dat == Bit32.Zero) j = jx;
                    else
                    {
                        for (; j < jx; j++)
                        {
                            buf[j] = (int)~(dat & 1) + 1;
                            dat >>= 1;
                        }
                    }
                    i += len;
                }
                return buf;
            }
        }
        public IEnumerator<int> GetEnumerator()
        {
            var offset = 0;
            for (var i = _stride + 1; i < (_hei + 1) * _stride; i++)
            {
                if (i % _stride == 0)
                {
                    offset = 0;
                    continue;
                }
                var len = (offset + DataSize) > _wid ? _wid % DataSize : DataSize;
                foreach (var j in _dat.Data[i].ToArray(0, len)) yield return j;
                offset += len;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < _hei + 2; y++)
            {
                var b = new Bits(_dat.Data.Skip(y * _stride).Take(_stride), _wid + DataSize);
                sb.AppendLine(b.ToString());
            }
            return sb.ToString();
        }
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public static Map operator &(Map a, Map b) => new Map(a._wid, a._hei, a._dat & b._dat);
        public static Map operator |(Map a, Map b) => new Map(a._wid, a._hei, a._dat | b._dat);
        public static Map operator ^(Map a, Map b) => new Map(a._wid, a._hei, a._dat ^ b._dat);
        public static Map operator ~(Map a) => new Map(a._wid, a._hei, ~a._dat);
        public static bool operator ==(Map a, Map b)
        {
            if (a == (object)null && b == (object)null) return true;
            if (a == (object)null || b == (object)null) return false;
            if (a.Width != b.Width) return false;
            if (a.Height != b.Height) return false;
            for (var i = a._stride + 1; i < (a._hei + 1) * a._stride; i++)
            {
                if (i % a._stride == 0) continue;
                if (a._dat.Data[i] != b._dat.Data[i]) return false;
            }
            return true;
        }
        public static bool operator !=(Map a, Map b) => !(a == b);
        public static Map Random(int width, int height) => Random(width, height, 50);
        public static Map Random(int width, int height, int per)
        {
            var map = new Map(width, height);
            var rnd = new Random(DateTime.Now.Millisecond);
            for (var y = 0; y < map.Height; y++) for (var x = 0; x < map.Width; x++) map[x, y] = rnd.Next(100) < per;
            return map;
        }
    }
}
