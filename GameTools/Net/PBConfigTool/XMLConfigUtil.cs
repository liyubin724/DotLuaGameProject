using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DotTool.NetMessage
{
    public static class XMLConfigUtil
    {
        public static T ReadConfig<T>(string path, bool createIfNot = true) where T:class,new()
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

        public static void WriterConfig<T>(string path, T config) where T:class,new()
        {
            if(config == null)
            {
                config = new T();
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir)&&!Directory.Exists(dir)) 
            {
                Directory.CreateDirectory(dir);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                xmlSerializer.Serialize(sw, config);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
