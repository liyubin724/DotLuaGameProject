using UnityEditor;
using UnityEngine;
using System;

namespace DotEditor.AAS.Reprocessor
{
    [Serializable]
    public class TexturePlatformSetting
    {
        public int maxSize = 1024;
        public TextureImporterFormat format = TextureImporterFormat.RGBA32;
    }

    [Serializable]
    public class TexturePropertySetting
    {
        public TextureImporterType textureType = TextureImporterType.Default;
        public TextureImporterShape textureShape = TextureImporterShape.Texture2D;

        public bool sRGBTexture = true;
        public TextureImporterAlphaSource alphaSource = TextureImporterAlphaSource.None;
        public bool alphaIsTransparency = true;

        public TextureImporterNPOTScale npotScale = TextureImporterNPOTScale.None;
        public bool isReadable = false;
        public bool mipmapEnabled = false;

        public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
        public FilterMode filterMode = FilterMode.Bilinear;
        public int anisoLevel = 0;
    }

    [Serializable]
    [CustomReprocessMenu("Texture Importer")]
    public class TextureImporterReprocess : AAssetImporterReprocess
    {
        public TexturePropertySetting propertySetting = new TexturePropertySetting();

        public TexturePlatformSetting standaloneSetting = new TexturePlatformSetting();
        public TexturePlatformSetting androidSetting = new TexturePlatformSetting();
        public TexturePlatformSetting iOSSetting = new TexturePlatformSetting();

        protected override bool IsValid(UnityEngine.Object uObj)
        {
            return uObj != null && typeof(Texture).IsAssignableFrom(uObj.GetType());
        }

        protected override void SetImporter(AssetImporter importer)
        {
            TextureImporter ti = importer as TextureImporter;
            SetPropertySetting(ti);
            SetPlatformSetting(ti);
        }

        private void SetPropertySetting(TextureImporter ti)
        {
            ti.textureType = propertySetting.textureType;
            ti.textureShape = propertySetting.textureShape;

            ti.sRGBTexture = propertySetting.sRGBTexture;
            ti.alphaSource = propertySetting.alphaSource;
            ti.alphaIsTransparency = propertySetting.alphaIsTransparency;

            ti.npotScale = propertySetting.npotScale;
            ti.isReadable = propertySetting.isReadable;
            ti.mipmapEnabled = propertySetting.mipmapEnabled;
            ti.streamingMipmaps = false;
            ti.borderMipmap = true;
            ti.mipmapFilter = TextureImporterMipFilter.BoxFilter;
            ti.mipMapsPreserveCoverage = false;
            ti.fadeout = false;

            ti.wrapMode = propertySetting.wrapMode;
            ti.filterMode = propertySetting.filterMode;
            ti.anisoLevel = propertySetting.anisoLevel;
        }

        private void SetPlatformSetting(TextureImporter ti)
        {
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
