using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lib.Collections;

namespace Lib.Reflection
{
    public class PropertyMap : MultiMap<string, Property>
    {
        public static PropertyMap Of(object instance) => new PropertyMap(
            instance.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SelectMany(_ =>
                    MappingAttribute.GetMappingNames(_).Select(n =>
                    new KeyValuePair<string, Property>(n, new Property(instance, _)))));
        public PropertyMap() : base() { }
        public PropertyMap(IEnumerable<KeyValuePair<string, Property>> dictionary) : base(dictionary) { }
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var _ in this)
            {
                sb.AppendFormat("[{0}] {1}", _.Key, _.Value);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
