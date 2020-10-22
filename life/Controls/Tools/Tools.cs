using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace life.Controls
{
    public static class Tools
    {
        public static Pen Pen1  => new Pen(1);
        public static Pen Pen5  => new Pen(5);
        public static Pen Pen10 => new Pen(10);
        public static Pen Pen20 => new Pen(20);
        public static Pen Pen50 => new Pen(50);
        public static Eraser Eraser1  => new Eraser(1);
        public static Eraser Eraser5  => new Eraser(5);
        public static Eraser Eraser10 => new Eraser(10);
        public static Eraser Eraser20 => new Eraser(20);
        public static Eraser Eraser50 => new Eraser(50);
    }
}
