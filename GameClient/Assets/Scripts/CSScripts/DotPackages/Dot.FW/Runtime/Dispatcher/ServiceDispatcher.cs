using DotEngine.Framework.Services;
using DotEngine.Framework.Updater;
using System;
using System.Collections.Generic;

namespace DotEngine.Framework.Dispatcher
{
    public class ServiceDispatcher : ADispatcher<string, IService>, IServiceDispatcher
    {
        private List<IUpdate> updateServices = new List<IUpdate>();
        private List<ILateUpdate> lateUpdateServices = new List<ILateUpdate>();
        private List<IFixedUpdate> fixedUpdateServices = new List<IFixedUpdate>();

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            updateServices.ForEach((service) =>
            {
                service.DoUpdate(deltaTime, unscaleDeltaTime);
            });
        }

        public void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            lateUpdateServices.ForEach((service) =>
            {
                service.DoLateUpdate(deltaTime, unscaleDeltaTime);
            });
        }

        public void DoFixedUpdate(float deltaTime,float unscaleDeltaTime)
        {
            fixedUpdateServices.ForEach((service) =>
            {
                service.DoFixedUpdate(deltaTime, unscaleDeltaTime);
            });
        }

        protected override void DoDisposed()
        {
            foreach(var service in itemDic.Values)
            {
                service.DoUnregistered();
            }
            updateServices.Clear();
            lateUpdateServices.Clear();
            fixedUpdateServices.Clear();
        }

        protected override void DoInitalized()
        {
        }

        protected override void DoRegisterItem(string key, IService service)
        {
            Type serviceType = service.GetType();
            if (typeof(IUpdate).IsAssignableFrom(serviceType))
            {
                updateServices.Add((IUpdate)service);
            }
            if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
            {
                lateUpdateServices.Add((ILateUpdate)service);
            }
            if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
            {
                fixedUpdateServices.Add((IFixedUpdate)service);
            }

            service.DoRegistered();
        }

        protected override void DoUnregisterItem(string key, IService service)
        {
            Type serviceType = service.GetType();
            if (typeof(IUpdate).IsAssignableFrom(serviceType))
            {
                updateServices.Remove((IUpdate)service);
            }
            if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
            {
                lateUpdateServices.Remove((ILateUpdate)service);
            }
            if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
            {
                fixedUpdateServices.Remove((IFixedUpdate)service);
            }

            service.DoUnregistered();
        }
    }
}