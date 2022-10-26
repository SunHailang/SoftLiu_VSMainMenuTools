using System;
using System.Collections.Generic;
using System.Text;

namespace TDebug
{
    public class LogInfo : ILog
    {
        public void Log(string msg)
        {
            
        }

        public void Log(string msg, string arg1)
        {
            throw new NotImplementedException();
        }

        public void Log(string msg, string arg1, string arg2)
        {
            throw new NotImplementedException();
        }

        public void Log(string msg, string arg1, string arg2, string arg3)
        {
            throw new NotImplementedException();
        }

        public void Log(string msg, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
