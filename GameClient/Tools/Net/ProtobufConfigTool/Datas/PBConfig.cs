using System;
using System.Xml.Serialization;

namespace DotTools.PB.Datas
{
    [Serializable]
    [XmlRoot("config")]
    public class PBConfig
    {
        [XmlAttribute("space")]
        public string Space { get; set; }
        [XmlElement("c2s")]
        public PBCategory C2S { get; set; } = new PBCategory();
        [XmlElement("s2c")]
        public PBCategory S2C { get; set; } = new PBCategory();
    }
}
