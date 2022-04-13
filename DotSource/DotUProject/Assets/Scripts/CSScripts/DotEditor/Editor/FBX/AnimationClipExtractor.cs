using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.FBX
{
    public static class AnimationClipExtractor
    {
        public static string[] ExtractClipFromFBX(string fbxAssetPath, string targetAssetFolder = null)
        {
            if (string.IsNullOrEmpty(fbxAssetPath) || Path.GetExtension(fbxAssetPath).ToLower() != ".fbx")
            {
                Debug.LogError("the path of asset is not a fbx!!");
                return null;
            }

            if (string.IsNullOrEmpty(targetAssetFolder))
            {
                targetAssetFolder = Path.GetDirectoryName(fbxAssetPath);
            }

            string assetName = Path.GetFileNameWithoutExtension(fbxAssetPath);

            List<string> clipAssetPaths = new List<string>();
            UnityObject[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(fbxAssetPath);
            foreach (var obj in objects)
            {
                AnimationClip clipInFBX = obj as AnimationClip;
                if (clipInFBX == null)
                {
                    continue;
                }

                string clipAssetPath;
                if (assetName.EndsWith(clipInFBX.name))
                {
                    clipAssetPath = $"{targetAssetFolder}/{assetName}.anim";
                }
                else
                {
                    clipAssetPath = $"{targetAssetFolder}/{assetName}_{clipInFBX.name}.anim";
                }

                if (AssetDatabase.LoadAssetAtPath(clipAssetPath, typeof(UnityObject)) != null)
                {
                    AssetDatabase.DeleteAsset(clipAssetPath);
                }

                AnimationClip newClip = UnityObject.Instantiate<AnimationClip>(clipInFBX);
                AssetDatabase.CreateAsset(newClip, clipAssetPath);

                clipAssetPaths.Add(clipAssetPath);
            }

            clipAssetPaths.ForEach((assetPath) =>
            {
                AssetDatabase.ImportAsset(assetPath);
            });

            return clipAssetPaths.ToArray();
        }
    }
}
