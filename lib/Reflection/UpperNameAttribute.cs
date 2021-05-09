using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib
{
    [AttributeUsage(AttributeTargets.All)]
    public class UpperNameAttribute : NameAttribute
    {
        public override string GetValue(MemberInfo info) => info?.Name.ToUpper();
    }
}
