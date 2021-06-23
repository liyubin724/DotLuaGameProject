using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.NetCore.TCPNetwork
{
    public enum ServerSessionState
    {
        Connected = 0,
        Disconnected,
        Error,
    }

    public class ServerListener : IDisposable
    {
        private IMessageEncoder messageEncoder = null;
        private IMessageDecoder messageDecoder = null;

        private Dictionary<Guid, ServerSession> sessionDic = new Dictionary<Guid, ServerSession>();

        public void Dispose()
        {
           
        }
    }
}
