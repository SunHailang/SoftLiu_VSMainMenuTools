using System;
using System.Collections.Generic;
using System.Linq;
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
                Client client = new Client();

                while (true)
                {
                    string send = Console.ReadLine();
                    client.ClientSendMsg(send);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

           
        }
    }
}
