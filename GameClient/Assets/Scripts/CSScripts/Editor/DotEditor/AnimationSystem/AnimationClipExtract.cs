using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AnimationSystem
{
    public static class AnimationClipExtract
    {
        [MenuItem("Game/Animation Clip/extract clips from fbx")]
        public static void ExtractClipsFromFBX()
        {
            GameObject[] gameObjects = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
            if(gameObjects == null || gameObjects.Length == 0)
            {
                return;
            }

            List<AnimationClip> clips = new List<AnimationClip>();
            foreach(var go in gameObjects)
            {
                string assetPath = AssetDatabase.GetAssetPath(go);
                AnimationClip[] extractClips = ExtractClips(assetPath);
                if (extractClips != null && extractClips.Length > 0)
                {
                    clips.AddRange(extractClips);
                }
            }

            SelectionUtility.ActiveObjects(clips.ToArray());
        }

        [MenuItem("Game/Animation Clip/extract and compress clips from fbx")]
        public static void ExtractAnCompressClipsFromFBX()
        {
            GameObject[] gameObjects = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
            if (gameObjects == null || gameObjects.Length == 0)
            {
                return;
            }

            List<AnimationClip> clips = new List<AnimationClip>();
            foreach (var go in gameObjects)
            {
                string assetPath = AssetDatabase.GetAssetPath(go);
                AnimationClip[] extractClips = ExtractClips(assetPath);

                if (extractClips != null && extractClips.Length > 0)
                {
                    foreach(var clip in extractClips)
                    {
                        string clipAssetPath = AssetDatabase.GetAssetPath(clip);
                        AnimationClip newClip = AnimationClipCompress.RemoveScaleCurve(clip, clipAssetPath);
                        newClip = AnimationClipCompress.CompressFloatPrecision(newClip, clipAssetPath,4);

                        clips.Add(newClip);
                    }
                }
            }

            SelectionUtility.ActiveObjects(clips.ToArray());
        }

        private static AnimationClip[] ExtractClips(string assetPath)
        {
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            string dirName = Path.GetDirectoryName(assetPath);

            List<AnimationClip> clips = new List<AnimationClip>();
            UnityObject[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            foreach(var obj in objects)
            {
                AnimationClip clip = obj as AnimationClip;
                if(clip!=null)
                {
                    AnimationClip newClip = UnityObject.Instantiate<AnimationClip>(clip);
                    string newClipAssetPath = $"{dirName}/{assetName}_{clip.name}.anim";
                    
                    AssetDatabase.CreateAsset(newClip, newClipAssetPath);
                    AssetDatabase.ImportAsset(newClipAssetPath);

                    clips.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(newClipAssetPath));
                }
            }

            return clips.ToArray();
        }
    }
}
