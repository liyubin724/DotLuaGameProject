using DotEngine.NativeDrawer.Visible;

namespace DotEngine.BD.Datas.Actions
{
    public enum AnimatorSetParameterType
    {
        Int = 0,
        Bool,
        Float,
        Trigger,
    }

    [ActionData("Animator", "Set Parameter", ActionCategory.Animator, Target = DataTarget.Client)]
    public class AnimatorSetParameterData : EventActionData
    {
        public string ParameterName = string.Empty;
        public AnimatorSetParameterType ParameterType = AnimatorSetParameterType.Int;

        [VisibleIf("IsBoolParameter")]
        public bool BoolValue = false;
        [VisibleIf("IsIntParameter")]
        public int IntValue = 0;
        [VisibleIf("IsFloatParameter")]
        public float FloatValue = 0.0f;

#if UNITY_EDITOR
        private bool IsBoolParameter() => ParameterType == AnimatorSetParameterType.Bool;
        private bool IsIntParameter() => ParameterType == AnimatorSetParameterType.Int;
        private bool IsFloatParameter() => ParameterType == AnimatorSetParameterType.Float;
#endif
    }
}
