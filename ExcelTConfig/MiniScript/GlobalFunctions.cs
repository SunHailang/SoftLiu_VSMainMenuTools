using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniScript
{
    public static class GlobalFunctions
    {
        internal struct NativeFunctionInfo
        {
            public string name;
            public NativeFunction func;
        }

        internal static NativeFunctionInfo[] functions =
        {
            new NativeFunctionInfo
            {
                name = "Log",
                func = (ref CallInfo callInfo) =>
                {
                    Console.WriteLine(string.Join(",", callInfo));
                }
            },

            new NativeFunctionInfo
            {
                name = "Abs",
                func = (ref CallInfo callInfo) =>
                {
                    var arg = callInfo[0];
                    if(arg.type == VM.Type.Float)
                    {
                        float v = (float)arg;
                        if(v < 0) arg = -v;
                    }
                    else if(arg.type == VM.Type.Integer)
                    {
                        int v = (int)arg;
                        if(v < 0) arg = -v;
                    }
                    else throw new Exception();
                    callInfo.returnValue = arg;
                }
            },

            new NativeFunctionInfo
            {
                name = "Sqrt",
                func = (ref CallInfo callInfo) =>
                {
                    var arg = callInfo[0];
                    if(arg.type == VM.Type.Float)
                    {
                        arg = (float)Math.Sqrt((float)arg);
                    }
                    else if(arg.type == VM.Type.Integer)
                    {
                        arg = (float)Math.Sqrt((int)arg);
                    }
                    else throw new Exception();
                    callInfo.returnValue = arg;
                }
            },
        };

        public static string[] globalFunctionNames = functions.Select(info => info.name).ToArray();
    }
}
