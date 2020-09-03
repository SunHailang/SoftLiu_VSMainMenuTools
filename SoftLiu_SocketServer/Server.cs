using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftLiu_SocketServer
{
    public class Server :IDisposable
    {
        private Socket m_socServer = null;

        private Dictionary<string, Socket> m_clientConnectionItem = new Dictionary<string, Socket>();

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
            Console.WriteLine("Start Receive.");
            // 开始接受消息
            Thread threadWatch = new Thread(StartReceive);
            // 将窗体线程设置为与后台同步，随着主线程结束而结束
            threadWatch.IsBackground = true;

            threadWatch.Start();
        }

        private void StartReceive()
        {
            Socket client = null;
            while (true)
            {
                try
                {
                    client = m_socServer.Accept();
                }
                catch (Exception error)
                {
                    break;
                }
                if (client == null)
                {
                    Console.WriteLine("StartReceive client is null.");
                    return;
                }
                // 获取客户端IP和端口号
                IPAddress clientIP = (client.RemoteEndPoint as IPEndPoint).Address;
                int clientPort = (client.RemoteEndPoint as IPEndPoint).Port;

                // 给客户端回消息 告诉连接成功
                string sendMsg = $"Connect Success, IP:{clientIP}, Port:{clientPort}";

                Console.WriteLine($"{sendMsg}");

                byte[] sendBuffer = Encoding.UTF8.GetBytes(sendMsg);
                client.Send(sendBuffer);

                // 客户端网络节点号
                string remotePoint = client.RemoteEndPoint.ToString();
                // 显示与客户端连接情况

                // 添加客户端信息
                m_clientConnectionItem.Add(remotePoint, client);

                // 创建一个通信线程
                ParameterizedThreadStart pts = new ParameterizedThreadStart(Recv);
                Thread threadRecv = new Thread(pts);
                threadRecv.IsBackground = true;
                threadRecv.Start(client);
            }
        }

        private void Recv(object socketClient)
        {
            Socket client = socketClient as Socket;

            while (true)
            {
                // 创建一个缓冲区 大小 1M
                byte[] buffer = new byte[1024 * 1024];

                // 将接收到的信息存入到内存缓冲区，并返回其字节数组的长度
                try
                {
                    int length = client.Receive(buffer);

                    // 将机器接收到的自己数组转换成字符串
                    string strRecv = Encoding.UTF8.GetString(buffer, 0, length);
                    Console.WriteLine($"Recv: {strRecv}");
                    // 给客户端返回消息
                    string sendMsg = $"test server";
                    client.Send(Encoding.UTF8.GetBytes(sendMsg));

                }
                catch (Exception error)
                {
                    if (client != null)
                    {
                        if (m_clientConnectionItem.ContainsKey(client.RemoteEndPoint.ToString()))
                            m_clientConnectionItem.Remove(client.RemoteEndPoint.ToString());
                    }

                    Console.WriteLine($"clientConnectionItem Count: {m_clientConnectionItem.Count}");
                    client.Close();
                    break;
                }
            }
        }

        public void Dispose()
        {
            if (m_socServer!=null)
            {
                m_socServer.Close();
                m_socServer = null;
            }
        }
    }
}
