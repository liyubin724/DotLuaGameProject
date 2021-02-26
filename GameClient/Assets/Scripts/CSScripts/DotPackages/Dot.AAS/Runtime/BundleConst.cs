using UnityEngine;

namespace DotEngine.AAS
{
    public static class BundleConst
    {
        public static readonly string ASSET_CONFIG_NAME = "asset_config";
        public static readonly string BUNDLE_CONFIG_NAME = "bundle_config";

        public static readonly string TXT_FILE_EXT = ".txt";

        public static readonly string ASSET_CONFIG_FILE_NAME = $"{ASSET_CONFIG_NAME}{TXT_FILE_EXT}";
        public static readonly string BUNDLE_CONFIG_FILE_NAME = $"{BUNDLE_CONFIG_NAME}{TXT_FILE_EXT}";

        public static readonly string ROOT_FOLDER_NAME = "AssetBundle";

        private static string rootPath = null;
        public static string RootPath
        {
            get
            {
                if(string.IsNullOrEmpty(rootPath))
                {
                    rootPath = $"{Application.dataPath.Replace("/Assets", "/AAS")}/${ROOT_FOLDER_NAME}";
                }
                return rootPath;
            }
            set
            {
                rootPath = value;
            }
        }

        public static string AssetConfigPath
        {
            get
            {
                return $"{RootPath}/{ASSET_CONFIG_FILE_NAME}";
            }
        }

        public static string BundleConfigPath
        {
            get
            {
                return $"{RootPath}/{BUNDLE_CONFIG_FILE_NAME}";
            }
        }

    }
}

