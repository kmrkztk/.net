using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Twitter.Objects
{
    public class Withheld
    {
        [LowerName] public string CopyRight { get; set; }
        [SnakeCaseName] public List<string> CountryCodes { get; set; }
    }
}
