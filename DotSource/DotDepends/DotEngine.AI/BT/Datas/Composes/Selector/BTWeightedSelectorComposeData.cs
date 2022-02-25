using System.Collections.Generic;

namespace DotEngine.AI.BT.Datas
{
    public class BTWeightedSelectorComposeData : BTComposeNodeData
    {
        public int Count = 0;
        public bool CanRepeat = false;

        public List<float> Weights = new List<float>();
    }
}
