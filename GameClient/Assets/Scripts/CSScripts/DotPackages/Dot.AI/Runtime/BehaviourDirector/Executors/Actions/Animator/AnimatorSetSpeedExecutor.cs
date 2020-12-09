using DotEngine.BD.Datas;
using DotEngine.BD.Enities;
using UnityEngine;

namespace DotEngine.BD.Executors
{
    [BDExecutor(typeof(AnimatorSetSpeedData))]
    public class AnimatorSetSpeedExecutor : EventActionExecutor
    {
        public override void DoTrigger()
        {
            AnimatorSetSpeedData data = GetData<AnimatorSetSpeedData>();
            IActor actor = GetContextData<IActor>();

            Animator animator = actor.GetAnimator();
            animator.speed = data.Speed;
        }
    }
}
