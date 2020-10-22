using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public partial class DiffAlgorithm<T>
    {
        class DpAlgorithm : DiffAlgorithm<T>
        {
            protected override EditScripts Calc(T[] left, T[] right, DiffComparison<T> comparison)
            {
                EditScript min(params EditScript[] s)
                {
                    var s_ = s[0];
                    for (var i = 1; i < s.Length; i++) if (s_.Distance > s[i].Distance) s_ = s[i];
                    return s_;
                }

                var eg = new EditGraph(left.Length, right.Length);
                for (var i = 0; i < left.Length; i++) for (var j = 0; j < right.Length; j++)
                    {
                        var es = eg[i - 1, j - 1];
                        eg[i, j] = min(
                            comparison(left[i], right[j]) ? es.None : es.Replace,
                            eg[i - 1, j].Delete,
                            eg[i, j - 1].Insert
                            );
                    }
                return eg.GetShortestEditScript();
            }
        }
    }
}
