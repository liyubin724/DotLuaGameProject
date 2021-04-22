﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Connected,
    }

    public interface INetworkSessionHandler
    {
        void OnMessageHandler(INetworkSession session, int messageID, byte[] dataBytes);
        void OnSessionError(NetworkSessionError error, INetworkSession session, object userdata);
        void OnSessionOperation(NetworkSessionOperation operation, INetworkSession session, object userdata);
    }
}
