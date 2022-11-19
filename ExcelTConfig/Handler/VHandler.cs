using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using ExcelTConfig.Base;

namespace ExcelTConfig
{
    public static class VHandler
    {
        public class VKlass
        {
            public VKlass(string name) { this.name = name; }

            public string name { get; private set; }
            public List<VProperty> properties { get; private set; } = new List<VProperty>();
            public List<object> values { get; private set; } = new List<object>();

            public int CalculateBinarySize()
            {
                return 4 * properties.Count;
            }
        }

        public class VProperty
        {
            public string type { get; }
            public string name { get; }
            public string description { get; }
            internal Property.BasicType basicType { get; private set; }
            public bool isArray { get; }
            public bool isFunctor => functorArguments != null;
            public string[] functorArguments { get; private set; }

            public VProperty(string type, string name, string description)
            {
                if(type.StartsWith(FunctorPrefix) && type.EndsWith(FunctorPostfix))
                {
                    var argContent = type.Substring(FunctorPrefix.Length, type.Length - FunctorPrefix.Length - FunctorPostfix.Length);
                    functorArguments = argContent.Split(',').Select(a => a.Trim()).ToArray();

                    if (functorArguments.Any(a => !Util.IsValidName(a))) throw new ConfigException($"error type: {type}");

                    type = "string";
                }

                if (type.EndsWith("[]"))
                {
                    isArray = true;
                    type = type.Substring(0, type.Length - "[]".Length);
                }

                this.type = type;
                this.name = name;
                this.description = description;
                CheckBasicType();
            }

            private void CheckBasicType()
            {
                if (type == "int") basicType = Property.BasicType.Integer;
                else if (type == "float") basicType = Property.BasicType.Float;
                else if (type == "string") basicType = Property.BasicType.String;
                else throw new ConfigException("should not reach here");
            }
        }

        public static Dictionary<string, VKlass> vKlasses;
        private const int VDescriptionCol = 1, VKeyCol = 2, VTypeCol = 3, VValueCol = 4, VContentRow = 2;
        private static string GetAddress(int row, int column) => ExcelCellBase.GetAddress(row, column);
        private static HashSet<string> validTypes = new HashSet<string>()
        {
            "int", "float", "string", "int[]", "float[]", "string[]",
        };
        private const string FunctorPrefix = "Functor<";
        private const string FunctorPostfix = ">";
        private static PEG.Pattern functorPattern;

        private static object ParseValue(VProperty property, ExcelRange cell)
        {
            var type = property.type;

            if (property.isArray)
            {
                var str = cell.GetValue<string>();

                if (string.IsNullOrEmpty(str)) return null;
                JArray jArray = JArray.Parse(str);
                switch (property.basicType)
                {
                    case Property.BasicType.Float: return jArray.Select(t => (float)t).ToArray();
                    case Property.BasicType.Integer: return jArray.Select(t => (int)t).ToArray();
                    case Property.BasicType.String: return jArray.Select(t => (string)t).ToArray();
                }
            }
            else
            {
                string stringValue = cell.GetValue<string>();
                if (string.IsNullOrEmpty(stringValue))
                {
                    if (property.basicType != Property.BasicType.String) throw new Exception();
                }

                switch (property.basicType)
                {
                    case Property.BasicType.Float: return float.Parse(stringValue);
                    case Property.BasicType.String:
                        {
                            if(property.isFunctor)
                            {
                                if (functorPattern == null) functorPattern = MiniScript.Parser.BuildPattern();

                                var localVarNames = property.functorArguments;

                                try
                                {
                                    var result = functorPattern.Match(stringValue);

                                    //var pyEngine = new MiniScript.PythonEngine(MiniScript.GlobalFunctions.globalFunctionNames, localVarNames);
                                    //var ret = pyEngine.Emit(stringValue, result.Select(o => (MiniScript.Nodes.Node)o).ToArray());
                                    //Console.WriteLine(ret);

                                    var engine = new MiniScript.OpCodes.OpCodeEngine(MiniScript.GlobalFunctions.globalFunctionNames, localVarNames);
                                    return engine.Emit(stringValue, result.Select(o => (MiniScript.Nodes.Node)o).ToArray());
                                }
                                catch (Exception e)
                                {
                                    throw new ConfigException(e.Message);
                                }
                            }
                            else return stringValue;
                        }
                    case Property.BasicType.Integer: return int.Parse(stringValue);
                }
            }

            throw new ConfigException("should not reach here");
        }

        public static void ExtractData()
        {
            if (string.IsNullOrEmpty(Entry.VExcelFolderPath)) return;
            vKlasses = new Dictionary<string, VKlass>();

            foreach (var f in Directory.GetFiles(Entry.VExcelFolderPath, "*.xls?", SearchOption.AllDirectories))
            {
                //skip hidden excel
                if ((File.GetAttributes(f) & FileAttributes.Hidden) != 0) continue;
                if (!File.Exists(f))
                {
                    Entry.UpdateLogInfo($"ExtractData File:{f} not exists.", LogLevelType.ErrorType);
                    continue;
                }
                using (var fs = new FileStream(f, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(fs))
                    {
                        ExtractExcelData(excelPackage, Path.GetFileNameWithoutExtension(f));
                    }
                }
            }
        }

        private static void ExtractExcelData(ExcelPackage excelPackage, string excelFileName)
        {
            var vKlass = new VKlass(excelFileName);
            if (!Util.IsValidName(vKlass.name)) throw new ConfigException($"invalid klass name [{vKlass.name}]");
            if (Entry.klasses.ContainsKey(vKlass.name) || vKlasses.ContainsKey(vKlass.name)) throw new ConfigException($"vklass name [{vKlass.name}] confict");
            vKlasses[vKlass.name] = vKlass;

            var workbook = excelPackage.Workbook;
            if (workbook.Worksheets.Count == 0)
            {
                throw new ConfigException($"excel [{excelFileName}] has no worksheet");
            }

            var dataSheet = workbook.Worksheets[1];
            var cells = dataSheet.Cells;

            for (int it = VContentRow; ; it++)
            {
                string key = cells[it, VKeyCol].GetValue<string>();
                if (string.IsNullOrEmpty(key)) break;
                if (!Util.IsValidName(key)) throw new ConfigException($"invalid key [{key}] in <{excelFileName}!{GetAddress(it, VKeyCol)}>");

                string type = cells[it, VTypeCol].GetValue<string>();
                if (!validTypes.Contains(type) && !(type.StartsWith(FunctorPrefix) && type.EndsWith(FunctorPostfix))) throw new ConfigException($"invalid type [{type}] in <{excelFileName}!{GetAddress(it, VTypeCol)}>");

                string description = cells[it, VDescriptionCol].GetValue<string>();

                var valueCell = cells[it, VValueCol];
                var property = new VProperty(type, key, description);
                object value;
                try
                {
                    value = ParseValue(property, valueCell);
                }
                catch (Exception e)
                {
                    throw new ConfigException($"invalid cell content: [{valueCell.GetValue<string>()}] in <{excelFileName}!{GetAddress(it, VValueCol)}> {e}");
                }

                vKlass.properties.Add(property);
                vKlass.values.Add(value);
            }
        }

        private static BinaryWriter writer;
        private static MemoryStream stream;

        private static Dictionary<string, int> stringCache;

        [StructLayout(LayoutKind.Sequential)]
        struct Header
        {
            private int zero;
            public int count;
            public int keyTableOffset;
        }
        private static Header header = new Header();

        [StructLayout(LayoutKind.Sequential)]
        struct MainHeader
        {
            private int zero;
            public int count;
        }

        public static void Export()
        {
            ExtractData();
            if (vKlasses == null || vKlasses.Count == 0) return;
            if (!Directory.Exists(Entry.BinaryFolderPath)) throw new ConfigException("Binary Folder Not Exists");

            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            stringCache = new Dictionary<string, int>();

            //ExportMain();

            foreach (var kv in vKlasses)
            {
                var vKlass = kv.Value;

                stream.SetLength(0L);
                stream.Position = 0L;
                stringCache.Clear();

                ExportVKlass(vKlass);
            }

            stream = null;
            writer = null;
            stringCache = null;
        }

        public static Dictionary<string, int> ExportToPack(MemoryStream packStream)
        {
            var ret = new Dictionary<string, int>();
            ExtractData();
            if (vKlasses == null || vKlasses.Count == 0) return ret;
            if (!Directory.Exists(Entry.BinaryPackOnePath)) throw new ConfigException("Binary Folder Not Exists");

            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            stringCache = new Dictionary<string, int>();

            foreach (var kv in vKlasses)
            {
                var vKlass = kv.Value;

                stream.SetLength(0L);
                stream.Position = 0L;
                stringCache.Clear();

                ret[vKlass.name] = (int)packStream.Position;
                ExportVKlass(vKlass, packStream);

                int pos = (int)packStream.Position;
                //4byte对齐
                if (pos % 4 != 0)
                {
                    pos = ((pos / 4) + 1) * 4;
                    packStream.Position = pos;
                }
            }

            stream = null;
            writer = null;
            stringCache = null;
            return ret;
        }

        private static void ExportMain()
        {
            var header = new MainHeader { count = vKlasses.Count };

            Util.WriteStructure(writer, header);

            foreach (var kv in vKlasses)
            {
                var vKlass = kv.Value;

                WriteStringRaw(vKlass.name);
            }

            using (var fileStream = new FileStream(Path.Combine(Entry.BinaryFolderPath, "vmain.bytes"), FileMode.Create))
            {
                stream.WriteTo(fileStream);
            }
        }

        private static void PutToStringCache(string content)
        {
            if (content == null) content = string.Empty;
            if (stringCache.ContainsKey(content)) return;
            stringCache[content] = (int)stream.Position;
            WriteStringRaw(content);
        }

        private static int GetFromStringCache(string content)
        {
            if (content == null) content = string.Empty;
            return stringCache[content];
        }

        private static void ExportVKlass(VKlass vKlass, MemoryStream packStream = null)
        {
            int count = vKlass.properties.Count;

            header = new Header { count = count };
            Util.WriteStructure(writer, header);

            //先一遍写入所有string
            for (int it = 0; it < count; it++)
            {
                var property = vKlass.properties[it];
                PutToStringCache(property.name);

                var value = vKlass.values[it];
                if (value is string s) PutToStringCache(s);
                else if (value is string[] sa) foreach (var ss in sa) PutToStringCache(ss);
            }

            Do4ByteAlign();

            var tableItems = new List<Util.TableItem>();

            for (int it = 0; it < count; it++)
            {
                var property = vKlass.properties[it];
                var tableItem = new Util.TableItem { hash = property.name.AscIIHash(), key = GetFromStringCache(property.name) };

                tableItem.value = (int)stream.Position;

                writer.Write((int)property.basicType | (property.isArray ? 0x80 : 0));
                WriteValue(property, vKlass.values[it]);

                tableItems.Add(tableItem);
            }

            header.keyTableOffset = (int)stream.Position;
            Util.WriteIDTable(writer, tableItems.GetEnumerator(), tableItems.Count);

            WriteBack(0, () => Util.WriteStructure(writer, header));

            if(packStream != null)
            {
                stream.WriteTo(packStream);
            }
            else
            {
                using (var fileStream = new FileStream(Path.Combine(Entry.BinaryFolderPath, vKlass.name + ".bytes"), FileMode.Create))
                {
                    stream.WriteTo(fileStream);
                }
            }
        }

        private static void WriteValue(VProperty property, object value, bool ignoreArray = false)
        {
            if (property.isArray && !ignoreArray)
            {
                var array = value as Array;

                if (array == null || array.Length == 0)
                {
                    writer.Write(0);
                    return;
                }

                int arrayLength = array.Length;
                writer.Write(arrayLength);
                for (int it = 0; it < arrayLength; it++) WriteValue(property, array.GetValue(it), true);
            }
            else
            {
                switch (property.basicType)
                {
                    case Property.BasicType.Integer: writer.Write((int)value); break;
                    case Property.BasicType.Float: writer.Write((float)value); break;
                    case Property.BasicType.String:
                        {
                            if(property.isFunctor)
                            {
                                writer.Write(4 + (int)stream.Position);
                                var bytes = (byte[])value;
                                writer.Write((UInt16)bytes.Length);
                                writer.Write(bytes);
                                Do4ByteAlign();
                            }
                            else writer.Write(GetFromStringCache((string)value));
                        }
                        break;
                }
            }
        }

        private static void Do4ByteAlign()
        {
            int cvPos = (int)stream.Position;
            //4byte对齐
            if (cvPos % 4 != 0)
            {
                cvPos = ((cvPos / 4) + 1) * 4;
                stream.Position = cvPos;
            }
        }

        private static void WriteStringRaw(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            writer.Write((UInt16)bytes.Length);
            writer.Write(bytes);
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
