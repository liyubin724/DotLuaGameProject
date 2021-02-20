using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.AAS
{
    public static class BundleConst
    {
        public static readonly string BUNDLE_DETAIL_FILE_NAME = "bundle_detail";
        public static readonly string BIN_FILE_EXT = ".bin";
        public static readonly string TXT_FILE_EXT = ".txt";

        public static readonly string BIN_BUNDLE_DETAIL_FILE = $"{BUNDLE_DETAIL_FILE_NAME}{BIN_FILE_EXT}";
        public static readonly string TXT_BUNDLE_DETAIL_FILE = $"{BUNDLE_DETAIL_FILE_NAME}{TXT_FILE_EXT}";

        public static readonly string BUNDLE_FOLDER = "AssetBundle";

        public static string GetBundleFolder()
        {
            return string.Empty;
        }

        public static string GetBundleFolderInEditor()
        {
            return $"{Application.dataPath.Replace("/Assets", "")}/AssetBundle";
        }
    }
}

