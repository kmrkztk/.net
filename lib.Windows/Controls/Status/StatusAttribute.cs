using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Windows.Controls.Status
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StatusAttribute: Attribute
    {
        public string Name { get; set; } = null;
        public string Format { get; set; } = null;
        public StatusAttribute() : this(null) { }
        public StatusAttribute(string name) : this(name, null) { }
        public StatusAttribute(string name, string format) 
        {
            Name = name;
            Format = format;
        }
    }
}
