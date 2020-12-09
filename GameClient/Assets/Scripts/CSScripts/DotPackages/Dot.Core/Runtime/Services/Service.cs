using System;
using System.Collections.Generic;

namespace DotEngine.Services
{
    public class Service 
    {
        private Dictionary<string, IServicer> m_ServicerDic = new Dictionary<string, IServicer>();

        private List<string> m_UpdateServicerNames = null;
        private List<string> m_LateUpdateServicerNames = null;
        private List<string> m_FixedUpdateServicerNames = null;

        private List<string> m_OrderedByAddtionServicerNames = new List<string>();
        public Service()
        {
            m_ServicerDic = new Dictionary<string, IServicer>();
            m_UpdateServicerNames = new List<string>();
            m_LateUpdateServicerNames = new List<string>();
            m_FixedUpdateServicerNames = new List<string>();
        }
        
        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            for (int i = m_UpdateServicerNames.Count - 1; i >= 0; --i)
            {
                string name = m_UpdateServicerNames[i];
                if (m_ServicerDic.TryGetValue(name, out IServicer value))
                {
                    ((IUpdate)value).DoUpdate(deltaTime);
                }
                else
                {
                    m_UpdateServicerNames.RemoveAt(i);
                }
            }
        }
        
        public void DoLateUpdate(float deltaTime,float unscaleDeltaTime)
        {
            for (int i = m_LateUpdateServicerNames.Count - 1; i >= 0; --i)
            {
                string name = m_LateUpdateServicerNames[i];
                if (m_ServicerDic.TryGetValue(name, out IServicer value))
                {
                    ((ILateUpdate)value).DoLateUpdate(deltaTime);
                }
                else
                {
                    m_LateUpdateServicerNames.RemoveAt(i);
                }
            }
        }

        public void DoFixedUpdate(float deltaTime, float unscaleDeltaTime)
        {
            for (int i = m_FixedUpdateServicerNames.Count - 1; i >= 0; --i)
            {
                string name = m_FixedUpdateServicerNames[i];
                if (m_ServicerDic.TryGetValue(name, out IServicer value))
                {
                    ((IFixedUpdate)value).DoFixedUpdate(deltaTime);
                }
                else
                {
                    m_FixedUpdateServicerNames.RemoveAt(i);
                }
            }
        }

        public bool HasServicer(string name)
        {
            return m_ServicerDic.ContainsKey(name);
        }

        public void RegisterServicer(IServicer servicer)
        {
            if (servicer == null || string.IsNullOrEmpty(servicer.Name))
            {
                throw new ArgumentNullException("The service or the name of service is empty");
            }

            if (m_ServicerDic.ContainsKey(servicer.Name))
            {
                throw new Exception($"The name of service has been added.name = {servicer.Name}.");
            }

            m_ServicerDic.Add(servicer.Name, servicer);
            m_OrderedByAddtionServicerNames.Add(servicer.Name);

            Type serviceType = servicer.GetType();
            if (typeof(IUpdate).IsAssignableFrom(serviceType))
            {
                m_UpdateServicerNames.Add(servicer.Name);
            }
            if (typeof(ILateUpdate).IsAssignableFrom(serviceType))
            {
                m_LateUpdateServicerNames.Add(servicer.Name);
            }
            if (typeof(IFixedUpdate).IsAssignableFrom(serviceType))
            {
                m_FixedUpdateServicerNames.Add(servicer.Name);
            }

            servicer.DoRegister();
        }

        public void RemoveServicer(string name)
        {
            if (m_ServicerDic.TryGetValue(name, out IServicer servicer))
            {
                m_ServicerDic.Remove(name);

                servicer.DoRemove();

                m_OrderedByAddtionServicerNames.Remove(name);
            }
        }

        public IServicer RetrieveServicer(string name)
        {
            return m_ServicerDic.TryGetValue(name, out IServicer servicer) ? servicer : null;
        }

        public void ClearServicer()
        {
            m_UpdateServicerNames.Clear();
            m_LateUpdateServicerNames.Clear();
            m_FixedUpdateServicerNames.Clear();

            m_OrderedByAddtionServicerNames.Reverse();
            string[] names = m_OrderedByAddtionServicerNames.ToArray();
            foreach(var name in names)
            {
                RemoveServicer(name);
            }
        }
    }
}
