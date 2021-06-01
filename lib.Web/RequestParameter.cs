using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web
{
    public class RequestParameter
    {
        readonly Dictionary<string, object> _parameters = new();
        public RequestParameter() { }
        public RequestParameter(IEnumerable<KeyValuePair<string, object>> parameters) => _parameters = parameters.ToDictionary(_ => _.Key, _ => _.Value);
        public RequestParameter(IDictionary<string, object> parameters) : this((IEnumerable < KeyValuePair<string, object>>)parameters) { }
        public object this[string key]
        {
            get => _parameters.ContainsKey(key) ? _parameters[key] : null;
            set { if (_parameters.ContainsKey(key)) if (value == null) _parameters.Remove(key); else _parameters[key] = value; else _parameters.Add(key, value); }
        }
        public ICollection<string> Keys => _parameters.Keys;
        public int Count => _parameters.Count;
        public void Clear() => _parameters.Clear();
    }
}
