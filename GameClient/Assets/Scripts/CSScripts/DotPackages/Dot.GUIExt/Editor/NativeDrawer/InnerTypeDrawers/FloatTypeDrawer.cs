using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [NativeTypeDrawer(typeof(float))]
    public class FloatTypeDrawer : NativeTypeDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, NativeField field)
        {
            field.Value = EditorGUI.FloatField(rect, label, (float)field.Value);
        }
    }
}