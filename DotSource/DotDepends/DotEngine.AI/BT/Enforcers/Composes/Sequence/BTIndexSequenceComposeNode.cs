namespace DotEngine.AI.BT.Enforcers
{
    public class BTIndexSequenceComposeNode : BTASequenceComposeNode
    {
        protected override void OnIndexRealign()
        {
            for(int i =0;i<ExecutorNodes.Count;i++)
            {
                sortedIndexes.Add(i);
            }
        }
    }
}
