using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData
{
    public class WebSocketMatchData
    {
        private System.Int32 m_tier = 0;
        public System.Int32 tier { get { return m_tier; } }

        private System.String[] m_sharkNameList = null;
        public System.String[] sharkNameList { get { return m_sharkNameList; } }

        private System.Int32[] m_matchCurrencyList = null;
        public System.Int32[] matchCurrencyList { get { return m_matchCurrencyList; } }

        private System.Int32[] m_matchModeList = null;
        public System.Int32[] matchModeList { get { return m_matchModeList; } }

    }
}
