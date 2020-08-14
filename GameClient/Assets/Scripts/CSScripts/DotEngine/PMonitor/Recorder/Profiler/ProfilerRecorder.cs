using DotEngine.Net.Server;
using DotEngine.PMonitor.Sampler;

namespace DotEngine.PMonitor.Recorder
{
    public class ProfilerRecorder : ARecorder
    {
        public const int SERVER_ID = 99;
        public const int SERVER_PORT = 3302;

        private ServerNetListener m_NetListener = null;
        public ProfilerRecorder() : base(RecorderCategory.Profiler)
        {
        }

        public override void Init()
        {
            m_NetListener = new ServerNetListener(SERVER_ID, new ProfilerServerMessageParser());
            m_NetListener.RegisterMessageHandler("Handler", new ProfilerServerMessageHandler());
            m_NetListener.Startup("127.0.0.1", SERVER_PORT,10);
        }

        public override void DoUpdate(float deltaTime)
        {
            m_NetListener.DoUpdate(deltaTime);
            m_NetListener.DoLateUpdate();
        }

        public override void Dispose()
        {
            m_NetListener?.Dispose();
            m_NetListener = null;
        }

        public override void HandleRecord(SamplerCategory category, Record[] records)
        {
            int messageID = -1;
            if(category == SamplerCategory.FPS)
            {
                messageID = ProfilerServerMessageID.PUSH_FPS_RECORDS;
            }else if(category == SamplerCategory.Log)
            {
                messageID = ProfilerServerMessageID.PUSH_LOG_RECORDS;
            }else if(category == SamplerCategory.Memory)
            {
                messageID = ProfilerServerMessageID.PUSH_MEMORY_RECORDS;
            }else if(category == SamplerCategory.System)
            {
                messageID = ProfilerServerMessageID.PUSH_SYSTEM_RECORDS;
            }
            if(messageID>0)
            {
                m_NetListener.SendMessage(messageID, records);
            }
        }
    }
}
