using System;

namespace TDebug
{
    public static class TDebug
    {
        public static event Action<string> TDebugLogEvent;

        public static event Action<string> TDebugLogWarningEvent;

        public static event Action<string> TDebugLogErrorEvent;
        public static event Action<string> TDebugExceptionEvent;

        public static LogInfo LogInfoData = null;

        public static void InitLogInfo(bool track = true)
        {
            if (track)
            {
                LogInfoData = new LogInfo();
            }
            else
            {
                LogInfoData = null;
            }
        }

        public static void Log(string msg)
        {
            TDebugLogEvent?.Invoke(msg);
        }
        public static void Log(string msg, string arg1)
        {
            msg = string.Format(msg, arg1);
            TDebugLogEvent?.Invoke(msg);
        }
        public static void Log(string msg, string arg1, string arg2)
        {
            msg = string.Format(msg, arg1, arg2);
            TDebugLogEvent?.Invoke(msg);
        }
        public static void Log(string msg, string arg1, string arg2, string arg3)
        {
            msg = string.Format(msg, arg1, arg2, arg3);
            TDebugLogEvent?.Invoke(msg);
        }

        public static void Log(string msg, params string[] args)
        {
            msg = string.Format(msg, args);
            TDebugLogEvent?.Invoke(msg);
        }

    }
}
