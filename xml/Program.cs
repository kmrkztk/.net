using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Lib;

namespace xml
{
    class Program
    {
        static void Main()
        {
            var a = FileArguments<Options>.Load();
            foreach (var xml in a.GetReaders()
                .Select(_ =>
                {
                    if (!a.Options.Raw) _.Seek('<');
                    return _;
                })
                .Select(_ =>
                {
                    var xml = new XmlDocument();
                    xml.Load(_);
                    return xml;
                })) 
                if (a.Options.HasTag) ShowTag(xml, a); else if (a.Options.HasID) ShowId(xml, a); else Format(xml, a);
        }
        static void Format(XmlNode xml, FileArguments<Options> a)
        {
            using (var ms = new MemoryStream())
            using (var xw = new XmlTextWriter(ms, Encoding.UTF8)
            {
                Formatting = a.Options.NoFormat ? Formatting.None : Formatting.Indented,
                Indentation = a.Options.Indent,
                IndentChar = a.Options.IndentChar,
            })
            using (var sr = new StreamReader(ms))
            {
                xml.WriteContentTo(xw);
                xw.Flush();
                ms.Flush();
                ms.Position = 0;
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        static void ShowTag(XmlDocument xml, FileArguments<Options> a)
        {
            foreach (var n in xml.GetElementsByTagName(a.Options.Tag).Cast<XmlNode>()) if (n.HasChildNodes) Format(n, a); else Console.WriteLine(n.InnerXml);
        }
        static void ShowId(XmlDocument xml, FileArguments<Options> a)
        {
            var n = xml.GetElementById(a.Options.ID);
            if (n.HasChildNodes) Format(n, a); else Console.WriteLine(n.InnerXml);
        }
        class Options
        {
            [Command]
            [CommandValue]
            public string Tag { get; set; }
            public bool HasTag => !string.IsNullOrEmpty(Tag);
            [Command]
            [CommandValue]
            public string ID { get; set; }
            public bool HasID => !string.IsNullOrEmpty(ID);
            [Command("indent-char")]
            [CommandValue]
            public char IndentChar { get; set; } = ' ';
            [Command("indent")]
            [CommandValue]
            public int Indent { get; set; } = 2;
            [Command("none")]
            public bool NoFormat { get; set; }
            [Command]
            public bool Raw { get; set; }
        }
    }
}
