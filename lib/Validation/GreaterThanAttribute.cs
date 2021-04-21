using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GreaterThanAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is greater than '{name2}'.";
        public bool Equal { get; }
        public string Name { get; }
        public GreaterThanAttribute(string name) => Name = name;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{name2}", Name);
        public override bool HasError(Property property, ValidationContext context)
            => decimal.TryParse(property.GetValue()?.ToString(), out var d1) 
            && decimal.TryParse(context.GetPropertyValue(Name)?.ToString(), out var d2)
            && (Equal ? d1 < d2 : d1 <= d2);
    }
}
