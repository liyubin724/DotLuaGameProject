using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotEngine.Config.NDB
{
    public sealed class NDBManager
    {
        private static NDBManager mgr = null;

        public static NDBManager GetInstance()
        {
            if(mgr == null)
            {
                mgr = new NDBManager();
            }
            return mgr;
        }

        private Func<string, byte[]> bytesLoaderFunc = null;

        private string languagePath = null;
        private NDBSheet languageData = null;
        private Dictionary<string, NDBSheet> cachedDataDic = new Dictionary<string, NDBSheet>();

        private NDBManager() 
        {
            bytesLoaderFunc = GetBytesFromFile;
        }

        public void SetLoader(Func<string, byte[]> loader)
        {
            bytesLoaderFunc = loader;
        }

        public NDBSheet LoadLanguageData(string path)
        {
            languagePath = path;
            languageData = LoadDataInternal(path);
            foreach(var kvp in cachedDataDic)
            {
                kvp.Value.SetText(languageData);
            }
            return languageData;
        }

        public NDBSheet LoadData(string path)
        {
            if(cachedDataDic.TryGetValue(path, out var data))
            {
                return data;
            }

            data = LoadDataInternal(path);
            if (data == null)
            {
                throw new Exception("the data can't found");
            }
            cachedDataDic.Add(path, data);

            if(languageData!=null)
            {
                data.SetText(languageData);
            }
            return data;
        }

        private NDBSheet LoadDataInternal(string path)
        {
            byte[] fileBytes = bytesLoaderFunc(path);
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return null;
            }
            NDBSheet data = new NDBSheet(path);
            data.SetData(fileBytes);

            return data;
        }

        public void UnloadData(string path)
        {
            if(cachedDataDic.ContainsKey(path))
            {
                cachedDataDic.Remove(path);
            }
        }

        public void ReloadAllData()
        {
            string[] dataPaths = cachedDataDic.Keys.ToArray();
            cachedDataDic.Clear();
            if(string.IsNullOrEmpty(languagePath))
            {
                LoadLanguageData(languagePath);
            }
            foreach(var path in dataPaths)
            {
                LoadData(path);
            }
        }

        private byte[] GetBytesFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            byte[] fileBytes = File.ReadAllBytes(filePath);
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return null;
            }
            return fileBytes;
        }
    }
}
