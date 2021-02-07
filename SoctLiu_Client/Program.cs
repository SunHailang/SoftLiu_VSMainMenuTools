using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoctLiu_Client
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                int total = 1150000;
                double month = 5.88f / 100 / 12;
                double monthPow = Math.Pow((1 + month), 360);

                //double monthDay = (total * (5.88f / 100 / 12) * Math.Pow((1 + (5.88f / 100 / 12)), 360)) / (Math.Pow((1 + (5.88f / 100 / 12)), 360) - 1);
                double monthDay = (total * month * monthPow) / (monthPow - 1);

                Console.WriteLine($"monthday: {monthDay}");

                Console.WriteLine("提前还款：");
                int tTotal = 200000;

                double tMonthDay = tTotal * (5.88f / 100 / 12) * 2;
                Console.WriteLine($"tMonthDay: {tMonthDay}");

                //UDPClient client = new UDPClient();
                //获取文本框中的IP地址
                //IPAddress address = IPAddress.Parse("10.192.91.40");
                ////将获取的IP地址和端口号绑定在网络节点上
                //IPEndPoint point = new IPEndPoint(address, 11060);
                //Client client = new Client(point);
                //while (true)
                //{
                //    string send = Console.ReadLine();
                //    client.ClientSendMsg(send);
                //}

                ////string localHostName = Dns.GetHostName();
                ////Console.WriteLine($"Local Host Name: {localHostName}");
                ////通过主机名获取该主机下存储所有IP地址信息的容器
                ////IPHostEntry local = Dns.GetHostEntry(localHostName);

                ////通过IPHostEntry对象的AddressList属性获取相关联主机的所有IP地址
                ////IPAddress[] ipList = local.AddressList;

                ////获取本机回环地址
                ////IPAddress loopbackIP = IPAddress.Loopback;

                ////通过它Parse函数构造IPAddress对象
                ////IPAddress localIp = IPAddress.Parse("10.192.91.40");

                ////通过IPAddress对象和端口号构造IPEndPoint对象
                ////IPEndPoint iep = new IPEndPoint(localIp, );

                Console.ReadKey();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }


        }
    }
}
