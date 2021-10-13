using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets.Operations
{
    public abstract class AOperation
    {
        public virtual bool IsFinished { get;} = false;

        public abstract float GetProgress();
        public abstract UnityObject GetResult();
        
        public abstract void DoStart(string path);
        public abstract void DoEnd();

    }
}
