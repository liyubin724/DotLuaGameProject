using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DotTools.PB.Datas
{
    [Serializable]
    public class PBGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("message")]
        public List<PBMessage> Messages { get; set; } = new List<PBMessage>();
    }
}
