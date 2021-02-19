namespace DotEngine.Log
{
    public interface ILogFormatter
    {
        string FormatMessage(LogLevel level, string tag, string message, string stackTrace);
    }
}