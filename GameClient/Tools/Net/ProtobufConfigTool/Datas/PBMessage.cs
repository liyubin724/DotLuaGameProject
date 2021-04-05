using System;
using System.Xml.Serialization;

namespace DotTools.PB.Datas
{
    [Serializable]
    public class PBMessage
    {
        [XmlAttribute("enable")]
        public bool Enable { get; set; } = true;
        [XmlAttribute("id")]
        public int ID { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("comment")]
        public string Comment { get; set; }
        [XmlAttribute("is_compressed")]
        public bool IsCompressed { get; set; } = false;
        [XmlAttribute("is_encrypted")]
        public bool IsEncrypted { get; set; } = false;
    }
}
