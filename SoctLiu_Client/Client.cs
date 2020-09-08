using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoctLiu_Client
{
    public class Client : IDisposable
    {
        //创建 1个客户端套接字 和1个负责监听服务端请求的线程
        Thread threadclient = null;
        Socket m_tcpClient = null;

        private byte[] m_buffer = new byte[1024 * 1024];

        public Client(IPEndPoint serverPoint)
        {
            //定义一个套接字监听
            m_tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获取文本框中的IP地址
            //IPAddress address = IPAddress.Parse("10.192.91.40");
            //将获取的IP地址和端口号绑定在网络节点上
            //IPEndPoint point = new IPEndPoint(address, 11060);
            try
            {
                //客户端套接字连接到网络节点上，用的是Connect
                m_tcpClient.BeginConnect(serverPoint, new AsyncCallback(ConnectCallback), m_tcpClient);
                //socketclient.Connect(serverPoint);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Connection Error: {error.Message}");
            }

            //threadclient = new Thread(recv);
            //threadclient.IsBackground = true;
            //threadclient.Start();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (m_tcpClient.Connected)
            {
                Socket server = ar.AsyncState as Socket;
                Console.WriteLine($"Client Connected Server: {server.RemoteEndPoint.ToString()}");

                StartRecv();
            }
            else
            {
                Console.WriteLine("Connect Failed.");
            }
        }

        private void StartRecv()
        {
            if (m_tcpClient.Connected)
            {
                m_tcpClient.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None, new AsyncCallback(RecvCallback), m_tcpClient);
            }
            else
            {
                Console.WriteLine("Please connect server...");
            }
        }

        private void RecvCallback(IAsyncResult iar)
        {
            Socket server = iar.AsyncState as Socket;
            int len = server.EndReceive(iar);
            if (len > 0)
            {
                string str = Encoding.UTF8.GetString(m_buffer, 0, len);
                Console.WriteLine($"Server-[{server.RemoteEndPoint.ToString()}]:\n{str}");
            }

            StartRecv();
        }

        // 接收服务端发来信息的方法
        void recv()
        {
            //持续监听服务端发来的消息
            while (true)
            {
                try
                {
                    //定义一个1M的内存缓冲区，用于临时性存储接收到的消息
                    byte[] arrRecvmsg = new byte[1024 * 1024];

                    //将客户端套接字接收到的数据存入内存缓冲区，并获取长度
                    int length = m_tcpClient.Receive(arrRecvmsg);

                    //将套接字获取到的字符数组转换为人可以看懂的字符串
                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);
                    Console.WriteLine($"Recv: {strRevMsg}");
                }
                catch (Exception ex)
                {

                    break;
                }
            }
        }



        //发送字符信息到服务端的方法
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            m_tcpClient.Send(arrClientSendMsg);
            //将发送的信息追加到聊天内容文本框中
        }

        public void Dispose()
        {
            if (m_tcpClient != null)
            {
                m_tcpClient.Close();
                m_tcpClient = null;
            }
        }
    }
}
