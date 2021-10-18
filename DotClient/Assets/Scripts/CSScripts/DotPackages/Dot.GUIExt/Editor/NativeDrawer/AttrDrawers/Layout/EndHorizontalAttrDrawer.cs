using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(EndHorizontalAttribute))]
    public class EndHorizontalAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndHorizontal();
        }
    }
}

