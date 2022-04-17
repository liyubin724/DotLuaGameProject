using UnityEngine;

namespace DotEngine.Assets
{
    public static class AssetConst
    {
        public static string ASSET_DETAIL_CONFIG_NAME = "asset_detail_config";
        public static string BUNDLE_DETAIL_CONFIG_NAME = "bundle_detail_config";

        
        public static string GetAssetDetailConfigPathInEditor()
        {
            return $"Assets/{ASSET_DETAIL_CONFIG_NAME}.json";
        }

        public static string GetAssetDetailConfigFileName()
        {
            return $"{ASSET_DETAIL_CONFIG_NAME}.json";
        }

        public static string GetBundleDetailConfigFileName()
        {
            return $"{BUNDLE_DETAIL_CONFIG_NAME}.json";
        }

        public static string GetAssetDetailConfigFullPathForResource()
        {
            return $"{Application.dataPath}/{GetAssetDetailConfigFileName()}";
//#if UNITY_EDITOR
//#else
//            throw new Exception();
//#endif
        }

        public static string GetAssetDetailConfigFullPathForDatabase()
        {
            return $"{Application.dataPath}/{GetAssetDetailConfigFileName()}";
//#if UNITY_EDITOR
//#else
//            throw new Exception();
//#endif
        }

        public static string GetRootFullDirForBundle()
        {
#if UNITY_EDITOR
            return $"{Application.dataPath.Replace("Assets", "AssetBundles")}";
#elif UNITY_STANDALONE
            return $"{Application.dataPath}/AssetBundles";
#elif UNITY_IOS
            return $"{Application.dataPath}/AssetBundles";
#elif UNITY_ANDROID
            return $"{Application.dataPath}/AssetBundles";
#else
            throw new Exception();
#endif
        }

        public static string GetAssetDetailConfigFullPathForBundle()
        {
            return $"{GetRootFullDirForBundle()}/{GetAssetDetailConfigFileName()}";
        }

        public static string GetBundleDetailConfigFullPath()
        {
            return $"{GetRootFullDirForBundle()}/{GetBundleDetailConfigFileName()}";
        }
    }
}
