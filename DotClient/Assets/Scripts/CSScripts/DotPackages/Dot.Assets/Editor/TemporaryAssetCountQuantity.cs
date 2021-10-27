using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace DotEditor.Assets
{
    [Serializable]
    public class AssetCountQuantityInfo
    {
        public string bundleName;
        public List<string> assetsInBundle = new List<string>();
    }

    public class AssetCountComparedInfo
    {
        public string bundleName;
        public List<string> addAssets = new List<string>();
        public List<string> deleteAssets = new List<string>();
    }

    public static class TemporaryAssetCountQuantity
    {
        [MenuItem("Game/Asset/Temp/Asset Count Quantity")]
        public static void TACQChecker()
        {
            List<AssetCountQuantityInfo> assetList = new List<AssetCountQuantityInfo>();

            AssetDatabase.RemoveUnusedAssetBundleNames();
            string[] abNames = AssetDatabase.GetAllAssetBundleNames();
            if (abNames != null && abNames.Length > 0)
            {
                for (int i = 0; i < abNames.Length; ++i)
                {
                    AssetCountQuantityInfo info = new AssetCountQuantityInfo();
                    info.bundleName = abNames[i];

                    string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(abNames[i]);
                    if (assetPaths != null && assetPaths.Length > 0)
                    {
                        info.assetsInBundle.AddRange(assetPaths);
                    }

                    assetList.Add(info);
                }
            }

            string jsonStr = JsonConvert.SerializeObject(assetList, Formatting.Indented);
            string logFilePath = $"D:/acq_{DateTime.Now.ToString("yyyy-MM-dd")}.log";
            File.WriteAllText(logFilePath, jsonStr);
        }

        [MenuItem("Game/Asset/Temp/Asset Count Compare")]
        public static void TACCChecker()
        {
            string leftFilePath = $"";
            string rightFilePath = $"";

            List<AssetCountQuantityInfo> leftInfos = ReadFromFile(leftFilePath);
            List<AssetCountQuantityInfo> rightInfos = ReadFromFile(rightFilePath);

            List<string> leftABNames = (from data in leftInfos select data.bundleName).ToList();
            List<string> rightABNames = (from data in rightInfos select data.bundleName).ToList();

            List<string> addedABNames = rightABNames.Intersect(leftABNames).ToList();
            List<string> deleteABNames = leftABNames.Intersect(rightABNames).ToList();

            List<AssetCountComparedInfo> changedCompInfos = new List<AssetCountComparedInfo>();
            foreach (var rightInfo in rightInfos)
            {
                AssetCountComparedInfo compInfo = new AssetCountComparedInfo();
                compInfo.bundleName = rightInfo.bundleName;

                AssetCountQuantityInfo leftInfo = null;
                foreach (var lInfo in leftInfos)
                {
                    if (lInfo.bundleName == rightInfo.bundleName)
                    {
                        leftInfo = lInfo;
                        break;
                    }
                }

                if(leftInfo != null)
                {
                    compInfo.addAssets = rightInfo.assetsInBundle.Intersect(leftInfo.assetsInBundle).ToList();
                    compInfo.deleteAssets = leftInfo.assetsInBundle.Intersect(rightInfo.assetsInBundle).ToList();
                }
                if(compInfo.addAssets.Count>0 || compInfo.deleteAssets.Count>0)
                {
                    changedCompInfos.Add(compInfo);
                }
            }

            List<AssetCountComparedInfo> addedCompInfos = new List<AssetCountComparedInfo>();
            if(addedABNames.Count>0)
            {
                AssetCountQuantityInfo[] addInfos = (from data in rightInfos where addedABNames.IndexOf(data.bundleName) >= 0 select data).ToArray();
                foreach (var info in addInfos)
                {
                    AssetCountComparedInfo compInfo = new AssetCountComparedInfo();
                    compInfo.bundleName = info.bundleName;
                    compInfo.deleteAssets.AddRange(info.assetsInBundle);

                    addedCompInfos.Add(compInfo);
                }
            }

            List<AssetCountComparedInfo> deletedCompInfos = new List<AssetCountComparedInfo>();
            if(deleteABNames.Count>0)
            {
                AssetCountQuantityInfo[] deleteInfos = (from data in leftInfos where deleteABNames.IndexOf(data.bundleName) >= 0 select data).ToArray();
                foreach(var info in deleteInfos)
                {
                    AssetCountComparedInfo compInfo = new AssetCountComparedInfo();
                    compInfo.bundleName = info.bundleName;
                    compInfo.deleteAssets.AddRange(info.assetsInBundle);

                    deletedCompInfos.Add(compInfo);
                }
            }

            string logFilePath = $"D:/acc_{DateTime.Now.ToString("yyyy-MM-dd")}.log";
            using (StreamWriter sw = new StreamWriter(new FileStream(logFilePath, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine("删除的AssetBundle：" + deleteABNames.Count);
                foreach(var name in deleteABNames)
                {
                    sw.WriteLine("    [-]    " + name);
                }
                sw.WriteLine("新增的AssetBundle：" + addedABNames.Count);
                foreach (var name in addedABNames)
                {
                    sw.WriteLine("    [+]    " + name);
                }
                sw.WriteLine("改变的AssetBundle：" + changedCompInfos.Count);
                foreach(var info in changedCompInfos)
                {
                    sw.WriteLine("    [*]    " + info);
                }
                sw.WriteLine();
                sw.WriteLine();

                sw.WriteLine("详细记录：");
                foreach(var info in deletedCompInfos)
                {
                    sw.WriteLine("    [-]    " + info.bundleName);
                    foreach(var name in info.deleteAssets)
                    {
                        sw.WriteLine("        [-]    " + name);
                    }
                }
                foreach (var info in addedCompInfos)
                {
                    sw.WriteLine("    [+]    " + info.bundleName);
                    foreach (var name in info.addAssets)
                    {
                        sw.WriteLine("        [+]    " + name);
                    }
                }
                foreach (var info in changedCompInfos)
                {
                    sw.WriteLine($"    [{(info.addAssets.Count > 0 ? "+" : "")}{(info.deleteAssets.Count > 0 ? "-" : "")}]    {info.bundleName}");
                    foreach (var name in info.addAssets)
                    {
                        sw.WriteLine("        [+]    " + name);
                    }
                    foreach (var name in info.deleteAssets)
                    {
                        sw.WriteLine("        [-]    " + name);
                    }
                }
                sw.Flush();
                sw.Close();
            }
        }

        private static List<AssetCountQuantityInfo> ReadFromFile(string filePath)
        {
            if(File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                List<AssetCountQuantityInfo> info = JsonConvert.DeserializeObject<List<AssetCountQuantityInfo>>(content);
                return info;
            }
            return new List<AssetCountQuantityInfo>();
        }
    }
}
