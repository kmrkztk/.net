using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NestedValidationAttribute : ValidationAttribute
    {
        public override string DefaultMessage => null;
        public override bool HasError(Property property, ValidationContext context) => Validator.HasError(property.GetValue());
        public override IEnumerable<ValidationMessage> GetMessages(Property property, ValidationContext context) => Validator.GetMessages(property.GetValue());
    }
}
