using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(int))]
    public class IntTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.IntField(Label, (int)ItemDrawer.Value);
        }
    }
}