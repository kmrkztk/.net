using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class StreamExtentions
    {
        public static void SeekTo(this Stream stream, SeekOrigin origin, byte c, int count = 1)
        {
            const int BUFFER_SIZE = 32;
            stream.Seek(0, origin);
            var len = stream.Length;
            var pos = stream.Position;
            var buf = new byte[BUFFER_SIZE];
            var cnt = 0;

            // 末尾から
            if (origin == SeekOrigin.End)
            {
                while(pos > 0)
                {
                    var l = pos - BUFFER_SIZE > 0 ? BUFFER_SIZE : (int)pos;
                    stream.Seek(-l, SeekOrigin.Current);
                    stream.Read(buf, 0, l);
                    stream.Seek(-l, SeekOrigin.Current);
                    pos -= l;
                    for (var i = l - 1; i >= 0; i--)
                    {
                        if (buf[i] != c) continue;
                        if (++cnt >= count)
                        {
                            stream.Seek(i + 1, SeekOrigin.Current);
                            return;
                        }
                    }
                }
            }
            // 先頭、もしくは現在位置から
            else
            {
                while (pos < len)
                {
                    var l = pos + BUFFER_SIZE < len ? BUFFER_SIZE : (int)(len - pos);
                    stream.Read(buf, 0, l);
                    pos += l;
                    for (var i = 0; i < l; i++)
                    {
                        if (buf[i] != c) continue;
                        if (++cnt >= count)
                        {
                            stream.Seek(i - l, SeekOrigin.Current);
                            return;
                        }
                    }
                }

            }
        }

    }
}
