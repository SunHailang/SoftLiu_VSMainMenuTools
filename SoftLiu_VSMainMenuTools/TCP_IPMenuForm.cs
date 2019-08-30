using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


            //为了方便在本机上同时运行Client和server，使用回环地址为服务的监听地址
            IPAddress ip = IPAddress.Loopback;
            //实例化一个Socket对象，确定网络类型、Socket类型、协议类型
            serverTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Socket对象绑定IP和端口号
            serverTcp.Bind(new IPEndPoint(ip, 8099));
            //挂起连接队列的最大长度为15，启动监听
            serverTcp.Listen(15);

            Console.WriteLine("启动监听{0}成功", serverTcp.LocalEndPoint.ToString());
            //一个客户端连接服务器时创建一个新的线程
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();

        }

        private void ListenClientConnect()
        {
            while (true)
            {
                Socket client = serverTcp.Accept();
                client.Send(Encoding.UTF8.GetBytes("Server said:\" Hello Client\"."));
                Thread recvThd = new Thread(ReciveMessage);
                recvThd.Start(client);
            }
        }

        private void ReciveMessage(object client)
        {
            if (client != null && client is Socket)
            {
                Socket m_client = client as Socket;
                while (true)
                {
                    try
                    {
                        int recvLength = m_client.Receive(m_serverDataBuffer);
                        textBox1.AppendText(string.Format("Client:{0} -> data:{1}\n", m_client.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(m_serverDataBuffer, 0, recvLength)));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Client Failed");
                        m_client.Shutdown(SocketShutdown.Both);
                        m_client.Close();
                    }
                }
            }
        }

        bool setBool = false;
        private void buttonSend_Click(object sender, EventArgs e)
        {
            string sendMessage = string.Format("{0} {1}", "Server 你好！", DateTime.Now.ToString());
            clientTcp.Send(Encoding.UTF8.GetBytes(sendMessage));
        }

        private void buttonConnectTcp_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Loopback;
            clientTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientTcp.Connect(new IPEndPoint(ip, 8099));
                this.radioConnectStatus.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Connect Filed!");
                return;
            }
            //通过clientSocket接收数据
            int receiveLength = clientTcp.Receive(m_clientDataBuffer);
            Console.WriteLine("接受服务器消息：{0}", Encoding.UTF8.GetString(m_clientDataBuffer, 0, receiveLength));
        }
    }
}
