using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CommandAttribute : MappingAttribute
    {
        public const string DEFAULT_OPTION_PREFIX = "/";
        public string Prefix { get; protected set; }
        public CommandAttribute() : this(null) { }
        public CommandAttribute(string name) : this(DEFAULT_OPTION_PREFIX, name) { }
        public CommandAttribute(string prefix, string name) : base(name?.ToLower()) => Prefix = prefix;
        public override string GetMappingName(PropertyInfo pi) => Prefix + (Name ?? pi.Name).ToLower();
    }
}
