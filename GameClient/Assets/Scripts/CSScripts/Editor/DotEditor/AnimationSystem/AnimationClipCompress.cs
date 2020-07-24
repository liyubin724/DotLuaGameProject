using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AnimationSystem
{
    public static class AnimationClipCompress
    {
        public static AnimationClip RemoveScaleCurve(AnimationClip aClip,string newAssetPath)
        {
            AnimationClip clip = UnityObject.Instantiate<AnimationClip>(aClip);

            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach(var curveBinding in curveBindings)
            {
                string propertyName = curveBinding.propertyName.ToLower();
                if(propertyName.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(clip, curveBinding, null);
                }
            }

            AssetDatabase.CreateAsset(clip, newAssetPath);
            AssetDatabase.ImportAsset(newAssetPath);

            return clip;
        }

        public static AnimationClip CompressFloatPrecision(AnimationClip aClip,string newAssetPath,int precision)
        {
            AnimationClip clip = UnityObject.Instantiate<AnimationClip>(aClip);

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

            AssetDatabase.CreateAsset(clip, newAssetPath);
            AssetDatabase.ImportAsset(newAssetPath);

            return clip;
        }
    }
}
