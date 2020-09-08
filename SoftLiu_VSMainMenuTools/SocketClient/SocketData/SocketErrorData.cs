using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData
{
    public class SocketErrorData
    {
        private int m_errorCode = -1;
        public int ErrorCode { get { return m_errorCode; } }
        private string m_errorStr = null;

        public string ErrorStr { get { return m_errorStr; } }

        public SocketErrorData(int errorCode, string error)
        {
            this.m_errorCode = errorCode;
            this.m_errorStr = error;
        }
    }
}
