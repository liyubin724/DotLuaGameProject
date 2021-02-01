using DotEditor.GUIExtension;
using DotEngine.Config.Ini;
using ReflectionMagic;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Config.Ini
{
    public class IniConfigWindow :EditorWindow
    {
        [MenuItem("Game/Ini Window")]
        private static IniConfigWindow ShowWin()
        {
            IniConfigWindow win = EditorWindow.GetWindow<IniConfigWindow>();
            win.titleContent = Contents.WinTitleContent;
            win.Show();
            return win;
        }

        private IniConfig iniConfig = null;
        private string configFilePath = null;
        private void LoadConfig(string filePath)
        {
            iniConfig = null;
            configFilePath = null;

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                configFilePath = filePath;
                iniConfig = IniConfigUtil.ReadConfigFrom(filePath);
            }

            Repaint();
        }

        private DeleteData deleteData = null;
        private void OnGUI()
        {
            DrawToolbar();

            if(iniConfig!=null)
            {
                dynamic config = iniConfig.AsDynamic();
                Dictionary<string, IniGroup> groupDic = config.groupDic;

                if(deleteData!=null)
                {
                    if(string.IsNullOrEmpty(deleteData.dataKey))
                    {
                        if(groupDic.ContainsKey(deleteData.groupName))
                        {
                            groupDic.Remove(deleteData.groupName);
                        }
                    }else
                    {
                        if(groupDic.TryGetValue(deleteData.groupName,out IniGroup g))
                        {
                            g.DeleteData(deleteData.dataKey);
                        }
                    }

                    deleteData = null;
                }

                foreach(var kvp in groupDic)
                {
                    DrawGroup(kvp.Value);
                }
            }
        }

        private ToolbarSearchField searchField = null;
        private string searchText = string.Empty;
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar,GUILayout.ExpandWidth(true));
            {
                if (EGUILayout.ToolbarButton(Contents.OpenStr))
                {
                    string filePath = EditorUtility.OpenFilePanel(Contents.OpenStr, Application.dataPath, "txt");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        LoadConfig(filePath);
                    }
                }
                
                if (EGUILayout.ToolbarButton(Contents.NewStr))
                {
                    configFilePath = null;
                    iniConfig = new IniConfig();
                }
                if(iniConfig!=null)
                {
                    if (EGUILayout.ToolbarButton(Contents.SaveStr))
                    {
                        string filePath = configFilePath;

                        if (string.IsNullOrEmpty(filePath))
                        {
                            filePath = EditorUtility.SaveFilePanel(Contents.SaveStr, Application.dataPath, "ini_config", "txt");
                        }

                        if(!string.IsNullOrEmpty(filePath))
                        {
                            configFilePath = filePath;
                            IniConfigUtil.WriteConfigTo(filePath, iniConfig);
                        }
                    }
                }

                GUILayout.FlexibleSpace();

                if(iniConfig!=null)
                {
                    if (EGUILayout.ToolbarButton(Contents.AddGroupContent,70))
                    {
                        PopupWindow.Show(new Rect(Event.current.mousePosition,Vector2.zero), new CreateIniGroupContent(iniConfig, (groupName)=>
                        {
                            Repaint();
                        }));
                    }
                }

                if(searchField == null)
                {
                    searchField = new ToolbarSearchField((text) =>
                    {
                        searchText = text==null?"":text.ToLower();
                    },null);
                }
                searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawGroup(IniGroup group)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    EditorGUILayout.LabelField(new GUIContent(group.Name, group.Comment));

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(Contents.DeleteGroupContent, EditorStyles.toolbarButton, GUILayout.Width(80)))
                    {
                        deleteData = new DeleteData() { groupName = group.Name };
                    }

                    if (GUILayout.Button(Contents.AddDataContent,EditorStyles.toolbarButton,GUILayout.Width(80)))
                    {
                        PopupWindow.Show(new Rect(Event.current.mousePosition, Vector2.zero), new CreateIniDataContent(group, (groupName,dataKey) =>
                        {
                            Repaint();
                        }));
                    }
                }
                EditorGUILayout.EndHorizontal();

                foreach(var dKVP in group.AsDynamic().dataDic)
                {
                    if(string.IsNullOrEmpty(searchText) || dKVP.Key.ToLower().IndexOf(searchText) >= 0)
                    {
                        DrawData(group, dKVP.Value);
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawData(IniGroup group,IniData data)
        {
            string value = data.Value;
            
            EditorGUILayout.BeginHorizontal();
            {
                if(data.OptionValues!=null && data.OptionValues.Length>0)
                {
                    value = EGUILayout.StringPopup(new GUIContent(data.Key,data.Comment), data.Value, data.OptionValues);
                }else
                {
                    value = EditorGUILayout.TextField(new GUIContent(data.Key, data.Comment), data.Value);
                }

                if(GUILayout.Button(Contents.DeleteContent,GUILayout.Width(20)))
                {
                    deleteData = new DeleteData() { groupName = group.Name, dataKey = data.Key };
                }
            }
            EditorGUILayout.EndHorizontal();

            if(data.Value!=value)
            {
                data.Value = value;
            }
        }

        class DeleteData
        {
            public string groupName;
            public string dataKey;
        }

        static class Contents
        {
            internal static GUIContent WinTitleContent = new GUIContent("Ini Config");
            internal static GUIContent ChangedWinTitleContent = new GUIContent("Ini Config *");

            internal static string OpenStr = "Open";
            internal static string NewStr = "New";
            internal static string SaveStr = "Save";

            internal static GUIContent AddGroupContent = new GUIContent("Add Group", "Add a new group");
            internal static GUIContent DeleteGroupContent = new GUIContent("Delete Group", "Delete the group");
            internal static GUIContent AddDataContent = new GUIContent("Add Data", "Add a new data");
            internal static GUIContent DeleteContent = new GUIContent("-");
        }
    }
}
