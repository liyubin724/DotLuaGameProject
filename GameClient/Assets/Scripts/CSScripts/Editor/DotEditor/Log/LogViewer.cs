using DotEngine.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
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

        public static readonly int PORT = 8899;

        private object locker = new object();
        private TcpClientSocket clientSocket = null;
        private List<string> logMessages = new List<string>();
        private List<string> tempLogMessages = new List<string>();

        [SerializeField]
        private string ipAddressString = "127.0.0.1";


        private void Awake()
        {
            EditorApplication.update += OnUpdate;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.FlexibleSpace();
                ipAddressString = EditorGUILayout.TextField("IP:", ipAddressString, EditorStyles.toolbarTextField);
                if(GUILayout.Button("Connected"))
                {
                    CreateClientSocket();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical();
            {
                foreach (var message in logMessages)
                {
                    EditorGUILayout.LabelField(message);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnUpdate()
        {
            lock (locker)
            {
                logMessages.AddRange(tempLogMessages);
                tempLogMessages.Clear();
            }
            Debug.Log("FFFFFFFFFFFF" + Thread.CurrentThread.ManagedThreadId);
            Repaint();
        }

        private void OnDestroy()
        {
            EditorApplication.update -= OnUpdate;
        }

        private void CreateClientSocket()
        {
            clientSocket = new TcpClientSocket();
            clientSocket.OnConnect += OnConnected;
            clientSocket.OnReceive += OnReceived;
            clientSocket.Connect(IPAddress.Parse(ipAddressString), PORT);
        }

        private void OnConnected(object sender,EventArgs eventArgs)
        {
            
        }

        private void OnReceived(object sender, ReceiveEventArgs eventArgs)
        {
            lock(locker)
            {
                tempLogMessages.Add(Encoding.UTF8.GetString(eventArgs.bytes));
            }

            Debug.Log("SSSSSSSSSSSSSSS" + Thread.CurrentThread.ManagedThreadId);
        }

        private void OnDisconnected(object sender,EventArgs eventArgs)
        {

        }
    }
}
