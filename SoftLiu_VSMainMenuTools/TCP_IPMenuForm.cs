using Newtonsoft.Json;
using SoftLiu_VSMainMenuTools.Data;
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

namespace SoftLiu_VSMainMenuTools
{
    public partial class TCP_IPMenuForm : Form
    {

        Socket clientTcp;
        Socket clientUdp;

        //IPAddress ip = IPAddress.Loopback;

        IPAddress m_tcpIP = IPAddress.Parse("192.168.218.128");

        //IPAddress m_tcpIP = IPAddress.Parse("202.59.232.58");
        int m_tcpPort = 11060;

        //Socket serverTcp;

        //创建一个数据缓冲区
        private static byte[] m_clientDataBuffer = new byte[1024];
        private static byte[] m_serverDataBuffer = new byte[1024];

        public TCP_IPMenuForm()
        {
            InitializeComponent();
        }
        private void TCP_IPMenuForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Hello World !";

            toolStripProgressBar1.Value = 0;

            EventManager<TCPEvents>.Instance.RegisterEvent(TCPEvents.TrainCheckCodeType, OnTrainCheckCodeType);

        }

        ~TCP_IPMenuForm()
        {
            
        }

        private void OnTrainCheckCodeType(TCPEvents arg1, object[] arg2)
        {
            textBoxTCPRecv.AppendText(arg2.ToString() + "\n");
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
                    pictureBoxCheckCode.Image = graphicPhoto;
                }
                else
                {
                    textBoxTCPRecv.AppendText("Error OnTrainCheckCodeType: jsonData is null." + "\n");
                }
            }
            else
            {
                textBoxTCPRecv.AppendText("Error OnTrainCheckCodeType: arg2" + "\n");
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (clientTcp.Connected)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("code", "TrainQueryType");
                    dic.Add("status", 0);
                    dic.Add("date", DateTime.Now.ToString("yyy-MM-dd"));
                    dic.Add("from_station", "上海");
                    dic.Add("to_station", "徐州东");
                    string sendMessage = string.Format("{0}", JsonConvert.SerializeObject(dic));
                    clientTcp.Send(Encoding.UTF8.GetBytes(sendMessage));
                    textBoxTCPSend.Text = JsonConvert.SerializeObject(dic);
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

        private void buttonConnectTcp_Click(object sender, EventArgs e)
        {
            if (clientTcp != null)
            {
                if (clientTcp.Connected)
                {
                    MessageBox.Show("Client had Connected");
                }
                else
                {
                    try
                    {
                        textBoxTCPRecv.AppendText("had client connect...\n");
                        clientTcp.Connect(new IPEndPoint(m_tcpIP, m_tcpPort));
                        this.radioConnectStatus.Checked = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Connect Filed!");
                        return;
                    }
                    //通过clientSocket接收数据
                    RecvThread(clientTcp);
                }
            }
            else
            {
                clientTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    textBoxTCPRecv.AppendText("new client connect...\n");
                    clientTcp.Connect(new IPEndPoint(m_tcpIP, m_tcpPort));
                    this.radioConnectStatus.Checked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Connect Filed!");
                    return;
                }
                //通过clientSocket接收数据
                RecvThread(clientTcp);
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
                                            textBoxTCPRecv.AppendText("Error Code Parse: " + jsonObject["code"].ToString() + "\n");
                                        }
                                    }
                                    else
                                    {
                                        textBoxTCPRecv.AppendText("Error Json Message: " + jsonMsg + "\n");
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
                            Console.WriteLine("Recv Data Thread End.\n" + msg.Message, "Error");
                            break;
                        }
                    }
                }
            });
            th.Start(soc);
        }

        private void radioConnectStatus_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonDisconnectTCP_Click(object sender, EventArgs e)
        {
            if (clientTcp != null)
            {
                if (clientTcp.Connected)
                {
                    clientTcp.Close();
                }
                clientTcp = null;
            }
            else
            {
                MessageBox.Show("please Connect Server.", "Connect Filed!");
            }
            radioConnectStatus.Checked = false;
        }

        private void TCP_IPMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventManager<TCPEvents>.Instance.DeregisterEvent(TCPEvents.TrainCheckCodeType, OnTrainCheckCodeType);

            if (clientTcp != null)
            {
                clientTcp.Close();
            }
            if (clientUdp != null)
            {
                clientUdp.Close();
            }
            clientTcp = null;
            clientUdp = null;
        }

        private void buttonCheckCode_Click(object sender, EventArgs e)
        {
            //string path = @"D:\PythonProgram\SoftLiu_PythonServerIO\pic.jpg";
            //Image img = Image.FromFile(path, false);
            //pictureBoxCheckCode.BackgroundImage = img;
            //textBoxTCPRecv.AppendText(string.Format("w:{0}, h:{1}", img.Width, img.Height));
            //return;
            try
            {
                if (clientTcp.Connected)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("code", "TrainCheckCodeType");
                    dic.Add("status", 0);
                    string sendMessage = string.Format("{0}", JsonConvert.SerializeObject(dic));
                    byte[] sendData = Encoding.UTF8.GetBytes(sendMessage);
                    byte[] sendDataLen = BitConverter.GetBytes(sendData.Length);
                    clientTcp.Send(sendDataLen);
                    //Console.WriteLine(sendData.Length);
                    clientTcp.Send(sendData);
                    textBoxTCPSend.Text = JsonConvert.SerializeObject(dic);
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

        public static byte[] int2bytes(int i)
        {
            byte[] b = new byte[4];

            b[0] = (byte)(0xff & i);
            b[1] = (byte)((0xff00 & i) >> 8);
            b[2] = (byte)((0xff0000 & i) >> 16);
            b[3] = (byte)((0xff000000 & i) >> 24);
            return b;
        }
    }
}
