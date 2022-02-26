using System;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTRandomSelectorComposeNode : BTAComposeNode
    {
        private int runningIndex = -1;

        public override void DoEnter()
        {
            base.DoEnter();

            if(ExecutorNodes.Count == 0)
            {
                runningIndex = -1;
            }
            else
            {
                var random = new Random((int)DateTime.Now.Ticks);
                runningIndex = random.Next(0, ExecutorNodes.Count);
                Controller.PushNode(ExecutorNodes[runningIndex]);
            }
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            if(runningIndex<0)
            {
                return EBTResult.Faiture;
            }

            var node = ExecutorNodes[runningIndex];
            var result = node.DoExecute(deltaTime);
            if (result != EBTResult.Running)
            {
                Controller.PopNode(node);
                runningIndex = -1;
            }
            return result;
        }

        public override void DoExit()
        {
            runningIndex = -1;

            base.DoExit();
        }
    }
}
