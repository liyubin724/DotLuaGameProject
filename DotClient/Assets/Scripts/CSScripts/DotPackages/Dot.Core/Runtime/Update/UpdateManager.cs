using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Core.Update
{
    public class UpdateManager : Singleton<UpdateManager>
    {
        private UpdateBehaviour updateBehaviour;

        private List<IUpdate> updaters = new List<IUpdate>();
        private List<ILateUpdate> lateUpdaters = new List<ILateUpdate>();
        private List<IFixedUpdate> fixedUpdaters = new List<IFixedUpdate>();

        public void AddUpdater(IUpdate updater)
        {
#if DEBUG
            if(updaters.Contains(updater))
            {
                Debug.LogError("The updater has been added to the list");
                return;
            }
#endif
            updaters.Add(updater);
        }

        public void RemoveUpdater(IUpdate updater)
        {
            updaters.Remove(updater);
        }

        public void AddLateUpdater(ILateUpdate lateUpdater)
        {
#if DEBUG
            if (lateUpdaters.Contains(lateUpdater))
            {
                Debug.LogError("The updater has been added to the list");
                return;
            }
#endif
            lateUpdaters.Add(lateUpdater);
        }

        public void RemoveLateUpdater(ILateUpdate lateUpdater)
        {
            lateUpdaters.Remove(lateUpdater);
        }

        public void AddFixedUpdater(IFixedUpdate fixedUpdater)
        {
#if DEBUG
            if (fixedUpdaters.Contains(fixedUpdater))
            {
                Debug.LogError("The updater has been added to the list");
                return;
            }
#endif
            fixedUpdaters.Add(fixedUpdater);
        }

        public void RemoveFixedUpdater(IFixedUpdate fixedUpdater)
        {
            fixedUpdaters.Remove(fixedUpdater);
        }

        protected override void OnInit()
        {
            base.OnInit();
            updateBehaviour = PersistentUObjectHelper.CreateComponent<UpdateBehaviour>();
        }

        protected override void OnDestroy()
        {
            if(updateBehaviour)
            {
                GameObject.Destroy(updateBehaviour.gameObject);
                updateBehaviour = null;
            }

            base.OnDestroy();
        }

        internal void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            foreach(var updater in updaters)
            {
                updater.DoUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        internal void DoLateUpdate(float deltaTime,float unscaleDeltaTime)
        {
            foreach (var updater in lateUpdaters)
            {
                updater.DoLateUpdate(deltaTime, unscaleDeltaTime);
            }
        }

        internal void DoFixedUpdate(float deltaTime,float unscaleDeltaTime)
        {
            foreach (var updater in fixedUpdaters)
            {
                updater.DoFixedUpdate(deltaTime, unscaleDeltaTime);
            }
        }

    }
}
