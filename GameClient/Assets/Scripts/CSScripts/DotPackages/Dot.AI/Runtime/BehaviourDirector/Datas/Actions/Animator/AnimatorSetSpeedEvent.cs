namespace DotEngine.BD.Datas
{
    [ActionData("Animator/Events", "Set Speed", ActionCategory.Animator, Target = BDDataTarget.Client)]
    public class AnimatorSetSpeedEvent : EventActionData
    {
        public float Speed = 1.0f;
    }
}
