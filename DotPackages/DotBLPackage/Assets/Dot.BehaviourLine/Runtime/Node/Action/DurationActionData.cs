namespace DotEngine.BL.Node.Action
{
    public class DurationActionData : ActionData
    {
        public float DurationTime  = 0.0f;
        public bool IsFixedDurationTime = false;

        public float StartTime => FireTime;
        public float EndTime => FireTime + DurationTime;
    }
}
