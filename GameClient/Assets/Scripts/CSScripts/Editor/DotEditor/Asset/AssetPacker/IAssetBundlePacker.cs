using DotEngine.Asset.Datas;

namespace DotEditor.Asset.AssetPacker
{
    public interface IAssetBundlePacker
    {
        AssetBundleConfig PackAssetBundle(AssetPackerConfig packerConfig, BundleBuildConfig buildConfig,string outputDir);
    }
}
