namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }

        LogLevel MinLogLevel { get; set; }
        ILogFormatter Formatter { get; set; }

        void OnLogReceived(LogLevel level, string tag, string message, string stackTrace);

        void DoStart();
        void DoEnd();
    }
}
