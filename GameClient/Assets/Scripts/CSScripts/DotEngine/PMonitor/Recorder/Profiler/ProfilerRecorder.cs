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
            
        }
    }
}
