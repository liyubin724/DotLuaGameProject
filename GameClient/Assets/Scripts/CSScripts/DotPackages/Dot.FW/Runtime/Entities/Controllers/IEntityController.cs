namespace DotEngine.Framework.Entities
{
    public interface IEntityController
    {
        IEntity Owner { get; }
        bool Enable { get; set; }

        void AddedToEntity(IEntity entity);
        void RegisterEvents();
        int[] ListEvents();
        void HandleEvent(object sender, int eventID, object data);
        void TriggerEvent(int eventID, object data);
        void UnregisterEvents();
        void RemovedFromEntity();
    }
}