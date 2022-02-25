using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTFrameLoopDecoratorNode : BTADecoratorNode
    {
        private int leftFrameCount = 0;

        public override void DoEnter()
        {
            base.DoEnter();
            leftFrameCount = !(DecoratorData is BTFrameLoopDecoratorData data) ? 0 : data.FrameCount;
        }

        public override EBTResult DoExecute(float deltaTime)
        {

            --leftFrameCount;
            ExecutorNode?.DoExecute(deltaTime);

            if (leftFrameCount <= 0)
            {
                return EBTResult.Success;
            }
            return EBTResult.Running;
        }

        public override void DoExit()
        {
            leftFrameCount = 0;

            base.DoExit();
        }
    }
}
