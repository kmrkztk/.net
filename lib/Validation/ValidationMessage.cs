using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    public class ValidationMessage
    {
        public Property Property { get; init; }
        public string Message { get; init; }
    }
}
