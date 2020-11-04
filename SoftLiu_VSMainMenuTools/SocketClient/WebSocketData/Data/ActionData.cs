using System;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public abstract class ActionData
    {
        public virtual void Init()
        {
            Console.WriteLine("Init AcctionData");
        }
    }
}
