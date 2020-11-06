using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionQueueData : ActionData
    {
        public override void Init(string responseJson)
        {
            Console.WriteLine($"ActionQueueData Response: {responseJson}");
        }
    }
}
