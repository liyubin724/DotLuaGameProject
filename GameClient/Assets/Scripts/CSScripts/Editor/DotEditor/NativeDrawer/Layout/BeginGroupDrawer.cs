using DotEngine.NativeDrawer;
using DotEngine.NativeDrawer.Layout;
using DotEditor.GUIExtension;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Layout
{
    [AttrDrawBinder(typeof(BeginGroupAttribute))]
    public class BeginGroupDrawer : LayoutDrawer
    {
        public BeginGroupDrawer(LayoutAttribute attr) : base(attr)
        {
        }

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
