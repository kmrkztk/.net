using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public class EditScript
    {
        public EditScript Prev { get; private set; } = null;
        public DiffType Diff { get; private set; } = null;
        public int Distance { get; private set; } = 0;
        public int Index { get; private set; } = 0;
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public EditScript() { }
        protected EditScript(EditScript prev, DiffType diff)
        {
            Prev = prev;
            Diff = diff;
            Index = prev.Index + 1;
            Distance = prev.Distance + diff.Distance;
            X = prev.X + diff.DX;
            Y = prev.Y + diff.DY;
        }
        public EditScript None => new EditScript(this, DiffType.None);
        public EditScript Insert => new EditScript(this, DiffType.Insert);
        public EditScript Delete => new EditScript(this, DiffType.Delete);
        public EditScript Replace => new EditScript(this, DiffType.Replace);
        public EditScripts GetScripts()
        {
            var ess = new List<EditScript>();
            var es = this;
            do ess.Add(es);
            while ((es = es.Prev) != null);
            ess.Reverse();
            return new EditScripts(ess);
        }
        public override string ToString() => string.Format("{0}[{1}]({2},{3})", Diff, Distance, X, Y);
    }
}
