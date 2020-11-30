using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Vector2))]
    public class Vector2Drawer : Property.PropertyDrawer
    {
        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(Vector2);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Vector2 value = DrawerProperty.GetValue<Vector2>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Vector2Field(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
