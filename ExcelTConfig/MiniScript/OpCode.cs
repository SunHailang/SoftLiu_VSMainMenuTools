using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniScript.OpCodes
{
    public enum OpCode : byte
    {
        None = 0,

        /// <summary>
        /// Loads
        /// </summary>
        LoadArgument,
        LoadLocalVariable,
        LoadGlobalVariable,
        LoadInteger32bit,
        LoadInteger16bit,
        LoadInteger8bit,
        LoadFloat,
        LoadTrue,
        LoadFalse,

        StoreArgument,
        StoreLocalVariable,
        StoreGlobalVariable,

        /// <summary>
        /// Math
        /// </summary>
        Add,
        Substract,
        Multify,
        Divide,
        Mods,
        Negate,
        And,
        Or,
        Not,

        /// <summary>
        /// Compare
        /// </summary>
        LT,
        LE,
        EQ,
        NE,
        GE,
        GT,

        /// <summary>
        /// Function
        /// </summary>
        CallArgument,
        CallGlobal,
        Ret,

        /// <summary>
        /// Stack
        /// </summary>
        Pop,
        Dup,

        /// <summary>
        /// Control
        /// </summary>
        GoTo,
        GoToOnTrue,
        GoToOnFalse,
    }
}
