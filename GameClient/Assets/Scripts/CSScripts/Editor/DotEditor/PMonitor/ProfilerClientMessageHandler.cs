using DotEngine.Log;
using DotEngine.Net.Client;
using DotEngine.PMonitor.Recorder;
using System;
using System.Collections.Generic;

namespace DotEditor.PMonitor
{
    public class ProfilerClientMessageHandler : IClientNetMessageHandler
    {
        public ClientNet Net { get; set; }

        private Dictionary<int, Action<int, object>> messageActionDic = new Dictionary<int, Action<int, object>>();

        public ProfilerClientMessageHandler()
        {
            messageActionDic.Add(ProfilerServerMessageID.OPEN_SAMPLER_RESPONSE, OnOpenSamplerResponse);
            messageActionDic.Add(ProfilerServerMessageID.CLOSE_SAMPLER_RESPONSE, OnCloseSamplerResponse);
        }

        public bool OnMessageHanlder(int messageID, object message)
        {
            if (messageActionDic.TryGetValue(messageID, out var action))
            {
                action?.Invoke(messageID, message);
                return true;
            }

            return false;
        }

        public void OnOpenSamplerResponse(int messageID, object message)
        {
            S2C_OpenSamplerResponse response = (S2C_OpenSamplerResponse)message;
            LogUtil.LogDebug("Profiler", "Result = " + response.result);
        }

        public void OnCloseSamplerResponse(int messageID, object message)
        {

        }

        
    }
}
