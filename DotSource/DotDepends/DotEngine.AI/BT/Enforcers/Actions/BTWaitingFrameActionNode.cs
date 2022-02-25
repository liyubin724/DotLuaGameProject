using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT
{
    public class BTWaitingFrameActionNode : ABTActionNode
    {
        private BTWaitingFrameActionData actionData;
        private int leftFrameCount = 0;

        public override void DoEnter()
        {
            base.DoEnter();
            if(actionData == null)
            {
                actionData = GetNodeData<BTWaitingFrameActionData>();
            }
            leftFrameCount = actionData.FrameCount;
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            leftFrameCount--;
            if(leftFrameCount>0)
            {
                return EBTResult.Running;
            }
            return EBTResult.Success;
        }

        public override void DoExit()
        {
            leftFrameCount = 0;

            base.DoExit();
        }
    }
}
