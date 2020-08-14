using DotEngine.Log;
using DotEngine.Net.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotEngine.PMonitor.Recorder
{
    public class ProfilerServerMessageParser : IMessageParser
    {
        public string SecretKey { get; set; }

        private Dictionary<int, Func<byte[], object>> m_DecodeHandlers = new Dictionary<int, Func<byte[], object>>();
        public ProfilerServerMessageParser()
        {
            m_DecodeHandlers.Add(ProfilerClientMessageID.OPEN_SAMPLER_REQUEST, OnOpenSamplerRequest);
            m_DecodeHandlers.Add(ProfilerClientMessageID.CLOSE_SAMPLER_REQUEST, OnCloseSamplerRequest);
        }

        public object DecodeMessage(int messageID, byte[] bytes)
        {
            if(m_DecodeHandlers.TryGetValue(messageID,out var handler))
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

        private C2S_OpenSamplerRequest OnOpenSamplerRequest(byte[] bytes)
        {
            return OnDecodeMessage<C2S_OpenSamplerRequest>(bytes);
        }

        private C2S_CloseSamplerRequest OnCloseSamplerRequest(byte[] bytes)
        {
            return OnDecodeMessage<C2S_CloseSamplerRequest>(bytes);
        }
    }

    
}
