using DotEditor.FBX;
using DotEngine.Context;

namespace DotEditor.Asset.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Extract Clip", "apr_extract_clip")]
    public class ExtractClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";

        public override void Execute(StringContext context, string assetPath)
        {
            AnimationClipExtract.ExtractClipFromFBX(assetPath, TargetFolder);
        }
    }
}
