using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SoctLiu_Client
{
    class Program
    {
        public delegate bool ControlCtrlDelegate(int CtrlType);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlDelegate HandlerRoutine, bool Add);


        static void Main(string[] args)
        {

            try
            {
                Client client = null;
                ControlCtrlDelegate cancelHandler = new ControlCtrlDelegate(HandlerRoutine);

                SetConsoleCtrlHandler(cancelHandler, true);
                //UDPClient client = new UDPClient();
                //获取文本框中的IP地址
                //IPAddress address = IPAddress.Parse("10.20.24.22");
                IPAddress address = IPAddress.Parse("127.0.0.1");
                //将获取的IP地址和端口号绑定在网络节点上
                IPEndPoint point = new IPEndPoint(address, 13000);
                client = new Client(point);
                while (true)
                {
                    string send = Console.ReadLine();
                    client.ClientSendMsg(send);
                }

                bool HandlerRoutine(int CtrlType)
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                    switch (CtrlType)
                    {
                        case 0:
                            Console.WriteLine("0工具被强制关闭"); //Ctrl+C关闭  
                            break;
                        case 2:
                            Console.WriteLine("2工具被强制关闭");//按控制台关闭按钮关闭  
                            break;
                    }
                    return false;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }


        }
    }
}
