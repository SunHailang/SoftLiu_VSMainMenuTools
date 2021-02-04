using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData
{
    public class ServerSocketData
    {
        private System.String m_key;
        public System.String Key { get { return m_key; } }

        private System.String m_name;
        public System.String Name { get { return m_name; } }

        private System.String m_serverIP;
        public System.String ServerIP { get { return m_serverIP; } }

        private System.Int32 m_serverPort;
        public System.Int32 ServerPort { get { return m_serverPort; } }
    }
}
