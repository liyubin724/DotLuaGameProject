using DotEditor.GUIExtension;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static DotEditor.Lua.Gen.GenSelectionWindow;

namespace DotEditor.Lua.Gen
{
    internal class BlackListTabViewer : GenTabViewer
    {
        private Vector2 scrollPos = Vector2.zero;
        public BlackListTabViewer(GenConfig config, List<AssemblyTypeData> data) : base(config, data)
        {
        }

        protected internal override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
                {
                    EGUILayout.DrawBoxHeader("Assembly Type List", GUILayout.ExpandWidth(true));
                    foreach (var typeData in assemblyTypes)
                    {
                        string fullName = typeData.Type.FullName;

                        if(typeData.IsEnum())
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(searchText) && fullName.ToLower().IndexOf(searchText.ToLower()) < 0)
                        {
                            continue;
                        }
                        if (genConfig.callCSharpTypeNames.IndexOf(fullName) < 0 && genConfig.callLuaTypeNames.IndexOf(fullName) < 0)
                        {
                            continue;
                        }
                        EGUI.BeginIndent();
                        {
                            typeData.IsFoldout = EditorGUILayout.Foldout(typeData.IsFoldout, fullName,true);
                            if(typeData.IsFoldout)
                            {
                                EGUI.BeginIndent();
                                {
                                    Rect fieldRect = GUILayoutUtility.GetRect(new GUIContent("Fields"), EGUIStyles.BoxedHeaderStyle, GUILayout.ExpandWidth(true));
                                    EditorGUI.LabelField(fieldRect, GUIContent.none, EGUIStyles.BoxedHeaderStyle);
                                    typeData.IsFieldFoldout = EditorGUI.Foldout(fieldRect, typeData.IsFieldFoldout, "Fields");
                                    if(typeData.IsFieldFoldout)
                                    {
                                        EGUI.BeginIndent();
                                        {
                                            foreach (var field in typeData.Fields)
                                            {
                                                string fieldName = $"{typeData.Type.FullName}${field.Name}";
                                                bool isSelected = genConfig.blackDatas.IndexOf(fieldName) >= 0;
                                                bool tempIsSelected = EditorGUILayout.ToggleLeft(fieldName, isSelected);
                                                if (isSelected != tempIsSelected)
                                                {
                                                    if (tempIsSelected)
                                                    {
                                                        genConfig.blackDatas.Add(fieldName);
                                                    }
                                                    else
                                                    {
                                                        genConfig.blackDatas.Remove(fieldName);
                                                    }
                                                }
                                            }
                                        }
                                        EGUI.EndIndent();
                                    }
                                    

                                    Rect propertyRect = GUILayoutUtility.GetRect(new GUIContent("Properties"), EGUIStyles.BoxedHeaderStyle, GUILayout.ExpandWidth(true));
                                    EditorGUI.LabelField(propertyRect, GUIContent.none, EGUIStyles.BoxedHeaderStyle);
                                    typeData.IsPropertyFoldout = EditorGUI.Foldout(propertyRect, typeData.IsPropertyFoldout, "Properties");
                                    if(typeData.IsPropertyFoldout)
                                    {
                                        EGUI.BeginIndent();
                                        {
                                            foreach (var property in typeData.Properties)
                                            {
                                                string propertyName = $"{typeData.Type.FullName}${property.Name}";
                                                bool isSelected = genConfig.blackDatas.IndexOf(propertyName) >= 0;
                                                bool tempIsSelected = EditorGUILayout.ToggleLeft(propertyName, isSelected);
                                                if (isSelected != tempIsSelected)
                                                {
                                                    if (tempIsSelected)
                                                    {
                                                        genConfig.blackDatas.Add(propertyName);
                                                    }
                                                    else
                                                    {
                                                        genConfig.blackDatas.Remove(propertyName);
                                                    }
                                                }
                                            }
                                        }
                                        EGUI.EndIndent();
                                    }
                                    

                                    Rect methodRect = GUILayoutUtility.GetRect(new GUIContent("Methods"), EGUIStyles.BoxedHeaderStyle, GUILayout.ExpandWidth(true));
                                    EditorGUI.LabelField(methodRect, GUIContent.none, EGUIStyles.BoxedHeaderStyle);
                                    typeData.IsMethodFoldout = EditorGUI.Foldout(methodRect, typeData.IsMethodFoldout, "Methods");
                                    if(typeData.IsMethodFoldout)
                                    {
                                        EGUI.BeginIndent();
                                        {
                                            foreach (var method in typeData.Methods)
                                            {
                                                if (method.Name.StartsWith("set_") || method.Name.StartsWith("get_") || method.Name.StartsWith("op_"))
                                                {
                                                    continue;
                                                }

                                                ParameterInfo[] paramInfos = method.GetParameters();
                                                string paramStr = "";
                                                foreach (var pi in paramInfos)
                                                {
                                                    paramStr += $"@{pi.ParameterType.FullName}";
                                                }

                                                string methodName = $"{typeData.Type.FullName}${method.Name}{paramStr}";
                                                bool isSelected = genConfig.blackDatas.IndexOf(methodName) >= 0;
                                                bool tempIsSelected = EditorGUILayout.ToggleLeft(methodName, isSelected);
                                                if (isSelected != tempIsSelected)
                                                {
                                                    if (tempIsSelected)
                                                    {
                                                        genConfig.blackDatas.Add(methodName);
                                                    }
                                                    else
                                                    {
                                                        genConfig.blackDatas.Remove(methodName);
                                                    }
                                                }
                                            }
                                        }
                                        EGUI.EndIndent();
                                    }
                                    
                                }
                                EGUI.EndIndent();
                            }
                        }
                        EGUI.EndIndent();
                        
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
    }
}
