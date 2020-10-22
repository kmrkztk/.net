using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace Lib
{
    public class PropertyMap : Dictionary<string, (object Target, PropertyInfo Property)>
    {
        public PropertyMap(params object[] obj) => Map(obj);
        public void Map(params object[] obj)
        {
            Clear();
            foreach(var o in obj) Add(o);
        }
        public void Add(object obj)
        {
            foreach (var p in obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                foreach (var a in p.GetCustomAttributes<MappingAttribute>().Select(_ => GetMappingName(_, p))) this.Add(a, (obj, p));
        }
        public bool Has(string name) => this.ContainsKey(name);
        public bool IsList(string name) => this[name].Property.PropertyType.IsGenericType && this[name].Property.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
        public T GetValue<T>(string name) => Has(name) ? (T)this[name].Property.GetValue(this[name].Target) : default;
        public void SetValue(string name, object value)
        {
            var (o, p) = this[name];
            p.SetValue(o, value.GetType() == p.PropertyType ? value : Convert.ChangeType(value, p.PropertyType));
        }
        protected virtual string GetMappingName(MappingAttribute a, PropertyInfo p) => a.Name ?? p.Name;
        public IEnumerable<string> GetNames(PropertyInfo p)
        {
            foreach (var k in this.Keys) if (this[k].Property.Equals(p)) yield return k;
        }
        public IEnumerable<object> Targets => this.Values.Select(_ => _.Target).Distinct();
        public IEnumerable<PropertyInfo> Properties => this.Values.Select(_ => _.Property).Distinct();
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendLine();
            s.AppendLine(Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));
            foreach (var o in Targets) foreach(var c in DetailAttribute.GetContents(o)) s.AppendLine(c);
            s.AppendLine();
            var ps = Properties;
            var names = ps.ToDictionary(_ => _, _ => string.Join(" or ", GetNames(_)));
            var padding = names.Values.Max(_ => _.Length);
            foreach (var p in ps) s.AppendLine("  " + names[p].PadRight(padding) + " : " + string.Concat(DetailAttribute.GetContents(p)));
            return s.ToString();
        }
    }
}
