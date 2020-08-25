using DotEditor.FBX;
using DotEngine.Context;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [CreateAssetMenu(menuName = "Asset Post/Ruler/Extract Clip", fileName = "apr_extract_clip")]
    public class ExtractClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";

        public override void Execute(StringContext context, string assetPath)
        {
            AnimationClipExtract.ExtractClipFromFBX(assetPath, TargetFolder);
        }
    }
}
