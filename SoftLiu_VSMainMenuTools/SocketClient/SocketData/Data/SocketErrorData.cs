using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.SocketData
{
    public class SocketErrorData
    {
        private ClientErrorCode m_errorCode = ClientErrorCode.None;
        public ClientErrorCode ErrorCode { get { return m_errorCode; } }
        private string m_errorStr = null;

        public string ErrorStr { get { return m_errorStr; } }

        public SocketErrorData(ClientErrorCode errorCode, string error)
        {
            this.m_errorCode = errorCode;
            this.m_errorStr = error;
        }
    }
}
