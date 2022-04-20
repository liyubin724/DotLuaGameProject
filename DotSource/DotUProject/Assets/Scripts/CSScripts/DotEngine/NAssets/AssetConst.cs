using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotEngine.NAssets
{
    internal static class AssetConst
    {
        public static string ASSET_CONFIG_NAME = "asset_config";
        public static string BUNDLE_CONFIG_NAME = "bundle_config";

        public static string GetAssetConfigPath()
        {
#if UNITY_EDITOR
            return $"{Application.dataPath}/{ASSET_CONFIG_NAME}.json";
#else
            return $"{Application.persistentDataPath}/{ASSET_CONFIG_NAME}.json";
#endif
        }

        public static string GetBundleConfigPath()
        {
#if UNITY_EDITOR
            return $"{Application.dataPath}/{BUNDLE_CONFIG_NAME}.json";
#else
            return $"{Application.persistentDataPath}/{BUNDLE_CONFIG_NAME}.json";
#endif
        }

        public static string GetBundlePath()
        {
#if UNITY_EDITOR
            return Application.dataPath.Replace("Assets", "AssetBundles");
#else
            return $"{Application.persistentDataPath}/AssetBundles";
#endif
        }
    }
}
