using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionLoginData : ActionData
    {
        public override void Init(string responseJson)
        {
            //Console.WriteLine($"ActionLoginData Response: {responseJson}");
            Dictionary<string, object> dataRecvDic = JsonUtils.Instance.JsonToDictionary(responseJson);
            if (dataRecvDic.ContainsKey("errcode"))
            {
                int errcode = Convert.ToInt32(dataRecvDic["errcode"]);
                string err = dataRecvDic["err"].ToString();
                Console.WriteLine($"ActionLoginData Response: errcode:{errcode}, err:{err}");
            }
            else if (dataRecvDic.ContainsKey("uuid"))
            {
                // 登录success
                Console.WriteLine($"ActionLoginData Login Success.");
            }
        }
    }
}
