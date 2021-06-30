using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web
{
    public class RequestParameter
    {
        readonly Dictionary<string, object> _params = new();
        public RequestParameter() { }
        public RequestParameter(IEnumerable<KeyValuePair<string, object>> parameters) => _params = parameters.ToDictionary(_ => _.Key, _ => _.Value);
        public RequestParameter(IDictionary<string, object> parameters) : this((IEnumerable < KeyValuePair<string, object>>)parameters) { }
        public object this[string key]
        {
            get => _params.ContainsKey(key) ? _params[key] : null;
            set { if (_params.ContainsKey(key)) if (value == null) _params.Remove(key); else _params[key] = value; else _params.Add(key, value); }
        }
        public ICollection<string> Keys => _params.Keys;
        public int Count => _params.Count;
        public void Clear() => _params.Clear();
    }
}
