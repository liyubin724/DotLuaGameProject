using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Vector2))]
    public class Vector2TypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            FieldDrawer.Value = EditorGUILayout.Vector2Field(Label, (Vector2)FieldDrawer.Value);
        }
    }
}
