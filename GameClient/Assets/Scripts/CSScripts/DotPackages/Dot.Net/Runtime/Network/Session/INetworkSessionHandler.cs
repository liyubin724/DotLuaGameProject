namespace DotEngine.Net
{
    public enum NetworkSessionError
    {
        ProcessConnectSocketError,
        HandleSocketEventError,
        DoSendFailedError,
        ProcessSendSocketError,
        ReadMessageSeriousError,
        ProcessReceiveSocketError,
    }

    public enum NetworkSessionOperation
    {
        Connecting = 0,
        ConnectFailed,
        Connected,
        Disconnecting,
        Disconnected,
    }

    public enum NetworkSessionLogType
    {
        Connect = 0,
        SendMessage,
        ReceiveMessage,
        Disconnect,
    }

    public interface INetworkSessionHandler
    {
        void OnMessageHandler(INetworkSession session, int messageID, byte[] dataBytes);
        void OnSessionError(NetworkSessionError error, INetworkSession session, object userdata);
        void OnSessionOperation(NetworkSessionOperation operation, INetworkSession session, object userdata);
        void OnSessionLog(NetworkSessionLogType logType, string log);
    }
}
