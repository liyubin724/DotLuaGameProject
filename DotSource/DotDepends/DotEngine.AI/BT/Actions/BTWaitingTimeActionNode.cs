using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT
{
    public class BTWaitingTimeActionNode : ABTActionNode
    {
        private BTWaitingTimeActionData actionData;
        private float leftDuration = 0.0f;

        public override void DoInitilize(BTData data)
        {
            base.DoInitilize(data);

            actionData = GetData<BTWaitingTimeActionData>();
        }

        public override void DoEnter()
        {
            base.DoEnter();

            leftDuration = actionData.Duration;
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            leftDuration -= deltaTime;
            if(leftDuration>=0)
            {
                return EBTResult.Running;
            }
            return EBTResult.Success;
        }

        public override void DoDestroy()
        {
            leftDuration = 0.0f;
            actionData = null;

            base.DoDestroy();
        }
    }
}
