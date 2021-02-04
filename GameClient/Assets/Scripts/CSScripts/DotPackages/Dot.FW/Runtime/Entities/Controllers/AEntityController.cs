namespace DotEngine.Framework.Entities
{
    public abstract class AEntityController : IEntityController
    {
        public IEntity Owner { get; private set; }

        public bool Enable { get; set; } = true;

        public void AddedToEntity(IEntity entity)
        {
            Owner = entity;
            RegisterEvents();
            DoAdded();
        }

        public abstract void DoAdded();
        public abstract void HandleEvent(object sender, int eventID, object data);
        public abstract int[] ListEvents();
        public abstract void DoRemoved();

        public void RegisterEvents()
        {
            int[] eventIDs = ListEvents();
            if (eventIDs != null && eventIDs.Length > 0)
            {
                foreach(var eventID in eventIDs)
                {
                    Owner.RegisterEvent(eventID, HandleEvent);
                }
            }
        }

        public void UnregisterEvents()
        {
            int[] eventIDs = ListEvents();
            if (eventIDs != null && eventIDs.Length > 0)
            {
                foreach (var eventID in eventIDs)
                {
                    Owner.UnregisterEvent(eventID, HandleEvent);
                }
            }
        }

        public void RemovedFromEntity()
        {
            UnregisterEvents();
            DoRemoved();
            Owner = null;
        }

        public void TriggerEvent(int eventID, object data)
        {
            Owner.TriggerEvent(this, eventID, data);
        }
    }
}
