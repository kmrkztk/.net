using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pull_tw
{
    class User
    {
        public string Name { get; init; }
        public string ID { get; init; }
        public override string ToString() => string.Format("({0}){1}", ID, Name);
    }
    class Media
    {
        public string Url { get; init; }
    }
    class Tweet
    {
        public User User { get; init; }
        public string ID { get; init; }
        public string Text { get; init; }
        public IEnumerable<Media> Medias { get; init; }
        public override string ToString() => string.Format("[{0}]({1}){2}", User, ID, Text.Replace("\n", " "));
    }
}
