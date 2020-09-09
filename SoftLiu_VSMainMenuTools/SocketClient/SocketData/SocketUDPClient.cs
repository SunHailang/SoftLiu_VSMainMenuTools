using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData
{
    public class SocketUDPClient : IDisposable
    {

        private Socket m_client = null;

        private IPEndPoint m_serverPoint = null;

        private EndPoint m_clientEndPoint = null;

        private byte[] m_recvBuffer = null;

        private Action<SocketReceiveData> m_receiveCallback = null;

        public SocketUDPClient(IPEndPoint serverPoint, Action<SocketReceiveData> callback)
        {
            Console.WriteLine("udp client start.");

            //IPAddress address = IPAddress.Parse("10.192.91.40");
            //m_targetPoint = new IPEndPoint(address, 11080);

            m_serverPoint = serverPoint;
            m_receiveCallback = callback;

            m_client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_client.Bind(new IPEndPoint(IPAddress.Parse("10.192.91.40"), 11088));

            m_clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // 开始接收
            m_recvBuffer = new byte[1024 * 1024];
            m_client.BeginReceiveFrom(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None, ref m_clientEndPoint, new AsyncCallback(AcceptCallback), m_client);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            int len = 0;
            try
            {
                len = m_client.EndReceiveFrom(ar, ref m_clientEndPoint);
                //string recv = Encoding.UTF8.GetString(m_recvBuffer, 0, len);
                //Console.WriteLine($"recv callback:{recv} , ip:{m_clientEndPoint.ToString()}");
                if (m_receiveCallback != null)
                    m_receiveCallback(new SocketReceiveData(m_recvBuffer, len));
            }
            catch (Exception error)
            {
                Console.WriteLine($"SocketTCPServer AcceptCallback Error: {error.Message}");
            }
            finally
            {
                if (m_client != null)
                    m_client.BeginReceiveFrom(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None,
                ref m_clientEndPoint, new AsyncCallback(AcceptCallback), m_client);
            }
        }

        public void SendTo(byte[] buffer)
        {
            m_client.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, m_serverPoint,
                new AsyncCallback(SendToCallback), m_client);
        }

        private void SendToCallback(IAsyncResult ar)
        {
            //
        }

        public void Dispose()
        {

        }
    }
}
