using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lib
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MappingAttribute : Attribute
    {
        public virtual string Name { get; protected set; }
        public MappingAttribute(string name) => Name = name;
        public MappingAttribute() : this(null) { }
        public static IEnumerable<string> GetNames(PropertyInfo pi) => pi.GetCustomAttributes<CommandAttribute>().Select(_ => _.Name ?? pi.Name);
    }
}
