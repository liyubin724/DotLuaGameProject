using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DotTool.NetMessage
{
    [Serializable]
    public class Message
    {
        [XmlAttribute("enable")]
        public bool Enable { get; set; } = true;

        [XmlAttribute("id")]
        public int Id { get; set; } = 0;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("comment")]
        public string Comment { get; set; } = string.Empty;

        [XmlAttribute("class_name")]
        public string ClassName { get; set; } = string.Empty;

        [XmlAttribute("is_compress")]
        public bool IsCompress { get; set; } = false;

        [XmlAttribute("is_crypto")]
        public bool IsCrypto { get; set; } = false;
    }

    [Serializable]
    public class MessageGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("message")]
        public List<Message> Messages { get; set; }
    }

    [Serializable]
    [XmlRoot("configs")]
    public class MessageConfig
    {
        [XmlAttribute("namespace")]
        public string Space { get; set; } = string.Empty;

        [XmlElement("c2s")]
        public MessageGroup C2SGroup { get; set; } = new MessageGroup();

        [XmlElement("s2c")]
        public MessageGroup S2CGroup { get; set; } = new MessageGroup();
    }

    public static class MessageConfigUtil
    {
        public static MessageConfig ReadConfig(string path, bool createIfNot = false)
        {
            MessageConfig config = null;
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MessageConfig));
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    config = xmlSerializer.Deserialize(sr) as MessageConfig;
                    sr.Close();
                }
            }

            if (config == null && createIfNot)
            {
                config = new MessageConfig();
            }
            return config;
        }

        public static void WriterConfig(string path, MessageConfig config)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MessageConfig));
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                xmlSerializer.Serialize(sw, config);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
