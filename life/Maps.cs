using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace life
{
    public static class Maps
    {
        public static bool IsEmpty(Map map) => map.Width == 1 && map.Height == 1 && !map[0, 0];
        public static Map Empty => new Map(0, 0);
        public static Map Block(int size) => Block(size, size);
        public static Map Block(int width, int height)
        {
            var dat = new int[width, height];
            for (var x = 0; x < width; x++) for (var y = 0; y < height; y++) dat[x, y] = 1;
            return new Map(dat);
        }
        public static Map EmptyBlock(int size) => EmptyBlock(size, size);
        public static Map EmptyBlock(int width, int height) => new Map(new int[width, height]);
        public static Map Glider => new Map(3, 3, new[]{
            0, 1, 0,
            0, 0, 1,
            1, 1, 1,
        });
    }
}
