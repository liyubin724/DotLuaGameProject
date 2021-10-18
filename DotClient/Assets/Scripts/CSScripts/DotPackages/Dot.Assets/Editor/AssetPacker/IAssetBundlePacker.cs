using DotEngine.Assets;

namespace DotEditor.Asset.AssetPacker
{
    public interface IAssetBundlePacker
    {
        BundleDetailConfig PackAssetBundle(AssetPackerConfig packerConfig, BundleBuildConfig buildConfig,string outputDir);
    }
}
