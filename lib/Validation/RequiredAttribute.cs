using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is required.";
        public override bool HasError(Property property, ValidationContext context) 
            => property.PropertyType == typeof(string) ? 
            string.IsNullOrEmpty((string)property.GetValue()) : 
            property.GetValue() == null;  
    }
}
