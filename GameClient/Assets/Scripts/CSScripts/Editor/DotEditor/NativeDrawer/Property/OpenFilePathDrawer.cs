using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [AttrDrawBinder(typeof(OpenFilePathAttribute))]
    public class OpenFilePathDrawer : PropertyDrawer
    {
        public OpenFilePathDrawer(NativeDrawerProperty drawerProperty, PropertyDrawerAttribute attr) : base(drawerProperty, attr)
        {
        }

        protected override bool IsValidProperty()
        {
            return DrawerProperty.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = DrawerProperty.GetValue<string>();
            var attr = GetAttr<OpenFilePathAttribute>();

            EditorGUI.BeginChangeCheck();
            {
                if(attr.Filters!=null && attr.Filters.Length>0)
                {
                    value = EGUILayout.DrawOpenFileWithFilter(label, value, attr.Filters, attr.IsAbsolute);
                }else
                {
                    value = EGUILayout.DrawOpenFile(label, value, attr.Extension, attr.IsAbsolute);
                }
            }
            if(EditorGUI.EndChangeCheck())
            {
                DrawerProperty.Value = value;
            }
        }
    }
}
