using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTRateSelectorComposeNode : BTAComposeNode
    {
        private int runningIndex = -1;

        public override void DoEnter()
        {
            base.DoEnter();

            if (ExecutorNodes.Count == 0)
            {
                runningIndex = -1;
            }
            else
            {
                var data = ComposeData as BTRateSelectorComposeData;

                float sum = 0.0f;
                for(int i =0;i<data.Rates.Count;i++)
                {
                    sum += data.Rates[i];
                }
                var tmpRates = new List<float>();
                for (int i = 0; i < data.Rates.Count; ++i)
                {
                    tmpRates[i] = data.Rates[i] / sum;
                    if (i > 0)
                    {
                        tmpRates[i] += tmpRates[i - 1];
                    }
                }
                var random = new Random((int)DateTime.Now.Ticks);
                var rate = (float)random.NextDouble();
                for (int i = 0; i < tmpRates.Count; ++i)
                {
                    if (tmpRates[i] >= rate)
                    {
                        runningIndex = i;
                        break;
                    }
                }
                Controller.PushNode(ExecutorNodes[runningIndex]);
            }
        }

        public override EBTResult DoExecute(float deltaTime)
        {
            if (runningIndex < 0)
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
