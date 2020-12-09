using DotEngine.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine
{
    public class UpdateRunner : MonoBehaviour
    {
        private const string NAME = "Update Hanlder";

        private static UpdateRunner sm_Runner = null;
        internal static UpdateRunner GetRunner()
        {
            if (sm_Runner == null)
            {
                sm_Runner = DontDestroyUtility.CreateComponent<UpdateRunner>(NAME);
            }
            return sm_Runner;
        }

        private List<IUpdate> m_Updates = new List<IUpdate>();
        private List<IUnscaleUpdate> m_UnscaleUpdates = new List<IUnscaleUpdate>();
        private List<ILateUpdate> m_LateUpdates = new List<ILateUpdate>();
        private List<IFixedUpdate> m_FixedUpdates = new List<IFixedUpdate>();

        internal void AddUpdater(IUpdater updater)
        {
            if(updater is IUpdate update)
            {
                m_Updates.Add(update);
            }
            if(updater is IUnscaleUpdate unscaleUpdate)
            {
                m_UnscaleUpdates.Add(unscaleUpdate);
            }
            if(updater is ILateUpdate lateUpdate)
            {
                m_LateUpdates.Add(lateUpdate);
            }
            if(updater is IFixedUpdate fixedUpdate)
            {
                m_FixedUpdates.Add(fixedUpdate);
            }
        }

        internal void RemoveUpdater(IUpdater updater)
        {
            if (updater is IUpdate update)
            {
                m_Updates.Remove(update);
            }
            if (updater is IUnscaleUpdate unscaleUpdate)
            {
                m_UnscaleUpdates.Remove(unscaleUpdate);
            }
            if (updater is ILateUpdate lateUpdate)
            {
                m_LateUpdates.Remove(lateUpdate);
            }
            if (updater is IFixedUpdate fixedUpdate)
            {
                m_FixedUpdates.Remove(fixedUpdate);
            }
        }

        private void Update()
        {
            m_Updates.ForEach((updater) =>
            {
                updater.DoUpdate(Time.deltaTime);
            });

            m_UnscaleUpdates.ForEach((updater) =>
            {
                updater.DoUnscaleUpdate(Time.unscaledDeltaTime);
            });
        }

        private void LateUpdate()
        {
            m_LateUpdates.ForEach((updater) =>
            {
                updater.DoLateUpdate(Time.deltaTime);
            });
        }

        private void FixedUpdate()
        {
            m_FixedUpdates.ForEach((updater) =>
            {
                updater.DoFixedUpdate(Time.fixedDeltaTime);
            });
        }

        private void OnDestroy()
        {
            m_FixedUpdates.Clear();
            m_UnscaleUpdates.Clear();
            m_LateUpdates.Clear();
            m_FixedUpdates.Clear();

            sm_Runner = null;
        }
    }
}
