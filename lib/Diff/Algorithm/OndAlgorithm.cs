using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public partial class DiffAlgorithm<T>
    {
        class OndAlgorithm : DiffAlgorithm<T>
        {
            protected override EditScripts Calc(T[] left, T[] right, DiffComparison<T> comparison)
            {
                var m = left.Length;
                var n = right.Length;
                var v = new EditScript[m + n + 1];
                var o = m;
                EditScript max(params EditScript[] s)
                {
                    var s_ = s[0];
                    for (var i = 1; i < s.Length; i++) if (s_.Y < s[i].Y) s_ = s[i];
                    return s_;
                }
                EditScript snake(EditScript s)
                {
                    while (
                        s.X < m &&
                        s.Y < n &&
                        comparison(left[s.X], right[s.Y])) s = s.None;
                    return s;
                }
                for (var d = 0; d <= m + n; d++)
                {
                    for (
                        var k = (d <= m ? -d + o : d - 2 * m + o);
                        k <= (d <= n ? d + o : -d + 2 * n + o);
                        k += 2)
                    {
                        v[k] = snake(d == 0 ? new EditScript()
                            : k == -d + o ? v[k + 1].Delete
                            : k == d + o ? v[k - 1].Insert
                            : max(snake(v[k].Replace), v[k + 1].Delete, v[k - 1].Insert));
                        if (v[k].X >= m && v[k].Y >= n) return v[k].GetScripts();
                    }
                }
                throw new SystemException();
            }
        }
    }
}
