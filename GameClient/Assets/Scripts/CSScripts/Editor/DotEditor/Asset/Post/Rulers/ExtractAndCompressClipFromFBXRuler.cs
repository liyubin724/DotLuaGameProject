using DotEditor.FBX;
using DotEngine.Context;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Extract&Compress Clip", "apr_extract_compress_clip")]
    public class ExtractAndCompressClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";
        [Range(3, 6)]
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
