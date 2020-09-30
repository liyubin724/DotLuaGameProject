using DotEditor.GUIExtension.DataGrid;
using DotEngine.Monitor.Sampler;
using DotEngine.Net.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Monitor
{
    public class ProfilerWindow : EditorWindow
    {
        public static ProfilerWindow window = null;

        [MenuItem("Game/Monitor/Profiler Window")]
        public static void ShowWin()
        {
            var win = GetWindow<ProfilerWindow>();
            win.wantsMouseMove = true;
            win.titleContent = Contents.titleContent;
            win.Show();
        }

        private string ipAddress = "127.0.0.1";
        private int port = 3302;

        private ClientNet m_ClientNet;
        private double m_PreUpdateTimer;
        public ProfilerModel Model { get; private set; }

        private void Awake()
        {
            EditorApplication.update += DoUpdate;
            m_PreUpdateTimer = EditorApplication.timeSinceStartup;

            Model = new ProfilerModel((type)=>
            {
                if(type == MonitorSamplerType.FPS)
                    m_FPSTreeView?.Reload();
                Repaint();
            });

            window = this;
        }

        void DoUpdate()
        {
            double deltaTime = EditorApplication.timeSinceStartup - m_PreUpdateTimer;
            m_PreUpdateTimer = EditorApplication.timeSinceStartup;

            if (m_ClientNet != null && m_ClientNet.IsConnected())
            {
                m_ClientNet.DoUpdate((float)deltaTime);
                m_ClientNet.DoLateUpdate();
            }

            Repaint();
        }

        private ProfilerTabTreeView m_FPSTreeView = null;
        private void OnGUI()
        {
            DrawToolbar();
            if(m_FPSTreeView == null)
            {
                m_FPSTreeView = new ProfilerTabTreeView(Model.GetTabModel(MonitorSamplerType.FPS),new string[] {
                    "Time",
                    "FrameIndex",
                    "FPS",
                });
            }

            m_FPSTreeView.OnGUILayout();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if(GUILayout.Button("Start",EditorStyles.toolbarButton,GUILayout.Width(120)))
                {
                    StartRecord();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void StartRecord()
        {
            if(m_ClientNet!=null && m_ClientNet.IsConnected())
            {
                return;
            }
            m_ClientNet = new ClientNet(1, new ProfilerMessageParser());
            m_ClientNet.NetConnectedSuccess += (net) =>
            {

            };
            m_ClientNet.NetDisconnected += (net) =>
            {

            };
            m_ClientNet.NetConnectedFailed += (net) =>
            {

            };
            m_ClientNet.RegisterMessageHandler("Handler", new ProfilerMessageHandler());
            m_ClientNet.Connect(ipAddress, port);
        }

        private void StopRecord()
        {
            m_ClientNet?.Dispose();
        }

        private void OnDestroy()
        {
            EditorApplication.update -= DoUpdate;
            m_ClientNet?.Dispose();
            m_ClientNet = null;

            window = null;
        }
    }

    class Contents
    {
        public static GUIContent titleContent = new GUIContent("Monitor Profiler");
    }
}


