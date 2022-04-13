using DotEditor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.FBX
{
    public static class AnimationClipCompressor
    {
        public static string Compressed(string clipAssetPath,int precision = 4)
        {
            if(!PathUtility.IsAssetPath(clipAssetPath))
            {
                Debug.LogError("The file is not found");
                return null;
            }

            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipAssetPath);
            if(clip == null)
            {
                Debug.LogError($"The clip is not found At ({clipAssetPath})!");
                return null;
            }

            AnimationClip newClip = UnityObject.Instantiate(clip);
            RemoveScaleCurve(newClip);
            ReduceFloatPrecision(newClip, precision);

            AssetDatabase.DeleteAsset(clipAssetPath);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return clipAssetPath;
        }

        public static AnimationClip Compressed(AnimationClip clip, int precision = 4)
        {
            if (clip == null)
            {
                Debug.LogError($"The clip is null!");
                return null;
            }

            var newClip = UnityObject.Instantiate<AnimationClip>(clip);
            RemoveScaleCurve(newClip);
            ReduceFloatPrecision(newClip, precision);

            string clipAssetPath = AssetDatabase.GetAssetPath(clip);
            AssetDatabase.DeleteAsset(clipAssetPath);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        public static string CompressedScaleCurve(string clipAssetPath)
        {
            if (!PathUtility.IsAssetPath(clipAssetPath))
            {
                Debug.LogError("The file is not found");
                return null;
            }

            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipAssetPath);
            if (clip == null)
            {
                Debug.LogError($"The clip is not found At ({clipAssetPath})!");
                return null;
            }

            AnimationClip newClip = UnityObject.Instantiate(clip);
            RemoveScaleCurve(newClip);

            AssetDatabase.DeleteAsset(clipAssetPath);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return clipAssetPath;
        }

        public static AnimationClip CompressedScaleCurve(AnimationClip clip)
        {
            if (clip == null)
            {
                Debug.LogError($"The clip is null!");
                return null;
            }

            var newClip = UnityObject.Instantiate<AnimationClip>(clip);
            RemoveScaleCurve(newClip);

            string clipAssetPath = AssetDatabase.GetAssetPath(clip);
            AssetDatabase.DeleteAsset(clipAssetPath);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        public static string CompressedPrecision(string clipAssetPath, int precision = 4)
        {
            if (!PathUtility.IsAssetPath(clipAssetPath))
            {
                Debug.LogError("The file is not found");
                return null;
            }

            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipAssetPath);
            if (clip == null)
            {
                Debug.LogError($"The clip is not found At ({clipAssetPath})!");
                return null;
            }

            AnimationClip newClip = UnityObject.Instantiate(clip);
            ReduceFloatPrecision(newClip, precision);

            AssetDatabase.DeleteAsset(clipAssetPath);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return clipAssetPath;
        }

        public static AnimationClip CompressedPrecision(AnimationClip clip, int precision = 4)
        {
            if (clip == null)
            {
                Debug.LogError($"The clip is null!");
                return null;
            }

            var newClip = UnityObject.Instantiate<AnimationClip>(clip);
            ReduceFloatPrecision(newClip, precision);

            string clipAssetPath = AssetDatabase.GetAssetPath(clip);
            AssetDatabase.DeleteAsset(clipAssetPath);
            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        private static void RemoveScaleCurve(AnimationClip clip)
        {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach (var curveBinding in curveBindings)
            {
                string propertyName = curveBinding.propertyName.ToLower();
                if (propertyName.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(clip, curveBinding, null);
                }
            }
        }

        private static void ReduceFloatPrecision(AnimationClip clip, int precision)
        {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            AnimationClipCurveData[] curves = new AnimationClipCurveData[curveBindings.Length];
            for (int index = 0; index < curves.Length; ++index)
            {
                curves[index] = new AnimationClipCurveData(curveBindings[index]);
                curves[index].curve = AnimationUtility.GetEditorCurve(clip, curveBindings[index]);
            }
            foreach (AnimationClipCurveData curveDate in curves)
            {
                var keyFrames = curveDate.curve.keys;
                for (int i = 0; i < keyFrames.Length; i++)
                {
                    var key = keyFrames[i];

                    string precisionFormat = $"f{precision}";
                    key.value = float.Parse(key.value.ToString(precisionFormat));
                    key.inTangent = float.Parse(key.inTangent.ToString(precisionFormat));
                    key.outTangent = float.Parse(key.outTangent.ToString(precisionFormat));
                    key.inWeight = float.Parse(key.inWeight.ToString(precisionFormat));
                    key.outWeight = float.Parse(key.outWeight.ToString(precisionFormat));

                    keyFrames[i] = key;
                }
                curveDate.curve.keys = keyFrames;
                clip.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
            }
        }
    }
}
