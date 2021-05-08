using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lib.Reflection;

namespace Lib.Web.Twitter
{
    public class TimelineOption
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string SinceId { get; set; }
        public string UntilId { get; set; }
        public int? MaxResult { get; set; }
        public string NextToken { get; set; }
        public ExpansionsOptions Expansions { get; set; }
        public TweetFieldsOptions TweetFields { get; set; }
        public MediaFieldsOptions MediaFields { get; set; }
        public PlaceFieldsOptions PlaceFields { get; set; }
        public PollFieldsOptions PollFields { get; set; }
        public UserFieldsOptions UserFields { get; set; }
        public ExcludeOptions Exclude { get; set; }

#pragma warning disable IDE0051 
#pragma warning disable IDE1006 
        [Name("start_time")]        string _StartTime => StartTime?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        [Name("end_time")]          string _EndTime => EndTime?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        [Name("since_id")]          string _SinceId => SinceId;
        [Name("until_id")]          string _UntilId => UntilId;
        [Name("max_results")]       string _MaxResult => MaxResult?.ToString();
        [Name("pagination_token")]  string _NextToken => NextToken;
        [Name("expansions")]        string _Expansions  => ToParameters(Expansions );
        [Name("tweet.fields")]      string _TweetFields => ToParameters(TweetFields);
        [Name("media.fields")]      string _MediaFields => ToParameters(MediaFields);
        [Name("place.fields")]      string _PlaceFields => ToParameters(PlaceFields);
        [Name("poll.fields")]       string _PollFields  => ToParameters(PollFields );
        [Name("user.fields")]       string _UserFields => ToParameters(UserFields);
        [Name("exclude")]           string _Exclude => ToParameters(Exclude);
#pragma warning restore IDE0051 
#pragma warning restore IDE1006 

        public override string ToString() => string.Join("&",
            this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(_ => _.HasAttribute<NameAttribute>())
            .Select(_ => (NameAttribute.GetName(_), _.GetValue(this) as string))
            .Where(_ => !string.IsNullOrEmpty(_.Item2))
            .Select(_ => string.Format("{0}={1}", _.Item1, _.Item2)));
        public static TEnum FlagsOf<TEnum>(params string[] values) where TEnum : struct, Enum => Enum.GetValues<TEnum>()
            .Where(_ => values.Any(v => v == NameAttribute.GetName(typeof(TEnum).GetField(_.ToString()))))
            .Sum();
        public static string ToParameters<TEnum>(TEnum flags) where TEnum : struct, Enum => (int)(object)flags == 0x00 
            ? null 
            : string.Join(",", Enum.GetValues<TEnum>()
                .Where(_ => (int)(object)_ > 0)
#pragma warning disable CA2248
                .Where(_ =>  flags.HasFlag(_))
#pragma warning restore CA2248
                .Select(_ => NameAttribute.GetName(typeof(TEnum).GetField(_.ToString()))));
    }
}
