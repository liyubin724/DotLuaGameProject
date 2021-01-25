using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Vector2))]
    public class Vector2TypeDrawer : TypeDrawer
    {
        public override void OnGUILayout()
        {
            ItemDrawer.Value = EditorGUILayout.Vector2Field(Label, (Vector2)ItemDrawer.Value);
        }
    }
}