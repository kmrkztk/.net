﻿using System;
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
    public class KanaValidationAttribute : RegexMatchValidationAttribute
    {
        public override string DefaultMessage => "'{name}' is only kana.";
        public KanaValidationAttribute() : base(RegexPattern.OnlyKana) { }
    }
}
