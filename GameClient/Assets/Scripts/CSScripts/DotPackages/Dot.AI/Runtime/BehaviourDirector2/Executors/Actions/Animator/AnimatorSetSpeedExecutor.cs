using DotEngine.BD.Datas;
using DotEngine.BD.Enities;
using UnityEngine;

namespace DotEngine.BD.Executors
{
    [BDExecutor(typeof(AnimatorSetSpeedEvent))]
    public class AnimatorSetSpeedExecutor : EventActionExecutor
    {
        public override void DoTrigger()
        {
            AnimatorSetSpeedEvent data = GetData<AnimatorSetSpeedEvent>();
            IActor actor = GetContextData<IActor>();

            Animator animator = actor.GetAnimator();
            animator.speed = data.Speed;
        }
    }
}
