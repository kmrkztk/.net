using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CommandValueAttribute : Attribute
    {
        public char Separator { get; set; } = char.MinValue;
    }
}
