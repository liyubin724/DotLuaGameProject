﻿using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public class ServerSession : TcpSession
    {
        public ServerSession(ServerNetwork server) : base(server)
        {
        }

    }
}
