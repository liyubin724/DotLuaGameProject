using System;
using System.Collections.Generic;

namespace DotEngine.Services
{
    public class ServiceCenter 
    {
        private Dictionary<string, IService> serviceDic = new Dictionary<string, IService>();

        private List<string> updateServices = null;
        private List<string> lateUpdateServices = null;
        private List<string> fixedUpdateServices = null;

        public ServiceCenter()
        {
            serviceDic = new Dictionary<string, IService>();
            updateServices = new List<string>();
            lateUpdateServices = new List<string>();
            fixedUpdateServices = new List<string>();
        }
        
        public void DoUpdate(float deltaTime)
        {
            for (int i = updateServices.Count - 1; i >= 0; --i)
            {
                string name = updateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IUpdate)value).DoUpdate(deltaTime);
                }
                else
                {
                    updateServices.RemoveAt(i);
                }
            }
        }

        public void DoLateUpdate(float deltaTime)
        {
            for (int i = lateUpdateServices.Count - 1; i >= 0; --i)
            {
                string name = lateUpdateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((ILateUpdate)value).DoLateUpdate(deltaTime);
                }
                else
                {
                    lateUpdateServices.RemoveAt(i);
                }
            }
        }

        public void DoFixedUpdate(float deltaTime)
        {
            for (int i = fixedUpdateServices.Count - 1; i >= 0; --i)
            {
                string name = fixedUpdateServices[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IFixedUpdate)value).DoFixedUpdate(deltaTime);
                }
                else
                {
                    fixedUpdateServices.RemoveAt(i);
                }
            }
        }

        public bool HasService(string name)
        {
            return serviceDic.ContainsKey(name);
        }

        public void RegisterService(IService service)
        {
            if (service == null || string.IsNullOrEmpty(service.Name))
            {
                throw new ArgumentNullException("The service or the name of service is empty");
            }

            if (serviceDic.ContainsKey(service.Name))
            {
                throw new Exception($"The name of service has been added.name = {service.Name}.");
            }

            serviceDic.Add(service.Name, service);

            Type serviceType = service.GetType();
            if (typeof(IUpdate).IsAssignableFrom(serviceType))
            {
                updateServices.Add(service.Name);
            }
            if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
            {
                lateUpdateServices.Add(service.Name);
            }
            if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
            {
                fixedUpdateServices.Add(service.Name);
            }

            service.DoRegister();
        }

        public void RemoveService(string name)
        {
            if (serviceDic.TryGetValue(name, out IService servicer))
            {
                servicer.DoRemove();
                serviceDic.Remove(name);
            }
        }

        public IService RetrieveService(string name)
        {
            return serviceDic.TryGetValue(name, out IService servicer) ? servicer : null;
        }
    }
}
