using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfProcessorMetric : IGPerfMetric
    {
        public const string PROCESSOR_COUNT_KEY = "count";
        public const string PROCESSOR_TYPE_KEY = "type";
        public const string PROCESSOR_FREQUENCY_KEY = "frequency";
        public const string PROCESSOR_USAGE_RATE_KEY = "usageRate";

        public virtual float GetProcessorUsageRate()
        {
            return 0.0f;
        }

        public virtual string GetMetricInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();

            infoBuilder.AppendLine($"{PROCESSOR_COUNT_KEY}:{SystemInfo.processorCount}");
            infoBuilder.AppendLine($"{PROCESSOR_TYPE_KEY}:{SystemInfo.processorType}");
            infoBuilder.AppendLine($"{PROCESSOR_FREQUENCY_KEY}:{SystemInfo.processorFrequency}");
            infoBuilder.AppendLine($"{PROCESSOR_USAGE_RATE_KEY}:{GetProcessorUsageRate()}");

            return infoBuilder.ToString();
        }
    }
}
