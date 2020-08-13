using DotEngine.Net.Client;
using DotEngine.Net.Server;
using DotEngine.Net.Services;
using Game;

namespace DotEngine.PMonitor.Recorder
{
    public static class ProfilerClientMessageHandler
    {
        public static void RegisterHanlder(ClientNet clientNet)
        {
            clientNet.RegisterMessageHandler(S2C_ProfilerMessageID.OPEN_SAMPLER_RESPONSE, OnOpenSamplerResponse);
            clientNet.RegisterMessageHandler(S2C_ProfilerMessageID.CLOSE_SAMPLER_RESPONSE, OnCloseSamplerResponse);
        }

        public static void OnOpenSamplerResponse(int messageID, object message)
        {

        }

        public static void OnCloseSamplerResponse(int messageID, object message)
        {

        }
    }

    public static class ProfilerServerMessageHandler
    {
        
        public static void RegisterHanlder(ServerNetListener serverNetListener)
        {
            serverNetListener.RegisterMessageHandler(C2S_ProfilerMessageID.OPEN_SAMPLER_REQUEST, OnOpenSamplerRequest);
            serverNetListener.RegisterMessageHandler(C2S_ProfilerMessageID.CLOSE_SAMPLER_REQUEST, OnCloseSamplerRequest);
        }

        public static void OnOpenSamplerRequest(int netID,int messageID, object message)
        {
            ServerNetService serverNetService = GameFacade.GetInstance().GetService<ServerNetService>(ServerNetService.NAME);
            MonitorService monitorService = GameFacade.GetInstance().GetService<MonitorService>(MonitorService.NAME);
            
            C2S_OpenSamplerRequest request = (C2S_OpenSamplerRequest)message;

            bool result = monitorService.OpenSampler(request.category);

            S2C_OpenSamplerResponse response = new S2C_OpenSamplerResponse()
            {
                result = result
            };
            serverNetService.GetNet(ProfilerRecorder.SERVER_ID).SendMessage(netID, S2C_ProfilerMessageID.OPEN_SAMPLER_RESPONSE, response);
        }

        public static void OnCloseSamplerRequest(int netID,int messageID, object message)
        {
            ServerNetService serverNetService = GameFacade.GetInstance().GetService<ServerNetService>(ServerNetService.NAME);
            MonitorService monitorService = GameFacade.GetInstance().GetService<MonitorService>(MonitorService.NAME);

            C2S_CloseSamplerRequest request = (C2S_CloseSamplerRequest)message;

            bool result = monitorService.CloseSampler(request.category);

            S2C_CloseSamplerResponse response = new S2C_CloseSamplerResponse()
            {
                result = result
            };
            serverNetService.GetNet(ProfilerRecorder.SERVER_ID).SendMessage(netID, S2C_ProfilerMessageID.CLOSE_SAMPLER_RESPONSE, response);
        }
    }
}
