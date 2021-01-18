using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.EditDrawer
{
    public class HorizontalToolbar : ILayoutDrawable
    {
        private bool isExpandWidth = true;
        public bool ExpandWidth
        {
            get
            {
                return isExpandWidth;
            }
            set
            {
                if (isExpandWidth != value)
                {
                    isExpandWidth = value;
                    expandOption = GUILayout.ExpandWidth(isExpandWidth);
                }
            }
        }

        public ILayoutDrawable LeftDrawable { get; set; } = null;
        public ILayoutDrawable RightDrawable { get; set; } = null;

        private GUILayoutOption expandOption = GUILayout.ExpandWidth(true);

        public void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, expandOption);
            {
                LeftDrawable?.OnGUILayout();
                GUILayout.FlexibleSpace();
                RightDrawable?.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
