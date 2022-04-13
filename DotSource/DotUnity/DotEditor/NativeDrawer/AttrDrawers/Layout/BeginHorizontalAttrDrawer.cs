using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(BeginHorizontalAttribute))]
    public class BeginHorizontalAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        }
    }
}

