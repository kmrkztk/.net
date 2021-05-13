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

        public static Includes OfVer1(JsonObject json) => new()
        {
            Media = json["extended_entities"]?["media"]?.AsArray()
                    .Select(_ => new Media()
                    {
                        ID = _["id"].Value,
                        Type = _["type"].Value,
                        Url =
                            _["type"].Value == "photo" ? _["media_url"].Value :
                            _["type"].Value == "video" ? _["video_info"]["variants"]
                                .AsArray()
                                .OrderBy(_ => _["bitrate"]?.Value ?? "a")
                                .FirstOrDefault()?["url"].Value :
                            null,
                    })
                    .Where(_ => _.Url != null)
                    .ToList(),
        };
    }
}
