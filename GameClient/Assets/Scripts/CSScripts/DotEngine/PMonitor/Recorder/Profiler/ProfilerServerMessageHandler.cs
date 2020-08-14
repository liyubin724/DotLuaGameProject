using DotEngine.Net.Server;
using Game;
using System;
using System.Collections.Generic;

namespace DotEngine.PMonitor.Recorder
{
    public class ProfilerServerMessageHandler : IServerNetMessageHandler
    {
        public ServerNetListener Listener { get; set ; }

        private Dictionary<int, Action<int, int, object>> messageActionDic = new Dictionary<int, Action<int, int, object>>();

        public ProfilerServerMessageHandler()
        {
            messageActionDic.Add(ProfilerClientMessageID.OPEN_SAMPLER_REQUEST, OnOpenSamplerRequest);
            messageActionDic.Add(ProfilerClientMessageID.CLOSE_SAMPLER_REQUEST, OnCloseSamplerRequest);
        }

        public bool OnMessageHanlder(int netID, int messageID, object message)
        {
            if(messageActionDic.TryGetValue(messageID,out var action))
            {
                action?.Invoke(netID, messageID, message);
                return true;
            }

            return false;
        }

        public void OnOpenSamplerRequest(int netID,int messageID, object message)
        {
            MonitorService monitorService = GameFacade.GetInstance().GetService<MonitorService>(MonitorService.NAME);
            
            C2S_OpenSamplerRequest request = (C2S_OpenSamplerRequest)message;

            bool result = monitorService.OpenSampler(request.category);

            S2C_OpenSamplerResponse response = new S2C_OpenSamplerResponse()
            {
                result = result
            };
            Listener.SendMessage(netID, ProfilerServerMessageID.OPEN_SAMPLER_RESPONSE, response);
        }

        public void OnCloseSamplerRequest(int netID,int messageID, object message)
        {
            MonitorService monitorService = GameFacade.GetInstance().GetService<MonitorService>(MonitorService.NAME);

            C2S_CloseSamplerRequest request = (C2S_CloseSamplerRequest)message;

            bool result = monitorService.CloseSampler(request.category);

            S2C_CloseSamplerResponse response = new S2C_CloseSamplerResponse()
            {
                result = result
            };
            Listener.SendMessage(netID, ProfilerServerMessageID.CLOSE_SAMPLER_RESPONSE, response);
        }
    }
}
