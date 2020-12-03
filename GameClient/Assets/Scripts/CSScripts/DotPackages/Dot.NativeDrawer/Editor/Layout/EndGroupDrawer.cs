using DotEngine.NativeDrawer.Layout;
using UnityEditor;

namespace DotEditor.NativeDrawer.Layout
{
    [Binder(typeof(EndGroupAttribute))]
    public class EndGroupDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
