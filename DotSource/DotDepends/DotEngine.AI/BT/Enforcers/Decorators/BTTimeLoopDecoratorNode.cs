using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTTimeLoopDecoratorNode : BTADecoratorNode
    {
        private float leftTimeDuration = 0.0f;

        public override void DoEnter()
        {
            base.DoEnter();
            leftTimeDuration = !(DecoratorData is BTTimeLoopDecoratorData data) ? 0 : data.TimeDuration;
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            leftTimeDuration -= deltaTime;
            ExecutorNode?.DoExecute(deltaTime);

            if(leftTimeDuration <= 0)
            {
                return EBTResult.Success;
            }
            return EBTResult.Running;
        }

        public override void DoExit()
        {
            leftTimeDuration = 0.0f;

            base.DoExit();
        }
    }
}
