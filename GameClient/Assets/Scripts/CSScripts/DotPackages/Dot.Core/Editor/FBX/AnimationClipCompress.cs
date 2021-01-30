using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.FBX
{
    public static class AnimationClipCompress
    {
        public static AnimationClip CreateCompressedClip(AnimationClip clip, int precision = 4)
        {
            var newClip = UnityObject.Instantiate<AnimationClip>(clip);
            string clipAssetPath = AssetDatabase.GetAssetPath(clip);

            AssetDatabase.DeleteAsset(clipAssetPath);

            AnimationClipCompress.RemoveScaleCurve(newClip);
            AnimationClipCompress.CompressFloatPrecision(newClip, precision);

            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        public static AnimationClip CreateCompressedScaleCurveClip(AnimationClip clip)
        {
            var newClip = UnityObject.Instantiate<AnimationClip>(clip);
            string clipAssetPath = AssetDatabase.GetAssetPath(clip);

            AssetDatabase.DeleteAsset(clipAssetPath);

            AnimationClipCompress.RemoveScaleCurve(newClip);

            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        public static AnimationClip CreateCompressPrecisionClip(AnimationClip clip, int precision = 4)
        {
            var newClip = UnityObject.Instantiate<AnimationClip>(clip);
            string clipAssetPath = AssetDatabase.GetAssetPath(clip);

            AssetDatabase.DeleteAsset(clipAssetPath);
            AnimationClipCompress.CompressFloatPrecision(newClip, precision);

            AssetDatabase.CreateAsset(newClip, clipAssetPath);
            AssetDatabase.ImportAsset(clipAssetPath);

            return newClip;
        }

        private static void RemoveScaleCurve(AnimationClip clip)
        {
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
            foreach(var curveBinding in curveBindings)
            {
                string propertyName = curveBinding.propertyName.ToLower();
                if(propertyName.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(clip, curveBinding, null);
                }
            }
        }

        private static void CompressFloatPrecision(AnimationClip clip, int precision)
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
