using DotEngine.Log;

namespace DotEngine.Net
{
    public static class NetUtil
    {
        public static bool EnableLog { get; set; } = false;
        public static void LogInfo(string tag,string message)
        {
            if(EnableLog)
            {
                LogUtil.Info(tag, message);
            }
        }

    }
}
