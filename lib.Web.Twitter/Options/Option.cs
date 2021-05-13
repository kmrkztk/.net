using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lib.Reflection;

namespace Lib.Web.Twitter.Options
{
    public abstract class Option
    {
        public override string ToString() => string.Join("&",
          this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
          .Where(_ => _.HasAttribute<NameAttribute>())
          .Select(_ => (NameAttribute.GetMemberName(_), FormatAttribute.GetFormatter(_).ToString(_.GetValue(this))))
          .Where(_ => !string.IsNullOrEmpty(_.Item2))
          .Select(_ => string.Format("{0}={1}", _.Item1, _.Item2)));
    }
}
