using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExtension.RList
{
    internal static class ReorderableListUtility
    {
        private static readonly Dictionary<bool, GUIStyle> sm_FoldoutStyles;

        static ReorderableListUtility()
        {
            sm_FoldoutStyles = new Dictionary<bool, GUIStyle>();
        }

        public static void DrawClosedFoldoutRect(Rect rect)
        {
            // Outermost border
            Color color = Color.white * 0.63f;
            color.a = 1f;
            EditorGUI.DrawRect(rect, color);

            // Inset border
            color = Color.white * 0.80f;
            color.a = 1f;
            rect.x += 1f;
            rect.width -= 2f;
            rect.y += 1f;
            rect.height -= 2f;
            EditorGUI.DrawRect(rect, color);

            // Main body color
            color = Color.white * 0.87f;
            color.a = 1f;
            rect.x += 1f;
            rect.width -= 1f;
            rect.y += 1f;
            rect.height -= 1f;
            EditorGUI.DrawRect(rect, color);

            // White top inset border
            color = Color.white * 0.95f;
            rect.y -= 1f;
            rect.height = 1f;
            EditorGUI.DrawRect(rect, color);
        }

        public static string GetPropertyDisplayNameFormatted(SerializedProperty property)
        {
            return $"{property.displayName}  [{property.arraySize}]";
        }

        /// <summary>
        /// Returns GuiStyle for the foldout.
        /// </summary>
        /// <param name="isOpenStyle">
        /// True, if the GuiStyle being requested is for an open foldout. Else, (false) returns GuiStyle
        /// for closed foldout.
        /// </param>
        public static GUIStyle GetFoldoutStyle(bool isOpenStyle)
        {
            ConfirmFoldoutGuiStyles();
            return sm_FoldoutStyles[isOpenStyle];
        }

        private static void ConfirmFoldoutGuiStyles()
        {
            if (sm_FoldoutStyles.ContainsKey(true) && sm_FoldoutStyles.ContainsKey(false))
            {
                return;
            }

            GUIStyle guiStyle = new GUIStyle(EditorStyles.foldout);
            Color defaultTextColor = guiStyle.normal.textColor;
            guiStyle.hover.textColor = defaultTextColor;
            guiStyle.onHover.textColor = defaultTextColor;
            guiStyle.focused.textColor = defaultTextColor;
            guiStyle.onFocused.textColor = defaultTextColor;
            guiStyle.active.textColor = defaultTextColor;
            guiStyle.onActive.textColor = defaultTextColor;
            sm_FoldoutStyles[true] = guiStyle;

            guiStyle.margin.top += 1;
            guiStyle.margin.left += 11;
            sm_FoldoutStyles[false] = guiStyle;
        }
    }
}
