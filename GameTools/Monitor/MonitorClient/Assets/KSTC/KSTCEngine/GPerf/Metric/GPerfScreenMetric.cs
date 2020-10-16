using System.Text;
using UnityEngine;

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfScreenMetric : IGPerfMetric
    {
        public const string BRIGHTNESS_KEY = "brightness";
        public const string WIDTH_KEY = "width";
        public const string HEIGHT_KEY = "height";
        public const string DPI_KEY = "dpi";
        public const string RESOLUTION_WIDTH_KEY = "rWidth";
        public const string RESOLUTION_HEIGHT_KEY = "rHeight";
        public const string RESOLUTION_REFRESH_RATE_KEY = "rRefreshRate";

        public float GetBrightness()
        {
            return Screen.brightness;
        }

        public int GetWidth()
        {
            return Screen.width;
        }

        public int GetHeight()
        {
            return Screen.height;
        }

        public float GetDPI()
        {
            return Screen.dpi;
        }

        public int GetResolutionWidth()
        {
            return Screen.currentResolution.width;
        }

        public int GetResolutionHeight()
        {
            return Screen.currentResolution.height;
        }

        public int GetResolutionRefreshRate()
        {
            return Screen.currentResolution.refreshRate;
        }

        public string GetMetricInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();

            infoBuilder.AppendLine($"{BRIGHTNESS_KEY}:{GetBrightness()}");
            infoBuilder.AppendLine($"{WIDTH_KEY}:{GetWidth()}");
            infoBuilder.AppendLine($"{HEIGHT_KEY}:{GetHeight()}");
            infoBuilder.AppendLine($"{DPI_KEY}:{GetDPI()}");
            infoBuilder.AppendLine($"{RESOLUTION_WIDTH_KEY}:{GetResolutionWidth()}");
            infoBuilder.AppendLine($"{RESOLUTION_HEIGHT_KEY}:{GetResolutionHeight()}");
            infoBuilder.AppendLine($"{RESOLUTION_REFRESH_RATE_KEY}:{GetResolutionRefreshRate()}");

            return infoBuilder.ToString();
        }
    }
}
