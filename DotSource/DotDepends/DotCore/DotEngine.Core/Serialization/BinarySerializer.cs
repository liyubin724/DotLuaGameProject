using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotEngine.Core.Serialization
{
    public static class BinarySerializer
    {
        public static T ReadFromFile<T>(string filePath) where T : class
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

        public static void WriteToFile<T>(string filePath, T data) where T : class
        {
            if (data == null)
            {
                return;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var dirPath = Path.GetDirectoryName(filePath).Replace("\\", "/");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            FileStream fileStream = null;
            try
            {
                if (typeof(ISerialization).IsAssignableFrom(typeof(T)))
                {
                    ((ISerialization)data).DoSerialize();
                }

                BinaryFormatter bf = new BinaryFormatter();
                fileStream = File.Create(filePath);
                bf.Serialize(fileStream, data);
            }
            catch
            {
            }
            finally
            {
                fileStream?.Close();
            }
        }
    }
}
