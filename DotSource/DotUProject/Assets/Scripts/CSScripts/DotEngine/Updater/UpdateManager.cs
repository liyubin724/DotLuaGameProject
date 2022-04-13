using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Updater
{
    public class UpdateManager : Singleton<UpdateManager>
    {
        private static string ROOT_NAME = "UpdateMgr";

        private GameObject rootGO;

        private List<WeakReference<Action<float, float>>> updateHandlers = new List<WeakReference<Action<float, float>>>();
        private List<WeakReference<Action<float, float>>> lateUpdateHandlers = new List<WeakReference<Action<float, float>>>();
        private List<WeakReference<Action<float, float>>> fixedUpdateHandlers = new List<WeakReference<Action<float, float>>>();

        public UpdateManager()
        {
        }

        protected override void OnInit()
        {
            rootGO = PersistentUObjectHelper.CreateGameObject(ROOT_NAME);

            UpdateBehaviour updateBehaviour = rootGO.AddComponent<UpdateBehaviour>();
            updateBehaviour.Handler = (deltaTime, unscaleDeltaTime) =>
            {
                InvokeHanlders(updateHandlers, deltaTime, unscaleDeltaTime);
            };

            LateUpdateBehaviour lateUpdateBehaviour = rootGO.AddComponent<LateUpdateBehaviour>();
            lateUpdateBehaviour.Handler = (deltaTime, unscaleDeltaTime) =>
            {
                InvokeHanlders(lateUpdateHandlers, deltaTime, unscaleDeltaTime);
            };

            FixedUpdateBehaviour fixedUpdateBehaviour = rootGO.AddComponent<FixedUpdateBehaviour>();
            fixedUpdateBehaviour.Handler = (deltaTime, unscaleDeltaTime) =>
            {
                InvokeHanlders(fixedUpdateHandlers, deltaTime, unscaleDeltaTime);
            };

            base.OnInit();
        }

        protected override void OnDestroy()
        {
            updateHandlers.Clear();
            lateUpdateHandlers.Clear();
            fixedUpdateHandlers.Clear();

            UnityObject.Destroy(rootGO.gameObject);
            rootGO = null;

            base.OnDestroy();
        }

        private void InvokeHanlders(List<WeakReference<Action<float, float>>> handlers, float deltaTime, float unscaleDeltaTime)
        {
            for (int i = 0; i < handlers.Count;)
            {
                var handler = handlers[i];
                if (handler.TryGetTarget(out var action))
                {
                    action.Invoke(deltaTime, unscaleDeltaTime);
                    i++;
                }
                else
                {
                    handlers.RemoveAt(i);
                }
            }
        }
        private void Register(List<WeakReference<Action<float, float>>> handlers, Action<float, float> updater)
        {
            if (updater == null)
            {
                return;
            }
#if DEBUG
            foreach (var handler in updateHandlers)
            {
                if (handler.TryGetTarget(out var action) && action == updater)
                {
                    throw new Exception("The action has been registered");
                }
            }
#endif
            updateHandlers.Add(new WeakReference<Action<float, float>>(updater));
        }
        private void Unregister(List<WeakReference<Action<float, float>>> handlers, Action<float, float> updater)
        {
            if (updater == null)
            {
                return;
            }

            for (int i = 0; i < updateHandlers.Count; i++)
            {
                var handler = updateHandlers[i];
                if (handler.TryGetTarget(out var action) && action == updater)
                {
                    updateHandlers.RemoveAt(i);
                    break;
                }
            }
        }

        public void RegisterUpdater(Action<float, float> updater) => Register(updateHandlers, updater);
        public void UnregisterUpdater(Action<float, float> updater) => Unregister(updateHandlers, updater);

        public void RegisterLateUpdater(Action<float, float> lateUpdater) => Register(lateUpdateHandlers, lateUpdater);
        public void UnregisterLateUpdater(Action<float, float> lateUpdater) => Unregister(lateUpdateHandlers, lateUpdater);

        public void RegisterFixedUpdater(Action<float, float> fixedUpdater) => Register(fixedUpdateHandlers, fixedUpdater);
        public void UnregisterFixedUpdater(Action<float, float> fixedUpdater) => Unregister(fixedUpdateHandlers, fixedUpdater);

    }
}
