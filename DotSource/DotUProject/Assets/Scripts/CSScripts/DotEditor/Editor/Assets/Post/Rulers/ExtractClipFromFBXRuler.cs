using DotEditor.FBX;
using DotEngine.Injection;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Assets.Post.Rulers
{
    [AssetPostRulerMenu("Animation Clip/Extract Clip", "apr_extract_clip")]
    public class ExtractClipFromFBXRuler : AssetPostRuler
    {
        public string TargetFolder = "Assets";

        [InjectUsage(AssetPostContextKeys.OPERATE_CLIP_RESULT_KEY, EInjectOperationType.Extract)]
        private Dictionary<string, List<AnimationClip>> results = null;

        public override void Execute()
        {
            results = new Dictionary<string, List<AnimationClip>>();

            if (assetPaths!=null && assetPaths.Length>0)
            {
                foreach(var assetPath in assetPaths)
                {
                    string[] clipAssetPaths = AnimationClipExtractor.ExtractClipFromFBX(assetPath, TargetFolder);
                    if (clipAssetPaths != null && clipAssetPaths.Length > 0)
                    {
                        var clips = (from clipAssetPath in clipAssetPaths select AssetDatabase.LoadAssetAtPath<AnimationClip>(clipAssetPath)).ToList();
                        results.Add(assetPath, clips);
                    }
                }
            }
        }
    }
}
