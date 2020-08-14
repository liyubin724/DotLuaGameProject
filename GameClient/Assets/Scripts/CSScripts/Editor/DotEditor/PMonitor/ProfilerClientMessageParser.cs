using DotEngine.Log;
using DotEngine.Net.Message;
using DotEngine.PMonitor.Recorder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotEditor.PMonitor
{
    public class ProfilerClientMessageParser : IMessageParser
    {
        public string SecretKey { get; set; }

        private Dictionary<int, Func<byte[], object>> m_DecodeHandlers = new Dictionary<int, Func<byte[], object>>();

        public ProfilerClientMessageParser()
        {
            m_DecodeHandlers.Add(ProfilerServerMessageID.OPEN_SAMPLER_RESPONSE, OnOpenSamplerResponse);
            m_DecodeHandlers.Add(ProfilerServerMessageID.CLOSE_SAMPLER_RESPONSE, OnCloseSamplerResponse);
        }

        public object DecodeMessage(int messageID, byte[] bytes)
        {
            if (m_DecodeHandlers.TryGetValue(messageID, out var handler))
            {
                return handler.Invoke(bytes);
            }
            LogUtil.LogWarning("ProfilerServerMessageParser", "DecodeHandler not found.messageID = " + messageID);
            return null;
        }

        public byte[] EncodeMessage(int messageID, object message)
        {
            string jsonStr = JsonConvert.SerializeObject(message, Formatting.None);
            return Encoding.UTF8.GetBytes(jsonStr);
        }

        private T OnDecodeMessage<T>(byte[] bytes)
        {
            string jsonStr = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        private S2C_OpenSamplerResponse OnOpenSamplerResponse(byte[] bytes)
        {
            return OnDecodeMessage<S2C_OpenSamplerResponse>(bytes);
        }

        private S2C_CloseSamplerResponse OnCloseSamplerResponse(byte[] bytes)
        {
            return OnDecodeMessage<S2C_CloseSamplerResponse>(bytes);
        }
    }
}
