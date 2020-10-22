using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Bit;

namespace life
{
    public static class Calculator
    {
        public static Map Next(Map map)
        {
            var next = map.Clone();
            var wid = map.Width;
            var hei = map.Height;
            var stride = map.Stride;
            var size = Map.DataSize;
            var dat = next.Data.Data;
            var prv = map.Data.Data;
            var msb = 0x80000000u;
            for (var i = stride + 1; i < stride * (hei + 1); i++)
            {
                if (i % stride == 0)
                {
                    if (wid % size > 0) dat[i - 1] &= Bit32.ByLength(wid % size);
                    continue;
                }
                var ui = i - stride;
                var di = i + stride;
                var upleft   = prv[ui - 1]; var up     = prv[ui]; var upright   = prv[ui + 1];
                var left     = prv[ i - 1]; var center = prv[ i]; var right     = prv[ i + 1];
                var downleft = prv[di - 1]; var down   = prv[di]; var downright = prv[di + 1];
                if (!up && !left && !center && !right && !down) continue;
                var a = (up     << 1) | (upleft   & msb ? 1u : 0u); var b = up  ; var c = (up     >> 1) | (upright   & 1u ? msb : 0u);
                var d = (center << 1) | (left     & msb ? 1u : 0u);               var e = (center >> 1) | (right     & 1u ? msb : 0u);
                var f = (down   << 1) | (downleft & msb ? 1u : 0u); var g = down; var h = (down   >> 1) | (downright & 1u ? msb : 0u);
                var s2 = a & b;
                var s1 = a ^ b;
                var s0 = ~(a | b);
                var s3 = s2 & c;
                s2 = (s2 & ~c) | (s1 & c);
                s1 = (s1 & ~c) | (s0 & c);
                s0 &= ~c;
                s3 = (s3 & ~d) | (s2 & d);
                s2 = (s2 & ~d) | (s1 & d);
                s1 = (s1 & ~d) | (s0 & d);
                s0 &= ~d;
                s3 = (s3 & ~e) | (s2 & e);
                s2 = (s2 & ~e) | (s1 & e);
                s1 = (s1 & ~e) | (s0 & e);
                s0 &= ~e;
                s3 = (s3 & ~f) | (s2 & f);
                s2 = (s2 & ~f) | (s1 & f);
                s1 = (s1 & ~f) | (s0 & f);
                s0 &= ~f;
                s3 = (s3 & ~g) | (s2 & g);
                s2 = (s2 & ~g) | (s1 & g);
                s1 = (s1 & ~g) | (s0 & g);
                //s0 &= ~g;
                s3 = (s3 & ~h) | (s2 & h);
                s2 = (s2 & ~h) | (s1 & h);
                //s1 = (s1 & ~h) | (s0 & h);
                //s0 &= ~h; 
                dat[i] = (center & s2) | s3;
            }
            return next;
        }
    }
}
