using UnityEditor;

namespace DotEditor.NativeDrawer.DefaultDrawer
{
    [CustomTypeDrawer(typeof(int))]
    public class DefaultIntDrawer : NativeTypeDrawer
    {
        public DefaultIntDrawer(NativeDrawerProperty property) : base(property)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(int);
        }

        protected override void OnDrawProperty(string label)
        {
            label = label ?? "";
            int value = DrawerProperty.GetValue<int>();
            EditorGUI.BeginChangeCheck();
            {
                value = EditorGUILayout.IntField(label, value);
            }
            if(EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
