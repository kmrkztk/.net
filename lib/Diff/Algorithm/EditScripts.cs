using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public class EditScripts : IEnumerable<EditScript>
    {
        readonly List<EditScript> _scripts;
        public EditScripts(IEnumerable<EditScript> scripts) => _scripts = scripts.ToList();
        public EditScripts Reverse()
        {
            IEnumerable<EditScript> reverse()
            {
                EditScript es = null;
                foreach (var s in this)
                {
                    if (s.Diff == null) es = new EditScript();
                    else switch (s.Diff.Value)
                    {
                        case DiffType.N: es = es.None; break;
                        case DiffType.I: es = es.Delete; break;
                        case DiffType.D: es = es.Insert; break;
                        case DiffType.R: es = es.Replace; break;
                    }
                    yield return es;
                }
            }
            return new EditScripts(reverse());
        }
        public IEnumerable<Diff<T>> GetDiffs<T>(T[] left, T[] right)
        {
            int i = 0, j = 0;
            foreach (var s in this)
            {
                if (s.Diff == null) continue;
                switch (s.Diff.Value)
                {
                    case DiffType.N:
                        yield return Diff<T>.None(left[i], i);
                        i++; j++;
                        break;
                    case DiffType.I:
                        yield return Diff<T>.Insert(right[j], j);
                        j++;
                        break;
                    case DiffType.D:
                        yield return Diff<T>.Delete(left[i], i);
                        i++;
                        break;
                    case DiffType.R:
                        yield return Diff<T>.Delete(left[i], i);
                        yield return Diff<T>.Insert(right[j], j);
                        i++; j++;
                        break;
                }
            }
        }
        public IEnumerator<EditScript> GetEnumerator() => _scripts.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        public override string ToString() => string.Concat(this.Select(_ => _.Diff));
    }
}
