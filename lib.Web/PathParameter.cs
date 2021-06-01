using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web
{
    public class PathParameter : RequestParameter
    {
        public PathParameter() : base() { }
        public PathParameter(IEnumerable<KeyValuePair<string, object>> parameters) : base(parameters) { }
        public PathParameter(IDictionary<string, object> parameters) : base(parameters) { }
        public PathParameter(IList<object> parameters) : this(parameters.Select((_, i) => (Key: i, Value: _)).ToDictionary(_ => _.Key.ToString(), _ => _.Value)) { }
        public Uri Replace(Uri uri) => new(uri.AbsolutePath.ReplaceKeywords(Keys.ToArray(), Keys.Select(_ => this[_].ToString()).ToArray()));
    }
}
