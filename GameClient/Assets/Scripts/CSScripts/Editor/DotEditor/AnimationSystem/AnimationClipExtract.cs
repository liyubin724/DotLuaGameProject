using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AnimationSystem
{
    public static class AnimationClipExtract
    {
        [MenuItem("Game/Animation Clip/compress clips")]
        public static void CompressClip()
        {
            AnimationClip[] selectedClips = Selection.GetFiltered<AnimationClip>(SelectionMode.Assets);
            if (selectedClips == null || selectedClips.Length == 0)
            {
                return;
            }

            var clipInfos = (from clip in selectedClips
                             let clipAssetPath = AssetDatabase.GetAssetPath(clip)
                             let newClip = UnityObject.Instantiate<AnimationClip>(clip)
                             select new { path = clipAssetPath, newClip = newClip }
                             ).ToList();

            clipInfos.ForEach(clipInfo =>
            {
                AssetDatabase.DeleteAsset(clipInfo.path);

                AnimationClipCompress.RemoveScaleCurve(clipInfo.newClip);
                AnimationClipCompress.CompressFloatPrecision(clipInfo.newClip, 4);

                AssetDatabase.CreateAsset(clipInfo.newClip, clipInfo.path);
                AssetDatabase.ImportAsset(clipInfo.path);
            });

            SelectionUtility.ActiveObjects((from clip in clipInfos select clip.newClip).ToArray()) ;
        }

        [MenuItem("Game/Animation Clip/extract clips from fbx")]
        public static AnimationClip[] ExtractClipsFromFBX()
        {
            GameObject[] gameObjects = Selection.GetFiltered<GameObject>(SelectionMode.Assets);
            if(gameObjects == null || gameObjects.Length == 0)
            {
                return null;
            }

            var clips = (from go in gameObjects
                         let assetPath = AssetDatabase.GetAssetPath(go)
                         where Path.GetExtension(assetPath).ToLower() == ".fbx"
                         let extractClips = FindClipFromFBX(assetPath)
                         from clip in extractClips
                         select ExtractClipFromFBX(clip)).ToList();

            SelectionUtility.ActiveObjects(clips.ToArray());

            return clips.ToArray();
        }

        [MenuItem("Game/Animation Clip/extract and compress clips from fbx")]
        public static void ExtractAnCompressClipsFromFBX()
        {
            AnimationClip[] extractClips = ExtractClipsFromFBX();
            if(extractClips!=null && extractClips.Length>0)
            {
                CompressClip();
            }
        }

        private static AnimationClip ExtractClipFromFBX(AnimationClip clipInFBX)
        {
            string assetPath = AssetDatabase.GetAssetPath(clipInFBX);

            string dirName = Path.GetDirectoryName(assetPath);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            string clipAssetPath = $"{dirName}/{assetName}_{clipInFBX.name}.anim";

            if (AssetDatabase.LoadAssetAtPath(clipAssetPath, typeof(UnityObject)) != null)
            {
                AssetDatabase.DeleteAsset(clipAssetPath);
            }

            AnimationClip newClip = UnityObject.Instantiate<AnimationClip>(clipInFBX);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        private static AnimationClip[] FindClipFromFBX(string assetPath)
        {
            List<AnimationClip> clips = new List<AnimationClip>();
            UnityObject[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            foreach(var obj in objects)
            {
                AnimationClip clip = obj as AnimationClip;
                if (clip != null)
                {
                    clips.Add(clip);
                }
            }

            return clips.ToArray();
        }
    }
}
