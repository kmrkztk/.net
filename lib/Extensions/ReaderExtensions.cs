using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class ReaderExtensions
    {
        public static int Seek(this TextReader reader, params char[] c)
        {
            int cnt = 0;
            int c_;
            while ((c_ = reader.Peek()) >= 0)
            {
                if (c.Any(_ => _ == (char)c_)) break;
                cnt++;
                reader.Read();
            }
            return cnt;
        }
    }
}
