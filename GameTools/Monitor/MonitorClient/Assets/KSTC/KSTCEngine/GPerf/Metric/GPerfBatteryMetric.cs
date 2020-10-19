using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfBatteryMetric : IGPerfMetric
    {
        public const string RATE_KEY = "rate";
        public const string CHARGING_STATUS_KEY = "changingStatus";
        public const string TEMPERATURE_KEY = "temperature";

        public virtual float GetTemperature()
        {
            return 0.0f;
        }

        public virtual int GetStatus()
        {
            return (int)SystemInfo.batteryStatus;
        }

        public virtual float GetRate()
        {
            return SystemInfo.batteryLevel;
        }

        public virtual string GetMetricInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();

            infoBuilder.AppendLine($"{RATE_KEY}:{GetRate()}");
            infoBuilder.AppendLine($"{CHARGING_STATUS_KEY}:{GetStatus()}");
            infoBuilder.AppendLine($"{TEMPERATURE_KEY}:{GetTemperature()}");

            return infoBuilder.ToString();
        }
    }
}
