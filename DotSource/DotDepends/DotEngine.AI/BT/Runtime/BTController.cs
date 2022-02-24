using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT
{
    public class BTController
    {
        private BTContext context = new BTContext();
        public BTContext Context
        {
            get
            {
                return context;
            }
        }

        private bool isRunning = false;
        public bool IsRunning => isRunning;

        private Stack<ABTExecutorNode> runningNodeStack = new Stack<ABTExecutorNode>();
        internal void PushNode(ABTExecutorNode node)
        {
            runningNodeStack.Push(node);
        }

        internal void PopNode(ABTExecutorNode node)
        {
            var currentNode = runningNodeStack.Pop();
            if(currentNode!=node)
            {
                throw new Exception();
            }
        }

        private BTRootNode rootNode = null;

        public void DoInitilize()
        {

        }

        public void DoActivate()
        {
            
        }

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {

        }

        public void DoDeactivate()
        {

        }

        public void DoDestroy()
        {

        }
    }
}
