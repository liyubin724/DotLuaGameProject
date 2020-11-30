using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(string))]
    public class DefaultStringDrawer : TypeDrawer
    {
        public DefaultStringDrawer(DrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            string value = DrawerProperty.GetValue<string>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.TextField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
