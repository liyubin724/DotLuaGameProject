using DotEditor.GUIExtension;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DotEditor.Lua.Gen.GenSelectionWindow;

namespace DotEditor.Lua.Gen
{
    internal class LuaCallCSharpTabViewer : GenTabViewer
    {
        private ReorderableList genericTypeRL = null;
        private Vector2 scrollPos = Vector2.zero;
        public LuaCallCSharpTabViewer(GenConfig config, List<AssemblyTypeData> data) : base(config, data)
        {
        }

        protected internal override void OnGUI(Rect rect)
        {
            if (genericTypeRL == null)
            {
                genericTypeRL = new ReorderableList(genConfig.callCSharpGenericTypeNames, typeof(string), true, true, true, true);
                genericTypeRL.elementHeight = EditorGUIUtility.singleLineHeight;
                genericTypeRL.drawHeaderCallback = (r) =>
                {
                    EditorGUI.LabelField(r, "Generic Type Names");
                };
                genericTypeRL.drawElementCallback = (r, index, isActive, isFocuse) =>
                {
                    Rect typeNameRect = new Rect(r.x, r.y, r.width - 60, r.height);
                    genConfig.callCSharpGenericTypeNames[index] = EditorGUI.TextField(typeNameRect, genConfig.callCSharpGenericTypeNames[index]);
                   
                    Rect btnRect = new Rect(r.x + r.width - 60, r.y, 60, r.height);
                    if(GUI.Button(btnRect,"Check"))
                    {
                        string typeName = genConfig.callCSharpGenericTypeNames[index];
                        bool isValid = true;
                        string errorMsg = string.Empty;

                        if (string.IsNullOrEmpty(typeName))
                        {
                            isValid = false;
                            errorMsg = "Name is empty";
                        }
                        else
                        {
                            string[] splitStr = typeName.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                            if (splitStr == null || splitStr.Length == 0)
                            {
                                isValid = false;
                                errorMsg = "The format of Name is Error";
                            }
                            else if (splitStr.Length <= 1)
                            {
                                isValid = false;
                                errorMsg = "The lenght of name is less than 2";
                            }
                            else
                            {
                                string gName = splitStr[0];
                                string[] pNames = new string[splitStr.Length - 1];
                                Array.Copy(splitStr, 1, pNames, 0, pNames.Length);
                                Type t = AssemblyUtility.GetGenericType(gName, pNames);
                                if (t == null)
                                {
                                    isValid = false;
                                    errorMsg = "Convert Failed";
                                }
                            }
                        }

                        if (!isValid)
                        {
                            EditorUtility.DisplayDialog("Failed", errorMsg, "OK");
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Success", "Success", "OK");
                        }
                    }
                    
                };
                genericTypeRL.onAddCallback = (list) =>
                {
                    list.list.Add(string.Empty);
                };
            }

            GUILayout.BeginArea(rect);
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
                {
                    EGUILayout.DrawBoxHeader("Generic Type List", GUILayout.ExpandWidth(true));
                    EditorGUILayout.LabelField("Example:List<int> == System.Collections.Generic.List`1@System.Int32");

                    genericTypeRL.DoLayoutList();
                    
                    EditorGUILayout.Space();

                    EGUILayout.DrawBoxHeader("Assembly Type List",GUILayout.ExpandWidth(true));
                    foreach(var typeData in assemblyTypes)
                    {
                        string fullName = typeData.Type.FullName;

                        if(!string.IsNullOrEmpty(searchText) && fullName.ToLower().IndexOf(searchText.ToLower())<0)
                        {
                            continue;
                        }

                        bool isSelected = genConfig.callCSharpTypeNames.IndexOf(fullName) >= 0;
                        bool tempIsSelected = EditorGUILayout.ToggleLeft(typeData.Type.FullName, isSelected);
                        if(tempIsSelected!=isSelected)
                        {
                            if(tempIsSelected)
                            {
                                genConfig.callCSharpTypeNames.Add(fullName);
                            }
                            else
                            {
                                genConfig.callCSharpTypeNames.Remove(fullName);
                            }
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }
    }
}
