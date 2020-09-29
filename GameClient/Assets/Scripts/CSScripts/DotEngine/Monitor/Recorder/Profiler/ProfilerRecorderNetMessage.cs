using DotEngine.Net.Message;
using DotEngine.Net.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Monitor.Recorder
{
    public static class ProfilerRecorderSendMessageID
    {
        public static int SEND_RECORDS_MESSAGE = 1;
    }

    public static class ProfilerRecorderReceiveMessageID
    {

    }

    public class ProfilerRecorderMessageParser : IMessageParser
    {
        public string SecretKey { get; set; }

        private Dictionary<int,Func<byte[],object>> m_DecodeHandlers = new Dictionary<int, Func<byte[], object>>();
        public ProfilerRecorderMessageParser()
        {
            
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
            string jsonStr = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.None,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            }) ;
            return Encoding.UTF8.GetBytes(jsonStr);
        }
    }

    public class ProfilerRecorderMessageHandler : IServerNetMessageHandler
    {
        public ServerNetListener Listener { get; set; }

        public bool OnMessageHanlder(int netID, int messageID, object message)
        {
            throw new NotImplementedException();
        }
    }
}
