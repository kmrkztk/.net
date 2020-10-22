using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public class EditGraph
    {
        readonly EditScript[,] _es;
        readonly int _wid;
        readonly int _hei;
        public int Height => _hei;
        public int Width => _wid;
        public EditScript this[int x, int y] { get => _es[x + 1, y + 1]; set => _es[x + 1, y + 1] = value; }
        public EditGraph(int width, int height) 
        {
            _wid = width;
            _hei = height;
            _es = new EditScript[width + 1, height + 1];
            _es[0, 0] = new EditScript();
            for (var i = 1; i <= _wid; i++) _es[i, 0] = _es[i - 1, 0].Delete;
            for (var i = 1; i <= _hei; i++) _es[0, i] = _es[0, i - 1].Insert;
        }
        public EditScripts GetShortestEditScript() => _es[_wid, _hei].GetScripts();
    }
}
