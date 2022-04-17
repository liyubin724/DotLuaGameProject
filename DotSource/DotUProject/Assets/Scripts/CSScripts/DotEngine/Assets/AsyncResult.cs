using DotEngine.Pool;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class AsyncResult : IElement
    {
        public int ID { get; private set; } = 0;

        private string[] addresses = null;
        private bool[] loadedFlags = null;
        private float[] progresses = null;
        private UnityObject[] uObjects = null;

        internal void DoInitialize(int id, string[] addresses)
        {
            ID = id;
            this.addresses = addresses;
            uObjects = new UnityObject[addresses.Length];
            progresses = new float[addresses.Length];
            loadedFlags = new bool[addresses.Length];

            for (int i = 0; i < addresses.Length; ++i)
            {
                uObjects[i] = null;
                progresses[i] = 0.0f;
                loadedFlags[i] = false;
            }
        }

        public int Count()
        {
            return addresses.Length;
        }

        public string GetAddressAt(int index)
        {
            return addresses[index];
        }

        public UnityObject GetUObject()
        {
            if (uObjects.Length > 0)
            {
                return uObjects[0];
            }
            return null;
        }

        public UnityObject[] GetUObjects()
        {
            return uObjects;
        }

        public UnityObject GetUObjectAt(int index)
        {
            return uObjects[index];
        }

        internal void SetUObjectAt(int index, UnityObject uObject)
        {
            uObjects[index] = uObject;
            loadedFlags[index] = true;
            progresses[index] = 1.0f;
        }

        public float GetProgress()
        {
            if (progresses != null && progresses.Length > 0)
            {
                return progresses[0];
            }
            return 0.0f;
        }

        public float[] GetProgresses()
        {
            return progresses;
        }

        public float TotalProgress()
        {
            float sum = 0;
            foreach (var progress in progresses)
            {
                sum += progress;
            }
            return sum / progresses.Length;
        }

        public float GetProgressAt(int index)
        {
            return progresses[index];
        }

        internal void SetProgressAt(int index, float progress)
        {
            progresses[index] = progress;
        }

        public bool IsDoneAt(int index)
        {
            return loadedFlags[index];
        }

        public bool IsDone()
        {
            foreach (var flag in loadedFlags)
            {
                if (!flag)
                {
                    return false;
                }
            }
            return true;
        }

        public void OnGetFromPool()
        {
        }

        public void OnReleaseToPool()
        {
            ID = 0;
            addresses = null;
            loadedFlags = null;
            progresses = null;
            uObjects = null;
        }
    }
}
