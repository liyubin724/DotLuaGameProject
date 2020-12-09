using DotEngine.BD.Datas;
using DotEngine.BD.Enities;
using UnityEngine;

namespace DotEngine.BD.Executors
{
    [BDExecutor(typeof(AnimatorSetParameterData))]
    public class AnimatorSetParameterExecutor : EventActionExecutor
    {
        public override void DoTrigger()
        {
            AnimatorSetParameterData data = GetData<AnimatorSetParameterData>();
            IActor actor = GetContextData<IActor>();

            Animator animator = actor.GetAnimator();

            if(data.ParameterType == AnimatorSetParameterType.Bool)
            {
                animator.SetBool(data.ParameterName, data.BoolValue);
            }else if(data.ParameterType == AnimatorSetParameterType.Int)
            {
                animator.SetInteger(data.ParameterName, data.IntValue);
            }else if(data.ParameterType == AnimatorSetParameterType.Float)
            {
                animator.SetFloat(data.ParameterName, data.FloatValue);
            }else if(data.ParameterType == AnimatorSetParameterType.Trigger)
            {
                animator.SetTrigger(data.ParameterName);
            }
            
        }
    }
}
