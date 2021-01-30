namespace DotEngine.BehaviourLine.Action
{
    public abstract class DurationActionData : ActionData
    {
        public float DurationTime = 0.0f;

        public virtual bool IsFixedDurationTime 
        { 
           get
            {
                return false;
            }
        }

    }
}
