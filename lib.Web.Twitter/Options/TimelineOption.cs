using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Web.Twitter.Options
{
    public class TimelineOption : Option
    {
        [SnakeCaseName] [Format("yyyy-MM-ddTHH:mm:ssZ")] public DateTime? StartTime { get; set; }
        [SnakeCaseName] [Format("yyyy-MM-ddTHH:mm:ssZ")] public DateTime? EndTime { get; set; }
        [SnakeCaseName]                      public string SinceId { get; set; }
        [SnakeCaseName]                      public string UntilId { get; set; }
        [SnakeCaseName]                      public int? MaxResults { get; set; }
        [SnakeCaseName]                      public string NextToken { get; set; }
        [SnakeCaseName] [FlagsFormat]        public ExcludeOptions Exclude { get; set; }
        [SnakeCaseName] [FlagsFormat]        public ExpansionsOptions Expansions { get; set; }
        [Name("tweet.fields")] [FlagsFormat] public TweetFieldsOptions TweetFields { get; set; }
        [Name("media.fields")] [FlagsFormat] public MediaFieldsOptions MediaFields { get; set; }
        [Name("place.fields")] [FlagsFormat] public PlaceFieldsOptions PlaceFields { get; set; }
        [Name("poll.fields")]  [FlagsFormat] public PollFieldsOptions PollFields { get; set; }
        [Name("user.fields")]  [FlagsFormat] public UserFieldsOptions UserFields { get; set; }
    }
}
