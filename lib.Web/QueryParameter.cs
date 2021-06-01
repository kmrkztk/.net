using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web
{
    public class QueryParameter : RequestParameter
    {
        public QueryParameter() : base() { }
        public QueryParameter(IEnumerable<KeyValuePair<string, object>> queries) : base(queries) { }
        public QueryParameter(IDictionary<string, object> parameters) : base(parameters) { }
        public override string ToString() => string.Join("&", Keys.Select(_ => string.Format("{0}={1}", _, this[_])));
    }
}
