using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Rect))]
    public class RectDrawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(Rect);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Rect value = Property.GetValue<Rect>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.RectField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
