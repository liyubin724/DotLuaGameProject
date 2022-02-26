using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTRateSequenceComposeNode : BTASequenceComposeNode
    {
        protected override void OnIndexRealign()
        {
            var data = ComposeData as BTRateSequenceComposeData;

            var list = new List<int>();
            for (int i = 0; i < ExecutorNodes.Count; ++i)
            {
                list.Add(i);
            }
            var tmpRates = new List<float>();
            var random = new Random((int)DateTime.Now.Ticks);
            while(list.Count>0)
            {
                float sum = 0.0f;
                for(int i =0;i<list.Count;++i)
                {
                    sum += data.Rates[list[i]];
                }

                tmpRates.Clear();
                for(int i =0;i<list.Count;++i)
                {
                    tmpRates[i] = data.Rates[list[i]] / sum;
                    if(i>0)
                    {
                        tmpRates[i] += tmpRates[i - 1];
                    }
                }

                var rate = (float)random.NextDouble();
                for(int i =0;i<list.Count;++i)
                {
                    if(tmpRates[i] >= rate)
                    {
                        sortedIndexes.Add(list[i]);
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
