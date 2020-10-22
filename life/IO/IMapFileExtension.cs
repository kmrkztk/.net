using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace life.IO
{
    public interface IMapFileExtension
    {
        string Extension { get; }
        string Format(MapFileInfo info);
        string Format(Map map);
        Map ReadMap(StreamReader reader);
        string ReadName(StreamReader reader);
        string ReadAuthor(StreamReader reader);
        string[] ReadComments(StreamReader reader);
        Size ReadSize(StreamReader reader);
    }
}
