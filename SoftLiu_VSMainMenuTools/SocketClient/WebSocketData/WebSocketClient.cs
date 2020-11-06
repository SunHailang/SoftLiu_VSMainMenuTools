using SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data;
using SoftLiu_VSMainMenuTools.UGUI;
using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.EventsManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData
{
    public partial class WebSocketClient : Form
    {

        private ClientWebSocket m_webSocketClent = null;

        readonly CancellationToken m_cancellation = new CancellationToken();

        private bool m_closeForm = false;

        public WebSocketClient()
        {
            InitializeComponent();
        }

        private void WebSocketClient_Load(object sender, EventArgs e)
        {
            // register event
            EventManager<MatchEvents>.Instance.RegisterEvent(MatchEvents.MatchCallbackType, OnMatchCallbackType);
            EventManager<MatchEvents>.Instance.RegisterEvent(MatchEvents.GameEndType, OnGameEndType);

            m_cancellation.Register(() =>
            {
                Console.WriteLine("CancellationToken Register Callback.");
            }, true);

            WebSocketManager.Instance.Init();
            // 初始化状态
            m_closeForm = false;
            radioWebSocketState.Checked = false;

            // 配置可连接的WebSocket服务器
            comboBoxServer.Items.Clear();
            if (WebSocketManager.Instance.ServerDatas != null)
            {
                foreach (WebSocketServerData item in WebSocketManager.Instance.ServerDatas)
                {
                    comboBoxServer.Items.Add(item.Name);
                }
                // 默认服务器是第一个
                comboBoxServer.SelectedIndex = 0;
                comboBoxServer.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        private async void WebSocketConnectAysnc(string url, Action callback = null)
        {
            try
            {
                m_webSocketClent = new ClientWebSocket();
                Uri uri = new Uri(url);
                await m_webSocketClent.ConnectAsync(uri, m_cancellation);
                if (callback != null)
                {
                    callback();
                }
            }
            catch (Exception error)
            {
                string errorMsg = $"WebSocketConnectAysnc Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private async void WebSocketCloseAysnc(System.Action callback = null)
        {
            try
            {
                if (m_webSocketClent == null || m_webSocketClent.State != WebSocketState.Open)
                {
                    if (callback != null)
                    {
                        callback();
                    }
                    return;
                }
                await m_webSocketClent.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", m_cancellation);
                if (callback != null)
                {
                    callback();
                }
            }
            catch (Exception error)
            {
                string errorMsg = $"WebSocketCloseAysnc Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private string m_url = "";//WebSocketManager.Instance.GetServerUrlByName(name);

        private async void WebSocketSendAysnc(byte[] buffer, Action<string> callback, WebSocketMessageType msgType = WebSocketMessageType.Binary)
        {
            try
            {
                if (m_webSocketClent == null || m_webSocketClent.State != WebSocketState.Open)
                {
                    string errorMsg = "WebSocketSendAysnc 不可用，请重新连接。";
                    Console.WriteLine(errorMsg);
                    textBoxError.AppendText($"{errorMsg}\n");
                    callback(errorMsg);
                    return;
                }
                await m_webSocketClent.SendAsync(new ArraySegment<byte>(buffer), msgType, true, m_cancellation);
                callback(null);
            }
            catch (Exception error)
            {
                if (m_closeForm)
                {
                    return;
                }
                string errorMsg = $"WebSocketSendAysnc Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
                callback(errorMsg);
            }
        }

        private void ReConnectCallback(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                if (!gameOver && gameing)
                {
                    WebSocketConnectAysnc(m_url, () =>
                    {
                        // start recveive
                        WebSocketReceiveAysnc(RecvData);
                        // 开始登陆
                        Dictionary<string, object> rejoinDic = new Dictionary<string, object>();
                        rejoinDic.Add("action", "rejoin");
                        rejoinDic.Add("match_id", m_matchID);
                        string sendData = JsonUtils.Instance.ObjectToJson(rejoinDic);
                        byte[] bsend = Encoding.UTF8.GetBytes(sendData);
                        WebSocketSendAysnc(bsend, (errorSend) =>
                        {

                        }, WebSocketMessageType.Text);
                    });
                }
            }
        }

        private async void WebSocketReceiveAysnc(Action<byte[]> callback)
        {
            try
            {
                while (true)
                {
                    if (m_webSocketClent == null || m_webSocketClent.State != WebSocketState.Open)
                    {
                        break;
                    }
                    byte[] m_recvBuffer = new byte[1024];
                    await m_webSocketClent.ReceiveAsync(new ArraySegment<byte>(m_recvBuffer), m_cancellation);
                    if (callback != null)
                    {
                        //byte[] head = null;
                        //int length = 0;
                        //byte[] result = null;
                        //GetReceiveData(m_recvBuffer, out head, out length, out result);
                        //callback(result);
                        callback(m_recvBuffer);
                    }
                }
            }
            catch (Exception error)
            {
                if (m_closeForm)
                {
                    return;
                }
                string errorMsg = $"WebSocketReceiveAysnc Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private byte[] GetSendData(byte[] data)
        {
            byte[] head = new byte[4] { 0x12, 0x13, 0x14, 0x15 };
            int length = data.Length;
            byte[] lengthArray = Int4ToBytesArray(length);

            byte[] result = new byte[head.Length + lengthArray.Length + data.Length];
            System.Buffer.BlockCopy(head, 0, result, 0, head.Length);
            System.Buffer.BlockCopy(lengthArray, 0, result, 4, lengthArray.Length);
            System.Buffer.BlockCopy(data, 0, result, 8, data.Length);

            return result;
        }
        private void GetReceiveData(byte[] data, out byte[] head, out int length, out byte[] result)
        {
            head = new byte[4];
            System.Buffer.BlockCopy(data, 0, head, 0, 4);
            byte[] lengthArrray = new byte[4];
            System.Buffer.BlockCopy(data, 4, lengthArrray, 0, 4);
            length = BytesArrayToInt4(lengthArrray);
            result = new byte[data.Length - 8];
            System.Buffer.BlockCopy(data, 8, result, 0, result.Length);
        }

        private byte[] Int4ToBytesArray(int data)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)(data >> 24);
            bytes[1] = (byte)(data >> 16);
            bytes[2] = (byte)(data >> 8);
            bytes[3] = (byte)(data >> 0);
            return bytes;
        }
        private int BytesArrayToInt4(byte[] data)
        {
            return ((data[0] & 0xff) << 24) |
                   ((data[1] & 0xff) << 16) |
                   ((data[2] & 0xff) << 8) |
                   (data[3] & 0xff);
        }
        private void webSocketBtnSend_Click(object sender, EventArgs e)
        {
            //f07f47cc-59a2-46ce-a37e-69540c60aadc
            Dictionary<string, object> login = new Dictionary<string, object>();
            login.Add("action", "login");
            login.Add("uuid", "f07f47cc-59a2-46ce-a37e-69540c60aadc");
            string jsonLogin = JsonUtils.Instance.ObjectToJson(login);

            //queue { "action":"queue", "queueInfo":{ "mode":1, "class":1, "shark":"shark-name", "currency":1 } }
            Dictionary<string, object> queue = new Dictionary<string, object>();
            queue.Add("action", "queue");
            Dictionary<string, object> queueInfo = new Dictionary<string, object>();
            queueInfo.Add("mode", 1);
            queueInfo.Add("class", 1);
            queueInfo.Add("shark", "BlueShark");
            queueInfo.Add("currency", 1);
            queue.Add("queueInfo", queueInfo);
            string jsonQueue = JsonUtils.Instance.ObjectToJson(queue);

            webSocketTextBoxSend.Text = jsonQueue;
            string sendData = webSocketTextBoxSend.Text.Trim();
            if (string.IsNullOrEmpty(sendData))
            {
                MessageBox.Show("发送的数据不能为空!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] bsend = Encoding.UTF8.GetBytes(sendData);

            //bsend = GetSendData(bsend);

            WebSocketSendAysnc(bsend, (error) =>
            {
                if (string.IsNullOrEmpty(error))
                {
                    MatchTimeCallback(bsend);
                }
            }, WebSocketMessageType.Text);
        }

        private void MatchTimeCallback(byte[] bsend)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                while (!gameing && !m_closeWebsocket)
                {
                    Thread.Sleep(10000);
                    if (!gameing && !m_closeWebsocket)
                    {
                        break;
                    }
                    WebSocketSendAysnc(bsend, ReConnectCallback, WebSocketMessageType.Text);
                }
            });
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            textBoxError.Text = string.Empty;
        }

        private void btnCleanRecv_Click(object sender, EventArgs e)
        {
            textBoxReceive.Text = string.Empty;
        }

        private void WebSocketClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_closeForm = true;
            WebSocketCloseAysnc(() =>
            {
                m_webSocketClent = null;
                FormManager.Instance.BackClose();
            });
        }
        private bool m_closeWebsocket = false;
        private void buttonCloseWebSocket_Click(object sender, EventArgs e)
        {
            if (m_webSocketClent != null && m_webSocketClent.State != WebSocketState.Closed)
            {
                WebSocketCloseAysnc(() =>
                {
                    m_closeWebsocket = true;
                    radioWebSocketState.Checked = false;
                    comboBoxServer.DropDownStyle = ComboBoxStyle.DropDown;
                    MessageBox.Show("断开连接完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
        }

        private void buttonConnServer_Click(object sender, EventArgs e)
        {
            string name = comboBoxServer.SelectedItem.ToString();
            m_url = WebSocketManager.Instance.GetServerUrlByName(name);
            if (string.IsNullOrEmpty(m_url))
            {
                MessageBox.Show("服务器URL不能为空!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_closeWebsocket = false;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            WebSocketConnectAysnc(m_url, () =>
             {
                 if (m_webSocketClent.State == WebSocketState.Open)
                 {
                     // connected
                     radioWebSocketState.Checked = true;
                     comboBoxServer.DropDownStyle = ComboBoxStyle.DropDownList;
                     // start recveive
                     WebSocketReceiveAysnc(RecvData);

                     Dictionary<string, object> login = new Dictionary<string, object>();
                     login.Add("action", "login");
                     login.Add("uuid", "f07f47cc-59a2-46ce-a37e-69540c60aadc");
                     string sendData = JsonUtils.Instance.ObjectToJson(login);

                     byte[] bsend = Encoding.UTF8.GetBytes(sendData);

                     //bsend = GetSendData(bsend);

                     WebSocketSendAysnc(bsend, (error) =>
                     {

                     }, WebSocketMessageType.Text);
                 }
             });
        }

        private int m_matchID = 0;

        private void RecvData(byte[] receiveData)
        {
            string recvStr = Encoding.UTF8.GetString(receiveData);
            recvStr = recvStr.TrimEnd('\0');
            if (string.IsNullOrEmpty(recvStr)) return;
            textBoxReceive.AppendText($"{recvStr}\n");

            Dictionary<string, object> dataRecvDic = JsonUtils.Instance.JsonToDictionary(recvStr) as Dictionary<string, object>;
            if (dataRecvDic != null && dataRecvDic.ContainsKey("action"))
            {
                if (dataRecvDic != null && dataRecvDic.ContainsKey("action"))
                {
                    string action = dataRecvDic["action"].ToString();

                    IEnumerable<WebSocketProtocolData> protocols = WebSocketManager.Instance.ProtocolDatas.Where(data => { return data.Protocol == action; });
                    WebSocketProtocolData protocol = protocols.FirstOrDefault();
                    if (protocols != null)
                    {
                        // 获取当前程序集 
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        //dynamic obj = assembly.CreateInstance("类的完全限定名（即包括命名空间）");
                        dynamic obj = assembly.CreateInstance($"SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data.{protocol.Type}");
                        if (obj is ActionData)
                        {
                            ActionData data = obj as ActionData;
                            data.Init(recvStr);
                        }
                        else
                        {
                            Console.WriteLine($"WebSocketProtocolData CreateInstance is null, action: {action}, type: {protocol.Type}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"WebSocketProtocolData is null, action: {action}");
                    }
                }
            }
        }
        private bool gameOver = false;
        private bool gameing = false;
        private void MatchSuccess(int matchID)
        {
            float time = 180;
            gameOver = false;
            int hp = 100;
            int score = 0;
            gameing = true;
            ThreadPool.QueueUserWorkItem((state) =>
            {
                while (!gameOver && !m_closeWebsocket)
                {
                    Thread.Sleep(500);
                    if (gameOver && !m_closeWebsocket)
                    {
                        break;
                    }
                    Dictionary<string, object> uploadData = new Dictionary<string, object>();
                    uploadData.Add("action", "upload");
                    Dictionary<string, object> upload = new Dictionary<string, object>();
                    hp -= 2;
                    if (hp <= 0)
                    {
                        Random ra = new Random();
                        hp = ra.Next(10, 100);
                    }
                    upload.Add("hp", hp);
                    score += 10;
                    upload.Add("score", score);
                    uploadData.Add("upload", upload);
                    string sendData = JsonUtils.Instance.ObjectToJson(uploadData);
                    byte[] bsend = Encoding.UTF8.GetBytes(sendData);
                    WebSocketSendAysnc(bsend, ReConnectCallback, WebSocketMessageType.Text);
                    time -= 0.5f;
                    if (time <= 0)
                    {
                        MatchGameEnd(m_matchID);
                    }
                }
            });
        }
        private void MatchGameEnd(int matchID)
        {
            gameOver = true;
            gameing = false;
            if (m_webSocketClent != null && m_webSocketClent.State != WebSocketState.Closed)
            {
                WebSocketCloseAysnc(() =>
                {
                    radioWebSocketState.Checked = false;
                    comboBoxServer.DropDownStyle = ComboBoxStyle.DropDown;
                    MessageBox.Show("比赛结束，断开连接完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
        }

        private void OnGameEndType(MatchEvents arg1, object[] arg2)
        {
            if (arg2 != null && arg2.Length > 0)
            {
                int matchID = Convert.ToInt32(arg2[0]);
                MatchGameEnd(matchID);
            }

        }

        private void OnMatchCallbackType(MatchEvents arg1, object[] arg2)
        {
            if (arg2 != null || arg2.Length > 0)
            {
                int matchID = Convert.ToInt32(arg2[0]);
                MatchSuccess(matchID);
            }

        }

        private void WebSocketClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            // deregister Event
            EventManager<MatchEvents>.Instance.DeregisterEvent(MatchEvents.MatchCallbackType, OnMatchCallbackType);
            EventManager<MatchEvents>.Instance.DeregisterEvent(MatchEvents.GameEndType, OnGameEndType);
        }
    }
}