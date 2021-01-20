using System;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt
{
    public abstract class LayoutDrawable : ILayoutDrawable
    {
        private bool isEnable = true;
        public bool Enable
        {
            get
            {
                if (EnableFunc != null)
                {
                    isEnable = EnableFunc();
                }
                return isEnable;
            }

            set
            {
                isEnable = value;
            }
        }

        public Func<bool> EnableFunc { get; set; } = null;

        private string text = string.Empty;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        text = string.Empty;
                    }
                    else
                    {
                        text = value;
                    }
                    Label = string.IsNullOrEmpty(text) ? GUIContent.none : new GUIContent(text, tooltip);
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
                if (tooltip != value)
                {
                    tooltip = value;

                    Label = string.IsNullOrEmpty(text) ? GUIContent.none : new GUIContent(text, tooltip);
                }
            }
        }

        public float LabelWidth { get; set; } = EditorGUIUtility.labelWidth;
        protected GUIContent Label { get; private set; }

        public void OnGUILayout()
        {
            EditorGUI.BeginDisabledGroup(!Enable);
            {
                EGUI.BeginLabelWidth(LabelWidth);
                {
                    OnLayoutDraw();
                }
                EGUI.EndLableWidth();
            }
            EditorGUI.EndDisabledGroup();
        }

        protected abstract void OnLayoutDraw();
    }
}
