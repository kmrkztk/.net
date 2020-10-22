using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Resources;
using System.Collections;
using life.IO;

namespace life
{
    public static class Patterns
    {
        static readonly Dictionary<string, MapFileInfo> _patterns;
        static Patterns()
        {
            var reg = new Regex(@"^life\.patterns\..+\.rle$");
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            _patterns = asm.GetManifestResourceNames().Where(_ => reg.IsMatch(_)).Select(_ => new MapFileInfo(asm, _, ".rle")).ToDictionary(_ => _.ResouceName);
        }
        public static IEnumerable<MapFileInfo> GetFiles() => _patterns.Select(_ => _.Value);
        public static MapFileInfo GetFile(string name) => _patterns[name];
    }
}
