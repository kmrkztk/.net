using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;

namespace Lib.Web.Twitter
{
    public class Tweet
    {
        public ID ID { get; init; }
        public string Text { get; init; }
        public DateTime CreatedAt { get; init; }
        public IEnumerable<Media> Medias { get; init; }
        public User User { get; init; }
        public override string ToString() => 
            string.Format("[{0}] ({1}){2}", User, ID, Text) + 
            (Medias != null ? string.Join(",", Medias) : "");
    }
    public class User
    {
        public ID ID { get; init; }
        public string Name { get; init; }
        public override string ToString() => string.Format("({0}){1}", ID, Name);
    }
    public class Media
    {
        public ID ID { get; init; }
        public string Key { get; init; }
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
