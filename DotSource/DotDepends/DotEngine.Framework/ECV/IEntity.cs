using DotEngine.Notification;
using System;

namespace DotEngine.Framework
{
    public interface IEntity : IObserver, INotifier
    {
        int UniqueId { get; }
        bool Enable { get; set; }
        bool EnableUpdate { get; set; }

        void DoInitilize();
        void DoRetain(int id, object paramValue);
        void DoActive();
        void DoUpdate(float deltaTime);
        void DoDeactive();
        void DoRelease();
        void DoDestroy();

        string[] GetControllerNames();

        int GetControllerCount();

        bool HasController(string name);
        bool HasControllers(string[] names);
        bool HasAnyController(string[] names);

        IController GetController(string name);
        T GetController<T>(string name) where T : IController;
        IController[] GetControllers(string[] names);
        T[] GetControllers<T>() where T : IController;
        IController[] GetAllControllers();

        bool AddController<T>(string name, T controller) where T : IController;
        IController CreateController(string name, Type controllerType);
        IController CreateController<T>(string name) where T : IController;

        bool RemoveController(string name);
        void RemoveControllers(string[] names);
        void RemoveAllControllers();

        void ReplaceController<T>(string name, T controller) where T : IController;
        T ReplaceController<T>(string name) where T : IController;
        IController ReplaceController(string name, Type controllerType);
    }
}
