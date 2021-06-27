using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenAssemblyWindow : EditorWindow
    {
        [MenuItem("Game/XLua/1 Gen Assembly Setting",priority =1)]
        public static GenAssemblyWindow ShowWin()
        {
            var win = GetWindow<GenAssemblyWindow>();
            win.titleContent = new GUIContent("Assembly Setting");
            win.Show();
            return win;
        }
        
        public Action ClosedCallback { get; set; }

        private GenConfig genConfig = null;
        private List<string> allAssemblyNames = new List<string>();
        private void OnEnable()
        {
            genConfig = GenConfig.GetConfig();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                string name = assembly.GetName().Name;
                if (name.IndexOf("Editor") >= 0)
                {
                    continue;
                }

                allAssemblyNames.Add(name);
            }

            allAssemblyNames.Sort((item1, item2) =>
            {
                return item1.CompareTo(item2);
            });
        }

        private Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);
            {
                foreach (var name in allAssemblyNames)
                {
                    Rect rect = GUILayoutUtility.GetRect(new GUIContent(name), EGUIStyles.BoxedHeaderStyle, GUILayout.ExpandWidth(true));
                    EditorGUI.LabelField(rect, GUIContent.none, EGUIStyles.BoxedHeaderStyle);
                    bool isSelected = genConfig.AssemblyNames.IndexOf(name) >= 0;
                    bool tempIsSelected = EditorGUI.ToggleLeft(rect, name, isSelected);
                    if(tempIsSelected!=isSelected)
                    {
                        if(tempIsSelected)
                        {
                            genConfig.AssemblyNames.Add(name);
                        }else
                        {
                            genConfig.AssemblyNames.Remove(name);
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();


            if (GUI.changed)
            {
                EditorUtility.SetDirty(genConfig);
            }
        }

        private void OnDestroy()
        {
            ClosedCallback?.Invoke();
        }

        private void OnLostFocus()
        {
            if(ClosedCallback!=null)
            {
                Close();
            }
        }
    }
}
