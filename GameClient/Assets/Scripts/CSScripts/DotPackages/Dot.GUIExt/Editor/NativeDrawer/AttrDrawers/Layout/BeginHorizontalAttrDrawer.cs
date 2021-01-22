using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public class BeginHorizontalAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        }
    }
}

