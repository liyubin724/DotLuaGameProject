using DotEngine;
using DotEngine.Net.Client;
using DotEngine.Net.Services;
using DotEngine.PMonitor.Recorder;
using DotEngine.PMonitor.Sampler;
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

        ClientNet clientNet;
        private void Awake()
        {
            ClientNetService clientNetService = Facade.GetInstance().GetService<ClientNetService>(ClientNetService.NAME);
            clientNet = clientNetService.CreateNet(98, new ProfilerClientMessageParser());
            ProfilerClientMessageHandler.RegisterHanlder(clientNet);

            clientNet.Connect("127.0.0.1", 3302);
        }

        private void OnGUI()
        {
            if(GUILayout.Button("Open Log Sampler"))
            {
                clientNet.SendMessage(C2S_ProfilerMessageID.OPEN_SAMPLER_REQUEST, new C2S_OpenSamplerRequest()
                {
                    category = SamplerCategory.Log
                });
            }
            if (GUILayout.Button("Open Memory Sampler"))
            {
                clientNet.SendMessage(C2S_ProfilerMessageID.OPEN_SAMPLER_REQUEST, new C2S_OpenSamplerRequest()
                {
                    category = SamplerCategory.Memory
                });
            }
        }
    }
}
