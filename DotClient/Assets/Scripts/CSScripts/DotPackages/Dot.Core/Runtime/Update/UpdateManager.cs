using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Core.Update
{
    public class UpdateManager
    {
        private UpdateBehaviour updateBehaviour;

        private List<IUpdate> updaters = new List<IUpdate>();
        private List<ILateUpdate> lateUpdaters = new List<ILateUpdate>();
        private List<IFixedUpdate> fixedUpdaters = new List<IFixedUpdate>();

        private static UpdateManager manager = null;
        public static UpdateManager GetInstance() => manager;
        public static UpdateManager InitMgr()
        {
            if(manager == null)
            {
                manager = new UpdateManager();
                manager.OnInit();
            }
            return manager;
        }
        public static void DisposeMgr()
        {
            if(manager!=null)
            {
                manager.DoDestroy();
            }
            manager = null;
        }

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

        private void OnInit()
        {
            updateBehaviour = PersistentUObjectHelper.CreateComponent<UpdateBehaviour>();
        }

        private void DoDestroy()
        {
            if(updateBehaviour)
            {
                UnityObject.Destroy(updateBehaviour.gameObject);
            }
            updateBehaviour = null;
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
