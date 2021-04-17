using DotEngine.Net.TcpNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Log
{
    public class NetLogAppender : ALogAppender
    {
        public static readonly string NAME = "Console";

        private ServerNetwork serverNetwork = null;
        public NetLogAppender() : this(LogLevelConst.All)
        {
        }

        public NetLogAppender(LogLevel validLevel) : base(NAME, validLevel)
        {
            Formatter = new JsonLogFormatter();
        }

        public override void DoStart()
        {
            serverNetwork = new ServerNetwork("NetLogServer");
            serverNetwork.RegistMessageHandler(typeof(NetLogServerHandler));
            serverNetwork.Listen(NetLogConst.NET_SERVER_PORT);
        }

        public override void DoEnd()
        {
            serverNetwork?.Disconnect();
            serverNetwork = null;
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            serverNetwork?.SendMessage(NetLogConst.S2C_LOG_MESSAGE_NOTIFY, Encoding.UTF8.GetBytes(message));
        }
    }
}
