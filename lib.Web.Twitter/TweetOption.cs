using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Twitter
{
    public class TweetOption
    {
        public override string ToString() =>
            "trim_user=true" +
            "&include_my_retweet=false" +
            "&include_entities=true" +
            "&tweet_mode=extended"
            ;
    }
}
