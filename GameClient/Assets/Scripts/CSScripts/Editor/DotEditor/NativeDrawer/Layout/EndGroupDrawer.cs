using DotEngine.NativeDrawer.Layout;
using UnityEditor;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrBinder(typeof(EndGroupAttribute))]
    public class EndGroupDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
