using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lib;

namespace sign
{
    class Program
    {
        static void Main()
        {
            var a = Arguments.Load();
            var config = Lib.Configuration.Config.Load<Config>();
            Console.WriteLine(config);
            Lib.Configuration.Config.Watch(config);
            do
            {
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine(config);
            }
            while (true);
        }
        class Config
        {
            [Mapping("key1")]
            public string Key1 { get; set; }
            [Mapping("key2")]
            public string Key2 { get; set; }
            [Mapping("key3")]
            public string Key3 { get; set; }
            [Mapping("key4")]
            public string Key4 { get; set; }
            public override string ToString() => string.Format("key1:{0} key2:{1} key3:{2} key4:{3}", Key1, Key2, Key3, Key4);
        }
    }
}
