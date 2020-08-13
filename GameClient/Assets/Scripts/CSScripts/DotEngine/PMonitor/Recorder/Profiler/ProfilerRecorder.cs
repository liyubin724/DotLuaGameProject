using DotEngine.Net.Server;
using DotEngine.Net.Services;
using DotEngine.PMonitor.Sampler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ServerNetService serverNetService = Facade.GetInstance().GetService<ServerNetService>(ServerNetService.NAME);
            m_NetListener = serverNetService.CreateNet(SERVER_ID, new ProfilerServerMessageParser(), SERVER_PORT);
            ProfilerServerMessageHandler.RegisterHanlder(m_NetListener);
        }

        public override void Dispose()
        {
            ServerNetService serverNetService = Facade.GetInstance().GetService<ServerNetService>(ServerNetService.NAME);
            serverNetService.DiposeNet(SERVER_ID);
            m_NetListener = null;
        }

        public override void HandleRecord(SamplerCategory category, Record[] records)
        {
            
        }
    }
}
