using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(string))]
    public class StringTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            FieldDrawer.Value = EditorGUILayout.TextField(Label, (string)FieldDrawer.Value);
        }
    }
}