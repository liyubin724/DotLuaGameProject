using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Field
{
    [TypeDrawable(typeof(string))]
    public class StringValueDrawer : FieldValueDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, FieldValueProvider provider)
        {
            provider.Value = EditorGUI.TextField(rect, label, (string)provider.Value);
        }
    }
}