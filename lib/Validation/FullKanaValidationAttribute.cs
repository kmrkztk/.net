using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FullKanaValidationAttribute : RegexMatchValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is only kana.";
        public FullKanaValidationAttribute() : base(Only(FullKanaPattern)) { }
    }
}
