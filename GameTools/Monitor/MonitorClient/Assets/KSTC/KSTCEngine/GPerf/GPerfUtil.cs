using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KSTCEngine.GPerf
{
    public static class GPerfUtil
    {
        public const string LOG_NAME = "GPerf-Log";

        public static string GeRootDir()
        {
            string rootDir = string.Empty;

#if UNITY_EDITOR
            rootDir = Application.dataPath.Replace("Assets", "GPerf/");
#else
#if UNITY_ANDROID || UNITY_IPHONE
        rootDir =  Application.persistentDataPath+"/GPerf/";
#else
        rootDir =  Application.dataPath.Replace("Assets", "GPerf/");
#endif
#endif
            if (!string.IsNullOrEmpty(rootDir))
            {
                if (!Directory.Exists(rootDir))
                {
                    Directory.CreateDirectory(rootDir);
                }
            }
            return rootDir;
        }

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
