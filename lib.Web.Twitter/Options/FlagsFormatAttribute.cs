using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Web.Twitter.Options
{
    public class FlagsFormatAttribute : FormatAttribute
    {
        public FlagsFormatAttribute() : base(new FlagsFormatter()) { }
        class FlagsFormatter : IFormatter
        {
            public string ToString(object obj)
            {
                if (obj == null) return null;
                var type = obj.GetType();
                if (!type.IsEnum) return obj.ToString();
                var flags = (int)obj;
                if (flags == 0x00) return null;
                return string.Join(",", Enum.GetValues(type).AsEnumerable<object>()
                    .Where(_ => (flags & (int)_) > 0)
                    .Select(_ => NameAttribute.GetMemberName(type.GetField(_.ToString())))
                    );
            }
        }
    }
}
