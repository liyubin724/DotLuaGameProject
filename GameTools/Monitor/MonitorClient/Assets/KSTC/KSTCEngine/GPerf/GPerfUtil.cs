
using System.Text.RegularExpressions;

namespace KSTCEngine.GPerf
{
    public static class GPerfUtil
    {
        public const string LOG_NAME = "GPerf-Log";

        private const string LINE_KV_PATTERN = @"\s*(?<name>\w*)\s*:\s*(?<value>\w*)\s*";
        public static void GetKeyValue(string line,out string name,out string value)
        {
            name = string.Empty;
            value = string.Empty;

            Regex regex = new Regex(LINE_KV_PATTERN);
            Match match = regex.Match(line);
            if(match.Success)
            {
                name = match.Groups["name"].Value;
                value = match.Groups["value"].Value;
            }
        }
    }
}
