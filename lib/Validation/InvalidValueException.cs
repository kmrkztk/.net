using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    public class InvalidValueException : Exception
    {
        public List<ValidationMessage> Messages { get; }
        public InvalidValueException(IEnumerable<ValidationMessage> messages) : base() => Messages = messages.ToList();
    }
}
