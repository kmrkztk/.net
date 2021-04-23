using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;
using Lib.Text.RegularExpressions;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericValidationAttribute : RegexMatchValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is only numeric.";
        public NumericValidationAttribute() : base(RegexPattern.OnlyNumerics) { }
    }
}
