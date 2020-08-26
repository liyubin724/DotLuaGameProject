using DotEditor.FBX;
using DotEngine.Context;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Asset.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Extract Clip", "apr_extract_clip")]
    public class ExtractClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";

        [ContextField(AssetPostContextKeys.OPERATE_CLIP_RESULT_KEY, ContextUsage.Out)]
        private Dictionary<string, List<AnimationClip>> results = null;

        public override void Execute()
        {
            results = new Dictionary<string, List<AnimationClip>>();

            if (assetPaths!=null && assetPaths.Length>0)
            {
                foreach(var assetPath in assetPaths)
                {
                    AnimationClip[] extractClips = AnimationClipExtract.ExtractClipFromFBX(assetPath, TargetFolder);
                    if (extractClips != null && extractClips.Length > 0)
                    {
                        results.Add(assetPath, new List<AnimationClip>(extractClips));
                    }
                }
            }
        }
    }
}
