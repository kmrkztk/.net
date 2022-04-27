using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Html
{
    public class HtmlText
    {
        List<HtmlTag> _tags = null;
        readonly HtmlReader _reader = null;
        public string Value { get; set; }
        public HtmlText(string value) => Value = value;

        public IEnumerable<HtmlTag> GetHtml()
        {
            var tags = new List<HtmlTag>();
            foreach(var _ in _tags ?? _reader.ReadTags())
            {
                tags.Add(_);
                yield return _;
            }
            _tags = tags;
        }
        public static implicit operator string(HtmlText value) => value?.Value;
        public static implicit operator HtmlText(string value) => new(value);
        public static HtmlText operator +(HtmlText a, HtmlText b) => new(a?.Value + b?.Value);
    }
}
