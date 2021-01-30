using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    [CustomAttrDrawer(typeof(BeginGroupAttribute))]
    public class BeginGroupAttrDrawer : LayoutAttrDrawer
    {
        public override void OnGUILayout()
        {
            BeginGroupAttribute attr = GetAttr<BeginGroupAttribute>();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true));
            if (!string.IsNullOrEmpty(attr.Label))
            {
                EGUILayout.DrawBoxHeader(attr.Label, GUILayout.ExpandWidth(true));
            }
        }
    }
}