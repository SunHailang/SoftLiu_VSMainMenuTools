using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework
{
    public unsafe partial class Table
    {
        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* TableInit();

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TableShutdown();

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTableIndex(string tableName);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* GetConfigDataPointer(int tableIndex);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* ConfigByID(int tableIndex, int id);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* ConfigByStringID(int tableIndex, IntPtr utf16String, int stringLength);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetConfigCount(int tableIndex);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* ConfigByIndex(int tableIndex, int index);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* ConfigByGroupKey(int tableIndex, int id);

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTableVersion();

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void* GetI18N(int i18nKlassHash, int id);

#if !UNITY_EDITOR && UNITY_ANDROID
        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetLang(int lang, string langName);
#else
        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetLang(int lang, string langName);
#endif

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetLang();

        [DllImport(FrameworkConst.DLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void OpenConfigToolNative(IntPtr L);

        public static bool EverSetup => GetTableVersion() != 0;

        public string ComponentName => nameof(Table);


        public static byte* GetDataPointer(int tableIndex)
        {
            return (byte*)GetConfigDataPointer(tableIndex);
        }

        public static T ByID<T>(int tableIndex, int id) where T : unmanaged, IConfigType
        {
            TableIndexCheck(tableIndex);

            T t = default;

            var p = ConfigByID(tableIndex, id);

            if (p == null)
            {
                Console.WriteLine($"Table index {tableIndex} id notfound: {id}");
            }
            t.SetPointer(p, GetTableVersion());

            return t;
        }

        public static T ByID<T>(int tableIndex, string id) where T : unmanaged, IConfigType
        {
            TableIndexCheck(tableIndex);

            T t = default;
            if (id == null) return t;

            fixed (char* pointer = id)
            {
                t.SetPointer(ConfigByStringID(tableIndex, (IntPtr)pointer, id.Length), GetTableVersion());
            }

            return t;
        }

        public static T ByIndex<T>(int tableIndex, int index) where T : unmanaged, IConfigType
        {
            TableIndexCheck(tableIndex);

            T t = default;
            t.SetPointer(ConfigByIndex(tableIndex, index), GetTableVersion());
            return t;
        }

        public static NConfigArray<T> ByGroupKey<T, V>(int tableIndex, V value) where T : unmanaged, IConfigType where V : unmanaged
        {
            TableIndexCheck(tableIndex);

            byte* p = (byte*)ConfigByGroupKey(tableIndex, *(int*)&value);

            return new NConfigArray<T>(p, tableIndex, GetTableVersion());
        }

        public static void SetI18NLanguage(I18NLanguage language)
        {
            var keepStr = "i18n_" + language.ToString();
            Console.WriteLine("SetI18NLanguage:" + keepStr);
#if !UNITY_EDITOR && UNITY_ANDROID
            var ret = SetLang((int)language, keepStr);
            act.debug.PrintSystem.Log($"SetI18NLanguage: {keepStr} ret {ret}");
#else
            SetLang((int)language, keepStr);
#endif

        }

        public static I18NLanguage GetI18NLanguage()
        {
            return (I18NLanguage)GetLang();
        }

        public static string GetI18N(byte* zh, string i18nKlassName, int i18nKlassHash, int id)
        {
            byte* str;
            if (GetLang() == 0) str = zh;
            else str = (byte*)GetI18N(i18nKlassHash, id);
            if (str == null) return $"<i18n null: {i18nKlassName} {id}>";
            int len = *(ushort*)str;
            return System.Text.Encoding.UTF8.GetString(str + sizeof(ushort), len);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void PointerCheck(void* p, int pVersion)
        {
#if UNITY_EDITOR
                if (p == null) throw new NullReferenceException();
                if (pVersion != GetTableVersion()) throw new Exception("Version不匹配，table已销毁或重新建立，该指针可能是野指针");
#endif
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void VersionCheck(int pVersion)
        {
#if UNITY_EDITOR
                if (pVersion != GetTableVersion()) throw new Exception("Version不匹配，table已销毁或重新建立，该指针可能是野指针");
#endif
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void TableIndexCheck(int tableIndex)
        {
#if UNITY_EDITOR
            if(tableIndex == 0) throw new Exception("table index should not be zero!");
#endif
        }

        public Table()
        {
            var ptr = TableInit();
            if (ptr != null) throw new Exception(Marshal.PtrToStringAnsi((IntPtr)ptr));
            TableIndex.SetData();
        }
        public static void LogTableData<T>() where T : unmanaged, IConfigType
        {
            var indexProperty = typeof(TableIndex).GetProperty(typeof(T).Name, System.Reflection.BindingFlags.GetProperty
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Static);
            if (indexProperty == null)
            {
                Console.WriteLine("Table could not find tableindex");
                return;
            }

            int tableIndex = (int)indexProperty.GetMethod.Invoke(null, null);

            TableIndexCheck(tableIndex);

            int count = GetConfigCount(tableIndex);
            Console.WriteLine($"Table { typeof(T).Name} count: {count}");

            var properties = typeof(T).GetProperties(
                System.Reflection.BindingFlags.GetProperty
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            var sb = new System.Text.StringBuilder();
            for (int it = 0; it < count; it++)
            {
                var config = ByIndex<T>(tableIndex, it);

                sb.Clear();
                sb.Append($"{typeof(T).Name} {it} - ");
                bool first = true;
                foreach (var property in properties)
                {
                    if (first) first = false;
                    else sb.Append(", ");
                    sb.Append(property.Name).Append(" : ");

                    var value = property.GetMethod.Invoke(config, null);
                    AppendLogValue(value, sb);
                }
                Console.WriteLine($"Table {sb.ToString()}");
            }
        }

        private static void AppendLogValue(object value, System.Text.StringBuilder sb)
        {
            if (value == null)
            {
                sb.Append("null");
                return;
            }

            var type = value.GetType();
            if (type.IsPrimitive)
            {
                sb.Append(value.ToString());

                return;
            }

            if (type.IsSubclassOf(typeof(Enum)))
            {
                sb.Append(value.ToString());
                return;
            }

            if (type == typeof(NString))
            {
                sb.Append($"\"{value.ToString()}\"");
                return;
            }

            if (value is IEnumerable em)
            {
                sb.Append("[");
                bool first = true;
                foreach (var element in em)
                {
                    if (first) first = false;
                    else sb.Append(", ");

                    AppendLogValue(element, sb);
                }
                sb.Append("]");
                return;
            }

            if (value is IConfigStruct)
            {
                var properties = type.GetProperties(
                System.Reflection.BindingFlags.GetProperty
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

                bool first = true;
                sb.Append("{");
                foreach (var property in properties)
                {
                    if (first) first = false;
                    else sb.Append(", ");
                    sb.Append(property.Name).Append(" : ");

                    var childValue = property.GetMethod.Invoke(value, null);
                    AppendLogValue(childValue, sb);
                }
                sb.Append("}");
                return;
            }

            if (value is IConfigType config)
            {
                sb.Append(type.ToString());

                if (config.IsNull) sb.Append("<null>");
                else sb.Append("<").Append(config.id).Append(">");
                return;
            }

            sb.Append(value.ToString());
        }

        internal void Disable()
        {
            TableShutdown();
        }

        internal void Reset()
        {

        }

#if UNITY_EDITOR
        public static void EditorAction(Action action)
        {
            LibFramework libFramework = null;
            Table table = null;
            if (!EverSetup)
            {
                libFramework = new LibFramework();
                table = new Table();
            }

            try
            {
                action();
            }
            finally
            {
                if (table != null) table.Disable();
                if (libFramework != null) libFramework.Disable();
            }
        }

        public static T[] EditorCopy<T, K>() where T : new() where K : unmanaged, IConfigType
        {
            T[] ret = null;
            EditorAction(() =>
            {
                var tableIndex = (int)typeof(TableIndex).GetProperty(typeof(K).Name).GetMethod.Invoke(null, null);

                int count = GetConfigCount(tableIndex);
                ret = new T[count];

                for (int it = 0; it < count; it++)
                {
                    ret[it] = EditorCopyOne<T, K>(ByIndex<K>(tableIndex, it));
                }
            });
            return ret;
        }

        public static T EditorCopyOne<T, K>(K from) where T : new() where K : struct, IConfigType
        {
            var toType = typeof(T);
            var fromType = typeof(K);
            object to = Activator.CreateInstance<T>();

            foreach (var fromProperty in fromType.GetProperties(System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty))
            {
                var toField = toType.GetField(fromProperty.Name, System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Instance);

                if (toField == null) continue;

                var fromPropertyValue = fromProperty.GetMethod.Invoke(from, null);

                object toFieldValue;
                if (!EditorCopyOneInternal(fromProperty.PropertyType, fromPropertyValue, toField.FieldType, out toFieldValue)) continue;

                toField.SetValue(to, toFieldValue);
            }

            return (T)to;
        }

        private static bool EditorCopyOneInternal(Type fromType, object from, Type toType, out object to)
        {
            to = null;

            if(fromType.IsPrimitive || fromType.IsEnum)
            {
                if(toType == fromType)
                {
                    to = from;
                    return true;
                }
                return false;
            }

            if (fromType == typeof(NString))
            {
                if (toType == typeof(string))
                {
                    to = from.ToString();
                    return true;
                }
                return false;
            }

            if(from is IConfigType config)
            {
                if(toType == typeof(int))
                {
                    to = config.IsNull ? -1 : config.id;
                    return true;
                }

                return false;
            }

            if(from is IEnumerable emFrom)
            {
                if (!toType.IsArray) return false;
                var toElementType = toType.GetElementType();
                var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(toElementType)) as IList;
                foreach(var fromElement in emFrom)
                {
                    if (fromElement == null) return false;

                    object toElement;
                    if (!EditorCopyOneInternal(fromElement.GetType(), fromElement, toElementType, out toElement)) return false;

                    list.Add(toElement);
                }

                var array = Array.CreateInstance(toElementType, list.Count);
                list.CopyTo(array, 0);
                to = array;

                return true;
            }

            if(from is IConfigStruct configStruct)
            {
                if (toType.Name != fromType.Name) return false;

                to = Activator.CreateInstance(toType);

                foreach(var fromProperty in fromType.GetProperties(System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty))
                {
                    var toField = toType.GetField(fromProperty.Name, System.Reflection.BindingFlags.Public
                        | System.Reflection.BindingFlags.Instance);

                    if (toField == null) continue;

                    var fromPropertyValue = fromProperty.GetMethod.Invoke(from, null);

                    object toFieldValue;
                    if (!EditorCopyOneInternal(fromProperty.PropertyType, fromPropertyValue, toField.FieldType, out toFieldValue)) continue;

                    toField.SetValue(to, toFieldValue);
                }

                return true;
            }

            return false;
        }
#endif
    }
}
