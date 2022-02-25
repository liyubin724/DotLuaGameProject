﻿using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTWaitingTimeActionNode : BTAActionNode
    {
        private float leftTimeDuration = 0.0f;

        public override void DoEnter()
        {
            base.DoEnter();

            leftTimeDuration = !(ActionData is BTWaitingTimeActionData data) ? 0 : data.TimeDuration;
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            leftTimeDuration -= deltaTime;
            if(leftTimeDuration>=0)
            {
                return EBTResult.Running;
            }
            return EBTResult.Success;
        }

        public override void DoExit()
        {
            leftTimeDuration = 0.0f;

            base.DoExit();
        }
    }
}
