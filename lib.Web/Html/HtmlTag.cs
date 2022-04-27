using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Html
{
    public class HtmlTag
    {
        public string Name { get; set; }
        public HtmlText Value { get; set; }
        public IEnumerable<HtmlTag> Children => Value?.GetHtml();
        public List<HtmlAttribute> Attributes { get; } = new List<HtmlAttribute>();
    }
}
