using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT
{
    public class BTWaitingTimeActionNode : ABTActionNode
    {
        private BTWaitingTimeActionData actionData;
        private float leftDuration = 0.0f;

        public override void DoEnter()
        {
            base.DoEnter();
            if(actionData == null)
            {
                actionData = GetNodeData<BTWaitingTimeActionData>();
            }

            leftDuration = actionData.TimeDuration;
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

        public override void DoExit()
        {
            leftDuration = 0.0f;

            base.DoExit();
        }
    }
}
