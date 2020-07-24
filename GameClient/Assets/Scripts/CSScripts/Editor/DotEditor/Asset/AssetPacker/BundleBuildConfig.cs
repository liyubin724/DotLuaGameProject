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
        public string bundleOutputDir = null;
        public bool cleanupBeforeBuild = false;

        public BundlePathFormatType pathFormat = BundlePathFormatType.Origin;

        public ValidBuildTarget buildTarget = ValidBuildTarget.StandaloneWindows64;
        public CompressOption compression = CompressOption.StandardCompression;
        public BuildAssetBundleOptions bundleOptions = BuildAssetBundleOptions.DeterministicAssetBundle;

        public BundleBuildConfig()
        {
        }

        public BuildTarget GetBuildTarget()
        {
            return (BuildTarget)buildTarget;
        }

        public BuildAssetBundleOptions GetBundleOptions()
        {
            BuildAssetBundleOptions options = bundleOptions;
            if (compression == CompressOption.Uncompressed)
            {
                return options | BuildAssetBundleOptions.UncompressedAssetBundle;
            }
            else if (compression == CompressOption.ChunkBasedCompression)
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
