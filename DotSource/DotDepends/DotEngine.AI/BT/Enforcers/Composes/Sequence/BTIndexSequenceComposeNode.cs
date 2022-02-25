using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTIndexSequenceComposeNode : BTAComposeNode
    {
        private int currentIndex = 0;

        public override void DoEnter()
        {
            base.DoEnter();
            currentIndex = 0;
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            for(int i =currentIndex;i<ExecutorNodes.Count;i++)
            {
                var node = ExecutorNodes[i];
                if(!node.CanExecute())
                {
                    return EBTResult.Faiture;
                }
                Controller?.PushNode(node);
                var result = node.DoExecute(deltaTime);
                if(result!=EBTResult.Running)
                {
                    Controller.PopNode(node);
                }
                else
                {
                    return EBTResult.Running;
                }
            }
            return EBTResult.Success;
        }

        public override void DoExit()
        {
            currentIndex = 0;
            base.DoExit();
        }
    }
}
