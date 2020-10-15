using SoftLiu_VSMainMenuTools.UGUI;
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

        private const string m_url = "wss://cs-s-1000106500.gamebean.net/echo";

        private byte[] m_recvBuffer = new byte[1024 * 1024];

        public WebSocketClient()
        {
            InitializeComponent();
        }

        private void WebSocketClient_Load(object sender, EventArgs e)
        {
            // 初始化状态
            radioWebSocketState.Checked = false;

            m_webSocketClent = new ClientWebSocket();
            WebSocketConnect(() =>
            {
                if (m_webSocketClent.State == WebSocketState.Open)
                {
                    // connected
                    radioWebSocketState.Checked = true;
                    // start recveive
                    WebSocketReceive((receiveData) =>
                    {
                        string recvStr = Encoding.UTF8.GetString(m_recvBuffer);
                        textBoxReceive.AppendText($"{recvStr.TrimEnd('\0')}\n");
                    });
                }
            });
        }

        private async void WebSocketConnect(Action callback = null)
        {
            try
            {
                Uri uri = new Uri(m_url);
                await m_webSocketClent.ConnectAsync(uri, m_cancellation);
                if (callback != null)
                {
                    callback();
                }
            }
            catch (Exception error)
            {
                string errorMsg = $"WebSocketConnect Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private async void WebSocketClose(System.Action callback = null)
        {
            try
            {
                await m_webSocketClent.CloseAsync(WebSocketCloseStatus.Empty, "Close", new CancellationToken());
                if (callback != null)
                {
                    callback();
                }
            }
            catch (Exception error)
            {
                string errorMsg = $"WebSocketClose Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private async void WebSocketSend(byte[] buffer)
        {
            try
            {
                await m_webSocketClent.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, m_cancellation);
            }
            catch (Exception error)
            {
                string errorMsg = $"WebSocketSend Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private async void WebSocketReceive(Action<byte[]> callback)
        {
            try
            {
                while (true)
                {
                    await m_webSocketClent.ReceiveAsync(new ArraySegment<byte>(m_recvBuffer), new CancellationToken());
                    if (callback != null)
                    {
                        callback(m_recvBuffer);
                    }
                }
            }
            catch (Exception error)
            {
                string errorMsg = $"WebSocketReceive Error: {error.Message}";
                textBoxError.AppendText($"{errorMsg}\n");
                Console.WriteLine(errorMsg);
            }
        }

        private void WebSocketClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_webSocketClent != null && m_webSocketClent.State != WebSocketState.Closed)
            {
                WebSocketClose(() =>
                {
                    //FormManager.Instance.BackClose();
                });
            }
            m_webSocketClent = null;
            FormManager.Instance.BackClose();
        }

        private void webSocketBtnSend_Click(object sender, EventArgs e)
        {
            string sendData = webSocketTextBoxSend.Text.Trim();
            if (string.IsNullOrEmpty(sendData))
            {
                MessageBox.Show("发送的数据不能为空!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (m_webSocketClent.State == WebSocketState.Open)
            {
                byte[] bsend = Encoding.UTF8.GetBytes(sendData);

                WebSocketSend(bsend);
            }
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            textBoxError.Text = string.Empty;
        }

        private void btnCleanRecv_Click(object sender, EventArgs e)
        {
            textBoxReceive.Text = string.Empty;
        }
    }
}
