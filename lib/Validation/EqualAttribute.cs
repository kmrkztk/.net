using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EqualAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is greater than '{name2}'.";
        public bool Equal { get; }
        public string Name { get; }
        public EqualAttribute(string name) => Name = name;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{name2}", Name);
        public override bool HasError(Property property, ValidationContext context)
            => property.GetValue()?.Equals(context.GetPropertyValue(Name))
            ?? context.GetPropertyValue(Name) == null;
    }
}
