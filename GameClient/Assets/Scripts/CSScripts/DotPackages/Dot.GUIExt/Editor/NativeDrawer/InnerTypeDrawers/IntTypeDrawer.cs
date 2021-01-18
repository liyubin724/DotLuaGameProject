using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [NativeTypeDrawer(typeof(int))]
    public class IntTypeDrawer : NativeTypeDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, NativeField field)
        {
            field.Value = EditorGUI.IntField(rect, label, (int)field.Value);
        }
    }
}