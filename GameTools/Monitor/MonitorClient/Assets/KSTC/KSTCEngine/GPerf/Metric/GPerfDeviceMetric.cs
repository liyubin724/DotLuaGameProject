using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfDeviceMetric : IGPerfMetric
    {
        public const string DEVICE_MODEL_KEY = "deviceModel";
        public const string DEVICE_NAME_KEY = "deviceName";
        public const string DEVICE_TYPE_KEY = "deviceType";
        public const string DEVICE_UNIQUE_IDENTIFIER_KEY = "deviceUniqueIdentifier";

        public virtual string GetMetricInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();

            infoBuilder.AppendLine($"{DEVICE_MODEL_KEY}:{SystemInfo.deviceModel}");
            infoBuilder.AppendLine($"{DEVICE_NAME_KEY}:{SystemInfo.deviceName}");
            infoBuilder.AppendLine($"{DEVICE_TYPE_KEY}:{SystemInfo.deviceType}");
            infoBuilder.AppendLine($"{DEVICE_UNIQUE_IDENTIFIER_KEY}:{SystemInfo.deviceUniqueIdentifier}");

            return infoBuilder.ToString();
        }
    }
}
