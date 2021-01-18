using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Field
{
    [TypeDrawable(typeof(float))]
    public class FloatValueDrawer : FieldValueDrawer
    {
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, string label, FieldValueProvider provider)
        {
            provider.Value = EditorGUI.FloatField(rect, label, (int)provider.Value);
        }
    }
}
