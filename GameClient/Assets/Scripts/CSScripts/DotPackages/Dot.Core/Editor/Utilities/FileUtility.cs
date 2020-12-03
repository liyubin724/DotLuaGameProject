using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotEditor.Utilities
{
    public static class FileUtility
    {
        /// <summary>
        /// 获取文件所在的所有目录的名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string[] GetFolderNames(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            filePath = filePath.Replace("\\", "/");
            var splitPath = filePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string[] result = new string[splitPath.Length - 1];
            Array.Copy(splitPath, 0, result, 0, splitPath.Length - 1);

            return result;
        }

        /// <summary>
        /// 获取文件所在的目录名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetParentFolderName(string filePath)
        {
            string[] folderNames = GetFolderNames(filePath);
            if(folderNames == null || folderNames.Length == 0)
            {
                return string.Empty;
            }

            return folderNames[folderNames.Length - 1];
        }

        /// <summary>
        /// 从指定的路径下读取序列化的二进制数据为指定的对象
        /// 
        /// example:
        /// [Serializable]
        /// public class TestData
        /// {
        ///     public string name;
        /// }
        /// 
        /// TestData data = FileUtil.ReadFromBinary<TestData>("D:/testdata.data");
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T ReadFromBinary<T>(string filePath) where T : class, new()
        {
            if(File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.OpenRead(filePath);

                var data = (T)bf.Deserialize(file);
                file.Close();

                if(data!=null)
                {
                    return data;
                }
            }

            return new T();
        }

        /// <summary>
        /// 将给定的数据序列化为二进制数据
        /// 
        /// example:
        /// [Serializable]
        /// public class TestData
        /// {
        ///     public string name;
        /// }
        /// 
        /// TestData data = new TestData();
        /// FileUtil.SaveToBinary<TestData>("D:/testdata.data",data);
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void SaveToBinary<T>(string filePath,T data) where T:class,new()
        {
            var dirPath = Path.GetDirectoryName(filePath).Replace("\\", "/");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            if (data == null)
            {
                data = new T();
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePath);

            bf.Serialize(file, data);
            file.Close();
        }
    }
}
