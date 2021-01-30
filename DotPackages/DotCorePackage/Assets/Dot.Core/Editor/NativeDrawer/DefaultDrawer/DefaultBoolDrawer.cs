using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(bool))]
    public class DefaultBoolDrawer : NativeTypeDrawer
    {
        public DefaultBoolDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            bool value = DrawerProperty.GetValue<bool>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.Toggle(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(bool);
        }
    }
}
