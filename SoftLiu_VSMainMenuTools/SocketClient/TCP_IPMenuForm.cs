using Newtonsoft.Json;
using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.SocketClient.SocketData;
using SoftLiu_VSMainMenuTools.UGUI;
using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.EventsManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;

namespace SoftLiu_VSMainMenuTools
{

    public partial class TCP_IPMenuForm : Form
    {

        SocketTCPClient clientTcp;
        SocketUDPClient clientUdp;
        // Server : 192.168.218.129 : 30010
        //IPAddress m_tcpIP = IPAddress.Parse("192.168.218.129");
        //int m_tcpPort = 30010;


        public TCP_IPMenuForm()
        {
            InitializeComponent();
        }
        private void TCP_IPMenuForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Hello World !";

            toolStripProgressBar1.Value = 0;
            // init udp ip and port
            //this.textBoxUDPIPAddress.Text = "10.192.91.40";
            //this.textBoxUDPPort.Text = "11080";
            this.textBoxUDPIPAddress.Text = "192.168.218.129";
            this.textBoxUDPPort.Text = "30010";
        }

        ~TCP_IPMenuForm()
        {
            if (clientUdp != null)
                clientUdp.Close();
        }

        private void OnTrainCheckCodeType(TCPEvents arg1, object[] arg2)
        {
            textBoxTCPTips.AppendText(arg2.ToString() + "\r\n");
            if (arg2 != null && arg2.Length > 0)
            {
                Dictionary<string, object> jsonData = arg2[0] as Dictionary<string, object>;
                if (jsonData != null)
                {
                    int status = (int)jsonData["status"];
                    string result = jsonData["result"].ToString();
                    byte[] data = Convert.FromBase64String(result);
                    MemoryStream ms = new MemoryStream(data);
                    ms.Position = 0;
                    Image graphicPhoto = Image.FromStream(ms);
                    ms.Close();
                }
                else
                {
                    textBoxTCPTips.AppendText("Error OnTrainCheckCodeType: jsonData is null." + "\r\n");
                }
            }
            else
            {
                textBoxTCPTips.AppendText("Error OnTrainCheckCodeType: arg2" + "\r\n");
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.textBoxTCPSend.Text.Trim()))
                {
                    MessageBox.Show("Send data can't empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sendMessage = this.textBoxTCPSend.Text.Trim();
                if (clientTcp.Connected)
                {
                    //Dictionary<string, object> dic = new Dictionary<string, object>();
                    //dic.Add("code", "TrainQueryType");
                    //dic.Add("status", 0);
                    //dic.Add("date", DateTime.Now.ToString("yyy-MM-dd"));
                    //dic.Add("from_station", "上海");
                    //dic.Add("to_station", "徐州东");
                    //string sendMessage = string.Format("{0}", JsonConvert.SerializeObject(dic));
                    clientTcp.SendData(Encoding.UTF8.GetBytes(sendMessage));
                    //textBoxTCPSend.Text = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    MessageBox.Show("Please Connect Server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message, "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectStatus(bool state)
        {
            this.radioConnectStatus.Checked = state;
            this.buttonConnectTcp.Text = state ? "Connect" : "Disconnect";
        }

        private void buttonConnectTcp_Click(object sender, EventArgs e)
        {
            if (clientTcp != null)
            {
                if (clientTcp.Connected)
                {
                    // 断开连接
                    clientTcp.DisconnectServer((error) =>
                    {
                        if (error.ErrorCode != -1)
                            textBoxTCPTips.AppendText($"{error.ErrorStr}\r\n");
                    });
                    clientTcp = null;
                    ConnectStatus(false);
                }
                else
                {
                    try
                    {
                        textBoxTCPTips.AppendText("had client connect...\r\n");
                        clientTcp.ConnectServer(ReceiveDataCallback);
                        ConnectStatus(true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Connect Filed!");
                        return;
                    }
                }
            }
            else
            {
                string ip = this.textBoxTCPAddress.Text.Trim();
                string port = this.textBoxTCPPort.Text.Trim();
                int tcpPort = 0;
                bool ipCheck = RegexUtils.IPCheck(ip);
                if (!ipCheck || !int.TryParse(port, out tcpPort))
                {
                    MessageBox.Show("Please enter right IP address or port id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                IPAddress tcpIP = IPAddress.Parse(ip);
                clientTcp = new SocketTCPClient(new IPEndPoint(tcpIP, tcpPort));
                try
                {
                    textBoxTCPTips.AppendText("new client connect...\r\n");
                    clientTcp.ConnectServer(ReceiveDataCallback);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connect Filed!");
                    return;
                }
                //通过clientSocket接收数据
                //RecvThread(clientTcp);
            }
        }

        private void ReceiveDataCallback(SocketErrorData errData, SocketReceiveData recvData)
        {
            // error
            if (errData != null)
            {
                if (errData.ErrorCode != -1)
                {
                    this.textBoxTCPTips.AppendText($"{errData.ErrorStr}\r\n");
                }
                return;
            }
            ConnectStatus(true);
            // 解析接收数据
            if (recvData.Length > 0)
            {
                string data = Encoding.UTF8.GetString(recvData.RecvBuffer, 0, recvData.Length);
                this.textBoxTCPRecv.AppendText($"RecvData: {data}\r\n");
            }
        }

        private void RecvThread(Socket soc)
        {
            Thread th = new Thread((object obj) =>
            {
                //if (hasThread) return;
                if (obj != null && obj is Socket)
                {
                    Socket client = obj as Socket;
                    while (client != null && client.Connected)
                    {
                        try
                        {
                            byte[] cacheBuf = null;
                            byte[] datasize = new byte[1024];
                            int receiveLength = 0;
                            while ((receiveLength = client.Receive(datasize)) > 0)
                            {
                                //size = BitConverter.ToInt32(datasize, 0);
                                if (cacheBuf == null)
                                {
                                    cacheBuf = new byte[receiveLength];
                                    Array.Copy(datasize, cacheBuf, receiveLength);
                                }
                                else
                                {
                                    byte[] t = new byte[cacheBuf.Length + receiveLength];
                                    Array.Copy(cacheBuf, t, cacheBuf.Length);
                                    Array.Copy(datasize, 0, t, cacheBuf.Length, receiveLength);
                                    cacheBuf = t;
                                }
                                if (cacheBuf.Length <= 4)
                                {
                                    continue;
                                }
                                int msgl = BitConverter.ToInt32(cacheBuf, 0);

                                while (cacheBuf != null && msgl + 4 <= cacheBuf.Length)
                                {
                                    byte[] msgbyte = new byte[msgl];
                                    Array.Copy(cacheBuf, 4, msgbyte, 0, msgl);

                                    //test(msgbyte, soc);//拿到完整消息，具体消息操作
                                    string jsonMsg = Encoding.UTF8.GetString(msgbyte);
                                    Dictionary<string, object> jsonObject = JsonUtils.Instance.JsonToDictionary(jsonMsg);
                                    if (jsonObject != null)
                                    {
                                        TCPEvents eventType = TCPEvents.None;
                                        bool parse = Enum.TryParse<TCPEvents>(jsonObject["code"].ToString(), out eventType);
                                        if (parse)
                                        {
                                            EventManager<TCPEvents>.Instance.TriggerEvent(eventType, jsonObject);
                                        }
                                        else
                                        {
                                            textBoxTCPTips.AppendText("Error Code Parse: " + jsonObject["code"].ToString() + "\r\n");
                                        }
                                    }
                                    else
                                    {
                                        textBoxTCPTips.AppendText("Error Json Message: " + jsonMsg + "\r\n");
                                    }

                                    if (msgl + 4 == cacheBuf.Length)
                                    {
                                        cacheBuf = null;
                                    }
                                    else
                                    {
                                        byte[] tmpByte = new byte[cacheBuf.Length - msgl - 4];
                                        Array.Copy(cacheBuf, msgl + 4, tmpByte, 0, cacheBuf.Length - msgl - 4);
                                        cacheBuf = tmpByte;
                                    }
                                }
                            }
                        }
                        catch (Exception msg)
                        {
                            Console.WriteLine("Recv Data Thread End.\r\n" + msg.Message, "Error");
                            break;
                        }
                    }
                }
            });
            th.Start(soc);
        }

        private void TCP_IPMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clientTcp != null)
            {
                clientTcp.DisconnectServer((error) =>
                {
                    if (error.ErrorCode != -1)
                    {
                        textBoxTCPTips.AppendText($"{error.ErrorStr}\r\n");
                    }
                });
            }
            clientTcp = null;
            clientUdp = null;

            FormManager.Instance.BackClose();
        }

        public static byte[] int2bytes(int i)
        {
            byte[] b = new byte[4];

            b[0] = (byte)(0xff & i);
            b[1] = (byte)((0xff00 & i) >> 8);
            b[2] = (byte)((0xff0000 & i) >> 16);
            b[3] = (byte)((0xff000000 & i) >> 24);
            return b;
        }

        private void buttonUDPSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (clientUdp == null)
                {
                    //Server : 192.168.218.129 : 30010
                    string ip = this.textBoxUDPIPAddress.Text.Trim();
                    int port = 0;
                    IPAddress address = IPAddress.Parse(ip);
                    string portStr = this.textBoxUDPPort.Text.Trim();
                    int.TryParse(portStr, out port);
                    IPEndPoint server = new IPEndPoint(address, port);
                    clientUdp = new SocketUDPClient(server, (recvData) =>
                    {
                        if (recvData.Length <= 0)
                        {
                            Console.WriteLine("receive data is null.");
                            return;
                        }
                        string data = Encoding.UTF8.GetString(recvData.RecvBuffer, 0, recvData.Length);
                        this.textBoxUDPReceive.AppendText($"{data}\r\n");
                    });
                }
                string sendMsg = this.textBoxUDPSendMessage.Text.Trim();
                if (string.IsNullOrEmpty(sendMsg))
                {
                    Console.WriteLine("send data is null.");
                    return;
                }
                byte[] sendBuffer = Encoding.UTF8.GetBytes(sendMsg);
                clientUdp.SendTo(sendBuffer);
            }
            catch (Exception error)
            {
                Console.WriteLine($"UDPSend_Click Error: {error.Message}");
            }
        }

        private void buttonToolsGetIP_Click(object sender, EventArgs e)
        {
            // get ip

        }

    }
}
