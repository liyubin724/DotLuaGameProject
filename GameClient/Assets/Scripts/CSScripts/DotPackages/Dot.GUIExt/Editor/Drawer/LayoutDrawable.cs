using System;
using System.Collections.Generic;
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

        public float LabelWidth { get; set; } = EditorGUIUtility.labelWidth;
        protected GUIContent Label { get; private set; }

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
        public float Width { get; set; } = 0;
        public float MinWidth { get; set; } = 0;
        public float MaxWidth { get; set; } = 0;

        public float Height { get; set; } = 0;
        public float MinHeight { get; set; } = 0;
        public float MaxHeight { get; set; } = 0;

        public bool IsExpandWidth { get; set; } = false;
        public bool IsExpandHeight { get; set; } = false;
        protected GUILayoutOption[] LayoutOptions 
        { 
            get
            {
                List<GUILayoutOption> options = new List<GUILayoutOption>();
                options.Add(GUILayout.ExpandWidth(IsExpandWidth));
                options.Add(GUILayout.ExpandHeight(IsExpandHeight));
                if(Width>0)
                {
                    options.Add(GUILayout.Width(Width));
                }
                if (MinWidth > 0)
                {
                    options.Add(GUILayout.MinWidth(MinWidth));
                }
                if (MaxWidth > 0)
                {
                    options.Add(GUILayout.MaxWidth(MaxWidth));
                }

                if (Height > 0)
                {
                    options.Add(GUILayout.Height(Height));
                }
                if (MinHeight > 0)
                {
                    options.Add(GUILayout.MinHeight(MinHeight));
                }
                if (MaxHeight > 0)
                {
                    options.Add(GUILayout.MaxHeight(MaxHeight));
                }
                return options.ToArray();
            } 
        }

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
