using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lib.Reflection;

namespace Lib.Validation
{
    public static class Validator
    {
        public static void Validate() { }
        public static IEnumerable<ValidationMessage> GetMessages(object instance) 
        {
            var context = new ValidationContext(instance);
            return context.Properties
                .SelectMany(_ => _.Info.GetCustomAttributes<ValidationAttribute>()
                .Select(a => (_, a)))
                .OrderBy(_ => _.a.Order)
                .Where(_ => _.a.HasError(_._, context))
                .Select(_ => new ValidationMessage() { 
                    Property = _._, 
                    Message = _.a.GetMessage(_._, context) 
                });
        }
    }
}
