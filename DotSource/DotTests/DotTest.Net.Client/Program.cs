using DotEngine.Net;
using System;
using System.Text;
using System.Threading;

namespace DotTest.Net.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ClientNetwork client = new ClientNetwork("127.0.0.1",9080);
            client.OnNetConnected = () =>
            {
                Console.WriteLine("Client connected");
            };
            client.OnDataReceived = (dataBytes) =>
            {
                Console.WriteLine("OnMessage:" + Encoding.UTF8.GetString(dataBytes));
            };
            client.ConnectAsync();
            while(true)
            {
                if(client.IsConnected)
                {
                    client.DoUpdate(0.1f);

                    Random r = new Random();
                    var v = r.Next(0, 10);
                    if(v>5)
                    {
                        client.SendAsync(Encoding.UTF8.GetBytes("Client Send Data,v = " + v));
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
