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

        [ContextField(AssetPostContextKeys.OPERATE_CLIP_RESULT_KEY, ContextUsage.InOut)]
        private Dictionary<string, List<AnimationClip>> results = null;

        public override void Execute()
        {
            if(results != null)
            {
                foreach(var kvp in results)
                {
                    for (int i = 0; i < kvp.Value.Count; ++i)
                    {
                        kvp.Value[i] = AnimationClipCompress.CreateCompressedClip(kvp.Value[i], precision);
                    }
                }
            }else
            {
                results = new Dictionary<string, List<AnimationClip>>();
                foreach(var assetPath in assetPaths)
                {
                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
                    if (clip != null)
                    {
                        AnimationClip newClip = AnimationClipCompress.CreateCompressedClip(clip, precision);
                        var clips = new List<AnimationClip>();
                        clips.Add(newClip);
                        results.Add(assetPath, clips);
                    }
                }
            }
        }
    }
}
