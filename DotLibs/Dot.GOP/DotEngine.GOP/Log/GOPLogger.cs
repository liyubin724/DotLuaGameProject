using HandlerAction = System.Action<string, string>;

namespace DotEngine.GOP
{
    public static class GOPLogger
    {
        private static HandlerAction logHandler;
        private static HandlerAction warningHandler;
        private static HandlerAction errorHandler;

        public static void SetHandler(HandlerAction log, HandlerAction warning, HandlerAction error)
        {
            logHandler = log;
            warningHandler = warning;
            errorHandler = error;
        }

        public static void Log(string tag, string message)
        {
            logHandler?.Invoke(tag, message);
        }

        public static void Warning(string tag, string message)
        {
            warningHandler?.Invoke(tag, message);
        }

        public static void Error(string tag, string message)
        {
            errorHandler?.Invoke(tag, message);
        }
    }
}
