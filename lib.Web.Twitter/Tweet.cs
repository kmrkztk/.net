using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Twitter
{
    public class Tweet
    {
        public string ID { get; init; }
        public string Text { get; init; }
        public IEnumerable<Media> Medias { get; init; }
        public User User { get; init; }
        public override string ToString() => 
            string.Format("[{0}] ({1}){2}", User, ID, Text) + 
            (Medias != null ? string.Join(",", Medias) : "");
    }
    public class User
    {
        public string ID { get; init; }
        public string Name { get; init; }
        public override string ToString() => string.Format("({0}){1}", ID, Name);
    }
    public class Media
    {
        public string ID { get; init; }
        public string Type { get; init; }
        public string Url { get; init; }
        public bool IsPhoto => Type == "photo";
        public bool IsVideo => Type == "video";
        public override string ToString() => string.Format("({0})[{1}]{2}", ID, Type, Url);
    }
    public class Place
    {
    }
    public class Poll
    {
    }
}
