using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DotTool.Net
{
    [Serializable]
    [XmlRoot("config")]
    public class PBMessageConfig
    {
        [XmlAttribute("class_name")]
        public string ClassName { get; set; }

        [XmlElement("group")]
        public List<PBMessageGroup> Groups = new List<PBMessageGroup>();
    }

    [Serializable]
    public class PBMessageGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("message")]
        public List<PBMessage> Messages = new List<PBMessage>();
    }

    [Serializable]
    public class PBMessage
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("id")]
        public int UniqueId { get; set; }
        [XmlAttribute("enable")]
        public bool Enable { get; set; } = true;
        [XmlAttribute("comment")]
        public string Comment { get; set; }
        [XmlAttribute("protocol")]
        public string ProtocolName { get; set; }
        [XmlAttribute("is_compress")]
        public bool NeedCompress { get; set; } = false;
        [XmlAttribute("is_crypto")]
        public bool NeedCrypto { get; set; } = false;
    }
}
