using DotEditor.GUIExtension;
using DotEngine.Log;
using DotEngine.Log.Appender;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Log
{
    public class LogViewer : EditorWindow
    {
        [MenuItem("Game/Log/Viewer")]
        public static void ShowWin()
        {
            var viewer = GetWindow<LogViewer>();
            viewer.titleContent = new GUIContent("Log Viewer");
            viewer.CenterOnMainWin();

            viewer.Show();
        }

        public static readonly int PORT = 8899;

        public static LogViewer Viewer = null;

        [SerializeField]
        private string ipAddressString = "127.0.0.1";

        private LogClientStatus clientStatus = LogClientStatus.None;
        private LogClientSocket clientSocket = null;

        private LogViewerData viewerData = new LogViewerData();
        internal LogViewerSetting viewerSetting = new LogViewerSetting();

        private LogGridView gridView = null;

        private string selectedLogDataText = string.Empty;
        private Vector2 selectedLogDataScrollPos = Vector2.zero;

        private ToolbarSearchField searchField = null;
        private void OnEnable()
        {
            Viewer = this;

            viewerData = new LogViewerData();
            viewerData.OnLogDataChanged = OnLogDataChanged;
            EditorApplication.update += OnUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnUpdate;
        }

        private void OnDestroy()
        {
            Viewer = null;
        }

        private void OnLogDataChanged()
        {
            gridView?.Reload();
        }

        private void OnGUI()
        {
            if(gridView == null)
            {
                gridView = new LogGridView(viewerData.GridViewModel);
                gridView.OnSelectedChanged += (logData) =>
                {
                    selectedLogDataScrollPos = Vector2.zero;
                    viewerData.SelectedLogData = logData;
                    if(viewerData.SelectedLogData!=null)
                    {
                        selectedLogDataText = viewerData.SelectedLogData.ToString();
                    }else
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

        void DrawToolbar()
        {
            if(searchField == null)
            {
                searchField = new ToolbarSearchField((text)=>
                {
                    viewerData.SearchText = text;
                },(category)=> {
                    viewerData.SearchCategory = category;
                });
                searchField.CategoryIndex = 0;
                searchField.Categories = viewerData.SearchCategories;
                searchField.Text = viewerData.SearchText;
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

                GUILayout.Space(10);
                if(GUILayout.Button("Clear",EditorStyles.toolbarButton))
                {
                    selectedLogDataText = string.Empty;
                    selectedLogDataScrollPos = Vector2.zero;
                    viewerData.Reset();
                }

                GUILayout.FlexibleSpace();

                Color oldBGColor = GUI.color;
                for (int i = (int)LogLevel.On + 1; i < (int)LogLevel.Off; ++i)
                {
                    LogLevel level = (LogLevel)i;

                    GUI.color = viewerData.GetIsLogLevelEnable(level) ? Color.cyan : oldBGColor;
                    if (GUILayout.Button(level.ToString()+"("+viewerData.GetLogLevelCount(level)+")", EditorStyles.toolbarButton))
                    {
                        viewerData.ReverseIsLogLevelEnable(level);
                    }
                }
                GUI.color = oldBGColor;

                GUILayout.Space(10);
                searchField.OnGUILayout();

                EditorGUI.BeginDisabledGroup(clientSocket == null);
                {
                    if (GUILayout.Button("Setting", EditorStyles.toolbarButton))
                    {
                        clientSocket.SendMessage(LogSocketUtil.C2S_GET_LOG_LEVEL_REQUEST, string.Empty);

                        Vector2 pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                        GUIExtension.Windows.PopupWindow.ShowWin(new Rect(pos.x, pos.y, 250, 400), new LogViewerSettingPopContent(viewerSetting), false, true);
                    }
                }
                EditorGUI.EndDisabledGroup();
                
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
            if (clientSocket!=null)
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
                        viewerData.AddLogDatas(datas);
                    }

                    Repaint();
                }
            }
        }

        public void ChangeGlobalLogLevel(LogLevel globalLogLevel)
        {
            if (clientSocket != null && clientStatus == LogClientStatus.Connected)
            {
                JObject messJObj = new JObject();
                messJObj.Add("global_log_level", (int)globalLogLevel);
                clientSocket.SendMessage(LogSocketUtil.C2S_SET_GLOBAL_LOG_LEVEL_REQUEST, messJObj.ToString());
            }
        }

        public void ChangeLoggerLogLevel(string name,LogLevel minLogLevel,LogLevel stacktraceLogLevel)
        {
            if (clientSocket != null && clientStatus == LogClientStatus.Connected)
            {
                JObject messJObj = new JObject();
                messJObj.Add("name", name);
                messJObj.Add("min_log_level", (int)minLogLevel);
                messJObj.Add("stacktrace_log_level", (int)stacktraceLogLevel);
                clientSocket.SendMessage(LogSocketUtil.C2S_SET_LOGGER_LOG_LEVEL_REQUEST, messJObj.ToString());
            }
        }
    }
}
