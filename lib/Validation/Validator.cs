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
        public static bool HasError(object instance)
        {
            var context = new ValidationContext(instance);
            return context.Properties
                .SelectMany(_ => _.Info.GetCustomAttributes<ValidationAttribute>()
                .Select(a => (_, a)))
                .Any(_ => _.a.HasError(_._, context));
        }
        public static IEnumerable<ValidationMessage> GetMessages(object instance) 
        {
            var context = new ValidationContext(instance);
            return context.Properties
                .SelectMany(_ => _.Info.GetCustomAttributes<ValidationAttribute>()
                .Select(a => (_, a)))
                .OrderBy(_ => _.a.Order)
                .SelectMany(_ => _.a.GetMessages(_._, context));
        }
    }
}
