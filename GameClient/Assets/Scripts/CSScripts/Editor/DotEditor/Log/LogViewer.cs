using DotEditor.GUIExtension;
using DotEngine.Log;
using DotEngine.Log.Appender;
using DotEngine.Log.Formatter;
using DotEngine.NetworkEx;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
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

            viewer.Show();
        }

        public static LogViewer Viewer = null;

        [SerializeField]
        private string m_IPAddressString = "127.0.0.1";

        private ClientNetworkStatus m_ClientStatus = ClientNetworkStatus.None;
        private ClientNetwork m_ClientNetwork = null;

        private LogViewerData m_ViewerData = new LogViewerData();
        internal LogNetSetting ViewerSetting { get; private set; }

        private LogGridView m_GridView = null;

        private string m_SelectedLogDataText = string.Empty;
        private Vector2 m_SelectedLogDataScrollPos = Vector2.zero;

        private ToolbarSearchField m_SearchField = null;
        private void OnEnable()
        {
            Viewer = this;

            ViewerSetting = new LogNetSetting()
            {
                GlobalSetting = new LogNetGlobalSetting(),
                LoggerSettings = new List<LogNetLoggerSetting>(),
            };

            m_ViewerData = new LogViewerData();
            m_ViewerData.OnLogDataChanged = OnLogDataChanged;

            EditorApplication.update += OnUpdate;
        }

        private void OnDisable()
        {
            Disconnect();

            EditorApplication.update -= OnUpdate;
        }

        private void OnDestroy()
        {
            Viewer = null;
        }

        private void OnLogDataChanged()
        {
            m_GridView?.Reload();
        }

        private void OnGUI()
        {
            if(m_GridView == null)
            {
                m_GridView = new LogGridView(m_ViewerData.GridViewModel);
                m_GridView.OnSelectedChanged += (logData) =>
                {
                    m_SelectedLogDataScrollPos = Vector2.zero;
                    m_ViewerData.SelectedLogData = logData;
                    if(m_ViewerData.SelectedLogData!=null)
                    {
                        m_SelectedLogDataText = m_ViewerData.SelectedLogData.ToString();
                    }else
                    {
                        m_SelectedLogDataText = string.Empty;
                    }
                };
            }

            DrawToolbar();
            DrawGridView();

            m_SelectedLogDataScrollPos = EditorGUILayout.BeginScrollView(m_SelectedLogDataScrollPos,GUILayout.Height(160),GUILayout.ExpandWidth(true));
            {
                EditorGUILayout.SelectableLabel(m_SelectedLogDataText,GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndScrollView();
        }

        void DrawToolbar()
        {
            if(m_SearchField == null)
            {
                m_SearchField = new ToolbarSearchField((text)=>
                {
                    m_ViewerData.SearchText = text;
                },(category)=> {
                    m_ViewerData.SearchCategory = category;
                });
                m_SearchField.CategoryIndex = 0;
                m_SearchField.Categories = m_ViewerData.SearchCategories;
                m_SearchField.Text = m_ViewerData.SearchText;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                m_IPAddressString = GUILayout.TextField(m_IPAddressString, EditorStyles.toolbarTextField,GUILayout.Width(220));
                if(m_ClientNetwork == null)
                {
                    if (GUILayout.Button("Connect", EditorStyles.toolbarButton))
                    {
                        m_ClientNetwork = new ClientNetwork("LogClientNetwork");
                        m_ClientNetwork.RegistAllMessageHandler(this);
                        m_ClientNetwork.Connect(m_IPAddressString, LogNetUtill.PORT);
                    }
                }else
                {
                    if (m_ClientStatus == ClientNetworkStatus.Connecting)
                    {
                        GUILayout.Label("Connecting", EditorStyles.toolbarButton);
                    }
                    else if (m_ClientStatus == ClientNetworkStatus.Connected)
                    {
                        if (GUILayout.Button("Disconnect", EditorStyles.toolbarButton))
                        {
                            Disconnect();
                        }
                    }
                    else if (m_ClientStatus == ClientNetworkStatus.Disconnecting)
                    {
                        GUILayout.Label("Disconnecting", EditorStyles.toolbarButton);
                    }
                }

                GUILayout.Space(10);
                if(GUILayout.Button("Clear",EditorStyles.toolbarButton))
                {
                    m_SelectedLogDataText = string.Empty;
                    m_SelectedLogDataScrollPos = Vector2.zero;
                    m_ViewerData.Reset();
                }

                GUILayout.FlexibleSpace();

                Color oldBGColor = GUI.color;
                for (int i = (int)LogLevel.On + 1; i < (int)LogLevel.Off; ++i)
                {
                    LogLevel level = (LogLevel)i;

                    GUI.color = m_ViewerData.GetIsLogLevelEnable(level) ? Color.cyan : oldBGColor;
                    if (GUILayout.Button(level.ToString()+"("+m_ViewerData.GetLogLevelCount(level)+")", EditorStyles.toolbarButton))
                    {
                        m_ViewerData.ReverseIsLogLevelEnable(level);
                    }
                }
                GUI.color = oldBGColor;

                GUILayout.Space(10);
                m_SearchField.OnGUILayout();

                EditorGUI.BeginDisabledGroup(m_ClientNetwork == null);
                {
                    if (GUILayout.Button("Setting", EditorStyles.toolbarButton))
                    {
                        m_ClientNetwork.SendMessage(LogNetUtill.C2S_GET_LOG_LEVEL_REQUEST, null);

                        Vector2 pos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                        GUIExtension.Windows.PopupWindow.ShowWin(new Rect(pos.x, pos.y, 250, 400), new LogViewerSettingPopContent(ViewerSetting), false, true);
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
                if (m_GridView != null)
                {
                    m_GridView.OnGUILayout();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnUpdate()
        {
            if (m_ClientNetwork!=null)
            {
                ClientNetworkStatus curStatus = m_ClientNetwork.Status;
                if(curStatus!= m_ClientStatus)
                {
                    m_ClientStatus = curStatus;

                    if(m_ClientStatus == ClientNetworkStatus.Disconnected)
                    {
                        ShowNotification(new GUIContent("Disconnected"));
                        m_ClientNetwork = null;
                    }else if(m_ClientStatus == ClientNetworkStatus.Connecting)
                    {
                        ShowNotification(new GUIContent("Connecting..."));
                    }else if(m_ClientStatus == ClientNetworkStatus.Connected)
                    {
                        ShowNotification(new GUIContent("Connected"));
                    }else if(m_ClientStatus == ClientNetworkStatus.Disconnecting)
                    {
                        ShowNotification(new GUIContent("Disconnecting..."));
                    }

                    Repaint();
                }
            }
        }

        public void ChangeGlobalLogLevel(LogLevel globalLogLevel)
        {
            if (m_ClientNetwork != null && m_ClientStatus == ClientNetworkStatus.Connected)
            {
                LogNetGlobalSetting globalSetting = new LogNetGlobalSetting()
                {
                    GlobalLogLevel = globalLogLevel,
                };
                SendMessage(LogNetUtill.C2S_SET_GLOBAL_LOG_LEVEL_REQUEST, JsonConvert.SerializeObject(globalSetting));
            }
        }

        public void ChangeLoggerLogLevel(string name,LogLevel minLogLevel,LogLevel stacktraceLogLevel)
        {
            if (m_ClientNetwork != null && m_ClientStatus == ClientNetworkStatus.Connected)
            {
                LogNetLoggerSetting loggerSetting = new LogNetLoggerSetting()
                {
                    Name = name,
                    MinLogLevel = minLogLevel,
                    StackTraceLogLevel = stacktraceLogLevel,
                };
                SendMessage(LogNetUtill.C2S_SET_LOGGER_LOG_LEVEL_REQUEST, JsonConvert.SerializeObject(loggerSetting));
            }
        }

        private void SendMessage(int id, string message)
        {
            if (m_ClientNetwork != null && m_ClientNetwork.IsConnected)
            {
                if(string.IsNullOrEmpty(message))
                {
                    m_ClientNetwork.SendMessage(id, null);
                }else
                {
                    m_ClientNetwork.SendMessage(id, Encoding.UTF8.GetBytes(message));
                }
            }
        }

        private void Disconnect()
        {
            if(m_ClientNetwork!=null)
            {
                m_ClientNetwork.Disconnect();
                m_ClientNetwork = null;
            }
            m_ClientStatus = ClientNetworkStatus.None;
        }

        [ClientNetworkMessageHandler(LogNetUtill.S2C_RECEIVE_LOG_REQUEST)]
        private void OnS2CReceivedLogRequest(byte[] messageBytes)
        {
            string jsonStr = Encoding.UTF8.GetString(messageBytes);
            LogData logData = JsonConvert.DeserializeObject<LogData>(jsonStr);
            m_ViewerData.AddLogData(logData);
        }

        [ClientNetworkMessageHandler(LogNetUtill.S2C_GET_LOG_LEVEL_RESPONSE)]
        private void OnS2CGetLogLevelResponse(byte[] messageBytes)
        {
            string jsonStr = Encoding.UTF8.GetString(messageBytes);
            LogNetSetting setting = JsonConvert.DeserializeObject<LogNetSetting>(jsonStr);

            if(setting!=null)
            {
                if(setting.GlobalSetting!=null)
                {
                    ViewerSetting.GlobalSetting.GlobalLogLevel = setting.GlobalSetting.GlobalLogLevel;
                }
                ViewerSetting.LoggerSettings.Clear();
                if(setting.LoggerSettings!=null)
                { 
                    ViewerSetting.LoggerSettings.AddRange(setting.LoggerSettings);
                }
            }
        }

        [ClientNetworkMessageHandler(LogNetUtill.S2C_SET_GLOBAL_LOG_LEVEL_RESPONSE)]
        private void OnS2CSetGlobalLogLevelResponse(byte[] messageBytes)
        {
        }

        [ClientNetworkMessageHandler(LogNetUtill.S2C_SET_LOGGER_LOG_LEVEL_RESPONSE)]
        private void OnS2CSetLoggerLogLevelResponse(byte[] messageBytes)
        {
        }
    }
}
