using DotEditor.Asset.Packer;
using DotEditor.Utilities;
using DotEngine.Assets;
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
            PackerData packerData = AssetPackerUtil.GetPackerData();
            AssetDetailConfig detailConfig = AssetPackerUtil.CreateAssetDetailConfig(packerData);

            string assetFilePath = AssetConst.GetAssetDetailConfigPathInProject();
            string diskFilePath = PathUtility.GetDiskPath(assetFilePath);
            AssetDetailConfig.WriteToFile(detailConfig, diskFilePath);
            AssetDatabase.ImportAsset(assetFilePath);
        }
    }
}
