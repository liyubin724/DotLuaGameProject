using DotEngine.NativeDrawer.Visible;

namespace DotEngine.BD.Datas
{
    public enum AnimatorSetParameterType
    {
        Int = 0,
        Bool,
        Float,
        Trigger,
    }

    [ActionData("Animator/Events", "Set Parameter", ActionCategory.Animator, Target = BDDataTarget.Client)]
    public class AnimatorSetParameterEvent : EventActionData
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
