using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.Layout
{
    public class ClickableSpringLabel : ILayoutDrawable
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
                if(isExpandWidth!=value)
                {
                    isExpandWidth = value;
                    expandOption = GUILayout.ExpandWidth(isExpandWidth);
                }
            }
        }

        private string text = string.Empty;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if(text!=value)
                {
                    if(string.IsNullOrEmpty(value))
                    {
                        text = string.Empty;
                    }else
                    {
                        text = value;
                    }
                    content = string.IsNullOrEmpty(text) ? GUIContent.none : new GUIContent(text, tooltip);
                }
            }
        }

        private string tooltip = null;
        public string Tooltip
        {
            get
            {
                return tooltip;
            }
            set
            {
                if(tooltip!=value)
                {
                    tooltip = value;

                    content = string.IsNullOrEmpty(text) ? GUIContent.none : new GUIContent(text, tooltip);
                }
            }
        }
        public GUIStyle Style { get; set; } = EditorStyles.label;

        private GUILayoutOption expandOption = GUILayout.ExpandWidth(true);
        private GUIContent content = GUIContent.none;

        public Action OnClicked { get; set; } = null;
        
        public void OnGUILayout()
        {
            if(GUILayout.Button(content,Style,expandOption))
            {
                OnClicked?.Invoke();
            }
            //Rect btnRect = GUILayoutUtility.GetLastRect();
            //EditorGUIUtility.AddCursorRect(btnRect, MouseCursor.Link);
        }
    }
}
