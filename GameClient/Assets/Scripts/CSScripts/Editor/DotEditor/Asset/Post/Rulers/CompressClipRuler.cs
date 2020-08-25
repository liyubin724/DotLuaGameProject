using DotEditor.FBX;
using DotEngine.Context;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [CreateAssetMenu(menuName = "Asset Post/Ruler/Compress Clip", fileName = "apr_compress_clip")]
    public class CompressClipRuler : AssetPostRuler
    {
        public int precision = 4;

        public override void Execute(StringContext context, string assetPath)
        {
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
            AnimationClipCompress.CreateCompressedClip(clip, precision);
        }
    }
}
