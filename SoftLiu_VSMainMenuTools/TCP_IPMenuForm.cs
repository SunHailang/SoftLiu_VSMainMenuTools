using Newtonsoft.Json;
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

        Socket serverTcp;

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

            toolStripProgressBar1.Value = 50;

        }
        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (clientTcp.Connected)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("code", "Tickets");
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

        bool hasThread = false;

        private void RecvThread(Socket soc)
        {
            Thread th = new Thread((object obj) =>
            {
                //if (hasThread) return;
                if (obj != null && obj is Socket)
                {

                    hasThread = false;
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
                                    textBoxTCPRecv.AppendText(msgbyte.Length + "\n");
                                    //test(msgbyte, soc);//拿到完整消息，具体消息操作

                                    MemoryStream ms = new MemoryStream(msgbyte);
                                    ms.Position = 0;
                                    Image graphicPhoto = Image.FromStream(ms);
                                    ms.Close();
                                    pictureBoxCheckCode.Image = graphicPhoto;
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
            if (clientTcp != null)
            {
                clientTcp.Close();
            }
            if (clientUdp != null)
            {
                clientUdp.Close();
            }
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
                    dic.Add("code", "TrainCheckCode");
                    dic.Add("status", 0);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
