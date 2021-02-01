using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.EventsManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData.Data
{
    public class ActionPushData : ActionData
    {
        public override void Init(string responseJson)
        {
            Console.WriteLine($"ActionPushData Response: {responseJson}");
            Dictionary<string, object> dataRecvDic = JsonUtils.Instance.JsonToDictionary(responseJson) as Dictionary<string, object>;
            //int errcode = Convert.ToInt32(dataRecvDic["errcode"]);
            //string err = dataRecvDic["err"].ToString();

            int eventID = Convert.ToInt32(dataRecvDic["event_id"]);
            int matchID = 0;
            switch (eventID)
            {
                case 1:
                    // 状态变更
                    // 匹配成功： {"action":"push","event_id":1,"match_id":16,"status":3,"uuid":"test-User-0"}
                    //{ "action":"push","event_id":1,"match_id":55,
                    //"matchconf":{ "users":[
                    //{ "equip":"","icon":0,"name":"name t0","shark":"shark-name","uuid":"test-User-0"},
                    //{ "equip":"","icon":9,"name":"未知鲨鱼4515","shark":"BlueShark","uuid":"3a8f3838-baba-40c1-b63c-d0ec2b21b42d"}]},
                    //"pvpvariation":{"pvpconf":[
                    //{ "key":"healthMax","showAsRand":false},
                    //{ "key":"boostSpeed","showAsRand":true},
                    //{ "key":"healthDrain","showAsRand":false},
                    //{ "key":"pollution","showAsRand":false},
                    //{ "key":"pollution","showAsRand":false}]},
                    //"status":3,"uuid":"test-User-0"}
                    if (dataRecvDic.ContainsKey("match_id"))
                    {
                        // 匹配成功直接进入游戏
                        matchID = Convert.ToInt32(dataRecvDic["match_id"]);
                        EventManager<MatchEvents>.Instance.TriggerEvent(MatchEvents.MatchCallbackType, matchID);
                        //MatchSuccess(matchID);
                    }
                    else
                    {
                        // NOT TODO
                    }
                    break;
                case 2: // 比赛结束
                    matchID = Convert.ToInt32(dataRecvDic["match_id"]);
                    EventManager<MatchEvents>.Instance.TriggerEvent(MatchEvents.GameEndType, matchID);
                    //MatchGameEnd(matchID);
                    break;
                case 3:
                    // 比赛状态
                    try
                    {
                        //{"action":"push","event_id":3,"match_id":28,"time_remains":2,"user_status":{"test-User-0":{"score":0,"hp":100,"finished":false},"test-User-1":{"score":0,"hp":100,"finished":false}}}
                        matchID = Convert.ToInt32(dataRecvDic["match_id"]);
                        int time_remains = Convert.ToInt32(dataRecvDic["time_remains"]);
                        Dictionary<string, object> user_status = dataRecvDic["user_status"] as Dictionary<string, object>;
                    }
                    catch (Exception errorEXstate)
                    {
                        Console.WriteLine($"WebSocketReceiveCallback eventID: {eventID}, error: {errorEXstate.Message}");
                    }
                    break;
                default:
                    Console.WriteLine($"WebSocketReceiveCallback eventID: {eventID}");
                    break;
            }

        }
    }
}
