using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    public class ValidationContext
    {
        public List<Property> Properties { get; }
        public ValidationContext(object instance)
        {
            Properties = Property.GetProperties(instance).ToList();
        }
        public object GetPropertyValue(string name) => Properties.First(_ => _.Info.Name == name).GetValue();
    }
}
