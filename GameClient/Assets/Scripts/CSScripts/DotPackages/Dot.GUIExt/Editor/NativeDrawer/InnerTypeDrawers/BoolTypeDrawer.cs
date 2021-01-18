using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [NativeTypeDrawer(typeof(bool))]
    public class BoolTypeDrawer : NativeTypeDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, NativeField field)
        {
            field.Value = EditorGUI.Toggle(rect, label, (bool)field.Value);
        }
    }
}
