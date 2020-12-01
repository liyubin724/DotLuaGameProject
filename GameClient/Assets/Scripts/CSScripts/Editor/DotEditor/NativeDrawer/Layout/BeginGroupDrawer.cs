using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Layout;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Layout
{
    [Binder(typeof(BeginGroupAttribute))]
    public class BeginGroupDrawer : LayoutDrawer
    {
        public override void OnGUILayout()
        {
            BeginGroupAttribute attr = GetAttr<BeginGroupAttribute>();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox,GUILayout.ExpandWidth(true));
            if(!string.IsNullOrEmpty(attr.Label))
            {
                EGUILayout.DrawBoxHeader(attr.Label, GUILayout.ExpandWidth(true));
            }
        }
    }
}
