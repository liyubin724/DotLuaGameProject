using DotEngine.Monitor.Recorder;
using DotEngine.Net.Client;
using DotEngine.Net.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DotEditor.Monitor
{
    public static class ProfilerSendMessageID
    {
    }

    public static class ProfilerReceiveMessageID
    {
        public static int RECEIVE_RECORDS_MESSAGE = 1;
    }

    public class ProfilerMessageParser : IMessageParser
    {
        public string SecretKey { get; set; }

        private Dictionary<int, Func<byte[], object>> m_DecodeHandlers = new Dictionary<int, Func<byte[], object>>();
        public ProfilerMessageParser()
        {
            m_DecodeHandlers.Add(ProfilerReceiveMessageID.RECEIVE_RECORDS_MESSAGE, DecodeRecordMessage);
        }

        public object DecodeMessage(int messageID, byte[] bytes)
        {
            if (m_DecodeHandlers.TryGetValue(messageID, out var handler))
            {
                return handler.Invoke(bytes);
            }
            return null;
        }

        public byte[] EncodeMessage(int messageID, object message)
        {
            string jsonStr = JsonConvert.SerializeObject(message, Formatting.None);
            return Encoding.UTF8.GetBytes(jsonStr);
        }

        public object DecodeRecordMessage(byte[] bytes)
        {
            string jsonStr = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject(jsonStr,new JsonSerializerSettings() { 
                TypeNameHandling = TypeNameHandling.All,
            });
        }
    }

    public class ProfilerMessageHandler : IClientNetMessageHandler
    {
        public ClientNet Net { get; set; }
        private Dictionary<int, Action<int, object>> m_MessageHandlerDic = new Dictionary<int, Action<int, object>>();

        public ProfilerMessageHandler()
        {
            m_MessageHandlerDic.Add(ProfilerReceiveMessageID.RECEIVE_RECORDS_MESSAGE, OnRecordsReceived);
        }

        public bool OnMessageHanlder(int messageID, object message)
        {
            if (m_MessageHandlerDic.TryGetValue(messageID, out var action))
            {
                action?.Invoke(messageID, message);
                return true;
            }

            return false;
        }

        public void OnRecordsReceived(int messageID,object message)
        {
            ProfilerRecordMessage recordMessage = (ProfilerRecordMessage)message;
            ProfilerWindow.window?.Model.AddRecordMessage(recordMessage);
        }

    }
}
