﻿using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTWaitingFrameActionNode : BTAActionNode
    {
        private int leftFrameCount = 0;

        public override void DoEnter()
        {
            base.DoEnter();

            leftFrameCount = !(ActionData is BTWaitingFrameActionData data) ? 0 : data.FrameCount;
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
