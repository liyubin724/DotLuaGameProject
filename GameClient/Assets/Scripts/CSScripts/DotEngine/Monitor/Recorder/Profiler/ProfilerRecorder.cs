using DotEngine.Monitor.Sampler;
using DotEngine.Net.Server;

namespace DotEngine.Monitor.Recorder
{
    public class ProfilerRecordMessage
    {
        public MonitorSamplerType Type { get; set; } = MonitorSamplerType.None;
        public MonitorRecord[] Records { get; set; } = new MonitorRecord[0];
    }

    public class ProfilerRecorder : MonitorRecorder
    {
        public const int SERVER_ID = 99;
        public const int SERVER_PORT = 3302;
        public override MonitorRecorderType Type => MonitorRecorderType.Profiler;

        private ServerNetListener m_NetListener = null;

        public ProfilerRecorder()
        {
        }

        public override void DoInit()
        {
            m_NetListener = new ServerNetListener(SERVER_ID, new ProfilerRecorderMessageParser());
            m_NetListener.RegisterMessageHandler("Handler", new ProfilerRecorderMessageHandler());
            m_NetListener.Startup("127.0.0.1", SERVER_PORT, 2);
        }

        public override void DoUpdate(float deltaTime)
        {
            m_NetListener?.DoUpdate(deltaTime);
            m_NetListener?.DoLateUpdate();
        }

        public override void HandleRecords(MonitorSamplerType type, MonitorRecord[] records)
        {
            if(m_NetListener!=null)
            {
                ProfilerRecordMessage recordMessage = new ProfilerRecordMessage
                {
                    Type = type,
                    Records = records
                };

                m_NetListener.SendMessage(ProfilerRecorderSendMessageID.SEND_RECORDS_MESSAGE, recordMessage);
            }
        }

        public override void Dispose()
        {
            m_NetListener?.Dispose();
            m_NetListener = null;

            base.Dispose();
        }
    }
}
