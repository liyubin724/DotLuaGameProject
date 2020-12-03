using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(OpenFilePathAttribute))]
    public class OpenFilePathDrawer : PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = Property.GetValue<string>();
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
                Property.Value = value;
            }
        }
    }
}
