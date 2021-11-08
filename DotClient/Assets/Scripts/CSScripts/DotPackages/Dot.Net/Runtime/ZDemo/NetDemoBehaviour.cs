using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DotEngine.Net
{
    public class NetDemoBehaviour : MonoBehaviour
    {
        private ServerNetwork serverNetwork = null;
        private StringBuilder serverLogSB = new StringBuilder();
        private string serverMessageContent = string.Empty;

        private ClientNetwork clientNetwork = null;
        private StringBuilder clientLogSB = new StringBuilder();
        private string clientMessageContent = string.Empty;

        void OnServerGUI()
        {
            if (serverNetwork == null && GUILayout.Button("Start Server"))
            {
                serverNetwork = new ServerNetwork(new MessageEncoder(), new MessageDecoder());
                serverNetwork.RegisterListener(1, OnHandMessage1);
                serverNetwork.NetStarted = () =>
                {
                    serverLogSB.AppendLine("Server Started");
                };
                serverNetwork.NetStopped = () =>
                {
                    serverLogSB.AppendLine("Server Started");
                };
                serverNetwork.Start(8899);
            }

            if(serverNetwork!=null && serverNetwork.IsStarted && GUILayout.Button("Stop Server"))
            {
                serverNetwork.Stop();
            }

            if(serverNetwork!=null && serverNetwork.IsStarted)
            {
                serverMessageContent = GUILayout.TextField(serverMessageContent, GUILayout.ExpandWidth(true));
                if(GUILayout.Button("Multicast message") && !string.IsNullOrEmpty(serverMessageContent))
                {
                    serverNetwork.MulticastText(101, serverMessageContent);
                }
            }

            GUILayout.Label(serverLogSB.ToString(), EditorStyles.wordWrappedLabel, GUILayout.ExpandHeight(true));
        }

        private void OnHandMessage1(Guid guid,int messageId,byte[] messageBody)
        {
            serverLogSB.AppendLine(Encoding.Unicode.GetString(messageBody));

            serverNetwork.MulticastText(102, "Message 1 Received");
        }

        void OnClientGUI()
        {
            if(clientNetwork == null && GUILayout.Button("Start client"))
            {
                clientNetwork = new ClientNetwork(new MessageEncoder(), new MessageDecoder());
                clientNetwork.RegisterListener(101, OnHandMessage101);
                clientNetwork.RegisterListener(102, OnHandMessage102);
                clientNetwork.NetConnected = () =>
                {
                    clientLogSB.AppendLine("Client Connected");
                };
                clientNetwork.NetDisconnected = () =>
                {
                    clientLogSB.AppendLine("Client Disconnected");
                };
                clientNetwork.NetError = () =>
                {
                    clientLogSB.AppendLine("Client Error");
                };
                clientNetwork.Connect("127.0.0.1", 8899);
            }

            if(clientNetwork!=null && !clientNetwork.IsConnected && !clientNetwork.IsConnnecting &&
                GUILayout.Button("Reconnect"))
            {
                clientNetwork.Reconnect();
            }

            if(clientNetwork!=null && GUILayout.Button("Disconnect"))
            {
                clientNetwork.Disconnect();
            }

            if(clientNetwork!=null && clientNetwork.IsConnected)
            {
                clientMessageContent = GUILayout.TextField(clientMessageContent, GUILayout.ExpandWidth(true));
                if (GUILayout.Button("Send message") && !string.IsNullOrEmpty(clientMessageContent))
                {
                    clientNetwork.SendText(1, clientMessageContent);
                }
            }

            GUILayout.Label(clientLogSB.ToString(), EditorStyles.wordWrappedLabel, GUILayout.ExpandHeight(true));
        }

        private void OnHandMessage101(int messageId,byte[] messageBody)
        {
            clientLogSB.AppendLine(Encoding.Unicode.GetString(messageBody));
        }

        private void OnHandMessage102(int messageId, byte[] messageBody)
        {
            clientLogSB.AppendLine(Encoding.Unicode.GetString(messageBody));
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    OnServerGUI();
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    OnClientGUI();
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();
            
        }

        private void Update()
        {
            serverNetwork?.DoUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            clientNetwork?.DoUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

    }
}
