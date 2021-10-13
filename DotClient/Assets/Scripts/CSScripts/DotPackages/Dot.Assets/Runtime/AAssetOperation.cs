namespace DotEngine.Assets.Operations
{
    public abstract class AAssetOperation : AOperation
    {
        protected bool isRunning = false;
        protected string assetPath = null;

        public override bool IsFinished
        {
            get
            {
                if(isRunning)
                {
                    return true;
                }
                return false;
            }
        }

        public override float GetProgress()
        {
            if(isRunning)
            {
                return 1.0f;
            }
            return 0.0f;
        }

        public override void DoStart(string path)
        {
            isRunning = true;
            assetPath = path;
        }

        public override void DoEnd()
        {
            isRunning = false;
            assetPath = null;
        }
    }
}
