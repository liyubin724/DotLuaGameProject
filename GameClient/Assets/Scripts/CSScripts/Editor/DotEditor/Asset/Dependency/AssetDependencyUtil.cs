using Boo.Lang;
using DotEditor.Utilities;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Dependency
{
    public static class AssetDependencyUtil
    {
        private static AllAssetDependencyData allAssetData = null;
        public static AllAssetDependencyData GetOrCreateAllAssetData()
        {
            if (allAssetData != null)
            {
                return allAssetData;
            }
            AllAssetDependencyData[] datas = AssetDatabaseUtility.FindInstances<AllAssetDependencyData>();
            if (datas != null && datas.Length > 0)
            {
                allAssetData = datas[0];
            }
            if (allAssetData == null)
            {
                allAssetData = ScriptableObject.CreateInstance<AllAssetDependencyData>();
            }

            return allAssetData;
        }

        public static void FindAllAssetData(Action<string, string, float> progressAction = null)
        {
            AllAssetDependencyData data = GetOrCreateAllAssetData();
            data.Clear();

            List<string> assetPaths = new List<string>();

            string assetDirectory = PathUtility.GetDiskPath("Assets");
            string[] allDirectoriesInAsset = Directory.GetDirectories(assetDirectory, "*", SearchOption.AllDirectories);
            for (int i = 0; i < allDirectoriesInAsset.Length; ++i)
            {
                string dir = allDirectoriesInAsset[i].Replace("\\", "/");
                if (dir.IndexOf("/Plugins") >= 0 || dir.IndexOf("/StreamingAssets") >= 0 || dir.IndexOf("/Editor") >= 0)
                {
                    continue;
                }

                progressAction?.Invoke("Search files", $"Search:{dir}", i / (float)allDirectoriesInAsset.Length);

                (from file in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                 where Path.GetExtension(file).ToLower() != ".meta"
                 select PathUtility.GetAssetPath(file)
                                  ).ToList().ForEach((f) => { assetPaths.Add(f); });
            }

            if (assetPaths.Count > 0)
            {
                for(int i =0;i<assetPaths.Count;++i)
                {
                    progressAction?.Invoke("Get Dependency", $"GetDependency:{assetPaths[i]}", i / (float)assetPaths.Count);
                    allAssetData.AddData(GetDependencyData(assetPaths[i]));
                }
            }
            EditorUtility.SetDirty(allAssetData);
        }

        public static AssetDependencyData[] GetAssetUsedBy(string assetPath, Action<string, string, float> progressAction = null)
        {
            AllAssetDependencyData allAssetData = GetOrCreateAllAssetData();

            List<AssetDependencyData> result = new List<AssetDependencyData>();
            for (int i =0;i<allAssetData.assetDatas.Count;++i)
            {
                AssetDependencyData data = allAssetData.assetDatas[i];

                progressAction?.Invoke("Get Asset Used By ", $"{data.assetPath}", i / (float)allAssetData.assetDatas.Count);

                if (ArrayUtility.IndexOf(data.allDepends, assetPath) >= 0 && data.assetPath != assetPath)
                {
                    result.Add(data);
                }
            }

            return result.ToArray();
        }

        public static AssetDependencyData GetDependencyData(string assetPath, string[] ignoreExt = null)
        {
            return new AssetDependencyData()
            {
                assetPath = assetPath,
                directlyDepends = AssetDatabaseUtility.GetDirectlyDependencies(assetPath, ignoreExt),
                allDepends = AssetDatabaseUtility.GetDependencies(assetPath, ignoreExt),
            };
        }
    }
}
