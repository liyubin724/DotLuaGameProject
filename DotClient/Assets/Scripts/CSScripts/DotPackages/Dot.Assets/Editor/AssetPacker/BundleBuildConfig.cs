using System;
using UnityEditor;

namespace DotEditor.Asset.AssetPacker
{
    public enum CompressOption
    {
        Uncompressed = 0,
        StandardCompression,
        ChunkBasedCompression,
    }

    public enum ValidBuildTarget
    {
        iOS = 9,
        Android = 13,
        StandaloneWindows64 = 19,
    }

    public enum BundlePathFormatType
    {
        Origin = 0,
        MD5,
    }

    [Serializable]
    public class BundleBuildConfig
    {
        public string OutputDir = null;
        public bool CleanupBeforeBuild = false;

        public BundlePathFormatType PathFormat = BundlePathFormatType.Origin;

        public ValidBuildTarget Target = ValidBuildTarget.StandaloneWindows64;
        public CompressOption Compression = CompressOption.StandardCompression;
        public BuildAssetBundleOptions BundleOptions = BuildAssetBundleOptions.DeterministicAssetBundle;

        public BuildTarget GetBuildTarget()
        {
            return (BuildTarget)Target;
        }

        public BuildAssetBundleOptions GetBundleOptions()
        {
            BuildAssetBundleOptions options = BundleOptions;
            if (Compression == CompressOption.Uncompressed)
            {
                return options | BuildAssetBundleOptions.UncompressedAssetBundle;
            }
            else if (Compression == CompressOption.ChunkBasedCompression)
            {
                return options | BuildAssetBundleOptions.ChunkBasedCompression;
            }
            else
            {
                return options;
            }
        }
    }
}
