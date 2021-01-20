using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(string))]
    public class StringTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.TextField(Label, (string)ItemDrawer.Value);
        }
    }
}