namespace DotEngine.BD.Datas
{
    [ActionData("Animator", "Set Speed", ActionCategory.Animator, Target = BDDataTarget.Client)]
    public class AnimatorSetSpeedData : EventActionData
    {
        public float Speed = 1.0f;
    }
}
