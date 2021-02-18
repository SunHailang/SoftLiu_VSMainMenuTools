using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData.ProtocolData
{
    public class ActionData
    {
        protected SocketReceiveData m_receiveData = null;

        public ActionData(SocketReceiveData recvData)
        {
            m_receiveData = recvData;
        }
        

        public virtual void Init()
        {
            if(m_receiveData !=null)
            {
                string data = Encoding.UTF8.GetString(m_receiveData.RecvBuffer, 0, m_receiveData.Length);
                Console.WriteLine($"RecvData: {data}");
            }
            else
            {
                Console.WriteLine("Receive data is null.");
            }
        }
    }
}
