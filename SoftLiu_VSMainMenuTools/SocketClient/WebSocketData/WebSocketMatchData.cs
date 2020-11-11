using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData
{
    public class WebSocketMatchData
    {
        private System.Int32 m_tier;
        public System.Int32 tier { get { return m_tier; } }

        private System.String[] m_sharkNameList;
        public System.String[] sharkNameList { get { return m_sharkNameList; } }

        private System.Int32[] m_matchCurrencyList;
        public System.Int32[] matchCurrencyList { get { return m_matchCurrencyList; } }

        private System.Int32[] m_matchModeList;
        public System.Int32[] matchModeList { get { return m_matchModeList; } }

    }
}
