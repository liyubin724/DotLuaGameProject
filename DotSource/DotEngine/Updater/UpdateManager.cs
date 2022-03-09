using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Updater
{
    public class UpdateManager : Singleton<UpdateManager>
    {
        public const string UPDATE_BEHAVIOUR_NAME = "Updater";

        private UpdateBehaviour updateBehaviour = null;

        private List<Action<float, float>> updateHandlers = new List<Action<float, float>>();
        private List<Action<float, float>> lateUpdateHandlers = new List<Action<float, float>>();
        private List<Action<float, float>> fixedUpdateHandlers = new List<Action<float, float>>();

        public UpdateManager()
        {
        }

        public void RegisterUpdater(IUpdate updater)
        {
            if(updater == null || updateHandlers.Contains(updater.DoUpdate))
            {
                return;
            }
            updateHandlers.Add(updater.DoUpdate);
        }

        public void RegisterUpdater(Action<float,float> updater)
        {
            if (updater == null || updateHandlers.Contains(updater))
            {
                return;
            }
            updateHandlers.Add(updater);
        }

        public void UnregisterUpdater(IUpdate updater)
        {
            if (updater == null || !updateHandlers.Contains(updater.DoUpdate))
            {
                return;
            }
            updateHandlers.Remove(updater.DoUpdate);
        }

        public void UnregisterUpdater(Action<float,float> updater)
        {
            if (updater == null || !updateHandlers.Contains(updater))
            {
                return;
            }
            updateHandlers.Remove(updater);
        }

        public void RegisterLateUpdater(ILateUpdate lateUpdater)
        {
            if (lateUpdater == null || lateUpdateHandlers.Contains(lateUpdater.DoLateUpdate))
            {
                return;
            }
            lateUpdateHandlers.Add(lateUpdater.DoLateUpdate);
        }

        public void RegisterLateUpdater(Action<float, float> lateUpdater)
        {
            if (lateUpdater == null || lateUpdateHandlers.Contains(lateUpdater))
            {
                return;
            }
            lateUpdateHandlers.Add(lateUpdater);
        }

        public void UnregisterLateUpdater(ILateUpdate lateUpdater)
        {
            if (lateUpdater == null || !lateUpdateHandlers.Contains(lateUpdater.DoLateUpdate))
            {
                return;
            }
            lateUpdateHandlers.Remove(lateUpdater.DoLateUpdate);
        }

        public void UnregisterLateUpdater(Action<float, float> lateUpdater)
        {
            if (lateUpdater == null || !lateUpdateHandlers.Contains(lateUpdater))
            {
                return;
            }
            lateUpdateHandlers.Remove(lateUpdater);
        }

        public void RegisterFixedUpdater(IFixedUpdate fixedUpdater)
        {
            if (fixedUpdater == null || !fixedUpdateHandlers.Contains(fixedUpdater.DoFixedUpdate))
            {
                return;
            }
            fixedUpdateHandlers.Add(fixedUpdater.DoFixedUpdate);
        }

        public void RegisterFixedUpdater(Action<float, float> fixedUpdater)
        {
            if (fixedUpdater == null || !fixedUpdateHandlers.Contains(fixedUpdater))
            {
                return;
            }
            fixedUpdateHandlers.Add(fixedUpdater);
        }

        public void UnregisterFixedUpdater(IFixedUpdate fixedUpdater)
        {
            if (fixedUpdater == null || !fixedUpdateHandlers.Contains(fixedUpdater.DoFixedUpdate))
            {
                return;
            }
            fixedUpdateHandlers.Remove(fixedUpdater.DoFixedUpdate);
        }

        public void UnregisterFixedUpdater(Action<float, float> fixedUpdater)
        {
            if (fixedUpdater == null || !fixedUpdateHandlers.Contains(fixedUpdater))
            {
                return;
            }
            fixedUpdateHandlers.Remove(fixedUpdater);
        }

        protected override void OnInit()
        {
            if(updateBehaviour == null)
            {
                updateBehaviour = PersistentUObjectHelper.CreateComponent<UpdateBehaviour>(UPDATE_BEHAVIOUR_NAME);
                updateBehaviour.SetHandler(OnUpdate, OnLateUpdate, OnFixedUpdate);
            }
        }

        protected override void OnDestroy()
        {
            if(updateBehaviour)
            {
                updateBehaviour.SetHandler(null, null, null);
                UnityObject.Destroy(updateBehaviour.gameObject);
            }
            updateBehaviour = null;

            base.OnDestroy();
        }

        private void OnUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if(updateHandlers.Count>0)
            {
                for(int i =0;i<updateHandlers.Count;++i)
                {
                    updateHandlers[i].Invoke(deltaTime, unscaleDeltaTime);
                }
            }
        }

        private void OnLateUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if (lateUpdateHandlers.Count > 0)
            {
                for (int i = 0; i < lateUpdateHandlers.Count; ++i)
                {
                    lateUpdateHandlers[i].Invoke(deltaTime, unscaleDeltaTime);
                }
            }
        }

        private void OnFixedUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if (fixedUpdateHandlers.Count > 0)
            {
                for (int i = 0; i < fixedUpdateHandlers.Count; ++i)
                {
                    fixedUpdateHandlers[i].Invoke(deltaTime, unscaleDeltaTime);
                }
            }
        }
    }
}
