using DotEditor.GUIExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;

namespace DotEditor.Lua.Gen
{
    public class GenSelectionWindow : EditorWindow
    {
        [MenuItem("Game/XLua/2 Gen Selection",priority =2)]
        public static void ShowWin()
        {
            var win = GetWindow<GenSelectionWindow>();
            win.titleContent = new GUIContent("Gen Selection");
            win.Show();
        }

        private static readonly int TOOLBAR_HEIGHT = 18;
        private static readonly int TOOLBAR_BTN_HEIGHT = 40;

        private GenConfig genConfig;
        private List<AssemblyTypeData> assemblyTypes = new List<AssemblyTypeData>();

        private int toolbarSelectedIndex = 0;
        private GUIContent[] toolbarContents = new GUIContent[]
        {
            new GUIContent("LuaCallCSharp"),
            new GUIContent("CSharpCallLua"),
            new GUIContent("GCOptimize"),
            new GUIContent("BlackList"),
        };
        private GenTabViewer[] tabViewers = null;

        private SearchField searchField = null;
        private string searchText = string.Empty;

        private void OnEnable()
        {
            genConfig = GenConfig.GetConfig();
            LoadAssemblyTypes();
            tabViewers = new GenTabViewer[]
            {
                new LuaCallCSharpTabViewer(genConfig,assemblyTypes),
                new CSharpCallLuaTabViewer(genConfig,assemblyTypes),
                new GCOptimizeTabViewer(genConfig,assemblyTypes),
                new BlackListTabViewer(genConfig,assemblyTypes),
            };
        }

        private void LoadAssemblyTypes()
        {
            assemblyTypes.Clear();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                string assemblyName = assembly.GetName().Name;
                if(genConfig.AssemblyNames.IndexOf(assemblyName)<0)
                {
                    continue;
                }
                
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsNotPublic || type.IsInterface || type.IsGenericType || (!type.IsSealed && type.IsAbstract) || (type.IsNested && !type.IsNestedPublic))
                    {
                        continue;
                    }
                    AssemblyTypeData typeData = new AssemblyTypeData();
                    typeData.Type = type;
                    if (!type.IsEnum)
                    {
                        typeData.Fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();
                        typeData.Fields.Sort((item1, item2) =>
                        {
                            return item1.Name.CompareTo(item2.Name);
                        });

                        typeData.Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();
                        typeData.Properties.Sort((item1, item2) =>
                        {
                            return item1.Name.CompareTo(item2.Name);
                        });

                        typeData.Methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();
                        typeData.Methods.Sort((item1, item2) =>
                        {
                            return item1.Name.CompareTo(item2.Name);
                        });
                    }
                    assemblyTypes.Add(typeData);
                }

                if (assemblyTypes.Count > 0)
                {
                    assemblyTypes.Sort((item1, item2) =>
                    {
                        return item1.Type.FullName.CompareTo(item2.Type.FullName);
                    });
                }
            }
        }

        private void OnGUI()
        {
            if(searchField == null)
            {
                searchField = new SearchField();
                searchField.autoSetFocusOnFindCommand = true;
            }

            Rect toolbarRect = new Rect(0, 0, position.width, TOOLBAR_HEIGHT);
            DrawToolbar(toolbarRect);

            Rect tabRect = toolbarRect;
            tabRect.y += tabRect.height;
            tabRect.height = TOOLBAR_BTN_HEIGHT;

            toolbarSelectedIndex = GUI.Toolbar(tabRect,toolbarSelectedIndex, toolbarContents);

            Rect contentRect = new Rect(tabRect.x + 1, tabRect.y + tabRect.height + 1, position.width - 2, position.height - tabRect.y - tabRect.height - 2);
            EGUI.DrawAreaLine(contentRect, Color.black);
            tabViewers[toolbarSelectedIndex].OnGUI(contentRect);

            if(GUI.changed)
            {
                EditorUtility.SetDirty(genConfig);
            }
        }

        private void DrawToolbar(Rect rect)
        {
            EditorGUI.LabelField(rect, GUIContent.none, EditorStyles.toolbar);

            Rect settingRect = new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height);
            if(GUI.Button(settingRect,"Setting",EditorStyles.toolbarButton))
            {
                var win = GenAssemblyWindow.ShowWin();
                win.ClosedCallback = () =>
                {
                    LoadAssemblyTypes();
                    Repaint();
                };
            }

            Rect searchRect = settingRect;
            searchRect.x -= 160;
            searchRect.width = 160;
            string tempSearchText = searchField.OnToolbarGUI(searchRect, searchText);
            if(tempSearchText!=searchText)
            {
                searchText = tempSearchText;
                tabViewers[toolbarSelectedIndex].OnSearch(searchText);
            }

        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(genConfig);
            AssetDatabase.SaveAssets();
        }
        
        internal class AssemblyTypeData
        {
            public bool IsFoldout = false;
            public Type Type;
            public bool IsFieldFoldout = false;
            public List<FieldInfo> Fields = new List<FieldInfo>();
            public bool IsPropertyFoldout = false;
            public List<PropertyInfo> Properties = new List<PropertyInfo>();
            public bool IsMethodFoldout = false;
            public List<MethodInfo> Methods = new List<MethodInfo>();

            public bool IsStruct()
            {
                return Type.IsValueType && !Type.IsPrimitive;
            }

            public bool IsEnum()
            {
                return Type.IsEnum;
            }
        }
    }
}
