using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Json;

namespace Lib.Web.Twitter.Objects
{
    public class Timeline : IList<Tweet>
    {
        readonly List<Tweet> _tweets;
        public Tweet this[int index] { get => _tweets[index]; set => _tweets[index] = value; }
        public User User { get; init; }
        public Includes Includes { get; init; }
        public Meta Meta { get; init; }

        public Timeline() => _tweets = new();
        public Timeline(JsonObject json) : this() 
        {
            _tweets = json["data"]?.AsArray().Select(_ => _.Cast<Tweet>()).ToList();
            Includes = json["includes"].Cast<Includes>();
            Meta = json["meta"].Cast<Meta>();
        }

        #region IList
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
        #endregion
    }
    public class Meta
    {
        [SnakeCaseName] public int ResultCount { get; init; }
        [SnakeCaseName] public ID OldestId { get; init; }
        [SnakeCaseName] public ID NewestId { get; init; }
        [SnakeCaseName] public string NextToken { get; init; }
    }
}
