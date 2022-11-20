using SoftLiu_SocketServer.ServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_SocketServer
{
    class Program
    {

        private static Socket socServer = null;

        private static byte[] buffer = new byte[1024];

        static void Main(string[] args)
        {
            Console.WriteLine("start server.");
            try
            {
                //Server server = new Server();
                //server.StartAsyncSocket();
                ServerRAW raw = new ServerRAW();
                raw.Server("10.192.91.40", 8080);

                //ServerUDP udp = new ServerUDP();
                ////udp.Server("10.192.91.40", 30010);
                //udp.Client("192.168.218.129", 30010);
                //while (true)
                //{
                //    string text = Console.ReadLine();
                //    udp.Send(text);
                //}
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error: {error.Message}");
            }

            //socServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //socServer.Bind(new IPEndPoint(IPAddress.Any, 11060));

            //socServer.Listen(10);


            //socServer.BeginAccept(new AsyncCallback(AcceptCallback), socServer);

            Console.ReadLine();

        }

        static void ReceiveCallback(IAsyncResult iar)
        {
            Socket client = iar.AsyncState as Socket;

            int len = client.EndReceive(iar);
            if (len == 0)
            {
                return;
            }
            string str = Encoding.UTF8.GetString(buffer, 0, len);
            Console.WriteLine($"{str}");

            StartReceive(client);
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            Socket client = server.EndAccept(ar);
            Send(client);
            StartReceive(client);

            socServer.BeginAccept(new AsyncCallback(AcceptCallback), server);
        }

        static void StartReceive(Socket client)
        {
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client);
        }

        static void Send(Socket client)
        {
            client.Send(Encoding.UTF8.GetBytes("Hello World"));
        }
    }
}
