namespace DotEngine.BD.Datas
{
    [ActionData("Time/Events", "Set TimeScale", ActionCategory.Time)]
    public class TimeSetTimeScaleEvent : EventActionData
    {
        public float TimeScale = 1.0f;
    }
}
