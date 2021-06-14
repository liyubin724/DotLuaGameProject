namespace DotTool.ETD.Log
{
    public enum LogType
    {
        Info = 0,
        Warning,
        Error,
    }

    public delegate void OnHandlerLog(LogType type, int logID, string msg);

    public class LogHandler
    {
        private OnHandlerLog handler = null;

        public LogHandler(OnHandlerLog handler)
        {
            this.handler = handler;
        }

        public void Log(LogType type,int id,params object[] datas)
        {
            handler?.Invoke(type, id, LogMessage.GetLogMsg(id, datas));
        }

        public void Log(LogType type,string message)
        {
            handler?.Invoke(type, 0, message);
        }
    }
}
