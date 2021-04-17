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

        private NetLogServer netLogServer = null;
        public NetLogAppender() : this(LogLevelConst.All)
        {
        }

        public NetLogAppender(LogLevel validLevel) : base(NAME, validLevel)
        {
            Formatter = new JsonLogFormatter();
        }

        public override void DoStart()
        {
        }

        public override void DoEnd()
        {
        }

        protected override void OutputLogMessage(LogLevel level, string message)
        {
            netLogServer?.OnLogMessage(message);
        }
    }
}
