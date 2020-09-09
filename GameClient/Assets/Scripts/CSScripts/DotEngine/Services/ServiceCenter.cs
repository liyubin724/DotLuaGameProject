using System;
using System.Collections.Generic;

namespace DotEngine.Services
{
    public class ServiceCenter 
    {
        private Dictionary<string, IService> serviceDic = new Dictionary<string, IService>();

        private List<string> updateServiceNames = null;
        private List<string> unscaleUpdateServiceNames = null;
        private List<string> lateUpdateServiceNames = null;
        private List<string> fixedUpdateServiceNames = null;

        private List<string> sortedByOrderNames = new List<string>();
        public ServiceCenter()
        {
            serviceDic = new Dictionary<string, IService>();
            updateServiceNames = new List<string>();
            unscaleUpdateServiceNames = new List<string>();
            lateUpdateServiceNames = new List<string>();
            fixedUpdateServiceNames = new List<string>();
        }
        
        public void DoUpdate(float deltaTime)
        {
            for (int i = updateServiceNames.Count - 1; i >= 0; --i)
            {
                string name = updateServiceNames[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IUpdate)value).DoUpdate(deltaTime);
                }
                else
                {
                    updateServiceNames.RemoveAt(i);
                }
            }
        }

        public void DoUnscaleUpdate(float deltaTime)
        {
            for (int i = unscaleUpdateServiceNames.Count - 1; i >= 0; --i)
            {
                string name = unscaleUpdateServiceNames[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IUnscaleUpdate)value).DoUnscaleUpdate(deltaTime);
                }
                else
                {
                    unscaleUpdateServiceNames.RemoveAt(i);
                }
            }
        }

        public void DoLateUpdate(float deltaTime)
        {
            for (int i = lateUpdateServiceNames.Count - 1; i >= 0; --i)
            {
                string name = lateUpdateServiceNames[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((ILateUpdate)value).DoLateUpdate(deltaTime);
                }
                else
                {
                    lateUpdateServiceNames.RemoveAt(i);
                }
            }
        }

        public void DoFixedUpdate(float deltaTime)
        {
            for (int i = fixedUpdateServiceNames.Count - 1; i >= 0; --i)
            {
                string name = fixedUpdateServiceNames[i];
                if (serviceDic.TryGetValue(name, out IService value))
                {
                    ((IFixedUpdate)value).DoFixedUpdate(deltaTime);
                }
                else
                {
                    fixedUpdateServiceNames.RemoveAt(i);
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
            sortedByOrderNames.Add(service.Name);

            Type serviceType = service.GetType();
            if (typeof(IUpdate).IsAssignableFrom(serviceType))
            {
                updateServiceNames.Add(service.Name);
            }
            if(typeof(IUnscaleUpdate).IsAssignableFrom(serviceType))
            {
                unscaleUpdateServiceNames.Add(service.Name);
            }
            if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
            {
                lateUpdateServiceNames.Add(service.Name);
            }
            if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
            {
                fixedUpdateServiceNames.Add(service.Name);
            }

            service.DoRegister();
        }

        public void RemoveService(string name)
        {
            if (serviceDic.TryGetValue(name, out IService servicer))
            {
                serviceDic.Remove(name);

                servicer.DoRemove();

                sortedByOrderNames.Remove(name);
            }
        }

        public IService RetrieveService(string name)
        {
            return serviceDic.TryGetValue(name, out IService servicer) ? servicer : null;
        }

        public void ClearService()
        {
            updateServiceNames.Clear();
            lateUpdateServiceNames.Clear();
            fixedUpdateServiceNames.Clear();

            sortedByOrderNames.Reverse();
            string[] names = sortedByOrderNames.ToArray();
            foreach(var name in names)
            {
                RemoveService(name);
            }
        }
    }
}
