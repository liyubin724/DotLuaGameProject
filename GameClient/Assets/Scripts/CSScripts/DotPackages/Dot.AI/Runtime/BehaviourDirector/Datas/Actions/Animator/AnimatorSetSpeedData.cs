namespace DotEngine.BD.Datas.Actions
{
    [ActionData("Animator","Set Speed",ActionCategory.Animator,Target = DataTarget.Client)]
    public class AnimatorSetSpeedData : EventActionData
    {
        public float Speed = 1.0f;
    }
}
