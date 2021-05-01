using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Json;

namespace Lib.Web.Twitter
{
    public class TwitterApiException : Exception
    {
        public IEnumerable<string> Messages => RawData["errors"].AsArray().Select(_ => _["message"]?.Value);
        public Json.Json RawData { get; init; }
        public TwitterApiException(Json.Json json) : base(json["errors"][0]["message"].Value) => RawData = json;
    }
}
