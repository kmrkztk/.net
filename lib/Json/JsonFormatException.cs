using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Security;

namespace Lib.Json
{
    public class JsonFormatException : Exception
    {
        const string _message1 = "failed to format json. point:({0}, {1}) near:{2}...";
        const string _message2 = "failed to format json, because {3}. point:({0}, {1}) near:{2}...";
        public JsonFormatException(int row, int col, string near) : this(string.Format(_message1, row, col, near)) { }
        public JsonFormatException(int row, int col, string near, string message) : this(string.Format(_message2, row, col, near, message)) { }
        public JsonFormatException(string message) : base(message) { }
        public JsonFormatException(string message, Exception innerException) : base(message, innerException) { }
        [SecuritySafeCritical]
        protected JsonFormatException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
