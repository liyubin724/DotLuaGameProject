using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(EndGroupAttribute))]
    public class EndGroupAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.EndVertical();
        }
    }
}
