using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib
{
    [AttributeUsage(AttributeTargets.All)]
    public class SnakeCaseNameAttribute : NameAttribute
    {
        public override string GetName(MemberInfo info) => info?.Name.ToSnakeCase();
    }
}
