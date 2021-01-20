using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(bool))]
    public class BoolTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.Toggle(Label, (bool)ItemDrawer.Value);
        }
    }
}