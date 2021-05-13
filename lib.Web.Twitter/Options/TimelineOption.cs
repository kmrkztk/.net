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
        [SnakeCaseName] [Format("yyyy-MM-ddTHH:mm:ssZ")]        public DateTime? StartTime { get; set; }
        [SnakeCaseName] [Format("yyyy-MM-ddTHH:mm:ssZ")]        public DateTime? EndTime { get; set; }
        [SnakeCaseName]                                         public string SinceId { get; set; }
        [SnakeCaseName]                                         public string UntilId { get; set; }
        [SnakeCaseName]                                         public int? MaxResult { get; set; }
        [SnakeCaseName]                                         public string NextToken { get; set; }
        [SnakeCaseName] [Format(typeof(FlagsFormatter))]        public ExcludeOptions Exclude { get; set; }
        [SnakeCaseName] [Format(typeof(FlagsFormatter))]        public ExpansionsOptions Expansions { get; set; }
        [Name("tweet.fields")] [Format(typeof(FlagsFormatter))] public TweetFieldsOptions TweetFields { get; set; }
        [Name("media.fields")] [Format(typeof(FlagsFormatter))] public MediaFieldsOptions MediaFields { get; set; }
        [Name("place.fields")] [Format(typeof(FlagsFormatter))] public PlaceFieldsOptions PlaceFields { get; set; }
        [Name("poll.fields")]  [Format(typeof(FlagsFormatter))] public PollFieldsOptions PollFields { get; set; }
        [Name("user.fields")]  [Format(typeof(FlagsFormatter))] public UserFieldsOptions UserFields { get; set; }
    }
}
