using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DotTools.PB.Datas
{
    public static class PBConfigUtility
    {
        public static PBConfig ReadFrom(string path,bool createIfNot = false)
        {
            PBConfig config = null;
            if(!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PBConfig));
                using(StreamReader sr = new StreamReader(path,Encoding.UTF8))
                {
                    config = xmlSerializer.Deserialize(sr) as PBConfig;
                    sr.Close();
                }
            }

            if(config == null && createIfNot)
            {
                config = new PBConfig();
            }

            return config;
        }

        public static void WriteTo(string path,PBConfig config)
        {
            if(string.IsNullOrEmpty(path))
            {
                return;
            }
            if(File.Exists(path))
            {
                File.Delete(path);
            }
            string dir = Path.GetDirectoryName(path);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PBConfig));
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                xmlSerializer.Serialize(sw,config);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
