using System;
using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTController
    {
        private BTBlackboard blackboard = new BTBlackboard();
        public BTBlackboard Blackboard
        {
            get
            {
                return blackboard;
            }
        }

        private bool isRunning = false;
        public bool IsRunning => isRunning;

        private Stack<BTAExecutorNode> runningNodeStack = new Stack<BTAExecutorNode>();
        internal void PushNode(BTAExecutorNode node)
        {
            runningNodeStack.Push(node);
        }

        internal void PopNode(BTAExecutorNode node)
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
