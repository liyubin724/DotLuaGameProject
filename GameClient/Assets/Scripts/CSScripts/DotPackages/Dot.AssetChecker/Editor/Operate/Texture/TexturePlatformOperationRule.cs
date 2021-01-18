using UnityEditor;

namespace DotEditor.AssetChecker
{
    public enum AssetPlatformType
    {
        Window = 0,
        Android,
        iOS,
    }

    [OperatationRule("Texture","Platform")]
    public class TexturePlatformOperationRule : ImportOperationRule
    {
        public AssetPlatformType platform = AssetPlatformType.Window;
        public int maxSize = 1024;
        public TextureImporterFormat format = TextureImporterFormat.RGBA32;

        private string GetPlatformName()
        {
            if(platform == AssetPlatformType.Android)
            {
                return "Android";
            }else if(platform == AssetPlatformType.iOS)
            {
                return "iPhone";
            }else if(platform == AssetPlatformType.Window)
            {
                return "Window";
            }
            return string.Empty;
        }

        protected override void CloneTo(OperationRule rule)
        {
            TexturePlatformOperationRule tr = rule as TexturePlatformOperationRule;
            tr.platform = platform;
            tr.maxSize = maxSize;
            tr.format = format;
        }

        protected override void ImportAsset(AssetImporter importer)
        {
            TextureImporter ti = importer as TextureImporter;
            TextureImporterPlatformSettings tips = ti.GetPlatformTextureSettings(GetPlatformName());

            tips.overridden = true; 
            tips.maxTextureSize = maxSize;
            tips.format = format;

            ti.SetPlatformTextureSettings(tips);
        }
    }
}
