using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(Rect))]
    public class DefaultRectDrawer : NativeTypeDrawer
    {
        public DefaultRectDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(Rect);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            Rect value = DrawerProperty.GetValue<Rect>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.RectField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
