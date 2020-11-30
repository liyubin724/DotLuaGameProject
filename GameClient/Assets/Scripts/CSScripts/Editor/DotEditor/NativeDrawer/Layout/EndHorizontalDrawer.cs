using DotEngine.NativeDrawer.Layout;
using UnityEditor;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrBinder(typeof(EndHorizontalAttribute))]
    public class EndHorizontalDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndHorizontal();
        }
    }
}
