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
        public class FlagsFormatter : IFormatter
        {
            public string ToString(object obj)
            {
                if (obj == null) return null;
                var type = obj.GetType();
                if (!type.IsEnum) return obj.ToString();
                if ((int)obj == 0x00) return null;
                IEnumerable<object> values()
                {
                    foreach (var _ in Enum.GetValues(type)) yield return _;
                }
                var flags = (int)obj;
                return string.Join(",", values()
                    .Where(_ => (flags & (int)_) > 0)
                    .Select(_ => NameAttribute.GetMemberName(type.GetField(_.ToString())))
                    );
            }
        }
    }
}
