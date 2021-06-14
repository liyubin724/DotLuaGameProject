using System;
using System.Collections.Generic;
using System.IO;

namespace DotEngine.NDB
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

        private NDBData languageData = null;
        private Dictionary<string, NDBData> cachedDataDic = new Dictionary<string, NDBData>();

        private Func<string, byte[]> bytesLoaderFunc = null;
        private Func<string, string> textPathGetterFunc = null;

        private NDBManager() 
        {
            bytesLoaderFunc = GetBytesFromFile;
            textPathGetterFunc = GetTextFilePath;
        }

        public void SetLoader(Func<string, byte[]> loader, Func<string,string> textPathGetter)
        {
            bytesLoaderFunc = loader;
            textPathGetterFunc = textPathGetter;
        }

        public void SetLanguageData(string path)
        {
            languageData = LoadDataInternal(path);
            foreach(var kvp in cachedDataDic)
            {
                kvp.Value.SetText(languageData);
            }
        }

        public NDBData LoadData(string path)
        {
            if(cachedDataDic.TryGetValue(path, out var data))
            {
                return data;
            }

            data = LoadDataInternal(path);
            if (data == null)
            {
                throw new Exception();
            }

            if(languageData!=null)
            {
                data.SetText(languageData);
            }else
            {
                string textFilePath = textPathGetterFunc(path);
                NDBData textData = LoadDataInternal(textFilePath);
                if(textData!=null)
                {
                    data.SetText(textData);
                }
            }
            
            return data;
        }

        private NDBData LoadDataInternal(string path)
        {
            byte[] fileBytes = bytesLoaderFunc(path);
            if (fileBytes == null || fileBytes.Length == 0)
            {
                return null;
            }
            NDBData data = new NDBData(path);
            data.SetData(fileBytes);

            return data;
        }

        public void ReloadAllDatas()
        {
            if(languageData!=null)
            {
                SetLanguageData(languageData.Name);
            }

            var pathes = cachedDataDic.Keys;
            cachedDataDic.Clear();
            foreach(var path in pathes)
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

        private string GetTextFilePath(string filePath)
        {
            return filePath.Replace(NDBConst.NDB_DATA_FILE_EXTERSION, $"{NDBConst.NDB_TEXT_APPEND_NAME}{NDBConst.NDB_TEXT_FILE_EXTERSION}");
        }
    }
}
