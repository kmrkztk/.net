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
        public CommandAttribute() : base() { }
        public CommandAttribute(string name) : base(name.ToLower()) { }
    }
}
