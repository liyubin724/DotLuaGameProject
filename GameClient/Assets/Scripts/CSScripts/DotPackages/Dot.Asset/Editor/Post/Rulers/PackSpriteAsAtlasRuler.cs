using DotEngine.Context;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using static DotEditor.Asset.Post.AssetPostProcess;

namespace DotEditor.Asset.Post.Rulers
{
    [AssetPostRulerMenu("Sprite Atlas/PackSpriteAsAtlas", "apr_pack_as_atlas")]
    public class PackSpriteAsAtlasRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";
        public string AtlasName = string.Empty;

        public bool AllowRotation = false;
        public bool TightPacking = false;
        public int Padding = 2;

        [ContextField(AssetPostContextKeys.ASSET_FILTER_KEY, ContextUsage.In, true)]
        private AssetPostFilter filter = null;
        [ContextField(AssetPostContextKeys.OPERATE_ATLAS_RESULT_KEY,ContextUsage.Out)]
        private SpriteAtlas atlas = null;

        public override void Execute()
        {
            atlas = null;

            if (assetPaths!=null && assetPaths.Length>0)
            {
                string atlasName = AtlasName;
                if(string.IsNullOrEmpty(atlasName))
                {
                    atlasName = Path.GetFileName(filter.Folder);
                }

                atlasName = atlasName.ToLower();
                string atlasAssetPath = $"{TargetFolder}/{atlasName}_atlas.spriteatlas";

                SpriteAtlas packAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasAssetPath);
                if (packAtlas == null)
                {
                    packAtlas = new SpriteAtlas();
                    AssetDatabase.CreateAsset(packAtlas, atlasAssetPath);
                }

                Sprite[] sprites = (from assetPath in assetPaths select AssetDatabase.LoadAssetAtPath<Sprite>(assetPath)).ToArray();

                packAtlas.Remove(packAtlas.GetPackables());
                packAtlas.Add(sprites);

                SpriteAtlasTextureSettings sats = packAtlas.GetTextureSettings();
                sats.readable = false;
                sats.sRGB = true;
                sats.generateMipMaps = false;
                sats.filterMode = FilterMode.Bilinear;
                packAtlas.SetTextureSettings(sats);

                SpriteAtlasPackingSettings saps = packAtlas.GetPackingSettings();
                saps.enableRotation = AllowRotation;
                saps.padding = Padding;
                saps.enableTightPacking = TightPacking;
                packAtlas.SetPackingSettings(saps);

                atlas = packAtlas;
            }
        }
    }
}
