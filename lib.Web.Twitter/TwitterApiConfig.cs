using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Configuration;

namespace Lib.Web.Twitter
{
    [JsonConfig("twitter-api.json")]
    public class TwitterApiConfig
    {
        public string Bearer { get; set; }
    }
}
