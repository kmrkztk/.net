using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Diff.Algorithm
{
    public partial class DiffAlgorithm<T>
    {
        class OnpAlgorithm : DiffAlgorithm<T>
        {
            protected override EditScripts Calc(T[] left, T[] right, DiffComparison<T> comparison)
            {
                if (left.Length > right.Length) return Calc(right, left, comparison).Reverse();

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
                for (var p = 0; p <= m; p++)
                {
                    for (var k = o - p; k < n; k++)
                    {
                        v[k] = snake(p == 0 ? (k == o ? new EditScript() : v[k - 1].Insert)
                            : k == o - p ? v[k + 1].Delete
                            : max(snake(v[k].Replace), v[k + 1].Delete, v[k - 1].Insert));
                    }
                    for (var k = n + p; k > n; k--)
                    {
                        v[k] = snake(k == n + p ? v[k - 1].Insert
                            : max(snake(v[k].Replace), v[k + 1].Delete, v[k - 1].Insert));
                    }
                    {
                        var k = n;
                        v[k] = snake(
                            p != 0 ? max(snake(v[k].Replace), v[k + 1].Delete, v[k - 1].Insert)
                            : k == o ? new EditScript() : v[k - 1].Insert
                            );
                        if (v[k].X == m && v[k].Y == n) return v[k].GetScripts();
                    }
                    
                }
                throw new SystemException();
            }
        }
    }
}
