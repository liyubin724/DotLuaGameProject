using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(float))]
    public class FloatTypeDrawer : TypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.FloatField(Label, (float)ItemDrawer.Value);
        }
    }
}