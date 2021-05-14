using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Lib.Jsons;

namespace Lib.Web.Twitter.Objects
{
    public static class JsonExtensions
    {
        public static DateTime? ToDateTime2(this Json json) =>
            json?.Value == null ? null :
            DateTime.ParseExact(json?.Value, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        public static DateTime? ToDateTime1_1(this Json json) => 
            json?.Value == null ? null : 
            DateTime.ParseExact(json?.Value, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);

    }
}
