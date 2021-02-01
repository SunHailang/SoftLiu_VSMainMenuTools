using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoctLiu_Client
{
    public class UDPClient
    {
        private Socket m_client = null;

        private EndPoint m_targetPoint = null;

        private EndPoint m_clientEndPoint = null;

        private byte[] m_recvBuffer = null;

        public UDPClient()
        {
            Console.WriteLine("udp client start.");
            // 192.168.218.129  30010
            IPAddress address = IPAddress.Parse("10.192.91.40");
            m_targetPoint = new IPEndPoint(address, 30010);

            m_client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //m_client.Bind(new IPEndPoint(IPAddress.Parse("10.192.91.40"), 40010));
            m_client.Connect(m_targetPoint);

            m_clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // 开始接收
            m_recvBuffer = new byte[1024 * 1024];
            m_client.BeginReceiveFrom(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None, ref m_clientEndPoint, new AsyncCallback(AcceptCallback), m_client);

            SendTo();
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            int len = 0;
            try
            {
                len = m_client.EndReceiveFrom(ar, ref m_clientEndPoint);
                string recv = Encoding.UTF8.GetString(m_recvBuffer, 0, len);
                Console.WriteLine($"recv callback:{recv} , ip:{m_clientEndPoint.ToString()}");

            }
            catch (Exception error)
            {
                Console.WriteLine($"UDPClient AcceptCallback Error: {error.Message}");
            }
            finally
            {
                if (m_client != null)
                    m_client.BeginReceiveFrom(m_recvBuffer, 0, m_recvBuffer.Length, SocketFlags.None,
                ref m_clientEndPoint, new AsyncCallback(AcceptCallback), m_client);
            }
        }

        private void SendTo()
        {
            while (true)
            {
                string send = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(send);
                m_client.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, m_targetPoint, 
                    new AsyncCallback(SendToCallback), m_client);
            }
        }

        private void SendToCallback(IAsyncResult ar)
        {
            //
        }

    }
}
