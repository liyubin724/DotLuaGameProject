using System;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace DotEditor.AAS.Reprocessor
{
    [Serializable]
    public class AtlasPackingSetting
    {
        public int padding = 2;
        public bool enableRotation = false;
        public bool enableTightPacking = false;
    }

    [Serializable]
    public class AltasTextureSetting
    {
        public bool readable = false;
        public bool sRGB = true;
        public bool generateMipMaps = false;
        public FilterMode filterMode = FilterMode.Bilinear;
    }

    [Serializable]
    public class AtlasPlatformSetting
    {
        public int maxSize = 1024;
        public TextureImporterFormat format = TextureImporterFormat.RGBA32;
    }

    [Serializable]
    [CustomReprocessMenu("SpriteAtlas Importer")]
    public class SpriteAtlasImporterReprocess : AAssetImporterReprocess
    {
        public AtlasPackingSetting packingSetting = new AtlasPackingSetting();
        public AltasTextureSetting textureSetting = new AltasTextureSetting();

        public AtlasPlatformSetting standaloneSetting = new AtlasPlatformSetting();
        public AtlasPlatformSetting androidSetting = new AtlasPlatformSetting();
        public AtlasPlatformSetting iOSSetting = new AtlasPlatformSetting();

        protected override bool IsValid(UnityEngine.Object uObj)
        {
            return typeof(SpriteAtlas).IsAssignableFrom(uObj.GetType());
        }

        protected override void SetImporter(AssetImporter importer)
        {
            SpriteAtlas packAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(importer.assetPath);

            SpriteAtlasTextureSettings sats = packAtlas.GetTextureSettings();
            sats.readable = textureSetting.readable;
            sats.sRGB = textureSetting.sRGB;
            sats.generateMipMaps = textureSetting.generateMipMaps;
            sats.filterMode = textureSetting.filterMode;
            packAtlas.SetTextureSettings(sats);

            SpriteAtlasPackingSettings saps = packAtlas.GetPackingSettings();
            saps.enableRotation = packingSetting.enableRotation;
            saps.padding = packingSetting.padding;
            saps.enableTightPacking = packingSetting.enableTightPacking;
            packAtlas.SetPackingSettings(saps);

            TextureImporterPlatformSettings tips = packAtlas.GetPlatformSettings("Standalone");
            tips.overridden = true;
            tips.maxTextureSize = standaloneSetting.maxSize;
            tips.format = standaloneSetting.format;
            packAtlas.SetPlatformSettings(tips);

            tips = packAtlas.GetPlatformSettings("Android");
            tips.overridden = true;
            tips.maxTextureSize = androidSetting.maxSize;
            tips.format = androidSetting.format;
            packAtlas.SetPlatformSettings(tips);

            tips = packAtlas.GetPlatformSettings("iPhone");
            tips.overridden = true;
            tips.maxTextureSize = iOSSetting.maxSize;
            tips.format = iOSSetting.format;
            packAtlas.SetPlatformSettings(tips);
        }
    }
}
