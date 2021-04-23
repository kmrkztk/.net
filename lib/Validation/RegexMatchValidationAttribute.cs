using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib.Reflection;
using Lib.Text.RegularExpressions;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegexMatchValidationAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' dose not match the pattern '{pattern}'";
        public RegexPattern Pattern { get; init; } = RegexPattern.Any;
        public RegexOptions Options { get; init; } = RegexOptions.None;
        public RegexMatchValidationAttribute(string pattern) : this(RegexPattern.Of(pattern)) { }
        public RegexMatchValidationAttribute(RegexPattern pattern) => Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        public override string GetMessage(Property property, ValidationContext context) => base.GetMessage(property, context).Replace("{pattern}", Pattern);
        public override bool HasError(Property property, ValidationContext context) => Pattern.IsMatch((string)property.GetValue() ?? "", Options);
    }
}
