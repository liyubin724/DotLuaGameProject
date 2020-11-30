using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(float))]
    public class DefaultFloatDrawer : TypeDrawer
    {
        public DefaultFloatDrawer(DrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(float);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            float value = DrawerProperty.GetValue<float>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.FloatField(label, value);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
