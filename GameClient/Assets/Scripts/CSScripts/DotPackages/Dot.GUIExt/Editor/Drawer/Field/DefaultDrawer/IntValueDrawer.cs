using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Field
{
    [TypeDrawable(typeof(int))]
    public class IntValueDrawer : FieldValueDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, FieldValueProvider provider)
        {
            provider.Value = EditorGUI.IntField(rect, label, (int)provider.Value);
        }
    }
}
