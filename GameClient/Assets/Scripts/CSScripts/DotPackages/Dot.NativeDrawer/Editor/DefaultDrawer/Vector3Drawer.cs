using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Vector3))]
    public class Vector3Drawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(Vector3);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Vector3 value = Property.GetValue<Vector3>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Vector3Field(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
