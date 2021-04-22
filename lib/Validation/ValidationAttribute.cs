using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        public abstract string DefaultMessage { get; }
        public string Message { get; private set; } = null;
        public int Order { get; private set; } = 0;
        
        public abstract bool HasError(Property property, ValidationContext context);
        public virtual string GetMessage(Property property, ValidationContext context) 
            => (Message ?? DefaultMessage)?
            .Replace("{name}", property.Info.Name)
            .Replace("{value}", property.GetValue()?.ToString());
        public virtual IEnumerable<ValidationMessage> GetMessages(Property property, ValidationContext context)
        {
            if (HasError(property, context)) yield return new()
            {
                Property = property,
                Message = GetMessage(property, context),
            };
        }
    }
}
