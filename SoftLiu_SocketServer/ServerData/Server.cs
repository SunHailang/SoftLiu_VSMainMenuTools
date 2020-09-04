using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftLiu_SocketServer.ServerData
{
    public class Server : IDisposable
    {
        private Socket m_socServer = null;

        private Dictionary<string, Socket> m_clientConnectionItem = new Dictionary<string, Socket>();

        private byte[] m_recvBuffer = new byte[1024 * 1024];

        public Server()
        {
            // 创建一个Socket对象
            m_socServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // IP 地址
            IPAddress address = IPAddress.Parse("10.192.91.40");
            // Port
            IPEndPoint point = new IPEndPoint(address, 11060);
            // 绑定 IP ，端口号 
            m_socServer.Bind(point);
            // 开启监听  监听长度
            m_socServer.Listen(10);

            Console.WriteLine("Socket Create Success.");
        }

        public void StartAsyncSocket()
        {
            // 异步接收消息
            m_socServer.BeginAccept(new AsyncCallback(AcceptCallback), m_socServer);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket server = ar.AsyncState as Socket;
                Socket client = server.EndAccept(ar);
                // 有客户端连接进来
                Console.WriteLine($"Client Connect Success, Client:{client.RemoteEndPoint.ToString()}");
                AddClientList(client);
                client.Send(Encoding.UTF8.GetBytes("Connected"));
                StartReceive(client);
            }
            catch (Exception error)
            {
                Console.WriteLine($"AcceptCallback Error: {error.Message}");
            }
            finally
            {
                m_socServer.BeginAccept(new AsyncCallback(AcceptCallback), m_socServer);
            }
        }

        private void StartReceive(Socket client)
        {
            try
            {
                client.BeginReceive(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None, ReceiveCallback, client);
            }
            catch (Exception error)
            {
                Console.WriteLine($"StartReceive Error: {error.Message}");
                if (client != null)
                {
                    Console.WriteLine($"StartReceive Close: {client.RemoteEndPoint.ToString()}");
                    RemoveClientList(client);
                    client.Close();
                }
            }

        }

        private void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                Socket client = iar.AsyncState as Socket;
                int len = client.EndReceive(iar);
                if (len == 0)
                {
                    return;
                }
                string str = Encoding.UTF8.GetString(m_recvBuffer, 0, len);
                Console.WriteLine($"{str}");

                StartReceive(client);
            }
            catch (Exception error)
            {
                Console.WriteLine($"ReceiveCallback Error: {error.Message}");
                if (iar != null && (iar.AsyncState as Socket) != null)
                {
                    Socket client = iar.AsyncState as Socket;
                    Console.WriteLine($"ReceiveCallback Close Client: {client.RemoteEndPoint.ToString()}");
                    RemoveClientList(client);
                    client.Close();
                }
            }

        }

        private void AddClientList(Socket client)
        {
            if (client == null)
            {
                return;
            }
            if (!this.m_clientConnectionItem.ContainsKey(client.RemoteEndPoint.ToString()))
            {
                this.m_clientConnectionItem.Add(client.RemoteEndPoint.ToString(), client);
            }
        }

        private void RemoveClientList(Socket client)
        {
            if (client == null)
            {
                return;
            }
            if (this.m_clientConnectionItem.ContainsKey(client.RemoteEndPoint.ToString()))
            {
                this.m_clientConnectionItem.Remove(client.RemoteEndPoint.ToString());
            }
        }

        public void Dispose()
        {
            if (m_socServer != null)
            {
                m_socServer.Close();
                m_socServer = null;
            }
        }
    }
}
