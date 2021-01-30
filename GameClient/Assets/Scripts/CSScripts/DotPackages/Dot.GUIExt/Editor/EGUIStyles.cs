using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt
{
    public static class EGUIStyles
    {
        private static GUIStyle middleCenterLabel = null;
        public static GUIStyle MiddleCenterLabel
        {
            get
            {
                if (middleCenterLabel == null)
                {
                    middleCenterLabel = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                }
                return middleCenterLabel;
            }
        }

        private static GUIStyle boldLabelStyle = null;
        public static GUIStyle BoldLabelStyle
        {
            get
            {
                if (boldLabelStyle == null)
                {
                    boldLabelStyle = new GUIStyle(EditorStyles.label)
                    {
                        fontStyle = FontStyle.Bold,
                        fixedHeight = 20,
                    };
                }
                return boldLabelStyle;
            }
        }
        private static GUIStyle middleLeftLabelStyle = null;
        public static GUIStyle MiddleLeftLabelStyle
        {
            get
            {
                if (middleLeftLabelStyle == null)
                {
                    middleLeftLabelStyle = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleLeft
                    };
                }
                return middleLeftLabelStyle;
            }
        }

        private static GUIStyle boxStyle = null;
        public static GUIStyle BoxStyle
        {
            get
            {
                if(boxStyle == null)
                {
                    boxStyle = new GUIStyle(UnityEngine.GUI.skin.box);
                }
                return boxStyle;
            }
        }

        private static GUIStyle boxedHeaderStyle = null;
        public static GUIStyle BoxedHeaderStyle
        {
            get
            {
                if(boxedHeaderStyle == null)
                {
                    boxedHeaderStyle = new GUIStyle(UnityEngine.GUI.skin.box)
                    {
                        fontSize = 12,
                        alignment = TextAnchor.MiddleLeft,
                        fontStyle = FontStyle.Bold,
                    };
                }
                return boxedHeaderStyle;
            }
        }

        private static GUIStyle boxedHeaderCenterStyle = null;
        public static GUIStyle BoxedHeaderCenterStyle
        {
            get
            {
                if (boxedHeaderCenterStyle == null)
                {
                    boxedHeaderCenterStyle = new GUIStyle(UnityEngine.GUI.skin.box)
                    {
                        fontSize = 12,
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold,
                        fixedHeight = 20,
                    };
                }
                return boxedHeaderCenterStyle;
            }
        }

        public static GUIStyle GetTextureStyle(Texture2D texture)
        {
            GUIStyle style = new GUIStyle();
            style.normal.background = texture;
            return style;
        }

        public static GUIStyle MakeStyle(Color color,int size,TextAnchor anchor,FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle style = new GUIStyle();
            style.alignment = anchor;
            style.fontStyle = fontStyle;
            style.fontSize = size;
            style.normal.textColor = color;

            return style;
        }
    }
}
