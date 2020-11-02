using SoftLiu_VSMainMenuTools.UGUI;
using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData
{
    public partial class WebSocketClient : Form
    {

        private ClientWebSocket m_webSocketClent = null;

        readonly CancellationToken m_cancellation = new CancellationToken();

        //private const string m_url = "wss://cs-s-1000106500.gamebean.net/echo";

        private bool m_closeForm = false;

        //private byte[] m_recvBuffer = new byte[1024];

        public WebSocketClient()
        {
            InitializeComponent();
        }

        private void WebSocketClient_Load(object sender, EventArgs e)
        {
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

        private async void WebSocketSendAysnc(byte[] buffer, WebSocketMessageType msgType = WebSocketMessageType.Binary)
        {
            try
            {
                if (m_webSocketClent == null || m_webSocketClent.State != WebSocketState.Open)
                {
                    MessageBox.Show("WebSocket不可用，请重新连接。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await m_webSocketClent.SendAsync(new ArraySegment<byte>(buffer), msgType, true, m_cancellation);
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

            WebSocketSendAysnc(bsend, WebSocketMessageType.Text);
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

        private void buttonCloseWebSocket_Click(object sender, EventArgs e)
        {
            if (m_webSocketClent != null && m_webSocketClent.State != WebSocketState.Closed)
            {
                WebSocketCloseAysnc(() =>
                {
                    radioWebSocketState.Checked = false;
                    comboBoxServer.DropDownStyle = ComboBoxStyle.DropDown;
                    MessageBox.Show("断开连接完成。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
        }

        private void buttonConnServer_Click(object sender, EventArgs e)
        {
            string name = comboBoxServer.SelectedItem.ToString();
            string url = WebSocketManager.Instance.GetServerUrlByName(name);
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("服务器URL不能为空!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            m_webSocketClent = new ClientWebSocket();
            WebSocketConnectAysnc(url, () =>
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

                     WebSocketSendAysnc(bsend, WebSocketMessageType.Text);
                 }
             });
        }

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
                    int errcode = -1;
                    string err = string.Empty;
                    switch (action)
                    {
                        case "conn": // {"action":"conn","err":"server is busy","errcode":1}
                            errcode = Convert.ToInt32(dataRecvDic["errcode"]);
                            err = dataRecvDic["err"].ToString();
                            break;
                        case "login": // 返回登录的状态
                            if (dataRecvDic.ContainsKey("errcode"))
                            {
                                errcode = Convert.ToInt32(dataRecvDic["errcode"]);
                                err = dataRecvDic["err"].ToString();
                            }
                            else if (dataRecvDic.ContainsKey("uuid"))
                            {
                                // 登录success
                            }
                            break;
                        case "queue": // 返回匹配队列的状态
                            int match_id = -1;
                            if (dataRecvDic.ContainsKey("errcode"))
                            {
                                // 报错
                                errcode = Convert.ToInt32(dataRecvDic["errcode"]);
                                err = dataRecvDic["err"].ToString();
                            }
                            else if (dataRecvDic.ContainsKey("match_id"))
                            {
                                // 匹配成功直接进入游戏
                                match_id = Convert.ToInt32(dataRecvDic["match_id"]);
                                MatchSuccess(match_id);
                            }
                            else
                            {
                                // 进入排队
                            }
                            break;
                        case "matchconf": // 获取比赛配置
                            List<Dictionary<string, object>> userList = null;
                            if (dataRecvDic.ContainsKey("errcode"))
                            {
                                // 报错
                                errcode = Convert.ToInt32(dataRecvDic["errcode"]);
                                err = dataRecvDic["err"].ToString();
                            }
                            else if (dataRecvDic.ContainsKey("users"))
                            {
                                userList = dataRecvDic["users"] as List<Dictionary<string, object>>;
                            }
                            break;
                        case "upload":
                            // {"action":"upload","err":"you are not in a match","errcode":8}
                            Console.WriteLine($"player upload game state error: {recvStr}");
                            break;
                        case "push": // 服务器端推送消息
                            int eventID = Convert.ToInt32(dataRecvDic["event_id"]);
                            switch (eventID)
                            {
                                case 1: // 状态变更
                                        // 匹配成功： {"action":"push","event_id":1,"match_id":16,"status":3,"uuid":"test-User-0"}
                                    if (dataRecvDic.ContainsKey("match_id"))
                                    {
                                        // 匹配成功直接进入游戏
                                        int pushmatch_id = Convert.ToInt32(dataRecvDic["match_id"]);
                                        MatchSuccess(pushmatch_id);
                                    }
                                    else
                                    {
                                        // NOT TODO
                                    }
                                    break;
                                case 2: // 比赛结束
                                    int endmatch_id = Convert.ToInt32(dataRecvDic["match_id"]);
                                    MatchGameEnd(endmatch_id);
                                    break;
                                case 3:
                                    // 比赛状态
                                    try
                                    {
                                        //{"action":"push","event_id":3,"match_id":28,"time_remains":2,"user_status":{"test-User-0":{"score":0,"hp":100,"finished":false},"test-User-1":{"score":0,"hp":100,"finished":false}}}
                                        int gamestatematch_id = Convert.ToInt32(dataRecvDic["match_id"]);
                                        int time_remains = Convert.ToInt32(dataRecvDic["time_remains"]);
                                        Dictionary<string, object> user_status = dataRecvDic["user_status"] as Dictionary<string, object>;
                                    }
                                    catch (Exception errorEXstate)
                                    {
                                        Console.WriteLine($"WebSocketReceiveCallback eventID: {eventID}, error: {errorEXstate.Message}");
                                    }
                                    break;
                                default:
                                    Console.WriteLine($"WebSocketReceiveCallback eventID: {eventID}");
                                    break;
                            }
                            break;
                        default:
                            Console.WriteLine($"WebSocketReceiveCallback action: {action}");
                            break;
                    }
                }
            }
        }
        private bool gameOver = false;
        private void MatchSuccess(int matchID)
        {
            float time = 180;
            gameOver = false;
            int hp = 100;
            int score = 0;
            ThreadPool.QueueUserWorkItem((state) =>
            {
                while (!gameOver)
                {
                    Thread.Sleep(500);
                    if (gameOver)
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
                    WebSocketSendAysnc(bsend, WebSocketMessageType.Text);
                    time -= 0.5f;
                    if (time <= 0)
                    {
                        gameOver = true;
                    }
                }
            });
        }
        private void MatchGameEnd(int matchID)
        {
            gameOver = true;
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
    }
}