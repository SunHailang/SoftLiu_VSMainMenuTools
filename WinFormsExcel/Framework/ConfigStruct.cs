using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public enum I18NLanguage : int
    {
        CN = 0,//中文
        EN = 1,//英文
        TW,    //繁体中文
        DE,    //德语
        JP,    //日语
        FR     //法语
    }

    public interface IConfigStruct
    {
        unsafe void SetPointer(byte* p, byte* dataPointer, int pVersion);
    }

    public interface IConfigType
    {
        unsafe void SetPointer(void* p, int pVersion);
        bool IsNull { get; }
        int id { get; }
    }
    public unsafe struct NString : IEnumerable<byte>, IConfigStruct
    {
        private byte* p;
        private int pVersion;
        public int Length { get; private set; }

        public NString(byte* p, int pVersion)
        {
            Length = *(ushort*)p;
            this.p = p + 2;
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* p, byte* dataPointer, int pVersion)
        {
            Length = *(ushort*)p;
            this.p = p + 2;
            this.pVersion = pVersion;
        }

        public override string ToString()
        {
            Table.VersionCheck(pVersion);
            return Encoding.UTF8.GetString(p, Length);
        }

        public byte this[int index]
        {
            get
            {
#if LH_DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, nstring length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return p[index];
            }
        }

        public static implicit operator string(NString ns)
        {
            return ns.ToString();
        }

        public IEnumerator<byte> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct I18NString
    {
        private byte* p;
        private int version;
        private string i18nKlassName;
        private int i18nKlassHash;
        private int id;

        public I18NString(byte* p, int version, string i18nKlassName, int i18nKlassHash, int id)
        {
            this.p = p;
            this.version = version;
            this.i18nKlassName = i18nKlassName;
            this.i18nKlassHash = i18nKlassHash;
            this.id = id;
        }

        public static implicit operator string(I18NString ns)
        {
            return ns.ToString();
        }

        public override string ToString()
        {
#if DEBUG
            Table.VersionCheck(version);
#endif

            return Table.GetI18N(p, i18nKlassName, i18nKlassHash, id);
        }
    }

    public unsafe struct NConfigArray<T> : IEnumerable<T> where T : unmanaged, IConfigType
    {
        private NArray<int> internalArray;
        private int tableIndex;
        public int Length { get => internalArray.Length; }

        public NConfigArray(byte* f, int tableIndex, int pVersion)
        {
            internalArray = new NArray<int>(f, pVersion);
            this.tableIndex = tableIndex;
        }

        public T this[int index]
        {
            get
            {
                //TODO 暂时使用ByID
                return Table.ByID<T>(tableIndex, internalArray[index]);
                //return Table.ByIndex<T>(tableIndex, internalArray[index]);
            }
        }

        public IEnumerator<T> GetEnumerator() { for (int it = 0; it < internalArray.Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < internalArray.Length; it++) yield return this[it]; }
    }

    public unsafe struct NStructArray<T> : IConfigStruct, IEnumerable<T> where T : struct, IConfigStruct
    {
        private int* p;
        private int pVersion;
        public int Length { get; private set; }
        public byte* dataPointer { get; private set; }

        public NStructArray(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.dataPointer = dataPointer;
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.dataPointer = dataPointer;
            this.pVersion = pVersion;
        }

        public T this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, nstructarray length: {Length}");
#endif

                Table.VersionCheck(pVersion);

                var t = default(T);
                t.SetPointer(dataPointer + p[index], dataPointer, pVersion);
                return t;
            }
        }

        public IEnumerator<T> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct NArray<T> : IConfigStruct, IEnumerable<T> where T : unmanaged
    {
        private T* p;
        private int pVersion;
        public int Length { get; private set; }

        public NArray(byte* f, int pVersion)
        {
#if DEBUG
            if (sizeof(T) != sizeof(int)) throw new Exception("narray element size must be 4!");
#endif

            Length = *(int*)f;
            p = (T*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (T*)(f + 4);
            this.pVersion = pVersion;
        }

        public T this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);

                return p[index];
            }
        }

        public IEnumerator<T> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct NUIntArray : IConfigStruct, IEnumerable<uint>
    {
        private uint* p;
        private int pVersion;
        public int Length { get; private set; }

        public NUIntArray(byte* f, int pVersion)
        {
            Length = *(int*)f;
            p = (uint*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (uint*)(f + 4);
            this.pVersion = pVersion;
        }

        public uint this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return (uint)p[index];
            }
        }

        public IEnumerator<uint> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct NBoolArray : IConfigStruct, IEnumerable<bool>
    {
        private int* p;
        private int pVersion;
        public int Length { get; private set; }

        public NBoolArray(byte* f, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public bool this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return p[index] != 0;
            }
        }
        public IEnumerator<bool> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct NShortArray : IConfigStruct, IEnumerable<short>
    {
        private int* p;
        private int pVersion;
        public int Length { get; private set; }

        public NShortArray(byte* f, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public short this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return (short)p[index];
            }
        }

        public IEnumerator<short> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct NUShortArray : IConfigStruct, IEnumerable<ushort>
    {
        private uint* p;
        private int pVersion;
        public int Length { get; private set; }

        public NUShortArray(byte* f, int pVersion)
        {
            Length = *(int*)f;
            p = (uint*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (uint*)(f + 4);
            this.pVersion = pVersion;
        }

        public ushort this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return (ushort)p[index];
            }
        }

        public IEnumerator<ushort> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }
    public unsafe struct NSByteArray : IConfigStruct, IEnumerable<sbyte>
    {
        private int* p;
        private int pVersion;
        public int Length { get; private set; }

        public NSByteArray(byte* f, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public sbyte this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return (sbyte)p[index];
            }
        }

        public IEnumerator<sbyte> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }

    public unsafe struct NByteArray : IConfigStruct, IEnumerable<byte>
    {
        private int* p;
        private int pVersion;
        public int Length { get; private set; }

        public NByteArray(byte* f, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public void SetPointer(byte* f, byte* dataPointer, int pVersion)
        {
            Length = *(int*)f;
            p = (int*)(f + 4);
            this.pVersion = pVersion;
        }

        public byte this[int index]
        {
            get
            {
#if DEBUG
                if ((uint)index >= Length) throw new IndexOutOfRangeException($"index: {index}, narray length: {Length}");
#endif
                Table.VersionCheck(pVersion);
                return (byte)p[index];
            }
        }

        public IEnumerator<byte> GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
        IEnumerator IEnumerable.GetEnumerator() { for (int it = 0; it < Length; it++) yield return this[it]; }
    }
}
