using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Lib.Entity;
using Lib.Json;

namespace Lib.Web.Twitter
{
    public class User
    {
        public ID ID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public override string ToString() => string.Format("({0}){1}", ID, Name);
        public User() { }
        public User(JsonObject json)
        {
            ID = json["id"]?.Value;
            Name = json["name"]?.Value;
            UserName = json["username"]?.Value;

        }
    }
}
