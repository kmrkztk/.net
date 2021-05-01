﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;

namespace Lib.Web.Twitter
{
    public class Timeline : IEnumerable<Tweet>
    {
        public Tweet this[int index] => _tweets[index];
        readonly List<Tweet> _tweets;
        public IEnumerator<Tweet> GetEnumerator() => _tweets.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Meta Meta { get; init; }
        public Timeline(IEnumerable<Tweet> tweets) => _tweets = tweets.ToList();
    }
    public class Meta
    {
        public int ResultCount { get; init; }
        public ID OldestId { get; init; }
        public ID NewestId { get; init; }
        public string NextToken { get; init; }
    }
}
