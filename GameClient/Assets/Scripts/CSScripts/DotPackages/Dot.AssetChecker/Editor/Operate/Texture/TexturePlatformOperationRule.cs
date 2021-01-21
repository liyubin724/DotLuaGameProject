using System;
using UnityEditor;

namespace DotEditor.AssetChecker
{
    public class TexturePlatformSetting : ICloneable
    {
        public int maxSize = 1024;
        public TextureImporterFormat format = TextureImporterFormat.RGBA32;

        public object Clone()
        {
            return new TexturePlatformSetting()
            {
                maxSize = maxSize,
                format = format,
            };
        }
    }

    [OperatationRule("Texture","Platform")]
    public class TexturePlatformOperationRule : ImportOperationRule
    {
        public TexturePlatformSetting standaloneSetting = new TexturePlatformSetting();
        public TexturePlatformSetting androidSetting = new TexturePlatformSetting();
        public TexturePlatformSetting iOSSetting = new TexturePlatformSetting();

        protected override void CloneTo(OperationRule rule)
        {
            TexturePlatformOperationRule tr = rule as TexturePlatformOperationRule;
            tr.standaloneSetting = (TexturePlatformSetting)standaloneSetting.Clone();
            tr.androidSetting = (TexturePlatformSetting)androidSetting.Clone();
            tr.iOSSetting = (TexturePlatformSetting)iOSSetting.Clone();
        }

        protected override void ImportAsset(AssetImporter importer)
        {
            TextureImporter ti = importer as TextureImporter;

            TextureImporterPlatformSettings tips = ti.GetPlatformTextureSettings("Standalone");
            tips.overridden = true; 
            tips.maxTextureSize = standaloneSetting.maxSize;
            tips.format = standaloneSetting.format;
            ti.SetPlatformTextureSettings(tips);

            tips = ti.GetPlatformTextureSettings("Android");
            tips.overridden = true;
            tips.maxTextureSize = androidSetting.maxSize;
            tips.format = androidSetting.format;
            ti.SetPlatformTextureSettings(tips);

            tips = ti.GetPlatformTextureSettings("iPhone");
            tips.overridden = true;
            tips.maxTextureSize = iOSSetting.maxSize;
            tips.format = iOSSetting.format;
            ti.SetPlatformTextureSettings(tips);
        }
    }
}
