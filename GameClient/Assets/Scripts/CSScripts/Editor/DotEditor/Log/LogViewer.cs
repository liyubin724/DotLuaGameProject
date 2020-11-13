using DotEditor.GUIExtension;
using DotEditor.GUIExtension.DataGrid;
using DotEngine.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Log
{
    public class LogData
    {
        public LogLevel Level { get; set; }
        public DateTime Time { get; set; }
        public string Tag { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public class LogLevelBtnState
    {
        public LogLevel Level { get; set; }
        public bool IsEnable { get; set; } = true;
    }

    public class LogViewer : EditorWindow
    {
        [MenuItem("Game/Log/Viewer")]
        public static void ShowWin()
        {
            var viewer = GetWindow<LogViewer>();
            viewer.titleContent = new GUIContent("Log Viewer");

            viewer.Show();
        }

        public static readonly int PORT = 8899;

        [SerializeField]
        private string ipAddressString = "127.0.0.1";

        private LogClientStatus clientStatus = LogClientStatus.None;
        private LogClientSocket clientSocket = null;

        private List<LogData> logDatas = new List<LogData>();
        private GridViewModel gridViewModel;
        private LogGridView gridView = null;

        private LogData selectedLogData = null;
        private string selectedLogDataText = string.Empty;
        private Vector2 selectedLogDataScrollPos = Vector2.zero;

        private ToolbarSearchField searchField = null;
        private string[] searchCategories = new string[]
        {
            "all",
            "tag",
            "message",
            "stacktrace"
        };
        private string searchCategory = "all";
        private string searchText = string.Empty;

        private Dictionary<LogLevel, bool> levelBtnEnableStateDic = new Dictionary<LogLevel, bool>();
        private Dictionary<LogLevel, int> levelLogCountDic = new Dictionary<LogLevel, int>();

        private void OnEnable()
        {
            gridViewModel = new GridViewModel();
            for(int i =(int)LogLevel.On+1;i<(int)LogLevel.Off; ++i)
            {
                levelBtnEnableStateDic.Add((LogLevel)i, true);
                levelLogCountDic.Add((LogLevel)i, 0);
            }

            EditorApplication.update += OnUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnUpdate;
        }

        private void OnGUI()
        {
            if(gridView == null)
            {
                gridView = new LogGridView(gridViewModel);
                gridView.OnSelectedChanged += (logData) =>
                {
                    selectedLogData = logData;
                    selectedLogDataScrollPos = Vector2.zero;
                    if (selectedLogData!=null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Level:{selectedLogData.Level}");
                        sb.AppendLine($"Time:{selectedLogData.Time}");
                        sb.AppendLine($"Tag:{selectedLogData.Tag}");
                        sb.AppendLine($"Message:{selectedLogData.Message}");
                        if(!string.IsNullOrEmpty(selectedLogData.StackTrace))
                        {
                            sb.AppendLine($"StackTrace:\n{selectedLogData.StackTrace}");
                        }
                        selectedLogDataText = sb.ToString();
                    }
                    else
                    {
                        selectedLogDataText = string.Empty;
                    }
                };
            }

            DrawToolbar();
            DrawGridView();

            selectedLogDataScrollPos = EditorGUILayout.BeginScrollView(selectedLogDataScrollPos,GUILayout.Height(160),GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.SelectableLabel(selectedLogDataText,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndScrollView();
        }

        void FilterLogDatas()
        {
            Debug.Log("SSSSSSSSSSSSSS====" + Thread.CurrentThread.ManagedThreadId);

            gridViewModel.Clear();

            foreach(var key in levelLogCountDic.Keys)
            {
                levelLogCountDic[key] = 0;
            }

            foreach(var logData in logDatas)
            {
                levelLogCountDic[logData.Level] += 1;
                if(!levelBtnEnableStateDic[logData.Level])
                {
                    continue;
                }

                if (string.IsNullOrEmpty(searchText))
                {
                    gridViewModel.AddData(new GridViewData("", logData));
                }else
                {
                    if((searchCategory == "all" || searchCategory == "tag") && logData.Tag.ToLower().IndexOf(searchText)>=0)
                    {
                        gridViewModel.AddData(new GridViewData("", logData));
                    }else if((searchCategory == "all" || searchCategory == "message") && logData.Message.ToLower().IndexOf(searchText) >= 0)
                    {
                        gridViewModel.AddData(new GridViewData("", logData));
                    }
                    else if ((searchCategory == "all" || searchCategory == "stacktrace") && logData.StackTrace.ToLower().IndexOf(searchText) >= 0)
                    {
                        gridViewModel.AddData(new GridViewData("", logData));
                    }
                }
            }
            gridView.Reload();
        }

        void DrawToolbar()
        {
            if(searchField == null)
            {
                searchField = new ToolbarSearchField((text)=>
                {
                    searchText = text;
                    FilterLogDatas();
                },(category)=> {
                    searchCategory = category;
                    FilterLogDatas();
                });
                searchField.Categories = searchCategories;
                searchField.CategoryIndex = 0;
                searchField.Text = searchText;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                ipAddressString = GUILayout.TextField(ipAddressString, EditorStyles.toolbarTextField,GUILayout.Width(220));
                
                if(clientSocket == null)
                {
                    if(GUILayout.Button("Connect",EditorStyles.toolbarButton))
                    {
                        clientSocket = new LogClientSocket();
                        clientSocket.Connect(ipAddressString, PORT);
                    }
                }else
                {
                    if(clientStatus == LogClientStatus.Connecting)
                    {
                        GUILayout.Label("Connecting", EditorStyles.toolbarButton);
                    }else if(clientStatus == LogClientStatus.Connected)
                    {
                        if (GUILayout.Button("Disconnect", EditorStyles.toolbarButton))
                        {
                            clientSocket.Disconnect();
                        }
                    }else if(clientStatus == LogClientStatus.Disconnecting)
                    {
                        GUILayout.Label("Disconnecting", EditorStyles.toolbarButton);
                    }
                }

                GUILayout.FlexibleSpace();

                Color oldBGColor = GUI.color;
                for (int i = (int)LogLevel.On + 1; i < (int)LogLevel.Off; ++i)
                {
                    GUI.color = levelBtnEnableStateDic[(LogLevel)i] ? Color.cyan : oldBGColor;
                    if (GUILayout.Button(((LogLevel)i).ToString()+"("+levelLogCountDic[(LogLevel)i]+")", EditorStyles.toolbarButton))
                    {
                        levelBtnEnableStateDic[(LogLevel)i] = !levelBtnEnableStateDic[(LogLevel)i];
                    }
                }
                GUI.color = oldBGColor;

                GUILayout.Space(10);
                searchField.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawGridView()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            {
                if (gridView != null)
                {
                    gridView.OnGUILayout();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnUpdate()
        {
            if(clientSocket!=null)
            {
                LogClientStatus curStatus = clientSocket.Status;
                if(curStatus!= clientStatus)
                {
                    clientStatus = curStatus;

                    if(clientStatus == LogClientStatus.Disconnected)
                    {
                        ShowNotification(new GUIContent("Disconnected"));
                        clientSocket = null;
                    }else if(clientStatus == LogClientStatus.Connecting)
                    {
                        ShowNotification(new GUIContent("Connecting..."));
                    }else if(clientStatus == LogClientStatus.Connected)
                    {
                        ShowNotification(new GUIContent("Connected"));
                    }else if(clientStatus == LogClientStatus.Disconnecting)
                    {
                        ShowNotification(new GUIContent("Disconnecting..."));
                    }

                    Repaint();
                }

                if(curStatus == LogClientStatus.Connected)
                {
                    LogData[] datas = clientSocket.LogDatas;
                    if(datas!=null && datas.Length>0)
                    {
                        logDatas.AddRange(datas);

                        FilterLogDatas();
                    }

                    Repaint();
                }
            }
        }
    }
}
