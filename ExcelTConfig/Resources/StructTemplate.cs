///Auto-generated file, do not modify

using System.Runtime.InteropServices;

namespace __namespace__
{
    public unsafe partial struct __className__ : IConfigStruct
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct __dataName__
        {
__dataFields__
        }

        public void SetPointer(byte* p, byte* dataPointer, int pVersion) { this.p = (__dataName__*)p; this.dataPointer = dataPointer; this.pVersion = pVersion; }
        public __className__(byte* p, byte* dataPointer, int pVersion) { this.p = (__dataName__*)p; this.dataPointer = dataPointer; this.pVersion = pVersion; }

        private __dataName__* p;
        private int pVersion;
        private byte* dataPointer;

__fields__
    }
}