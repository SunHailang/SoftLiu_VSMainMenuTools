///Auto-generated file, do not modify
///excelName: __excelName__

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace __namespace__
{
    public unsafe partial struct __className__ : IConfigType
    {
        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct __dataName__
        {
__dataFields__
        }

        private static byte* dataPointer { get { Table.TableIndexCheck(TableIndex.__className__); return Table.GetDataPointer(TableIndex.__className__); } }
        public static int Count { get { Table.TableIndexCheck(TableIndex.__className__); return Table.GetConfigCount(TableIndex.__className__); } }

        public bool IsNull { get { return p == null; } }
        public void SetPointer(void* p, int pVersion) { this.p = (__dataName__*)p; this.pVersion = pVersion; }
        private __dataName__* p;
        private int pVersion;
__fields__
    }
}