using DotEditor.GUIExtension;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DotEditor.Lua.Gen.GenSelectionWindow;

namespace DotEditor.Lua.Gen
{
    internal class GCOptimizeTabViewer : GenTabViewer
    {
        private Vector2 scrollPos = Vector2.zero;
        public GCOptimizeTabViewer(GenConfig config, List<AssemblyTypeData> data) : base(config, data)
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
                        if(!typeData.IsStruct() && !typeData.IsEnum())
                        {
                            continue;
                        }
                        string fullName = typeData.Type.FullName;
                        if (!string.IsNullOrEmpty(searchText) && fullName.ToLower().IndexOf(searchText.ToLower()) < 0)
                        {
                            continue;
                        }
                        if (genConfig.callCSharpTypeNames.IndexOf(fullName)<0 && genConfig.callLuaTypeNames.IndexOf(fullName)<0)
                        {
                            continue;
                        }
                        
                        bool isSelected = genConfig.optimizeTypeNames.IndexOf(fullName) >= 0;
                        bool tempIsSelected = EditorGUILayout.ToggleLeft(typeData.Type.FullName, isSelected);
                        if (tempIsSelected != isSelected)
                        {
                            if (tempIsSelected)
                            {
                                genConfig.optimizeTypeNames.Add(fullName);
                            }
                            else
                            {
                                genConfig.optimizeTypeNames.Remove(fullName);
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
