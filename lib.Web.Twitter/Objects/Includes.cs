using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Jsons;

namespace Lib.Web.Twitter.Objects
{
    public class Includes
    {
        [LowerName] public List<User> Users { get; set; }
        [LowerName] public List<Tweet> Tweets { get; set; }
        [LowerName] public List<Media> Media { get; set; }
        [LowerName] public List<Poll> Polls { get; set; }
        [LowerName] public List<Place> Places { get; set; }

        public static Includes OfVer1(Json json) => json == null ? null : new()
        {
            Media = json["extended_entities"]?["media"]?.AsArray()
                .Select(_ => Lib.Web.Twitter.Objects.Media.OfVer1(_))
                .Where(_ => _.Url != null)
                .ToList(),
        };
        public static Includes OfArrayVer1(Json json) => json == null ? null : Sum(json.AsArray().Select(_ => OfVer1(_)));
        public static Includes Sum(IEnumerable<Includes> includes) => new()
        {
            Users = includes.SelectMany(_ => _.Users ?? new()).ToList(),
            Tweets = includes.SelectMany(_ => _.Tweets ?? new()).ToList(),
            Media = includes.SelectMany(_ => _.Media ?? new()).ToList(),
            Places = includes.SelectMany(_ => _.Places ?? new()).ToList(),
            Polls = includes.SelectMany(_ => _.Polls ?? new()).ToList(),
        };
    }
}
