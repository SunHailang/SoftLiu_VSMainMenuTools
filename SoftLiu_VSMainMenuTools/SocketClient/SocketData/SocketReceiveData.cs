using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData
{
    public class SocketReceiveData
    {
        private byte[] m_recvBuffer = null;
        public byte[] RecvBuffer { get { return m_recvBuffer; } }

        private int m_length = 0;
        public int Length { get { return m_length; } }

        public SocketReceiveData(byte[] buffer, int length)
        {
            this.m_recvBuffer = buffer;
            this.m_length = length;

            string str = Encoding.UTF8.GetString(buffer, 0, length);
            Console.WriteLine($"Server:\n{str}");
        }
    }
}
