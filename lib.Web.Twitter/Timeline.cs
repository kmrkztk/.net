using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Json;

namespace Lib.Web.Twitter
{
    public class Timeline : IList<Tweet>
    {
        readonly List<Tweet> _tweets;
        public Tweet this[int index] { get => _tweets[index]; set => _tweets[index] = value; }
        public User User { get; init; }
        public Meta Meta { get; init; }

        public Timeline() => _tweets = new();
        public Timeline(IEnumerable<Tweet> tweets) => _tweets = tweets.ToList();
        public Timeline(JsonObject data) : this() { }

        public int IndexOf(Tweet item) => _tweets.IndexOf(item);
        public void Insert(int index, Tweet item) => _tweets.Insert(index, item);
        public void RemoveAt(int index) => _tweets.RemoveAt(index);
        public void Add(Tweet item) => _tweets.Add(item);
        public void Clear() => _tweets.Clear();
        public bool Contains(Tweet item) => _tweets.Contains(item);
        public void CopyTo(Tweet[] array, int arrayIndex) => _tweets.CopyTo(array, arrayIndex);
        public bool Remove(Tweet item)    => _tweets.Remove(item);
        public int Count => _tweets.Count;
        public bool IsReadOnly => false;
        public IEnumerator<Tweet> GetEnumerator() => _tweets.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class Meta
    {
        public int ResultCount { get; init; }
        public ID OldestId { get; init; }
        public ID NewestId { get; init; }
        public string NextToken { get; init; }

        public Meta() { }
        public Meta(JsonObject meta)
        {
            if (meta == null) return;
            NextToken = meta["next_token"]?.Value;
            NewestId = meta["newest_id"]?.Value;
            OldestId = meta["oldest_id"]?.Value;
            ResultCount = int.Parse(meta["result_count"]?.Value ?? "0");
        }
    }
}
