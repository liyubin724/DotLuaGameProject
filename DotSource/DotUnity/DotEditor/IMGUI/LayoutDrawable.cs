using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
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

        private bool isLabelChanged = false;
        private GUIContent label = null;
        protected GUIContent Label
        {
            get
            {
                if(label == null || isLabelChanged)
                {
                    label = new GUIContent(string.IsNullOrEmpty(Text) ? "" : Text, string.IsNullOrEmpty(Tooltip) ? "" : Tooltip);
                }
                return label;
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
                    isLabelChanged = true;
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

                    isLabelChanged = true;
                }
            }
        }

        private bool isOptionChanged = false;

        private float width = 0;
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                if(width!=value)
                {
                    width = value;
                    isOptionChanged = true;
                }
            }
        }

        private float minWidth = 0;
        public float MinWidth
        {
            get
            {
                return minWidth;
            }
            set
            {
                if(minWidth!=value)
                {
                    minWidth = value;
                    isOptionChanged = true;
                }
            }
        }

        private float maxWidth = 0;
        public float MaxWidth
        {
            get
            {
                return maxWidth;
            }
            set
            {
                if(maxWidth!=value)
                {
                    maxWidth = value;
                    isOptionChanged = true;
                }
            }
        }

        private float height = 0;
        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                if(height!=value)
                {
                    height = value;
                    isOptionChanged = true;
                }
            }
        }

        private float minHeight = 0;
        public float MinHeight
        {
            get
            {
                return minHeight;
            }
            set
            {
                if(minHeight!=value)
                {
                    minHeight = value;
                    isOptionChanged = true;
                }
            }
        }

        private float maxHeight = 0;
        public float MaxHeight
        {
            get
            {
                return maxHeight;
            }
            set
            {
                if(maxHeight!=value)
                {
                    maxHeight = value;
                    isOptionChanged = true;
                }
            }
        }

        private bool isExpandWidth = false;
        public bool IsExpandWidth
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
                    isOptionChanged = true;
                }
            }
        }

        private bool isExpandHeight = false;
        public bool IsExpandHeight
        {
            get
            {
                return isExpandHeight;
            }
            set
            {
                if(isExpandHeight!=value)
                {
                    isExpandHeight = value;
                    isOptionChanged = true;
                }
            }
        }

        private GUILayoutOption[] layoutOptions = null;
        protected GUILayoutOption[] LayoutOptions 
        { 
            get
            {
                if(layoutOptions == null || isOptionChanged)
                {
                    List<GUILayoutOption> options = new List<GUILayoutOption>();
                    options.Add(GUILayout.ExpandWidth(IsExpandWidth));
                    options.Add(GUILayout.ExpandHeight(IsExpandHeight));
                    if (Width > 0)
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
                    layoutOptions = options.ToArray();
                }
                return layoutOptions;
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
