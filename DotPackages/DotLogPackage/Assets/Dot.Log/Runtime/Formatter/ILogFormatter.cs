namespace DotEngine.Log.Formatter
{
    public interface ILogFormatter
    {
        string FormatMessage(LogLevel level,string tag,string message,string stackTrace);
    }
}