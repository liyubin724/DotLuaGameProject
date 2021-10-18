using DotEngine.NativeDrawer.Property;
using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(OpenFolderPathAttribute))]
    public class OpenFolderPathDrawer : PropertyContentDrawer
    {
        protected override bool IsValidProperty()
        {
            return Property.ValueType == typeof(string);
        }

        protected override void OnDrawProperty(string label)
        {
            string value = Property.GetValue<string>();
            var attr = GetAttr<OpenFolderPathAttribute>();

            EditorGUI.BeginChangeCheck();
            {
                value = EGUILayout.DrawOpenFolder(label, value, attr.IsAbsolute);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Property.Value = value;
            }
        }
    }
}
