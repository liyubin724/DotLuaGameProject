using DotEditor.Utilities;
using DotEngine.Assets;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Group
{
    public static class GroupMenuItems
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
            AssetGroupCreater[] groups = AssetDatabaseUtility.FindInstances<AssetGroupCreater>();
            if (groups != null && groups.Length > 0)
            {
                AssetDetailConfig config = new AssetDetailConfig();

                List<AssetDetail> details = new List<AssetDetail>();
                foreach(var group in groups)
                {
                    AssetResult[] results = group.GetResults();
                    foreach (var result in results)
                    {
                        if(!result.IsMainAsset)
                        {
                            continue;
                        }

                        AssetDetail detail = new AssetDetail()
                        {
                            Address = result.Address,
                            Path = result.Path,
                            Bundle = result.Bundle,
                            IsScene = result.IsScene,
                            Labels = result.Labels
                        };
                        details.Add(detail);
                    }
                }
                config.Details = details.ToArray();
                string assetFilePath = AssetConst.GetAssetDetailConfigPathInProject();
                
                AssetDetailConfig.WriteToFile(config, PathUtility.GetDiskPath(assetFilePath));
                AssetDatabase.ImportAsset(assetFilePath);
            }
        }
    }
}
