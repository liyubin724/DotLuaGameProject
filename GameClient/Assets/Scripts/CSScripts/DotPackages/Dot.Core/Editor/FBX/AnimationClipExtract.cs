using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.FBX
{
    public static class AnimationClipExtract
    {
        public static AnimationClip[] ExtractClipFromFBX(string assetPath,string targetFolder)
        {
            if(string.IsNullOrEmpty(assetPath) || Path.GetExtension(assetPath).ToLower() != ".fbx")
            {
                Debug.LogError("the path of asset is empty!!");
                return new AnimationClip[0];
            }
            if(Path.GetExtension(assetPath).ToLower() != ".fbx")
            {
                Debug.LogError("the asset is invalid!!");
                return new AnimationClip[0];
            }
            var extractClips = FindClipFromFBX(assetPath);
            if(extractClips == null || extractClips.Length == 0)
            {
                Debug.LogError($"the clip was not found in fbx({assetPath})");
                return new AnimationClip[0];
            }

            if(string.IsNullOrEmpty(targetFolder))
            {
                targetFolder = Path.GetDirectoryName(assetPath);
            }
            List<AnimationClip> results = new List<AnimationClip>();
            foreach(var clip in extractClips)
            {
                results.Add(CopyClipFromFBX(clip, assetPath, targetFolder));
            }

            return results.ToArray();
        }

        private static AnimationClip CopyClipFromFBX(AnimationClip clipInFBX,string fbxAssetPath, string targetFolder)
        {
            string assetName = Path.GetFileNameWithoutExtension(fbxAssetPath);
            string clipName = clipInFBX.name;
            string clipAssetPath = string.Empty;
            if(assetName.EndsWith(clipName))
            {
                clipAssetPath = $"{targetFolder}/{assetName}.anim";
            }else
            {
                clipAssetPath = $"{targetFolder}/{assetName}_{clipInFBX.name}.anim";
            }

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
