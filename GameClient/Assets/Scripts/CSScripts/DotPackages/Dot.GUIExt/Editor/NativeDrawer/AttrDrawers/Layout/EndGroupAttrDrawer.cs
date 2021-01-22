using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class EndGroupAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
