﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftLiu_VSMainMenuTools.Singleton;
using System.Windows.Forms;
using System.IO;
using SoftLiu_VSMainMenuTools.Utils;
using System.Reflection;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData
{
    public class WebSocketManager : AutoGeneratedSingleton<WebSocketManager>
    {
        private const string m_path = @"\Resources\WebSocket\WebSocketServers.json";


        private List<WebSocketServerData> m_ServerDatas = null;
        public List<WebSocketServerData> ServerDatas { get { return m_ServerDatas; } }

        public List<WebSocketProtocolData> ProtocolDatas { get; private set; }

        public WebSocketManager()
        {
            string jsonPath = Application.StartupPath + m_path;
            if (File.Exists(jsonPath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(File.Open(jsonPath, FileMode.Open, FileAccess.Read)))
                    {
                        string read = sr.ReadToEnd();
                        Dictionary<string, List<object>> jsonData = JsonUtils.Instance.JsonToObject<Dictionary<string, List<object>>>(read);
                        if (jsonData != null)
                        {
                            if (jsonData.ContainsKey("ServerData"))
                            {
                                List<object> serverData = jsonData["ServerData"];
                                m_ServerDatas = DataUtils.CreateInstances<WebSocketServerData>(serverData);
                            }
                            if(jsonData.ContainsKey("ReceiveProtocolData"))
                            {
                                List<object> protocolData = jsonData["ReceiveProtocolData"];
                                ProtocolDatas = DataUtils.CreateInstances<WebSocketProtocolData>(protocolData);
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show($"WebSocketManager Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("WebSocketManager Json not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Init()
        {

        }

        public string GetServerUrlByName(string name)
        {
            string result = null;
            if (m_ServerDatas != null)
            {
                IEnumerable<WebSocketServerData> data = m_ServerDatas.Where((server) => { return server.Name == name; });
                WebSocketServerData first = data.FirstOrDefault();
                result = first == null ? null : first.Url;
            }
            return result;
        }
    }
}
