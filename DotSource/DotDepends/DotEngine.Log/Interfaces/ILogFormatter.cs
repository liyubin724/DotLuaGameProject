namespace DotEngine.Log
{
    public interface ILogFormatter
    {
        string FormatMessage(string tag, LogLevel level,string message);
    }
}