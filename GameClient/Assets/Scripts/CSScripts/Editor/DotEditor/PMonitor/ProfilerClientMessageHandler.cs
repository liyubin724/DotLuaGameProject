using DotEngine.Log;
using DotEngine.Net.Client;
using DotEngine.PMonitor.Recorder;

namespace DotEditor.PMonitor
{
    public static class ProfilerClientMessageHandler
    {
        public static void RegisterHanlder(ClientNet clientNet)
        {
            clientNet.RegisterMessageHandler(ProfilerServerMessageID.OPEN_SAMPLER_RESPONSE, OnOpenSamplerResponse);
            clientNet.RegisterMessageHandler(ProfilerServerMessageID.CLOSE_SAMPLER_RESPONSE, OnCloseSamplerResponse);
        }

        public static void OnOpenSamplerResponse(int messageID, object message)
        {
            S2C_OpenSamplerResponse response = (S2C_OpenSamplerResponse)message;
            LogUtil.LogDebug("Profiler", "Result = " + response.result);
        }

        public static void OnCloseSamplerResponse(int messageID, object message)
        {

        }
    }
}
