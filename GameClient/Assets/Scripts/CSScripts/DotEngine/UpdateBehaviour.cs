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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnStartup()
        {
            DontDestroyHandler.CreateComponent<UpdateBehaviour>(NAME);
        }

        public static UpdateBehaviour Updater { get; private set; } = null;

        private List<IUpdate> m_Updates = new List<IUpdate>();
        private List<ILateUpdate> m_LateUpdates = new List<ILateUpdate>();
        private List<IFixedUpdate> m_FixedUpdates = new List<IFixedUpdate>();

        public void AddUpdate(IUpdate updater) => m_Updates.Add(updater);
        public void RemoveUpdate(IUpdate updater) => m_Updates.Remove(updater);

        public void AddLateUpdate(ILateUpdate updater) => m_LateUpdates.Add(updater);
        public void RemoveLateUpdate(ILateUpdate updater) => m_LateUpdates.Remove(updater);

        public void AddFixedUpdate(IFixedUpdate updater) => m_FixedUpdates.Add(updater);
        public void RemoveFixedUpdate(IFixedUpdate updater) => m_FixedUpdates.Remove(updater);

        private void Awake()
        {
            if(Updater!=null)
            {
                Destroy(this);
            }else
            {
                Updater = this;
            }
        }

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
            Updater = null;
            m_FixedUpdates.Clear();
            m_LateUpdates.Clear();
            m_FixedUpdates.Clear();
        }
    }
}
