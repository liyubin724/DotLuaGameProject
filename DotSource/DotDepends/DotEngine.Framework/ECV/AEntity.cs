using DotEngine.Generic;
using DotEngine.Notification;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Framework
{
    public class AEntity : IEntity
    {
        public int UniqueId { get; private set; }

        public bool Enable { get; set; } = true;
        public bool EnableUpdate { get; set; } = false;

        protected Dispatcher dispatcher = null;
        private ListDictionary<string, IController> controllerListDic = null;

        public void DoInitilize()
        {
            dispatcher = new Dispatcher();
            controllerListDic = new ListDictionary<string, IController>();
        }

        public virtual void DoRetain(int id, object paramValue)
        {
            UniqueId = id;
        }

        public void DoActive()
        {
            Enable = true;
        }

        public void DoUpdate(float deltaTime)
        {
            if (!EnableUpdate)
            {
                return;
            }
        }

        public void DoDeactive()
        {
        }

        public void DoRelease()
        {

        }

        public void DoDestroy()
        {

        }

        public string[] GetControllerNames()
        {
            return controllerListDic.Keys;
        }

        public int GetControllerCount()
        {
            return controllerListDic.Count;
        }

        public bool HasController(string name)
        {
            return controllerListDic.ContainsKey(name);
        }

        public bool HasControllers(string[] names)
        {
            if (names == null || names.Length == 0)
            {
                return false;
            }
            foreach (var name in names)
            {
                if (!controllerListDic.ContainsKey(name))
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasAnyController(string[] names)
        {
            if (names == null || names.Length == 0)
            {
                return false;
            }
            foreach (var name in names)
            {
                if (controllerListDic.ContainsKey(name))
                {
                    return true;
                }
            }

            return false;
        }

        public IController GetController(string name)
        {
            if (controllerListDic.TryGetValue(name, out var controller))
            {
                return controller;
            }
            return null;
        }

        public T GetController<T>(string name) where T : IController
        {
            if (controllerListDic.TryGetValue(name, out var controller))
            {
                return (T)controller;
            }
            return default;
        }

        public IController[] GetControllers(string[] names)
        {
            if (names == null || names.Length == 0)
            {
                return null;
            }
            var list = ListPool<IController>.Get();
            for (int i = 0; i < names.Length; i++)
            {
                var controller = controllerListDic[names[i]];
                if (controller != null)
                {
                    list.Add(controller);
                }
            }

            IController[] result = list.ToArray();
            ListPool<IController>.Release(list);

            return result;
        }

        public T[] GetControllers<T>() where T : IController
        {
            var list = new List<T>();
            for(int i =0;i<controllerListDic.Count;i++)
            {
                var controller = (T)controllerListDic[i];
                if(controller != null && typeof(T).IsAssignableFrom(controller.GetType()))
                {
                    list.Add(controller);
                }
            }
            return list.ToArray();
        }

        public IController[] GetAllControllers()
        {
            return controllerListDic.Values;
        }

        public bool AddController<T>(string name, T controller) where T : IController
        {
            if (string.IsNullOrEmpty(name) || controller == null)
            {
                return false;
            }
            if (controllerListDic.ContainsKey(name))
            {
                return false;
            }
            controllerListDic.Add(name, controller);
            return true;
        }

        public IController CreateController(string name, Type controllerType)
        {
            if (controllerType == null || !typeof(IController).IsAssignableFrom(controllerType))
            {
                return null;
            }
            var controller = (IController)Activator.CreateInstance(controllerType);
            if (AddController(name, controller))
            {
                return controller;
            }
            return null;
        }

        public IController CreateController<T>(string name) where T : IController
        {
            return CreateController(name, typeof(T));
        }

        public bool RemoveController(string name)
        {
            if(!controllerListDic.ContainsKey(name))
            {
                return false;
            }
            controllerListDic.Remove(name);
            return true;
        }

        public void RemoveControllers(string[] names)
        {
            if(names == null || names.Length == 0)
            {
                return;
            }

            foreach(var name in names)
            {
                RemoveController(name);
            }
        }

        public void RemoveAllControllers()
        {
            controllerListDic.Clear();
        }

        public void ReplaceController<T>(string name, T controller) where T : IController
        {
            if(string.IsNullOrEmpty(name) || controller == null)
            {
                return;
            }

            if (!controllerListDic.ContainsKey(name))
            {
                AddController(name, controller);
            }
            else
            {
                controllerListDic[name] = controller;
            }
        }

        public T ReplaceController<T>(string name) where T : IController
        {
            return (T)ReplaceController(name, typeof(T));
        }

        public IController ReplaceController(string name, Type controllerType)
        {
            if (controllerType == null || !typeof(IController).IsAssignableFrom(controllerType))
            {
                return null;
            }
            var controller = (IController)Activator.CreateInstance(controllerType);
            if(!controllerListDic.ContainsKey(name))
            {
                if(AddController(name,controller))
                {
                    return controller;
                }
                return null;
            }
            else
            {
                controllerListDic[name] = controller;
                return controller;
            }
        }

        public void HandleNotification(string name, object body = null)
        {
            throw new NotImplementedException();
        }

        public string[] ListInterestNotification()
        {
            throw new NotImplementedException();
        }

        public void SendNotification(string name, object body = null)
        {
            throw new NotImplementedException();
        }
    }
}
