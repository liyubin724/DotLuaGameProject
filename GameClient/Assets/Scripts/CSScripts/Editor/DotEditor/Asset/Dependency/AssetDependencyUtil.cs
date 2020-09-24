using Boo.Lang;
using DotEditor.Utilities;
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

        public static void FindAllAssetData()
        {
            AllAssetDependencyData data = GetOrCreateAllAssetData();
            data.Clear();

            List<string> assetPaths = new List<string>();

            string assetDirectory = PathUtility.GetDiskPath("Assets");
            string[] allDirectoriesInAsset = Directory.GetDirectories(assetDirectory, "*", SearchOption.AllDirectories);

            EditorUtility.DisplayCancelableProgressBar("Search files", "searchFiles", 0.0f);
            for (int i = 0; i < allDirectoriesInAsset.Length; ++i)
            {
                string dir = allDirectoriesInAsset[i].Replace("\\", "/");
                if (dir.IndexOf("/Plugins") >= 0 || dir.IndexOf("/StreamingAssets") >= 0 || dir.IndexOf("/Editor") >= 0)
                {
                    continue;
                }
                if (EditorUtility.DisplayCancelableProgressBar("Search files", $"Search:{dir}", i / (float)allDirectoriesInAsset.Length))
                {
                    return;
                }

                (from file in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                 where Path.GetExtension(file).ToLower() != ".meta"
                 select PathUtility.GetAssetPath(file)
                                  ).ToList().ForEach((f) => { assetPaths.Add(f); });
            }
            EditorUtility.ClearProgressBar();

            if (assetPaths.Count > 0)
            {
                foreach (var assetPath in assetPaths)
                {
                    allAssetData.AddData(GetDependencyData(assetPath));
                }
            }
            EditorUtility.SetDirty(allAssetData);
        }

        public static AssetDependencyData[] GetAssetUsedBy(string assetPath)
        {
            AllAssetDependencyData allAssetData = GetOrCreateAllAssetData();
            return (from data in allAssetData.assetDatas
                    where ArrayUtility.IndexOf(data.allDepends, assetPath) >= 0 && data.assetPath != assetPath
                    select data
                                            ).ToArray();
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
