using ExcelTConfig.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ExcelTConfig
{
    public static class I18NHandlerBinary
    {
        private static BinaryWriter writer;
        private static MemoryStream stream;

        private static int vPos;

        private static byte[] stringValueBuffer = new byte[4096];
        private static Dictionary<string, int> stringCache = new Dictionary<string, int>();

        private static Dictionary<int, int> idTable = new Dictionary<int, int>();

        [StructLayout(LayoutKind.Sequential)]
        private struct Header
        {
            public int zero;
            public int idTableOffset;
        }

        private static Header header;

        public static void ExportBinary()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);

            //无需导出中文
            for (int it = 1; it < Entry.LangCount; it++)
            {
                stringCache.Clear();
                ExportBinary(it);
            }

            stream = null;
            writer = null;
            stringCache.Clear();
        }
        private static void ExportBinary(int langIndex)
        {
            stream.Position = 0L;
            header = new Header();
            idTable.Clear();
            Util.WriteStructure(writer, header);
            foreach (var kv in Entry.i18NKlasses)
            {
                var i18nKlass = kv.Value;
                if (i18nKlass.data == null || i18nKlass.data.Count == 0) continue;

                foreach (var ir in i18nKlass.data)
                {
                    int id = ir.Key;
                    string value = ir.Value[langIndex];

                    int cacheIndex;
                    if (string.IsNullOrEmpty(value) || stringCache.TryGetValue(value, out cacheIndex)) continue;

                    int byteCount = Encoding.UTF8.GetByteCount(value);
                    if (byteCount > stringValueBuffer.Length) throw new ConfigException("corner case: string too long");
                    Encoding.UTF8.GetBytes(value, 0, value.Length, stringValueBuffer, 0);
                    stringCache[value] = (int)stream.Position;

                    writer.Write((ushort)byteCount);
                    writer.Write(stringValueBuffer, 0, byteCount);
                }
            }

            var subTable = new Dictionary<int, int>();
            foreach (var kv in Entry.i18NKlasses)
            {
                var i18nKlass = kv.Value;
                if (i18nKlass.data == null || i18nKlass.data.Count == 0) continue;

                subTable.Clear();
                foreach (var ir in i18nKlass.data)
                {
                    int id = ir.Key;
                    string value = ir.Value[langIndex];

                    int valueOffset;
                    if (string.IsNullOrEmpty(value)) valueOffset = 0;
                    else valueOffset = stringCache[value];

                    subTable[id] = valueOffset;
                }

                idTable[i18nKlass.hashCode] = (int)stream.Position;
                Util.WriteIDTable(writer, subTable);
            }

            header.idTableOffset = (int)stream.Position;
            Util.WriteIDTable(writer, idTable);

            WriteBack(0, () =>
            {
                Util.WriteStructure(writer, header);
            });

            if (Entry.ExportPack)
            {
                using (var fileStream = new FileStream(Path.Combine(Entry.BinaryPackOnePath, Entry.LangPrefix + Entry.Lang[langIndex] + ".bytes"), FileMode.Create))
                {
                    stream.WriteTo(fileStream);
                }
            }

            else
            {
                using (var fileStream = new FileStream(Path.Combine(Entry.BinaryFolderPath, Entry.LangPrefix + Entry.Lang[langIndex] + ".bytes"), FileMode.Create))
                {
                    stream.WriteTo(fileStream);
                }
            }
        }
        private static void WriteVContent(Action action)
        {
            writer.Write(vPos);
            long currentPosition = stream.Position;
            stream.Position = vPos;
            action();

            int cvPos = (int)stream.Position;
            if (cvPos > vPos) vPos = cvPos;

            stream.Position = currentPosition;
        }

        private static void EnsureLength(int length, Action action)
        {
            int p = (int)stream.Position + length;
            if (p > vPos) vPos = p;
            action();
            if (stream.Position < vPos) stream.Position = vPos;
        }

        private static void WriteBack(long backPosition, Action action)
        {
            long currentPosition = stream.Position;
            stream.Position = backPosition;
            action();
            stream.Position = currentPosition;
        }
    }
}
