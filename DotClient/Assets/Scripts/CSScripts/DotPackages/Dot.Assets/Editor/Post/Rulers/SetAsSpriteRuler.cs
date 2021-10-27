using UnityEditor;
using UnityEngine;

namespace DotEditor.Assets.Post.Rulers
{
    [AssetPostRulerMenu("Sprite Atlas/SetAsSprite", "apr_set_as_sprite")]
    public class SetAsSpriteRuler : AssetPostRuler
    {
        public override void Execute()
        {
            foreach(var assetPath in assetPaths)
            {
                TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(assetPath);
                
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePackingTag = "";
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                importer.alphaIsTransparency = true;
                importer.sRGBTexture = true;
                importer.isReadable = false;
                importer.mipmapEnabled = false;

                TextureImporterSettings tis = new TextureImporterSettings();
                importer.ReadTextureSettings(tis);

                tis.spriteMeshType = SpriteMeshType.FullRect;
                tis.spritePixelsPerUnit = 100;
                
                importer.SetTextureSettings(tis);

                importer.SaveAndReimport();
            }
        }
    }
}
