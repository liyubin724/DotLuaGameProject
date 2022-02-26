using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTASequenceComposeNode : BTAComposeNode
    {
        protected int currentIndex = -1;
        protected List<int> sortedIndexes = new List<int>();

        public override void DoEnter()
        {
            base.DoEnter();
            currentIndex = -1;
            sortedIndexes.Clear();

            OnIndexRealign();
        }

        protected abstract void OnIndexRealign();
        
        public override EBTResult DoExecute(float deltaTime)
        {
            if (currentIndex >= 0)
            {
                var runningNode = ExecutorNodes[sortedIndexes[currentIndex]];
                var result = runningNode.DoExecute(deltaTime);
                if (result == EBTResult.Running)
                {
                    return EBTResult.Running;
                }
                else
                {
                    Controller.PopNode(runningNode);
                    if (result == EBTResult.Faiture)
                    {
                        return result;
                    }
                }
            }
            ++currentIndex;
            while (currentIndex < sortedIndexes.Count)
            {
                var node = ExecutorNodes[sortedIndexes[currentIndex]];
                if (node.CanExecute())
                {
                    return EBTResult.Faiture;
                }
                Controller.PushNode(node);
                var result = node.DoExecute(deltaTime);
                if (result == EBTResult.Running)
                {
                    return result;
                }
                else
                {
                    Controller.PopNode(node);
                    if (result == EBTResult.Faiture)
                    {
                        return result;
                    }
                }
            }
            return EBTResult.Success;
        }

        public override void DoExit()
        {
            currentIndex = -1;
            sortedIndexes.Clear();

            base.DoExit();
        }
    }
}
