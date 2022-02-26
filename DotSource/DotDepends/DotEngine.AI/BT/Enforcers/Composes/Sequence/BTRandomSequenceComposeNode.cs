using System;
using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTRandomSequenceComposeNode : BTASequenceComposeNode
    {
        protected override void OnIndexRealign()
        {
            var list = new List<int>();
            for (int i = 0; i < ExecutorNodes.Count; ++i)
            {
                list.Add(i);
            }

            var random = new Random((int)DateTime.Now.Ticks);
            while (list.Count > 0)
            {
                var index = random.Next(0, list.Count);
                sortedIndexes.Add(list[index]);
                list.RemoveAt(index);
            }
        }
    }
}
