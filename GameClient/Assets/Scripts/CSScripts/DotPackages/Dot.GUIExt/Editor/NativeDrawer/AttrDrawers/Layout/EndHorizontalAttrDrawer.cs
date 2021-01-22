using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class EndHorizontalAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndHorizontal();
        }
    }
}

