using System;

namespace DotEngine.Framework
{
    public interface IEntity
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

        void AddController<T>(string name, T controller) where T : IController;
        IController CreateController(string name, Type controllerType);
        IController CreateController<T>(string name) where T : IController;

        bool RemoveController(string name);
        bool RemoveControllers(string[] names);
        void RemoveAllControllers();

        void ReplaceController<T>(string name, T controller) where T : IController;
        void ReplaceController<T>(string name) where T : IController;
        void ReplaceController(string name, Type controllerType);

        void BroadcastMessage(string name, object body);
        void BubbleMessage(string name, object body);
    }
}
