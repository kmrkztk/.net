using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LaterDateAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' must be later '{name2}'.";
        public bool Equal { get; }
        public string Name { get; }
        public LaterDateAttribute(string name) => Name = name;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{name2}", Name);
        public override bool HasError(Property property, ValidationContext context)
            => property.GetValue() is string ?
            DateTime.TryParse((string)property.GetValue(), out var d1)
            && DateTime.TryParse((string)context.GetPropertyValue(Name), out var d2)
            && (Equal ? d1 < d2 : d1 <= d2) :
            property.GetValue() is DateTime && (Equal ?
            (DateTime)property.GetValue() < (DateTime)context.GetPropertyValue(Name) :
            (DateTime)property.GetValue() <= (DateTime)context.GetPropertyValue(Name));
    }
}
