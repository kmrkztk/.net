using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Reflection
{
    public class LowerFormatAttribute : FormatAttribute
    {
        class LowerFormatter : IFormatter
        {
            public string ToString(object obj) => obj?.ToString().ToLower();
        }
        public LowerFormatAttribute() : base(new LowerFormatter()) { }
    }
}
