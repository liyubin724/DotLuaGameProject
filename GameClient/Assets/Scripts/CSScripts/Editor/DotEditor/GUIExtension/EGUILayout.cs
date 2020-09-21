using DotEditor.GUIExtension.RList;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.GUIExtension
{
    public static class EGUILayout
    {
        #region DrawLine
        public static void DrawHorizontalLine()
        {
            DrawHorizontalLine(EGUIResources.gray);
        }

        public static void DrawHorizontalLine(Color color,float thickness = 0.75f, float padding = 6.0f)
        {
            Rect rect = EditorGUILayout.GetControlRect(UnityEngine.GUILayout.Height(padding + thickness), UnityEngine.GUILayout.ExpandWidth(true));
            EGUI.DrawHorizontalLine(rect, color,thickness);
        }

        public static void DrawVerticalLine()
        {
            DrawVerticalLine(EGUIResources.gray);
        }

        public static void DrawVerticalLine(Color color,float thickness = 0.75f, float padding = 6.0f )
        {
            Rect rect = EditorGUILayout.GetControlRect(UnityEngine.GUILayout.Width(padding + thickness), UnityEngine.GUILayout.ExpandHeight(true));
            EGUI.DrawVerticalLine(rect, color,thickness);
        }

        #endregion

        public static void DrawAssetPreview(UnityObject uObj,float width = 64,float height = 64)
        {
            var previewTexture = AssetPreview.GetAssetPreview(uObj);
            if(previewTexture!=null)
            {
                width = Mathf.Clamp(width, 0, previewTexture.width);
                height = Mathf.Clamp(height, 0, previewTexture.height);
                var previewOptions = new GUILayoutOption[]
                {
                    UnityEngine.GUILayout.MaxWidth(width),
                    UnityEngine.GUILayout.MaxHeight(height),
                };
                Rect rect = EditorGUILayout.GetControlRect(true, height, previewOptions);
                EditorGUI.LabelField(rect, GUIContent.none, EGUIStyles.GetTextureStyle(previewTexture));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="options"></param>
        public static void DrawBoxHeader(string label,params GUILayoutOption[] options)
        {
            DrawBoxHeader(label, EGUIStyles.BoxedHeaderStyle,options);
        }

        public static void DrawBoxHeader(string label, GUIStyle style,params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, style, options);
        }

        public static void DrawScript(UnityObject target)
        {
            Type targetType = target.GetType();
            EditorGUI.BeginDisabledGroup(true);
            {
                if(typeof(MonoBehaviour).IsAssignableFrom(targetType))
                {
                    EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(MonoScript), false);
                }else if(typeof(ScriptableObject).IsAssignableFrom(targetType))
                {
                    EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(MonoScript), false);
                }else
                {
                    EditorGUILayout.LabelField("Script", targetType.FullName);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        #region Draw Enum As Button
        public static object DrawEnumButton(string label,Enum value,params GUILayoutOption[] options)
        {
            Type valueType = value.GetType();
            var flagAttrs = valueType.GetCustomAttributes(typeof(FlagsAttribute), false);
            bool isFlagEnum = false;
            if (flagAttrs != null && flagAttrs.Length > 0)
            {
                isFlagEnum = true;
            }

            int enumValue = Convert.ToInt32(value);
            if (!isFlagEnum)
            {
                enumValue = DrawEnumButton(label, valueType, enumValue,options);
            }else
            {
                enumValue = DrawFlagsEnumButton(label, valueType, enumValue,options);
            }

            return Enum.ToObject(valueType, enumValue);
        }


        private static int DrawFlagsEnumButton(string label,Type valueType,int value, params GUILayoutOption[] options)
        {
            string[] enumNames = Enum.GetNames(valueType);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Everything", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    value = 0;
                    for (int i = 0; i < enumNames.Length; ++i)
                    {
                        int tValue = Convert.ToInt32(Enum.Parse(valueType, enumNames[i]));
                        value |= tValue;
                    }
                }
                if (GUILayout.Button("Nothing", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    value = 0;
                }
            }
            EditorGUILayout.EndHorizontal();
            EGUI.BeginIndent();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    for (int i = 0; i < enumNames.Length; ++i)
                    {
                        int tValue = Convert.ToInt32(Enum.Parse(valueType, enumNames[i]));

                        bool isSelected = (value & tValue) > 0;
                        bool newIsSelected = GUILayout.Toggle(isSelected, enumNames[i], EditorStyles.toolbarButton,options);
                        if (newIsSelected != isSelected)
                        {
                            if (newIsSelected)
                            {
                                value |= tValue;
                            }
                            else
                            {
                                value &= ~tValue;
                            }
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EGUI.EndIndent();

            return value;
        }

        private static int DrawEnumButton(string label, Type valueType, int value, params GUILayoutOption[] options)
        {
            string[] enumNames = Enum.GetNames(valueType);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label);

                for (int i = 0; i < enumNames.Length; ++i)
                {
                    int tValue = Convert.ToInt32(Enum.Parse(valueType, enumNames[i]));

                    bool isSelected = tValue == value;

                    bool newIsSelected = GUILayout.Toggle(isSelected, enumNames[i], EditorStyles.toolbarButton,options);
                    if (newIsSelected != isSelected && newIsSelected)
                    {
                        value = tValue;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

        #endregion Draw Enum As Button

        #region Draw Open File Or  Folder

        public static string DrawOpenFileWithFilter(string label,string value,string[] filters,bool isAbsolute = false)
        {
            EditorGUILayout.BeginHorizontal();
            {
                value = EditorGUILayout.TextField(label, value);

                if (GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), GUIStyle.none, GUILayout.Width(17), GUILayout.Height(17)))
                {
                    string dir = "";
                    if (!string.IsNullOrEmpty(value))
                    {
                        dir = Path.GetDirectoryName(value);
                    }

                    string filePath = EditorUtility.OpenFilePanelWithFilters("Select file", dir, filters);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (isAbsolute)
                        {
                            value = filePath.Replace("\\", "/");
                        }
                        else
                        {
                            value = PathUtility.GetAssetPath(filePath);
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            return value;
        }

        public static string DrawOpenFile(string label,string value,string extension = "", bool isAbsolute = false)
        {
            EditorGUILayout.BeginHorizontal();
            {
                value = EditorGUILayout.TextField(label, value);

                if (GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), GUIStyle.none, GUILayout.Width(17), GUILayout.Height(17)))
                {
                    string dir = "";
                    if(!string.IsNullOrEmpty(value))
                    {
                        dir = Path.GetDirectoryName(value);
                    }

                    string filePath = EditorUtility.OpenFilePanel("Select File", dir, extension);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if(isAbsolute)
                        {
                            value = filePath.Replace("\\", "/");
                        }else
                        {
                            value = PathUtility.GetAssetPath(filePath);
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

        public static string DrawOpenFolder(string label,string value,bool isAbsolute = false)
        {
            EditorGUILayout.BeginHorizontal();
            {
                value = EditorGUILayout.TextField(label, value);

                if (GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), GUIStyle.none, GUILayout.Width(17), GUILayout.Height(17)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("Open Folder", value, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        if(isAbsolute)
                        {
                            value = folderPath.Replace("\\", "/");
                        }else
                        {
                            value = PathUtility.GetAssetPath(folderPath);
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            return value;
        }

        #endregion Draw Open File Or  Folder

        public static T DrawPopup<T>(string label, string[] contents, T[] values, T selectedValue)
        {
            int index = Array.IndexOf(values, selectedValue);
            if (index < 0) index = 0;
            int newIndex = EditorGUILayout.Popup(label, index, contents);

            return values[newIndex];
        }

        public static void DrawAssetFolderSelection(SerializedProperty property, bool isReadonly = true)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUILayout.PropertyField(property);
                }
                EditorGUI.EndDisabledGroup();

                if (UnityEngine.GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", property.stringValue, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        property.stringValue = PathUtility.GetAssetPath(folderPath);
                    }
                }
                if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    property.stringValue = "";
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        public static string DrawAssetFolderSelection(string label, string assetFolder, bool isReadonly = true)
        {
            string folder = assetFolder;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    folder = EditorGUILayout.TextField(label, assetFolder);
                }
                EditorGUI.EndDisabledGroup();

                if (UnityEngine.GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    string folderPath = EditorUtility.OpenFolderPanel("folder", folder, "");
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        folder = PathUtility.GetAssetPath(folderPath);
                    }
                }
                if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    folder = "";
                }
            }
            EditorGUILayout.EndHorizontal();
            return folder;
        }

        public static string DrawDiskFolderSelection(string label, string diskFolder, bool isReadonly = true)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(isReadonly);
                {
                    EditorGUILayout.TextField(label, diskFolder);
                }
                EditorGUI.EndDisabledGroup();

                if (UnityEngine.GUILayout.Button(new GUIContent(EGUIResources.DefaultFolderIcon), UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    diskFolder = EditorUtility.OpenFolderPanel("folder", diskFolder, "");
                }
                if (UnityEngine.GUILayout.Button("\u2716", UnityEngine.GUILayout.Width(20), UnityEngine.GUILayout.Height(20)))
                {
                    diskFolder = "";
                }
            }
            EditorGUILayout.EndHorizontal();

            return diskFolder;
        }

        public static string StringPopup(string label, string selected, string[] optionValues)
        {
            if (optionValues == null)
            {
                optionValues = new string[0];
            }

            int selectedIndex = Array.IndexOf(optionValues, selected);

            int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, optionValues);
            if (newSelectedIndex >= 0 && newSelectedIndex < optionValues.Length)
            {
                return optionValues[newSelectedIndex];
            }
            return selected;
        }

        public static string StringPopup(GUIContent label, string selected, string[] optionValues)
        {
            if (optionValues == null)
            {
                optionValues = new string[0];
            }

            int selectedIndex = Array.IndexOf(optionValues, selected);

            int newSelectedIndex = EditorGUILayout.Popup(label, selectedIndex, optionValues);
            if (newSelectedIndex >= 0 && newSelectedIndex < optionValues.Length)
            {
                return optionValues[newSelectedIndex];
            }
            return selected;
        }

         public static bool ToolbarButton(string text,float width = 60)
        {
            return ToolbarButton(new GUIContent(text), width);
        }

        public static bool ToolbarButton(GUIContent content,float width = 60)
        {
            return UnityEngine.GUILayout.Button(content, EditorStyles.toolbarButton, UnityEngine.GUILayout.Width(width));
        }

        //private static Dictionary<string, WeakReference<ReorderableListProperty>> sm_RListPropertyDic = new Dictionary<string, WeakReference<ReorderableListProperty>>();
        //public static void DrawRListProperty(SerializedProperty property)
        //{
        //    string propertyPath = $"{property.propertyPath}${property.name}";
            
        //    if(property.isArray) 
        //    {
        //        if(sm_RListPropertyDic.TryGetValue(propertyPath, out var weakRef))
        //        {
        //            if(weakRef.TryGetTarget(out var rlProperty))
        //            {
        //                rlProperty.OnGUILayout();
        //                return;
        //            }
        //        } 

        //        ReorderableListProperty rlp = new ReorderableListProperty(property);
        //        if(weakRef!=null)
        //        {
        //            weakRef.SetTarget(rlp);
        //        }else
        //        {
        //            sm_RListPropertyDic.Add(propertyPath, new WeakReference<ReorderableListProperty>(rlp));
        //        }
        //        rlp.OnGUILayout();
        //        return;
        //    }
        //    else
        //    {
        //        GUILayout.Label("Error:the property is not array");
        //    }
        //}
    }
}
