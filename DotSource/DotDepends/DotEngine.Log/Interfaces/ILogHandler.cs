namespace DotEngine.Log
{
    public interface ILogHandler
    {
        void OnLogReceived(string tag, LogLevel logLevel, string message);
    }
}
