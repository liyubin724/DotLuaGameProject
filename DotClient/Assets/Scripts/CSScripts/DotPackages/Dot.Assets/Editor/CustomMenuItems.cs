﻿using DotEditor.Assets.Group;
using DotEditor.Assets.Packer;
using DotEditor.Utilities;
using DotEngine.Assets;
using DotEngine.Core.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Assets
{
    public static class CustomMenuItems
    {
        [MenuItem("Game/Asset/Create Address Group", priority = 0)]
        [MenuItem("Assets/Create/Asset/Create Address Group", priority = 0)]
        public static void CreateGroupAsset()
        {
            string dirPath = SelectionUtility.GetSelectionDir();
            if(!string.IsNullOrEmpty(dirPath))
            {
                string filePath = $"{dirPath}/asset_group.asset";
                filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
                var config = ScriptableObject.CreateInstance<AssetGroupCreater>();
                config.RootFolder = dirPath;
                EditorUtility.SetDirty(config);

                AssetDatabase.CreateAsset(config, filePath);
                AssetDatabase.ImportAsset(filePath);
            }
            else
            {
                Debug.LogError("AssetMenuItems::CreateGroupAsset->The dir is not found");
            }
        }

        [MenuItem("Game/Asset/Build Asset Detail Config", priority = 1)]
        [MenuItem("Assets/Create/Asset/Build Asset Detail Config", priority = 1)]
        public static void BuildAssetDetailConfig()
        {
            PackerData packerData = AssetPackerUtil.GetPackerData();
            AssetDetailConfig detailConfig = AssetPackerUtil.CreateAssetDetailConfig(packerData);

            string assetFilePath = AssetConst.GetAssetDetailConfigPathInProject();
            string diskFilePath = PathUtility.GetDiskPath(assetFilePath);
            
            JSONWriter.WriteToFile<AssetDetailConfig>(detailConfig, diskFilePath);
            AssetDatabase.ImportAsset(assetFilePath);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnRuntimeInitializeOnLoad()
        {
            BuildAssetDetailConfig();
        }
    }
}
