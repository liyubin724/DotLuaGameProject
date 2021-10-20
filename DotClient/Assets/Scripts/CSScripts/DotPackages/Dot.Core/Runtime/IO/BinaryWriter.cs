using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotEngine.Core.IO
{
    public static class BinaryWriter
    {
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
