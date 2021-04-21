using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Configuration
{
    /*
    public static class Config
    {
        static FileSystemWatcher _watch;
        public static T Load<T>()
        {
            var settings = ConfigurationManager.AppSettings;
            var instance = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            var map = PropertyMap.Of(instance);
            foreach (var k in settings.AllKeys) 
                try { map.SetValue(k, settings[k]); } catch { }
            return instance;
        }
        public static void Watch(object instance)
        {
            var map = PropertyMap.Of(instance);
            var fi = new FileInfo(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);
            if (_watch != null) Release();
            _watch = new FileSystemWatcher(fi.DirectoryName, fi.Name);
            _watch.Changed += (sender, e) =>
            {
                try
                {
                    ConfigurationManager.RefreshSection("appSettings");
                    var settings = ConfigurationManager.AppSettings;
                    foreach (var k in settings.AllKeys)
                        try { map.SetValue(k, settings[k]); } catch { }
                }
                catch { }
            };
            _watch.EnableRaisingEvents = true;
        }
        public static void Release() 
        {
            _watch.EnableRaisingEvents = false;
            _watch?.Dispose();
            _watch = null;
        }
    }
    */
}
