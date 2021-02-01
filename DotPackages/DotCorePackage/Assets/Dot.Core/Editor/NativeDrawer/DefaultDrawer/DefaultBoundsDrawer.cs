using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Bounds))]
    public class DefaultBoundsDrawer : NativeTypeDrawer
    {
        public DefaultBoundsDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(Bounds);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Bounds value = DrawerProperty.GetValue<Bounds>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.BoundsField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
