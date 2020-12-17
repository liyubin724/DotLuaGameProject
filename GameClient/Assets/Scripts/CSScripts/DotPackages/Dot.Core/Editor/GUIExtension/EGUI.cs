using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExtension
{
    public static class EGUI
    {
        #region Draw lines
        /// <summary>
        /// 在指定的区域内绘制水平线
        /// </summary>
        /// <param name="rect"></param>
        public static void DrawHorizontalLine(Rect rect)
        {
            DrawHorizontalLine(rect, EGUIResources.gray);
        }

        /// <summary>
        /// 在指定的区域内绘制水平线
        /// </summary>
        /// <param name="rect">绘制区域</param>
        /// <param name="thickness">线宽</param>
        /// <param name="padding">与上方的间距</param>
        /// <param name="color">绘制使用的颜色</param>
        public static void DrawHorizontalLine(Rect rect , Color color , float thickness = 1.0f)
        {
            float padding = (rect.height - thickness) * 0.5f;
            padding = Mathf.Max(padding, 0.0f);
            rect.y += padding;
            rect.height = thickness;
            EditorGUI.DrawRect(rect, color);
        }

        /// <summary>
        /// 在指定区域内绘制垂直水平线
        /// </summary>
        /// <param name="rect"></param>
        public static void DrawVerticalLine(Rect rect)
        {
            DrawVerticalLine(rect, EGUIResources.gray);
        }

        public static void DrawVerticalLine(Rect rect, Color color, float thickness = 1.0f)
        {
            float padding = (rect.width - thickness) * 0.5f;
            padding = Mathf.Max(padding, 0.0f);
            rect.x += padding;
            rect.width = thickness;
            EditorGUI.DrawRect(rect, color);
        }

        public static void DrawAreaLine(Rect rect, Color color)
        {
            Handles.color = color;

            var points = new Vector3[] {
                new Vector3(rect.x, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y, 0),
                new Vector3(rect.x + rect.width, rect.y + rect.height, 0),
                new Vector3(rect.x, rect.y + rect.height, 0),
            };

            var indexies = new int[] {
                0, 1, 1, 2, 2, 3, 3, 0,
            };

            Handles.DrawLines(points, indexies);
        }

        #endregion

        /// <summary>
        /// 绘制对象的预览图
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="uObj"></param>
        public static void DrawAssetPreview(Rect rect,UnityObject uObj)
        {
            var previewTexture = AssetPreview.GetAssetPreview(uObj);
            if(previewTexture!=null)
            {
                EditorGUI.LabelField(rect, GUIContent.none, EGUIStyles.GetTextureStyle(previewTexture));
            }
        }

        public static void DrawBox(Rect rect)
        {
            GUIStyle boxStyle = EGUIStyles.BoxStyle;
            boxStyle.Draw(rect, false, false, false, false);
        }

        public static void DrawBoxHeader(Rect rect,string label)
        {
            DrawBoxHeader(rect, label, EGUIStyles.BoldLabelStyle);
        }

        public static void DrawBoxHeader(Rect rect,string label,GUIStyle labelStyle)
        {
            GUI.Box(rect, GUIContent.none, EditorStyles.helpBox);
            EditorGUI.LabelField(rect, label, labelStyle);
        }

        public static bool DrawBoxedFoldout(Rect rect,bool isFoldout,string title)
        {
            GUI.Box(rect, GUIContent.none, EditorStyles.toolbar);
            return EditorGUI.Foldout(rect, isFoldout, title, true);
        }

        public static bool DrawBoxedFoldout(Rect rect, bool isFoldout, GUIContent title)
        {
            GUI.Box(rect, GUIContent.none, EditorStyles.toolbar);
            return EditorGUI.Foldout(rect, isFoldout, title, true);
        }

        #region Label Width
        private static Stack<float> labelWidthStack = new Stack<float>();
        public static void BeginLabelWidth(float labelWidth)
        {
            labelWidthStack.Push(EditorGUIUtility.labelWidth);
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public static void EndLableWidth()
        {
            if (labelWidthStack.Count > 0)
                EditorGUIUtility.labelWidth = labelWidthStack.Pop();
        }
        #endregion

        #region GUI Color

        private static Stack<Color> guiColorStack = new Stack<Color>();
        public static void BeginGUIColor(Color color)
        {
            guiColorStack.Push(UnityEngine.GUI.color);
            UnityEngine.GUI.color = color;
        }
        public static void EndGUIColor()
        {
            if (guiColorStack.Count > 0)
                UnityEngine.GUI.color = guiColorStack.Pop();
        }
        #endregion

        #region GUI Background Color
        private static Stack<Color> guiBgColorStack = new Stack<Color>();
        public static void BeginGUIBackgroundColor(Color color)
        {
            guiBgColorStack.Push(UnityEngine.GUI.backgroundColor);
            UnityEngine.GUI.backgroundColor = color;
        }
        public static void EndGUIBackgroundColor()
        {
            if (guiBgColorStack.Count > 0)
                UnityEngine.GUI.backgroundColor = guiBgColorStack.Pop();
        }

        #endregion

        #region GUI Content Color
        private static Stack<Color> guiContentColorStack = new Stack<Color>();
        public static void BeginGUIContentColor(Color color)
        {
            guiContentColorStack.Push(UnityEngine.GUI.contentColor);
            UnityEngine.GUI.contentColor = color;
        }
        public static void EndGUIContentColor()
        {
            if (guiContentColorStack.Count > 0)
                UnityEngine.GUI.contentColor = guiContentColorStack.Pop();
        }

        #endregion

        #region Indent
        public static void BeginIndent()
        {
            EditorGUI.indentLevel++;
        }

        public static void EndIndent()
        {
            EditorGUI.indentLevel--;
        }
        #endregion

        public static T DrawPopup<T>(Rect rect, string label, string[] contents, T[] values, T selectedValue)
        {
            int index = Array.IndexOf(values, selectedValue);
            if (index < 0) index = 0;
            int newIndex = EditorGUI.Popup(rect, label, index, contents);

            return values[newIndex];
        }

        public static string DrawAssetFolderSelection(Rect rect, string label, string assetFolder, bool isReadonly = true)
        {
            string folder = assetFolder;

            EditorGUI.BeginDisabledGroup(isReadonly);
            {
                folder = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width - 40, rect.height), label, assetFolder);
            }
            EditorGUI.EndDisabledGroup();

            if (UnityEngine.GUI.Button(new Rect(rect.x + rect.width - 40, rect.y, 20, rect.height), new GUIContent(EGUIResources.DefaultFolderIcon)))
            {
                string folderPath = EditorUtility.OpenFolderPanel("folder", folder, "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    folder = PathUtility.GetAssetPath(folderPath);
                }
            }
            if (UnityEngine.GUI.Button(new Rect(rect.x + rect.width - 20, rect.y, 20, rect.height), "\u2716"))
            {
                folder = "";
            }
            return folder;
        }

        public static void DrawAssetFolderSelection(Rect rect,SerializedProperty property,bool isReadonly = true)
        {
            Rect drawRect = new Rect(rect.x, rect.y, rect.width - rect.height * 2, rect.height);
            EditorGUI.BeginDisabledGroup(isReadonly);
            {
                EditorGUI.PropertyField(drawRect,property);
            }
            EditorGUI.EndDisabledGroup();

            drawRect.x += drawRect.width;
            drawRect.width = rect.height;
            if (UnityEngine.GUI.Button(drawRect, new GUIContent(EGUIResources.DefaultFolderIcon)))
            {
                string folderPath = EditorUtility.OpenFolderPanel("folder", property.stringValue, "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    property.stringValue = PathUtility.GetAssetPath(folderPath);
                }
            }
            drawRect.x += drawRect.width;
            if (UnityEngine.GUI.Button(drawRect, "\u2716"))
            {
                property.stringValue = "";
            }
        }
    }

    public class LabelWidthScope : UnityEngine.GUI.Scope
    {
        private float cachedLabelWidth;
        public LabelWidthScope(float labelWidth)
        {
            cachedLabelWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = labelWidth;
        }

        protected override void CloseScope()
        {
            EditorGUIUtility.labelWidth = cachedLabelWidth;
        }
    }

    public class IndentScope : UnityEngine.GUI.Scope
    {
        private int indent = 1;
        public IndentScope()
        {
            EditorGUI.indentLevel += indent;
        }

        public IndentScope(int indent)
        {
            this.indent = indent;
            EditorGUI.indentLevel += indent;
        }

        protected override void CloseScope()
        {
            EditorGUI.indentLevel -= indent;
        }
    }

    public class GUIColorSope : UnityEngine.GUI.Scope
    {
        private Color cachedGUIColor;
        public GUIColorSope(Color color)
        {
            cachedGUIColor = UnityEngine.GUI.color;

            UnityEngine.GUI.color = color;
        }

        protected override void CloseScope()
        {
            UnityEngine.GUI.color = cachedGUIColor;
        }
    }
}
