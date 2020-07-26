using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DotTool.PBConfig
{
    [Serializable]
    [XmlRoot("protos")]
    public class ProtoConfig
    {
        [XmlAttribute("namespace")]
        public string SpaceName { get; set; }

        [XmlElement("c2s")]
        public ProtoGroup C2SGroup { get; set; }

        [XmlElement("s2c")]
        public ProtoGroup S2CGroup { get; set; }
    }

    [Serializable]
    public class ProtoGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("message")]
        public List<ProtoMessage> Messages { get; set; }
    }

    [Serializable]
    public class ProtoMessage
    {
        [XmlAttribute("enable")]
        public bool Enable { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlAttribute("class_name")]
        public string ClassName { get; set; }

        [XmlAttribute("is_compress")]
        public bool IsCompress { get; set; } = false;

        [XmlAttribute("is_crypto")]
        public bool IsCrypto { get; set; } = false;
    }
}
