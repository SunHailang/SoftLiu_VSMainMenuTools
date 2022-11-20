using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework
{
    public unsafe class NativeFramework
    {
        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void LibFrameworkInit(string rootPath, string tablePath);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void LibFrameworkDestroy();
#if UNITY_EDITOR_OSX
        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void RegisterDebugCallBack(Action<string> LogCInfo);
#endif

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* OpenMMapFile(string alias, string filepath, int state);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseMMapFile(string alias);
    }
}
