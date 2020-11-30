using DotEngine.NativeDrawer.Property;
using UnityEditor;

namespace DotEditor.NativeDrawer.Property
{
    [AttrBinder(typeof(IndentAttribute))]
    public class IndentDrawer : PropertyControlDrawer
    {
        public IndentDrawer(PropertyControlAttribute attr) : base(attr)
        {
        }

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
