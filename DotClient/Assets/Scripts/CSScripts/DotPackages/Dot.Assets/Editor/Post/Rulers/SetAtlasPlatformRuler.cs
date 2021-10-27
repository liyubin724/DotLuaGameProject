using DotEngine.Context.Attributes;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine.U2D;

namespace DotEditor.Assets.Post.Rulers
{
    [AssetPostRulerMenu("Sprite Atlas/Set Platform", "apr_set_atlas_platform")]
    public class SetAtlasPlatformRuler : AssetPostRuler
    {
        public string Platform;
        public int MaxSize = 2048;
        public int Format = 5;

        [ContextIE(AssetPostContextKeys.OPERATE_ATLAS_RESULT_KEY, ContextUsage.Inject)]
        private SpriteAtlas atlas = null;

        public override void Execute()
        {
            if(atlas!=null)
            {
                SetPlatformSetting(atlas);
            }else
            {
                foreach(var assetPath in assetPaths)
                {
                    SpriteAtlas sa = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
                    if(sa!=null)
                    {
                        SetPlatformSetting(sa);
                    }
                }
            }
        }

        private void SetPlatformSetting(SpriteAtlas spriteAtlas)
        {
            TextureImporterPlatformSettings tips = spriteAtlas.GetPlatformSettings(Platform);
            tips.overridden = true;
            tips.maxTextureSize = MaxSize;
            tips.format = (TextureImporterFormat)(Format);
            spriteAtlas.SetPlatformSettings(tips);
        }

    }
}
