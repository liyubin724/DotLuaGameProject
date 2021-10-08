namespace DotEngine.Core.Log
{
    public interface ILogger
    {
        void Debug(string tag, string message);
        void Info(string tag, string message);
        void Warning(string tag, string message);
        void Error(string tag, string message);
    }
}
