using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MiniScript.OpCodes;

namespace MiniScript
{
    public unsafe static class OpCodeDebuger
    {
        private static void Log(string s) => Console.WriteLine(s);

        public static void PCode(byte[] codes)
        {
            Log($"localVarCount: {codes[0]}");
            int i = 1;
            while(i < codes.Length)
            {
                int next = i + 1;

                var code = (OpCode)codes[i];

                switch(code)
                {
                    case OpCode.LoadArgument: 
                    case OpCode.LoadLocalVariable:
                    case OpCode.StoreArgument:
                    case OpCode.StoreLocalVariable:
                        Log($"{i} {code}: {codes[i + 1]}");
                        next += 1;
                        break;

                    case OpCode.LoadInteger32bit:
                        Log($"{i} {code}: {codes[i + 1] | (codes[i + 2] << 8) | (codes[i + 3] << 16) | (codes[i + 4] << 24) }");
                        next += 4;
                        break;

                    case OpCode.LoadInteger16bit:
                        Log($"{i} {code}: {(short)(codes[i + 1] | (codes[i + 2] << 8)) }");
                        next += 2;
                        break;

                    case OpCode.LoadInteger8bit:
                        Log($"{i} {code}: {(sbyte)codes[i + 1]}");
                        next += 1;
                        break;

                    case OpCode.LoadFloat:
                        {
                            int constI = codes[i + 1] | (codes[i + 2] << 8) | (codes[i + 3] << 16) | (codes[i + 4] << 24);
                            Log($"{i} {code}: {*(float*)&constI}");
                            next += 4;
                        }
                        break;

                    case OpCode.CallGlobal:
                        {
                            int index = codes[i + 1];
                            int argCount = codes[i + 2];
                            Log($"{i} {code}: {index}, {argCount}");
                            next += 2;
                        }
                        break;

                    case OpCode.GoTo:
                    case OpCode.GoToOnTrue:
                    case OpCode.GoToOnFalse:
                        Log($"{i} {code}: {codes[i + 1] | (codes[i + 2] << 8)}");
                        next += 2;
                        break;

                    default: Log($"{i} {code}"); break;
                }
                i = next;
            }
        }
    }
}
