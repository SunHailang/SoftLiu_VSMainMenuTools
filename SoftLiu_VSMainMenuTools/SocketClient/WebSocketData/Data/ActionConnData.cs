using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionConnData : ActionData
    {
        public override void Init(string responseJson)
        {
            //Console.WriteLine($"ActionConnData Response: {responseJson}");
            // {"action":"conn","err":"server is busy","errcode":1}
            Dictionary<string, object> dataRecvDic = JsonUtils.Instance.JsonToDictionary(responseJson) as Dictionary<string, object>;
            int errcode = Convert.ToInt32(dataRecvDic["errcode"]);
            string err = dataRecvDic["err"].ToString();
            Console.WriteLine($"ActionConnData Response: errcode:{errcode}, err:{err}");
        }
    }
}
