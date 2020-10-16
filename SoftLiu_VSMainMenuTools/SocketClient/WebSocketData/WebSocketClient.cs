using SoftLiu_VSMainMenuTools.UGUI;
using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private async void WebSocketSendAysnc(byte[] buffer)
        {
            try
            {
                if (m_webSocketClent == null || m_webSocketClent.State != WebSocketState.Open)
                {
                    MessageBox.Show("WebSocket不可用，请重新连接。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await m_webSocketClent.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, m_cancellation);
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
                        byte[] head = null;
                        int length = 0;
                        byte[] result = null;
                        GetReceiveData(m_recvBuffer, out head, out length, out result);
                        callback(result);
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
            string sendData = webSocketTextBoxSend.Text.Trim();
            if (string.IsNullOrEmpty(sendData))
            {
                MessageBox.Show("发送的数据不能为空!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] bsend = Encoding.UTF8.GetBytes(sendData);

            bsend = GetSendData(bsend);

            WebSocketSendAysnc(bsend);
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
            m_webSocketClent = new ClientWebSocket();
            WebSocketConnectAysnc(url, () =>
             {
                 if (m_webSocketClent.State == WebSocketState.Open)
                 {
                     // connected
                     radioWebSocketState.Checked = true;
                     comboBoxServer.DropDownStyle = ComboBoxStyle.DropDownList;
                     // start recveive
                     WebSocketReceiveAysnc((receiveData) =>
                      {
                          string recvStr = Encoding.UTF8.GetString(receiveData);
                          recvStr = recvStr.TrimEnd('\0');
                          if (string.IsNullOrEmpty(recvStr)) return;
                          textBoxReceive.AppendText($"{recvStr}\n");
                      });
                 }
             });
        }
    }
}
