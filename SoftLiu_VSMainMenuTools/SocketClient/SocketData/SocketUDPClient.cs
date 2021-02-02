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
        private const int bufSize = 1024;
        private State state = new State();

        private Socket m_client = null;

        private EndPoint m_clientEndPoint = new IPEndPoint(IPAddress.Any, 0);


        private Action<SocketReceiveData> m_receiveCallback = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public SocketUDPClient(IPEndPoint serverPoint, Action<SocketReceiveData> callback)
        {
            Console.WriteLine("udp client start.");

            m_receiveCallback = callback;

            m_client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //m_client.Bind(new IPEndPoint(IPAddress.Parse("10.192.91.40"), 11088));
            m_client.Connect(serverPoint);

            // 开始接收
            m_client.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref m_clientEndPoint, AcceptCallback, state);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                State so = (State)ar.AsyncState;
                // 接收的数据长度
                int bytesLen = m_client.EndReceiveFrom(ar, ref m_clientEndPoint);
                m_client.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref m_clientEndPoint, AcceptCallback, so);

                // callback
                if (m_receiveCallback != null)
                {
                    SocketReceiveData data = new SocketReceiveData(so.buffer, bytesLen);
                    m_receiveCallback(data);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"BeginReceiveFrom Callback Error:: {error.Message}");
            }
        }


        public void SendTo(byte[] buffer)
        {
            m_client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
                (ar) =>
                {
                    State so = (State)ar.AsyncState;
                    // 发送数据长度
                    int bytesLen = m_client.EndSend(ar);

                }, state);
        }

        public void Close()
        {
            if (m_client != null)
                m_client.Close();
            m_client = null;
        }

        public void Dispose()
        {

        }
    }
}
