using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionUploadData : ActionData
    {
        public override void Init(string responseJson)
        {
            Console.WriteLine($"ActionUploadData Response: {responseJson}");
            // {"action":"upload","err":"you are not in a match","errcode":8}
            //Console.WriteLine($"player upload game state error: {recvStr}");
        }
    }
}
