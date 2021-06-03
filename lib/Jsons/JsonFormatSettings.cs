using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Jsons
{
    public class JsonFormatSettings
    {
        int _indent;
        public int Indent { get; set; } = 2;
        public char IndentChar { get; set; } = ' ';
        public string LineSeparator { get; set; } = "\r\n";
        public bool Escape { get; set; } = true;
        public JsonFormatSettings InnerSetting {
            get
            {
                var clone = (JsonFormatSettings)MemberwiseClone();
                clone._indent++;
                return clone;
            } 
        }
        public string Indents => LineSeparator == null ? "" : new string(IndentChar, Indent * _indent);
        public string InnerIndents => LineSeparator == null ? "" : new string(IndentChar, Indent * (_indent + 1));
        public string Separator => LineSeparator ?? " ";
        public static JsonFormatSettings Default = new JsonFormatSettings();
    }
}
