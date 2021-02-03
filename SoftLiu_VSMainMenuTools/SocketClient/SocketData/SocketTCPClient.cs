﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData
{
    public class SocketTCPClient : IDisposable
    {
        private Socket m_tcpClient = null;

        private IPEndPoint m_serverEndPoint = null;

        /// <summary>
        /// 单次接收数据最大为 1M
        /// </summary>
        private byte[] m_recvBuffer = new byte[1024 * 1024];

        private bool m_connected = false;
        public bool Connected { get { return m_connected; } }

        private Action<SocketErrorData, SocketReceiveData> m_receiveDataCallback = null;

        public SocketTCPClient(IPEndPoint serverPoint)
        {
            m_serverEndPoint = serverPoint;
        }

        public void ConnectServer(Action<SocketErrorData, SocketReceiveData> receiveDataCallback)
        {
            m_receiveDataCallback = receiveDataCallback;
            if (m_serverEndPoint != null)
            {
                if (m_tcpClient == null)
                {
                    //定义一个套接字监听
                    m_tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                //获取文本框中的IP地址
                //IPAddress address = IPAddress.Parse("10.192.91.40");
                //将获取的IP地址和端口号绑定在网络节点上
                //IPEndPoint point = new IPEndPoint(address, 11060);
                try
                {
                    //客户端套接字连接到网络节点上，用的是Connect
                    m_tcpClient.BeginConnect(m_serverEndPoint, ConnectCallback, m_tcpClient);
                }
                catch (Exception error)
                {
                    Console.WriteLine($"Connection Error: {error.Message}");
                }
            }
            else
            {
                Console.WriteLine("Server EndPoint is null.");
                if (m_receiveDataCallback != null)
                {
                    m_receiveDataCallback(new SocketErrorData(10000, "Server EndPoint is null."), null);
                }
            }
        }

        public void DisconnectServer(Action<SocketErrorData> callback)
        {
            try
            {
                if (m_tcpClient != null)
                {
                    if (m_tcpClient.Connected)
                    {
                        m_tcpClient.Close();
                        if (callback != null) callback(new SocketErrorData(0, "Disconnected."));
                        //m_tcpClient.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), m_tcpClient);
                    }
                    else
                    {
                        Console.WriteLine("TCP Client disconnected.");
                        if (callback != null) callback(new SocketErrorData(10000, "TCP Client disconnected."));
                    }
                }
                else
                {
                    Console.WriteLine("Client is null, Please Re-Connect.");
                    if (callback != null) callback(new SocketErrorData(10000, "Client is null, Please Re-Connect."));
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"DisconnectServer Error: {error.Message}");
                if (callback != null) callback(new SocketErrorData(10000, $"DisconnectServer Error: {error.Message}"));
            }

        }

        private void DisconnectCallback(IAsyncResult iar)
        {
            Console.WriteLine("DisconnectCallback.");
            if (m_receiveDataCallback != null)
                m_receiveDataCallback(new SocketErrorData(0, "Callback Disconnected."), null);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            if (m_tcpClient.Connected)
            {
                Socket server = ar.AsyncState as Socket;
                if (server == null)
                {
                    Console.WriteLine("Connect error: server is null.");
                    return;
                }

                m_connected = true;
                byte[] connBytes = Encoding.UTF8.GetBytes($"Server:[{server.RemoteEndPoint.ToString()}]Connected.");
                if (m_receiveDataCallback != null) m_receiveDataCallback(null, new SocketReceiveData(connBytes, connBytes.Length));

                Console.WriteLine($"Connect Success, Server: {server.RemoteEndPoint.ToString()}");
                StartReceive();
            }
            else
            {
                Console.WriteLine("Connect Failed. Please Re-Connect!");
            }
        }

        public void SendData(byte[] buffer)
        {
            if (m_tcpClient.Connected)
            {
                m_tcpClient.Send(buffer);
            }
        }

        private void StartReceive()
        {
            try
            {
                if (m_tcpClient.Connected)
                    m_tcpClient.BeginReceive(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), m_tcpClient);
                else
                    Console.WriteLine("Disconnected. Please Re-Connect!");
            }
            catch (Exception error)
            {
                Console.WriteLine($"StartReceive Error: {error.Message}");
            }

        }

        private void ReceiveCallback(IAsyncResult iar)
        {
            try
            {
                Socket server = iar.AsyncState as Socket;
                int len = server.EndReceive(iar);
                if (m_receiveDataCallback != null) m_receiveDataCallback(null, new SocketReceiveData(m_recvBuffer, len));
            }
            catch (Exception error)
            {
                Console.WriteLine($"ReceiveCallback Error: {error.Message}");
            }
            finally
            {
                StartReceive();
            }
        }

        public void Dispose()
        {
            if (m_tcpClient != null)
            {
                m_tcpClient.Disconnect(false);
                m_tcpClient.Close();
            }
            m_connected = false;
            m_recvBuffer = null;
            m_serverEndPoint = null;
            m_tcpClient = null;
        }
    }
}
