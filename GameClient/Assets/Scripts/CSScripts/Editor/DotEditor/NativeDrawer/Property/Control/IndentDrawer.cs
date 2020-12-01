using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [Binder(typeof(IndentAttribute))]
    public class IndentDrawer : PropertyControlDrawer
    {
        public override void OnStartGUILayout()
        {
            IndentAttribute attr = GetAttr<IndentAttribute>();
            EditorGUI.indentLevel += attr.Indent;
        }

        public override void OnEndGUILayout()
        {
            IndentAttribute attr = GetAttr<IndentAttribute>();
            EditorGUI.indentLevel -= attr.Indent;
        }
    }
}
