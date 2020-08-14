using Boo.Lang;
using DotEngine.Net.Client;
using DotEngine.PMonitor.Recorder;
using DotEngine.PMonitor.Sampler;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace DotEditor.PMonitor
{
    public class MonitorProfilerWindow : EditorWindow
    {
        [MenuItem("Test/Profiler")]
        static void ShowWin()
        {
            MonitorProfilerWindow.GetWindow<MonitorProfilerWindow>().Show();
        }

        private ClientNet clientNet;
        private double preUpdateTime;
        private void Awake()
        {
            EditorApplication.update += DoUpdate;
            preUpdateTime = EditorApplication.timeSinceStartup;

            clientNet = new ClientNet(1, new ProfilerClientMessageParser());
            clientNet.NetConnectedSuccess += (net) =>
            {

            };
            clientNet.NetDisconnected += (net) =>
            {

            };
            clientNet.NetConnectedFailed += (net) =>
            {

            };
            clientNet.RegisterMessageHandler("Handler", new ProfilerClientMessageHandler());
            clientNet.Connect("127.0.0.1", 3302);
        }

        private void DoUpdate()
        {
            double deltaTime = EditorApplication.timeSinceStartup - preUpdateTime;
            preUpdateTime = EditorApplication.timeSinceStartup;

            if(clientNet!=null && clientNet.IsConnected())
            {
                clientNet.DoUpdate((float)deltaTime);
                clientNet.DoLateUpdate();
            }
        }

        private void OnGUI()
        {
            if(GUILayout.Button("Open Log Sampler"))
            {
                clientNet.SendMessage(ProfilerClientMessageID.OPEN_SAMPLER_REQUEST, new C2S_OpenSamplerRequest()
                {
                    category = SamplerCategory.Log
                });
            }
            if (GUILayout.Button("Open Memory Sampler"))
            {
                clientNet.SendMessage(ProfilerClientMessageID.OPEN_SAMPLER_REQUEST, new C2S_OpenSamplerRequest()
                {
                    category = SamplerCategory.Memory
                });
            }
        }

        private void OnDestroy()
        {
            EditorApplication.update -= DoUpdate;
            clientNet?.Dispose();
            clientNet = null;
        }
    }
}
