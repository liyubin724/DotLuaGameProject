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

        [ContextField(AssetPostContextKeys.OPERATE_CLIP_LIST_KEY, ContextUsage.Out)]
        private List<AnimationClip> clips = null;

        public override void Execute()
        {
            clips = new List<AnimationClip>();

            AnimationClip[] extractClips = AnimationClipExtract.ExtractClipFromFBX(assetPath, TargetFolder);
            if(extractClips!=null && extractClips.Length>0)
            {
                clips.AddRange(extractClips);
            }
        }
    }
}
