using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Lib.Configuration;

namespace Lib.Web.Twitter
{
    public class TwitterApi : HttpRequest
    {
        public const string BaseUri = "https://api.twitter.com";
        public string Bearer => Client.DefaultRequestHeaders.Authorization.Parameter;
        public TwitterApi() : this(Config.Load<TwitterApiConfig>()) { }
        public TwitterApi(TwitterApiConfig config) : this(config.Bearer) { }
        public TwitterApi(string bearer) : this(new HttpClient(), bearer) { }
        public TwitterApi(HttpClient client) : this(client, Config.Load<TwitterApiConfig>()) { }
        public TwitterApi(HttpClient client, TwitterApiConfig config) : this(client, config.Bearer) { }
        public TwitterApi(HttpClient client, string bearer) : base(client, BaseUri)
        {
            Client.DefaultRequestHeaders.Authorization = new("Bearer", bearer);
        }
        [Uri("1.1")] public class Version1 : TwitterApi 
        {
        }
        [Uri("2")] public class Version2 : TwitterApi 
        {
        }
    }
}
