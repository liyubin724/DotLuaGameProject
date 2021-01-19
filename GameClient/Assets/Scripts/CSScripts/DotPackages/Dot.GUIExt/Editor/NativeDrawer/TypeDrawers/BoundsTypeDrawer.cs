using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomTypeDrawer(typeof(Bounds))]
    public class BoundsTypeDrawer : NTypeDrawer
    {
        public override void OnGUILayout()
        {
            FieldDrawer.Value = EditorGUILayout.BoundsField(Label, (Bounds)FieldDrawer.Value);
        }
    }
}

