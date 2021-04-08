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
        public string Name { get; protected set; }
        public MappingAttribute(string name) => Name = name;
        public MappingAttribute() : this(null) { }
        public virtual string GetMappingName(PropertyInfo pi) => Name ?? pi.Name;
        public static IEnumerable<string> GetMappingNames(PropertyInfo pi) 
            => pi.GetCustomAttributes<MappingAttribute>().Select(_ => _.GetMappingName(pi));
    }
}
