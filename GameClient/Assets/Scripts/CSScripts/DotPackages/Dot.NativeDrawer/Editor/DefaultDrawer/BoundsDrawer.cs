using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Bounds))]
    public class BoundsDrawer : Property.PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(Bounds);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Bounds value = Property.GetValue<Bounds>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.BoundsField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
