using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Lib.Entity;
using Lib.Jsons;

namespace Lib.Web.Twitter.Objects
{
    public class User
    {
        [LowerName] public ID ID { get; set; }
        [LowerName] public string Name { get; set; }
        [LowerName] public string UserName { get; set; }
        [SnakeCaseName] public DateTime? CreatedAt { get; set; }
        [SnakeCaseName] public string Description { get; set; }
        [SnakeCaseName] public Entities Entities { get; set; }
        [SnakeCaseName] public string Location { get; set; }
        [SnakeCaseName] public ID? PinnedTweetId { get; set; }
        [SnakeCaseName] public string ProfileImageUrl { get; set; }
        [SnakeCaseName] public bool? Protected { get; set; }
        [SnakeCaseName] public Metrics PublicMetrics { get; set; }
        [SnakeCaseName] public string Url { get; set; }
        [SnakeCaseName] public bool? Verified { get; set; }
        [SnakeCaseName] public Withheld Withheld { get; set; }
        public override string ToString() => string.Format("({0}){1}", ID, Name);
        public class Metrics
        {
            [SnakeCaseName] public int? FollowersCount { get; set; }
            [SnakeCaseName] public int? FollowingCount { get; set; }
            [SnakeCaseName] public int? TweetCount { get; set; }
            [SnakeCaseName] public int? ListedCount { get; set; }
        }
        public static User OfVer1(Json json) => json == null ? null : new()
        {
            ID = json["id"]?.Value,
            Name = json["name"]?.Value,
            UserName = json["screen_name"]?.Value,
            CreatedAt = json["created_at"]?.ToDateTime1_1(),
            Description = json["description"]?.Value,
            Location = json["location"]?.Value,
            Url = json["url"]?.Value,
            Entities = Entities.OfVer1(json["entities"]?["url"]),
            Protected = json["protected"]?.Cast<bool>(),
            PublicMetrics = new()
            {
                FollowersCount = json["followers_count"]?.Cast<int>(),
                FollowingCount = json["friends_count"]?.Cast<int>(),
                ListedCount = json["listed_count"]?.Cast<int>(),
                TweetCount = json["statuses_count"]?.Cast<int>(),
            },
            ProfileImageUrl = json["profile_image_url"]?.Value,
            Verified = json["verified"]?.Cast<bool>(),
        };
    }
}
