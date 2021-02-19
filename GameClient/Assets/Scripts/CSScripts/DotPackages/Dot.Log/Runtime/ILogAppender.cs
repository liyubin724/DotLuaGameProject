namespace DotEngine.Log
{
    public interface ILogAppender
    {
        string Name { get; }
        ILogFormatter Formatter { get; }

        void OnLogReceived(LogLevel level, string tag, string message, string stackTrace);
        void OnAppended();
        void OnRemoved();
    }
}
