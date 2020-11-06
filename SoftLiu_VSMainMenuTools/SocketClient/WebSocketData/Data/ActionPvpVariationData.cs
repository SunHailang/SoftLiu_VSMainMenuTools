using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionPvpVariationData : ActionData
    {
        public override void Init(string responseJson)
        {
            Console.WriteLine($"ActionPvpVariationData Response: {responseJson}");

        }
    }
}
