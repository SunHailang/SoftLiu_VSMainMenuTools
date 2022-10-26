using System;
using System.Collections.Generic;
using System.Text;

namespace TDebug
{
    public interface ILog
    {
        void Log(string msg);
        void Log(string msg, string arg1);
        void Log(string msg, string arg1, string arg2);
        void Log(string msg, string arg1, string arg2, string arg3);
        void Log(string msg, params string[] args);
    }
}
