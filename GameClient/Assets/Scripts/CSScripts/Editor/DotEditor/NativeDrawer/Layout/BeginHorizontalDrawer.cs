using DotEngine.NativeDrawer.Layout;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrDrawBinder(typeof(BeginHorizontalAttribute))]
    public class BeginHorizontalDrawer : LayoutDrawer
    {
        public BeginHorizontalDrawer(LayoutAttribute attr) : base(attr)
        {
        }

        public override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        }
    }
}
