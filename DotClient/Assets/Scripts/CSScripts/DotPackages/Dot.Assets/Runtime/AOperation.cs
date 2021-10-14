using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public abstract class AOperation
    {
        protected bool isRunning = false;
        protected string assetPath = null;

        public abstract bool IsFinished { get; }
        public abstract float Progress { get; }

        public abstract UnityObject GetAsset();

        public virtual void DoInitilize(string path)
        {
            assetPath = path;
        }

        public virtual void DoStart()
        {
            isRunning = true;
        }
        public virtual void DoEnd()
        {
            isRunning = false;
            assetPath = null;
        }
    }
}
