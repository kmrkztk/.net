using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LessThanValidationAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' must be less than '{name2}'.";
        public bool Equal { get; init; }
        public string Name { get; init; }
        public LessThanValidationAttribute(string name) => Name = name;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{name2}", Name);
        public override bool HasError(Property property, ValidationContext context)
           => Compare(Parse(property.GetValue()), Parse(context.GetPropertyValue(Name)));
        bool Compare(decimal d1, decimal d2) => Equal ? d1 > d2 : d1 >= d2;
        static decimal Parse(object value) => Convert.ToDecimal(value);
    }
}
