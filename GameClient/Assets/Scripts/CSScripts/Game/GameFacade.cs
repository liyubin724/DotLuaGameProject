﻿using DotEngine;
using DotEngine.Net.Services;
using DotEngine.PMonitor;
using DotEngine.PMonitor.Sampler;

namespace Game
{
    public class GameFacade : Facade
    {
        public new static Facade GetInstance()
        {
            if(instance == null)
            {
                instance = new GameFacade();
            }
            return instance;
        }

        protected override void InitializeFacade()
        {
            base.InitializeFacade();
        }

        protected override void InitializeService()
        {
            base.InitializeService();

            ServerNetService serverNetService = new ServerNetService();
            serviceCenter.RegisterService(serverNetService);

            ClientNetService clientNetService = new ClientNetService();
            serviceCenter.RegisterService(clientNetService);

            MonitorService monitorService = new MonitorService();
            serviceCenter.RegisterService(monitorService);

            //monitorService.OpenSampler(SamplerCategory.Log);
            //monitorService.OpenSampler(SamplerCategory.FPS);
            //monitorService.OpenSampler(SamplerCategory.Memory);
            //monitorService.OpenSampler(SamplerCategory.System);
            monitorService.OpenFileRecorder("D:/");
            monitorService.OpenProfilerRecorder();
        }
    }
}
