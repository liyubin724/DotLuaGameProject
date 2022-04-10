using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DotTool.Net
{
    public static class XMLSerializeUtility
    {
        public static T ReadFromFile<T>(string path, bool createIfNot = true) where T : class, new()
        {
            T config = null;
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    config = (T)xmlSerializer.Deserialize(sr);
                    sr.Close();
                }
            }

            if (config == null && createIfNot)
            {
                config = new T();
            }

            return config;
        }

        public static void WriteToFile<T>(string path, T config) where T : class, new()
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("The path is empty");
            }

            if (config == null)
            {
                config = new T();
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                var dInfo = Directory.CreateDirectory(dir);
                if (dInfo == null || !dInfo.Exists)
                {
                    return;
                }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                XmlSerializerNamespaces spaces = new XmlSerializerNamespaces();
                spaces.Add(string.Empty, string.Empty);
                xmlSerializer.Serialize(sw, config, spaces);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
