using DotEditor.FBX;
using DotEngine.Context;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [CreateAssetMenu(menuName = "Asset Post/Ruler/Extract&Compress Clip", fileName = "apr_extract&compress_clip")]
    public class ExtractAndCompressClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";
        public int precision = 4;

        public override void Execute(StringContext context, string assetPath)
        {
            AnimationClip[] clips = AnimationClipExtract.ExtractClipFromFBX(assetPath, TargetFolder);
            foreach(var clip in clips)
            {
                AnimationClipCompress.CreateCompressedClip(clip, precision);
            }
        }
    }
}
