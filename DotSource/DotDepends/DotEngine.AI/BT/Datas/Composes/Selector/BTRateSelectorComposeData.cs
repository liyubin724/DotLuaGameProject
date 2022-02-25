using System.Collections.Generic;

namespace DotEngine.AI.BT.Datas
{
    public class BTRateSelectorComposeData : BTComposeNodeData
    {
        public int Count = 0;
        public bool CanRepeat = false;

        public List<float> Rates = new List<float>();
    }
}
