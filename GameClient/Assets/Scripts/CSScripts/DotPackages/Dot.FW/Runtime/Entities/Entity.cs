using DotEngine.Framework.Pool;
using System;
using System.Collections.Generic;

namespace DotEngine.Framework.Entities
{
    public class Entity : IEntity
    {
        public IEntityContext ContextInfo { get; private set;}
        public bool Enable { get; set; } = true;

        protected List<IEntityController> controllers = new List<IEntityController>();
        private Dictionary<int, List<EntityEventHandler>> eventHandlerDic = new Dictionary<int, List<EntityEventHandler>>();

        public bool HasController(Type type)
        {
            if(controllers.Count>0)
            {
                foreach(var controller in controllers)
                {
                    if(type.IsAssignableFrom(controller.GetType()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public IEntityController GetController(Type type)
        {
            if (controllers.Count > 0)
            {
                foreach (var controller in controllers)
                {
                    if (type.IsAssignableFrom(controller.GetType()))
                    {
                        return controller;
                    }
                }
            }
            return null;
        }

        public T GetController<T>()where T:IEntityController
        {
            return (T)GetController(typeof(T));
        }

        public IEntityController AddController(Type type)
        {
            IEntityController controller = (IEntityController)Activator.CreateInstance(type);
            controller.AddedToEntity(this);
            controllers.Add(controller);

            return controller;
        }
        public T AddController<T>() where T : IEntityController
        {
            return (T)AddController(typeof(T));
        }

        public IEntityController[] GetControllers(Type type)
        {
            List<IEntityController> list = ListPool<IEntityController>.Get();

            if (controllers.Count > 0)
            {
                foreach (var controller in controllers)
                {
                    if (type.IsAssignableFrom(controller.GetType()))
                    {
                        list.Add(controller);
                    }
                }
            }
            IEntityController[] results = list.ToArray();

            ListPool<IEntityController>.Release(list);
            return results;
        }

        public T[] GetControllers<T>() where T : IEntityController
        {
            List<T> list = ListPool<T>.Get();

            if (controllers.Count > 0)
            {
                Type targetType = typeof(T);
                foreach (var controller in controllers)
                {
                    if (targetType.IsAssignableFrom(controller.GetType()))
                    {
                        list.Add((T)controller);
                    }
                }
            }
            T[] results = list.ToArray();

            ListPool<T>.Release(list);
            return results;
        }

        public void RemoveController(Type type)
        {
            if (controllers.Count > 0)
            {
                foreach (var controller in controllers)
                {
                    if (type.IsAssignableFrom(controller.GetType()))
                    {
                        controllers.Remove(controller);
                        controller.RemovedFromEntity();
                        break;
                    }
                }
            }
        }

        public void RemoveController<T>() where T : IEntityController
        {
            RemoveController(typeof(T));
        }

        public void RemoveControllers(Type type)
        {
            if (controllers.Count > 0)
            {
                for(int i=0;i<controllers.Count;)
                {
                    IEntityController controller = controllers[i];
                    if (type.IsAssignableFrom(controller.GetType()))
                    {
                        controllers.RemoveAt(i);
                        controller.RemovedFromEntity();
                    }else
                    {
                        ++i;
                    }
                }
            }
        }

        public void RemoveControllers<T>() where T:IEntityController
        {
            RemoveControllers(typeof(T));
        }

        public void ClearControllers()
        {
            if (controllers.Count > 0)
            {
                for (int i = 0; i < controllers.Count;)
                {
                    IEntityController controller = controllers[0];
                    controllers.RemoveAt(i);
                    controller.RemovedFromEntity();
                }
            }
        }


        public void RegisterEvent(int eventID, EntityEventHandler handler)
        {
            if(!eventHandlerDic.TryGetValue(eventID,out var handlers))
            {
                handlers = new List<EntityEventHandler>();
                eventHandlerDic.Add(eventID, handlers);
            }
            handlers.Add(handler);
        }

        public void UnregisterEvent(int eventID,EntityEventHandler handler)
        {
            if (eventHandlerDic.TryGetValue(eventID, out var handlers))
            {
                for(int i =0;i<handlers.Count;)
                {
                    if(handlers[i] == null || handlers[i] == handler)
                    {
                        handlers.RemoveAt(i);
                    }else
                    {
                        ++i;
                    }
                }

                if(handlers.Count == 0)
                {
                    eventHandlerDic.Remove(eventID);
                }
            }
        }

        public void TriggerEvent(object sender,int eventID,object data = null)
        {
            if (eventHandlerDic.TryGetValue(eventID, out var handlers))
            {
                foreach(var handler in handlers)
                {
                    handler.Invoke(sender, eventID, data);
                }
            }
        }

        public void ClearEvents()
        {
            eventHandlerDic.Clear();
        }

    }
}
