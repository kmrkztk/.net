using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lib.Xml
{
    public static class XmlExtensions
    {
        public static XmlAttribute GetAttribute(this XmlNode xml, string attribute) => xml?.Attributes?.Cast<XmlAttribute>().FirstOrDefault(_ => _.Name == attribute);
        public static XmlNode First(this XmlNode xml, Func<XmlNode, bool> predicate) => predicate(xml) ? xml : xml.ChildNodes?.Cast<XmlNode>().Select(_ => _.First(predicate)).FirstOrDefault(_ => _ != null);
        public static XmlNode FirstByName(this XmlNode xml, string name) => xml.First(_ => _.Name == name);
        public static XmlNode FirstByAttribute(this XmlNode xml, string attribute, string value) => xml.First(_ => _.GetAttribute(attribute)?.Value == value);
        public static XmlNode FirstByID(this XmlNode xml, string id) => xml.FirstByAttribute("ID", id);
    }
}
