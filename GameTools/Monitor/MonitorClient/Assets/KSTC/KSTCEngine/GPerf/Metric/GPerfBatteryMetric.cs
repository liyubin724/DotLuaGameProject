using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfBatteryMetric : IGPerfMetric
    {
        public const string RATE_KEY = "rate";
        public const string IS_CHARGING_KEY = "isChanging";
        public const string TEMPERATURE_KEY = "temperature";

        public virtual float GetTemperature()
        {
            return 0.0f;
        }

        public virtual string GetMetricInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();

            infoBuilder.AppendLine($"{RATE_KEY}:{SystemInfo.batteryLevel}");
            infoBuilder.AppendLine($"{IS_CHARGING_KEY}:{SystemInfo.batteryStatus== BatteryStatus.Charging || SystemInfo.batteryStatus == BatteryStatus.Full}");
            infoBuilder.AppendLine($"{TEMPERATURE_KEY}:{GetTemperature()}");

            return infoBuilder.ToString();
        }
    }
}
