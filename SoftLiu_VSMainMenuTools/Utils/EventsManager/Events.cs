/// <summary>
/// 
/// __author__ = "sun hai lang"
/// __date__ 2019-07-17
/// 
/// </summary>


namespace SoftLiu_VSMainMenuTools.Utils.EventsManager
{
    public enum NomalEvents
    {
        UpdateAppConfigEvent,
        UpdateVersionEvent,
        UpdateVersionCompleteEvent,
    }

    public enum TCPEvents
    {
        None,
        TrainCheckCodeType,
        TrainLoginType,
        TrainQueryType
    }

    public enum MatchEvents
    {
        None,
        LoginStateType,
        MatchCallbackType,
        GameEndType,
    }
}
