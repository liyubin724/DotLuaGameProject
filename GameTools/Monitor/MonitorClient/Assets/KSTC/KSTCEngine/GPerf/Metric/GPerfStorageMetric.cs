using System.Text;

namespace KSTCEngine.GPerf.Metric
{
    public class GPerfStorageMetric : IGPerfMetric
    {
        private const string SDCARD_ENABLE_KEY = "sdcardEnable";
        private const string TOTAL_EXTERNAL_SIZE_KEY = "totalExternalSize";
        private const string AVAILABLE_EXTERNAL_SIZE_KEY = "availableExternalSize";
        private const string TOTAL_INTERNAL_SIZE_KEY = "totalInternalSize";
        private const string AVAILABLE_INTERNAL_SIZE_KEY = "availableInternalSize";

        public virtual string GetInternalStorageInfo()
        {
            return string.Empty;
        }

        public virtual string GetExternalStorageInfo()
        {
            return string.Empty;
        }

        public virtual string GetMetricInfo()
        {
            StringBuilder infoBuilder = new StringBuilder();

            string externalStr = GetExternalStorageInfo();
            if(string.IsNullOrEmpty(externalStr))
            {
                infoBuilder.AppendLine(GetExternalStorageInfo());
            }
            infoBuilder.AppendLine(GetInternalStorageInfo());

            return infoBuilder.ToString();
        }
    }
}
