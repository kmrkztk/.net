using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Html
{
    public class HtmlReader : StreamReader
    {
        public char MarkUpBracketStart { get; set; } = '<';
        public char MarkUpBracketEnd { get; set; } = '>';
        
        public HtmlReader(Stream stream) : base(stream) { }
        public HtmlTag ReadTag()
        {
            return default;
        }
        public IEnumerable<HtmlTag> ReadTags()
        {
            return default;
        }
    }
}
