using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(float))]
    public class FloatTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            FieldDrawer.Value = EditorGUILayout.FloatField(Label, (float)FieldDrawer.Value);
        }
    }
}