using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotEngine.Core.IO
{
    public static class BinaryReader
    {
        public static T ReadFromFile<T>(string filePath) where T:class
        {
            T data = null;
            if (File.Exists(filePath))
            {
                FileStream fileStream = null;
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    fileStream = File.OpenRead(filePath);
                    data = (T)bf.Deserialize(fileStream);

                    if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
                    {
                        ((ISerialization)data).DoDeserialize();
                    }
                }
                catch
                {
                }
                finally
                {
                    fileStream?.Close();
                }
            }

            return data;
        }
    }
}
