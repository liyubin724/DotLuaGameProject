using DotEditor.Utilities;
using DotEngine.Core.IO;
using DotEngine.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DotEditor.Assets.Dependency
{
    public static class AssetDependencyUtil
    {
        private static string ASSET_DEPENDENCY_CONFIG_NAME = "asset_dependency_config.json";
        private static string[] IngoreFolders = new string[]
        {
            "/Plugins",
            "/StreamingAssets",
            "/Editor"
        };
        private static string[] IngoreFileExts = new string[]
        {
            ".txt",
            ".lua",
            ".temp",
            ".tmp",
            ".cs",
            ".dll",
            ".meta",
            ".bak"
        };
        private static string GetConfigFilePath()
        {
            string dataPath = Application.dataPath;
            return dataPath.Substring(0, dataPath.IndexOf("Assets")) + "AssetConfig/" + ASSET_DEPENDENCY_CONFIG_NAME;
        }

        public static AssetDependencyConfig GetAssetDependencyConfig()
        {
            string filePath = GetConfigFilePath();
            AssetDependencyConfig dependencyConfig;
            if(File.Exists(filePath))
            {
                dependencyConfig = JSONReader.ReadFromFile<AssetDependencyConfig>(filePath);
            }else
            {
                dependencyConfig = new AssetDependencyConfig();
            }
            
            return dependencyConfig;
        }

        public static AssetDependencyConfig FindAllAssetData()
        {
            List<string> assetPaths = new List<string>();
            string assetDirectory = PathUtility.GetDiskPath("Assets");
            List<string> assetSubdirectories = new List<string>() { assetDirectory};
            while(assetSubdirectories.Count>0)
            {
                string dir = assetSubdirectories[0].Replace("\\","/");
                assetSubdirectories.RemoveAt(0);

                var isIngore = (from folder in IngoreFolders where dir.IndexOf(folder) >= 0 select folder).Any();
                if(isIngore)
                {
                    continue;
                }

                (from file in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                 let ext = Path.GetExtension(file).ToLower()
                 where Array.IndexOf(IngoreFileExts,ext) < 0
                 select PathUtility.GetAssetPath(file)
                                  ).ToList().ForEach((f) => { assetPaths.Add(f); });

                string[] subdirectories = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
                if(subdirectories!=null && subdirectories.Length>0)
                {
                    assetSubdirectories.AddRange(subdirectories);
                }
            }

            AssetDependencyConfig dependencyConfig = new AssetDependencyConfig();
            if (assetPaths.Count > 0)
            {
                for(int i =0;i<assetPaths.Count;++i)
                {
                    AssetDependency dependency = new AssetDependency()
                    {
                        assetPath = assetPaths[i],
                        directlyDepends = AssetDatabaseUtility.GetDirectlyDependencies(assetPaths[i], IngoreFileExts),
                        allDepends = AssetDatabaseUtility.GetDependencies(assetPaths[i], IngoreFileExts),
                    };

                    dependencyConfig.AddData(dependency);
                }
            }

            string filePath = GetConfigFilePath();
            JSONWriter.WriteToFile(dependencyConfig, filePath);

            return dependencyConfig;
        }

    }
}
