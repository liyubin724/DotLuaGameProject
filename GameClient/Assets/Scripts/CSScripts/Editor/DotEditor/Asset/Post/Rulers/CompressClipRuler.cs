using DotEditor.FBX;
using DotEngine.Context;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Compress Clip","apr_compress_clip")]
    public class CompressClipRuler : AssetPostRuler
    {
        [Range(3,6)]
        public int precision = 4;

        public override void Execute(StringContext context, string assetPath)
        {
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
            AnimationClipCompress.CreateCompressedClip(clip, precision);
        }
    }
}
