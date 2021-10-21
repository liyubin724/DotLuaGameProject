using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class AssetAsyncResult
    {
        internal int id = -1;

        private string[] addresses = null;
        private UnityObject[] uObjects = null;
        private float[] progresses = null;
        private bool[] loadedFlags = null;

        public bool IsDone()
        {
            foreach(var flag in loadedFlags)
            {
                if(!flag)
                {
                    return false;
                }
            }
            return true;
        }

        public UnityObject GetUObject()
        {
            if(uObjects.Length>0)
            {
                return uObjects[0];
            }
            return null;
        }

        public UnityObject[] GetUObjects()
        {
            return uObjects;
        }

        public float GetProgress()
        {
            if(progresses != null && progresses.Length>0)
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
            foreach(var progress in progresses)
            {
                sum += progress;
            }
            return sum / progresses.Length;
        }

        internal void SetAddress(string[] addresses)
        {
            this.addresses = addresses;
            uObjects = new UnityObject[addresses.Length];
            progresses = new float[addresses.Length];
            loadedFlags = new bool[addresses.Length];

            for (int i =0;i< addresses.Length;++i)
            {
                uObjects[i] = null;
                progresses[i] = 0.0f;
                loadedFlags[i] = false;
            }
        }

        internal void SetUObject(int index,UnityObject uObject)
        {
            uObjects[index] = uObject;
            loadedFlags[index] = true;
            progresses[index] = 1.0f;
        }

        internal void SetProgress(int index,float progress)
        {
            progresses[index] = progress;
        }
    }
}
