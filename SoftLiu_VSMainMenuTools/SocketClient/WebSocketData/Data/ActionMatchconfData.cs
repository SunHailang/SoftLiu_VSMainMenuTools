using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionMatchconfData : ActionData
    {
        public override void Init(string responseJson)
        {
            //Console.WriteLine($"ActionMatchconfData Response: {responseJson}");
            Dictionary<string, object> dataRecvDic = JsonUtils.Instance.JsonToDictionary(responseJson);
            List<Dictionary<string, object>> userList = null;
            if (dataRecvDic.ContainsKey("errcode"))
            {
                // 报错
                int errcode = Convert.ToInt32(dataRecvDic["errcode"]);
                string err = dataRecvDic["err"].ToString();
            }
            else if (dataRecvDic.ContainsKey("users"))
            {
                userList = dataRecvDic["users"] as List<Dictionary<string, object>>;
            }
        }
    }
}
