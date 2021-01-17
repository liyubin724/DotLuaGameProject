using UnityEditor;
using UnityEngine;

namespace DotEditor.AssetChecker
{
    public class TexturePropertyOperationRule : ImportOperationRule
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

        protected override void CloneTo(OperationRule rule)
        {
            TexturePropertyOperationRule tr = rule as TexturePropertyOperationRule;
            tr.textureType = textureType;
            tr.textureShape = textureShape;

            tr.sRGBTexture = sRGBTexture;
            tr.alphaSource = alphaSource;
            tr.alphaIsTransparency = alphaIsTransparency;

            tr.npotScale = npotScale;
            tr.isReadable = isReadable;
            tr.mipmapEnabled = mipmapEnabled;

            tr.wrapMode = wrapMode;
            tr.filterMode = filterMode;
            tr.anisoLevel = anisoLevel;
        }

        protected override void ImportAsset(AssetImporter importer)
        {
            TextureImporter ti = importer as TextureImporter;
            ti.textureType = textureType;
            ti.textureShape = textureShape;
            
            ti.sRGBTexture = sRGBTexture;
            ti.alphaSource = alphaSource;
            ti.alphaIsTransparency = alphaIsTransparency;

            ti.npotScale = npotScale;
            ti.isReadable = isReadable;
            ti.mipmapEnabled = mipmapEnabled;
            ti.streamingMipmaps = false;
            ti.borderMipmap = true;
            ti.mipmapFilter = TextureImporterMipFilter.BoxFilter;
            ti.mipMapsPreserveCoverage = false;
            ti.fadeout = false;

            ti.wrapMode = wrapMode;
            ti.filterMode = filterMode;
            ti.anisoLevel = anisoLevel;
        }
    }
}
