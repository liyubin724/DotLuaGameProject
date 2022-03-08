using System;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using DotEngine.Net;

namespace DotTest.Net.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ServerNetwork server = new ServerNetwork("127.0.0.1", 9080);
            server.OnDataReceived = (guid, dataBytes) =>
            {
                Console.WriteLine(Encoding.UTF8.GetString(dataBytes));
            };
            server.OnSessionConnected = (guid) =>
            {
                Console.WriteLine("Client connected,guid = " + guid);
            };
            if(server.Start())
            {
                while(true)
                {
                    server.DoUpdate(0.1f);

                    Random r = new Random();
                    var v = r.Next(0, 10);
                    if (v > 5)
                    {
                        server.Multicast(Encoding.UTF8.GetBytes("Server Send Data,v = " + v));
                    }

                    Thread.Sleep(100);
                }
            }
            server.Dispose();
        }
    }
}
