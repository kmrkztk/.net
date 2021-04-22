using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericValidationAttribute : RegexMatchValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is only numeric.";
        public NumericValidationAttribute() : base(Only(NumericPattern)) { }
    }
}
