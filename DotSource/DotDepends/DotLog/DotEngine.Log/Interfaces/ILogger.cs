namespace DotEngine.Log
{
    public interface ILogger
    {
        string Tag { get; }
        ILogHandler Handler { get; set; }

        void Trace(string message);
        void Debug(string message);
        void Info(string message);
        void Warning(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
