using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [AttrDrawBinder(typeof(OpenFolderPathAttribute))]
    public class OpenFolderPathDrawer : PropertyDrawer
    {
        public OpenFolderPathDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = DrawerProperty.GetValue<string>();
            var attr = GetAttr<OpenFolderPathAttribute>();

            EditorGUI.BeginChangeCheck();
            {
                value = EGUILayout.DrawOpenFolder(label, value, attr.IsAbsolute);
            }
            if (EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
