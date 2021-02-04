using System;

namespace DotEngine.Framework.Entities
{
    public delegate void EntityControllerChanged(IEntity entity, IEntityController entityController);

    public delegate void EntityControllerReplace(IEntity entity, IEntityController oldEntityController, IEntityController newEntityController);

    public delegate void EntityEvent(IEntity entity);
    
    public delegate void EntityEventHandler(object sender, int eventID, object data);

    public interface IEntity
    {
        IEntityContext ContextInfo { get; }
        bool Enable { get; set; }

        bool HasController(Type type);

        IEntityController GetController(Type type);
        T GetController<T>() where T : IEntityController;

        IEntityController AddController(Type type);
        T AddController<T>() where T : IEntityController;

        IEntityController[] GetControllers(Type type);
        T[] GetControllers<T>() where T : IEntityController;

        void RemoveController(Type type);
        void RemoveController<T>() where T : IEntityController;

        void RemoveControllers(Type type);
        void RemoveControllers<T>() where T : IEntityController;

        void ClearControllers();

        void RegisterEvent(int eventID, EntityEventHandler handler);
        void UnregisterEvent(int eventID, EntityEventHandler handler);
        void TriggerEvent(object sender, int eventID, object data = null);
        void ClearEvents();
    }
}