using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.FBX
{
    public static class FBXMenuOptions
    {
        [MenuItem("Game/FBX/Compress Selected Clips")]
        public static string[] CompressSelectedClips()
        {
            AnimationClip[] selectedClips = Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);
            if (selectedClips == null || selectedClips.Length == 0)
            {
                Debug.LogError("Please selected clips");
                return null;
            }

            List<string> clipAssetPaths = new List<string>();
            List<AnimationClip> clips = new List<AnimationClip>();
            Array.ForEach(selectedClips, (clip) =>
            {
                var newClip = AnimationClipCompressor.Compressed(clip, 4);
                clips.Add(newClip);
                clipAssetPaths.Add(AssetDatabase.GetAssetPath(newClip));
            });

            if(clips.Count>0)
            {
                SelectionUtility.ActiveObjects(clips.ToArray());
            }

            return clipAssetPaths.ToArray();
        }

        [MenuItem("Game/FBX/Extract Clips From Selected FBX")]
        public static string[] ExtractClipsFromSelectedFBX()
        {
            GameObject[] gameObjects = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
            if (gameObjects == null || gameObjects.Length == 0)
            {
                return null;
            }

            var clipAssetPaths = (from go in gameObjects
                         let assetPath = AssetDatabase.GetAssetPath(go)
                         where Path.GetExtension(assetPath).ToLower() == ".fbx"
                         let extractClipAssetPaths = AnimationClipExtractor.ExtractClipFromFBX(assetPath, null)
                         from clipAssetPath in extractClipAssetPaths
                         select clipAssetPath).ToArray();

            if(clipAssetPaths!=null && clipAssetPaths.Length>0)
            {
                var clips = (from clip in clipAssetPaths select AssetDatabase.LoadMainAssetAtPath(clip)).ToArray();
                SelectionUtility.ActiveObjects(clips);
            }

            return clipAssetPaths.ToArray();
        }
    }
}
