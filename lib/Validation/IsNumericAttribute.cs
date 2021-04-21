using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsNumericAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is not numeric.";
        public override bool HasError(Property property, ValidationContext context) =>
            decimal.TryParse(property.GetValue()?.ToString(), out _);
    }
}
