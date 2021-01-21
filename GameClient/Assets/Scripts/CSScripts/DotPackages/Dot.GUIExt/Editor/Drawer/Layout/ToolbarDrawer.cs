using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class ToolbarDrawer : ILayoutDrawable
    {
        public bool IsExpandWidth { get; set; } = true;

        public ILayoutDrawable LeftDrawable { get; set; } = null;
        public ILayoutDrawable RightDrawable { get; set; } = null;

        public void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(IsExpandWidth));
            {
                LeftDrawable?.OnGUILayout();
                GUILayout.FlexibleSpace();
                RightDrawable?.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
