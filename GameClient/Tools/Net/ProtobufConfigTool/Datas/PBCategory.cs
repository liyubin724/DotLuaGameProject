using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DotTools.PB.Datas
{
    [Serializable]
    public class PBCategory
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("comment")]
        public string Comment { get; set; } = string.Empty;
        [XmlElement("group")]
        public List<PBGroup> Groups { get; set; } = new List<PBGroup>();
    }
}
