using DotEditor.FBX;
using DotEngine.Context;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Compress Clip","apr_compress_clip")]
    public class CompressClipRuler : AssetPostRuler
    {
        [Range(3,6)]
        public int precision = 4;

        [ContextField(AssetPostContextKeys.OPERATE_CLIP_LIST_KEY, ContextUsage.In)]
        private List<AnimationClip> clips = null;

        public override void Execute()
        {
            if(clips != null)
            {
                for (int i = 0; i < clips.Count; ++i)
                {
                    clips[i] = AnimationClipCompress.CreateCompressedClip(clips[i], precision);
                }
            }else
            {
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
                if(clip!=null)
                {
                    AnimationClip newClip = AnimationClipCompress.CreateCompressedClip(clip, precision);
                    if(clips==null)
                    {
                        clips = new List<AnimationClip>();
                    }
                    clips.Add(newClip);
                }
            }
        }
    }
}
