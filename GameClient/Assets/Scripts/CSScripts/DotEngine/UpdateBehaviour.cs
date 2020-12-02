using DotEngine.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine
{
    public interface IUpdate
    {
        void DoUpdate(float deltaTime, float unscaleDeltaTime);
    }

    public interface ILateUpdate
    {
        void DoLateUpdate(float deltaTime, float unscaleDeltaTime);
    }

    public interface IFixedUpdate
    {
        void DoFixedUpdate(float deltaTime, float unscaleDeltaTime);
    }

    public class UpdateBehaviour : MonoBehaviour
    {
        public const string NAME = "Updater";
        public static UpdateBehaviour sm_Updater;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnStartup()
        {
            sm_Updater = DontDestroyUtility.CreateComponent<UpdateBehaviour>(NAME);
        }

        private List<IUpdate> m_Updates = new List<IUpdate>();
        private List<ILateUpdate> m_LateUpdates = new List<ILateUpdate>();
        private List<IFixedUpdate> m_FixedUpdates = new List<IFixedUpdate>();

        public static void AddUpdate(IUpdate updater) => sm_Updater?.m_Updates.Add(updater);
        public static void RemoveUpdate(IUpdate updater) => sm_Updater?.m_Updates.Remove(updater);

        public static void AddLateUpdate(ILateUpdate updater) => sm_Updater?.m_LateUpdates.Add(updater);
        public static void RemoveLateUpdate(ILateUpdate updater) => sm_Updater?.m_LateUpdates.Remove(updater);

        public static void AddFixedUpdate(IFixedUpdate updater) => sm_Updater?.m_FixedUpdates.Add(updater);
        public static void RemoveFixedUpdate(IFixedUpdate updater) => sm_Updater?.m_FixedUpdates.Remove(updater);

        private void Update()
        {
            m_Updates.ForEach((updater) =>
            {
                updater.DoUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            });
        }

        private void LateUpdate()
        {
            m_LateUpdates.ForEach((updater) =>
            {
                updater.DoLateUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            });
        }

        private void FixedUpdate()
        {
            m_FixedUpdates.ForEach((updater) =>
            {
                updater.DoFixedUpdate(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
            });
        }

        private void OnDestroy()
        {
            m_FixedUpdates.Clear();
            m_LateUpdates.Clear();
            m_FixedUpdates.Clear();

            sm_Updater = null;
        }
    }
}
