using DotEditor.FBX;
using DotEngine.Context.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Assets.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Extract Clip", "apr_extract_clip")]
    public class ExtractClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";

        [ContextIE(AssetPostContextKeys.OPERATE_CLIP_RESULT_KEY, ContextUsage.Extract)]
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
