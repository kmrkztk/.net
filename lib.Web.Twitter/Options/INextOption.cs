using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Web.Twitter.Objects;

namespace Lib.Web.Twitter.Options
{
    public interface INextOption
    {
        public bool Next(Meta meta);
    }
}
