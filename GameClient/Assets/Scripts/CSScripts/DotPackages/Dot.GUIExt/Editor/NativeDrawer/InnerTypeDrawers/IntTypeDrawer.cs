using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(int))]
    public class IntTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            FieldDrawer.Value = EditorGUILayout.IntField(Label, (int)FieldDrawer.Value);
        }
    }
}