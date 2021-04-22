using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Net
{
    public class ServerNetwork : ANetwork
    {
        private Socket socketListener = null;

        public void Startup(int port,int maxCount)
        {
            socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socketListener.Bind(new IPEndPoint(IPAddress.Any, port));
                socketListener.Listen(maxCount);

                DoAccept();
            }
            catch (Exception e)
            {

            }
        }

        public void Shuntdown()
        {

        }

        private void DoAccept()
        {
            socket.BeginAccept(ProcessAccept, socket);
        }

        private void ProcessAccept(IAsyncResult ar)
        {
            var server = (Socket)ar.AsyncState;
            AcceptedClientConnection(server.EndAccept(ar));
            DoAccept();
        }

        private void AcceptedClientConnection(Socket client)
        {
            ServerNetworkSession session = new ServerNetworkSession();
            session.BindSocket(client, this);
            ClientData clientData = new ClientData()
            {
                Index = sessionIndexCreator.GetNextID(),
                Session = session,
            };

            clientDic.Add(session, clientData);

            session.DoReceive();
        }
    }
}
