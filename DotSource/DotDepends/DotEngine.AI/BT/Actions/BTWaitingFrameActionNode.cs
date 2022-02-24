using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT
{
    public class BTWaitingFrameActionNode : ABTActionNode
    {
        private BTWaitingFrameActionData actionData;
        private int leftFrameCount = 0;

        public override void DoInitilize(BTData data)
        {
            base.DoInitilize(data);
            actionData = GetData<BTWaitingFrameActionData>();
        }

        public override void DoEnter()
        {
            base.DoEnter();

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

        public override void DoDestroy()
        {
            leftFrameCount = 0;
            actionData = null;

            base.DoDestroy();
        }
    }
}
