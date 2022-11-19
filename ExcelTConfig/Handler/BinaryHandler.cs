using ExcelTConfig.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ExcelTConfig
{
    public static class BinaryHandler
    {
        private static BinaryWriter writer;
        private static MemoryStream stream;

        private static BinaryWriter packWriter;
        private static MemoryStream packStream;

        private static int vPos;

        private static byte[] stringValueBuffer = new byte[4096];
        private static Dictionary<string, int> stringCache = new Dictionary<string, int>();

        [StructLayout(LayoutKind.Sequential)]
        public struct MainFileHeader
        {
            public int zero;
            public int count;
            public int maxLen;
            public int packInfoOffset;
            public int vmapOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Header
        {
            public int zero;
            public int width;
            public int count;
            public int dataOffset;
            public int idTableOffset;
            public int nameTableOffset;
            public int groupKeyOffset;
        }

        private static Header header;

        public static void Export()
        {
            if (Entry.ExportPack)
            {
                if (string.IsNullOrEmpty(Entry.BinaryPackOnePath)) throw new ConfigException(ErrorMessage.JsonConfigFileNoXXXXField, "Binary Folder Path");
                if (!Directory.Exists(Entry.BinaryPackOnePath)) throw new ConfigException(ErrorMessage.FolderNotExist, "Binary");
            }
            else
            {
                if (string.IsNullOrEmpty(Entry.BinaryFolderPath)) throw new ConfigException(ErrorMessage.JsonConfigFileNoXXXXField, "Binary Folder Path");
                if (!Directory.Exists(Entry.BinaryFolderPath)) throw new ConfigException(ErrorMessage.FolderNotExist, "Binary");
            }

            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
            vPos = 0;

            foreach (var kv in Entry.klasses) kv.Value.CalculateBinarySize();

            if (Entry.ExportPack)
            {
                packStream = new MemoryStream();
                packWriter = new BinaryWriter(packStream);

                ExportPack();
            }
            else
            {
                ExportMain();

                foreach (var kv in Entry.klasses)
                {
                    var klass = kv.Value;

                    stream.SetLength(0L);
                    stream.Position = 0L;
                    stringCache.Clear();
                    vPos = 0;

                    if (klass.type == Klass.KlassType.Config || klass.type == Klass.KlassType.Static) ExportOne(klass);
                }
            }

            stream = null;
            writer = null;
            packStream = null;
            packWriter = null;
        }
        private static void ExportMain()
        {
            var header = new MainFileHeader { zero = 0, maxLen = 1, packInfoOffset = 0 };

            Util.WriteStructure(writer, header);
            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;
                if (klass.type != Klass.KlassType.Config && klass.type != Klass.KlassType.Static) continue;

                int order = Entry.klassOrders[klass.name];

                var bytes = Encoding.UTF8.GetBytes(klass.name);
                writer.Write((UInt16)bytes.Length);
                writer.Write(bytes);
                writer.Write(order);

                header.count++;
                header.maxLen = Math.Max(header.maxLen, order + 1);
            }
            WriteBack(0L, () => Util.WriteStructure(writer, header));

            using (var fileStream = new FileStream(Path.Combine(Entry.BinaryFolderPath, "Main.bytes"), FileMode.Create))
            {
                stream.WriteTo(fileStream);
            }
        }

        private static void ExportPack()
        {
            var header = new MainFileHeader { zero = 0, maxLen = 1 };

            Util.WriteStructure(packWriter, header);

            var klassOffsets = new Dictionary<string, int>();

            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;

                stream.SetLength(0L);
                stream.Position = 0L;
                stringCache.Clear();
                vPos = 0;

                if (klass.type == Klass.KlassType.Config || klass.type == Klass.KlassType.Static)
                {
                    klassOffsets[klass.name] = (int)packStream.Position;
                    ExportOne(klass);

                    int pos = (int)packStream.Position;
                    //4byte对齐
                    if (pos % 4 != 0)
                    {
                        pos = ((pos / 4) + 1) * 4;
                        packStream.Position = pos;
                    }
                }
            }

            //vklass
            var vKlassOffsets = VHandler.ExportToPack(packStream);
            var vKlassItems = new List<Util.TableItem>();
            foreach (var kv in vKlassOffsets)
            {
                string vKlassName = kv.Key;
                int offset = kv.Value;

                vKlassItems.Add(new Util.TableItem { hash = vKlassName.AscIIHash(), key = (int)packStream.Position, value = offset });
                var bytes = Encoding.UTF8.GetBytes(vKlassName);
                packWriter.Write((UInt16)bytes.Length);
                packWriter.Write(bytes);
            }
            {
                int pos = (int)packStream.Position;
                //4byte对齐
                if (pos % 4 != 0)
                {
                    pos = ((pos / 4) + 1) * 4;
                    packStream.Position = pos;
                }
            }
            header.vmapOffset = (int)packStream.Position;
            Util.WriteIDTable(packWriter, vKlassItems.GetEnumerator(), vKlassItems.Count);

            header.packInfoOffset = (int)packStream.Position;

            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;
                if (klass.type != Klass.KlassType.Config && klass.type != Klass.KlassType.Static) continue;

                int order = Entry.klassOrders[klass.name];

                var bytes = Encoding.UTF8.GetBytes(klass.name);
                packWriter.Write((UInt16)bytes.Length);
                packWriter.Write(bytes);
                packWriter.Write(order);
                packWriter.Write(klassOffsets[klass.name]);

                header.count++;
                header.maxLen = Math.Max(header.maxLen, order + 1);
            }

            long currentPosition = packStream.Position;
            packStream.Position = 0L;
            Util.WriteStructure(packWriter, header);
            packStream.Position = currentPosition;

            using (var fileStream = new FileStream(Path.Combine(Entry.BinaryPackOnePath, "Main.bytes"), FileMode.Create))
            {
                packStream.WriteTo(fileStream);
            }
        }
        private static void ExportOne(Klass klass)
        {
            header = new Header();
            Util.WriteStructure(writer, header);
            header.count = klass.data.Count;
            header.width = klass.binarySize;
            header.dataOffset = (int)stream.Position;

            EnsureLength(klass.data.Count * klass.binarySize, () =>
            {
                foreach (var rowData in klass.data)
                {
                    int index = 0;
                    foreach (var property in klass)
                    {
                        index = HandleProperty(rowData, klass, property, index);
                    }
                }
            });

            ExportIDTable(klass);
            ExportNameTable(klass);
            ExportGroupKeyTable(klass);

            WriteBack(0L, () => Util.WriteStructure(writer, header));

            if (Entry.ExportPack)
            {
                stream.WriteTo(packStream);
            }
            else
            {
                WriteFile(klass);
            }
        }
        private static int HandleProperty(object[] rowData, Klass ownerKlass, Property property, int index, bool ignoreArray = false)
        {
            int ret = index + 1;

            if (property.isArray && !ignoreArray)
            {
                int width, binarySize;
                if (!property.isBasicType && property.refKlass.type == Klass.KlassType.Struct)
                {
                    width = property.refKlass.width;
                    //binarySize = property.refKlass.binarySize;
                    binarySize = 4;
                }
                else
                {
                    width = 1;
                    binarySize = 4;
                }

                int length = 0;
                for (; length < property.arrayLength; length++)
                {
                    if (rowData[index + length * width] == null) break;
                }

                if (length == 0) writer.Write(0);
                else
                {
                    WriteVContent(() =>
                    {
                        writer.Write(length);
                        EnsureLength(binarySize * length, () =>
                        {
                            for (int it = 0, pIndex = index; it < length; it++) pIndex = HandleProperty(rowData, ownerKlass, property, pIndex, true);
                        });
                    });
                }

                ret = index + width * property.arrayLength;
            }
            else if (property.isBasicType)
            {
                object value = rowData[index];

                if (property.isBasicArray)
                {
                    var array = value as Array;
                    int binarySize = 4;
                    if (array == null || array.Length == 0)
                    {
                        writer.Write(0);
                    }
                    else if (property.isBasic1DArray)
                    {
                        WriteVContent(() =>
                        {
                            writer.Write(array.Length);
                            EnsureLength(binarySize * array.Length, () =>
                            {
                                switch (property.basicType)
                                {
                                    case Property.BasicType.Integer:
                                        foreach (var v in array as int[]) writer.Write(v);
                                        break;

                                    case Property.BasicType.Float:
                                        foreach (var v in array as float[]) writer.Write(v);
                                        break;

                                    case Property.BasicType.String:
                                        foreach (var v in array as string[]) HandleString(v);
                                        break;
                                    case Property.BasicType.UShort:
                                        foreach (var v in array as ushort[]) writer.Write(v);
                                        break;
                                    case Property.BasicType.Short:
                                        foreach (var v in array as short[]) writer.Write(v);
                                        break;
                                    case Property.BasicType.UInt:
                                        foreach (var v in array as uint[]) writer.Write(v);
                                        break;
                                    case Property.BasicType.Byte:
                                        foreach (var v in array as byte[]) writer.Write(v);
                                        break;
                                }
                            });
                        });
                    }
                    else
                    {
                        WriteVContent(() =>
                        {
                            writer.Write(array.Length);
                            EnsureLength(binarySize * array.Length, () =>
                            {
                                switch (property.basicType)
                                {
                                    case Property.BasicType.Integer:
                                        foreach (var va in array as int[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    foreach (var v in va) writer.Write(v);
                                                });
                                            }
                                        }
                                        break;

                                    case Property.BasicType.Float:
                                        foreach (var va in array as float[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    foreach (var v in va) writer.Write(v);
                                                });
                                            }
                                        }
                                        break;

                                    case Property.BasicType.String:
                                        foreach (var va in array as string[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    EnsureLength(binarySize * va.Length, () =>
                                                    {
                                                        foreach (var v in va) HandleString(v);
                                                    });
                                                });
                                            }
                                        }
                                        break;
                                    case Property.BasicType.UInt:
                                        foreach (var va in array as uint[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    foreach (var v in va) writer.Write((uint)v);
                                                });
                                            }
                                        }
                                        break;
                                    case Property.BasicType.UShort:
                                        foreach (var va in array as ushort[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    foreach (var v in va) writer.Write((uint)v);
                                                });
                                            }
                                        }
                                        break;
                                    case Property.BasicType.Short:
                                        foreach (var va in array as short[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    foreach (var v in va) writer.Write((int)v);
                                                });
                                            }
                                        }
                                        break;
                                    case Property.BasicType.Byte:
                                        foreach (var va in array as byte[][])
                                        {
                                            if (va == null || va.Length == 0) writer.Write(0);
                                            else
                                            {
                                                WriteVContent(() =>
                                                {
                                                    writer.Write(va.Length);
                                                    foreach (var v in va) writer.Write((int)v);
                                                });
                                            }
                                        }
                                        break;
                                }
                            });
                        });
                    }
                }
                else
                {
                    switch (property.basicType)
                    {
                        case Property.BasicType.Boolean: writer.Write(true.Equals(value) ? 1 : 0); break;
                        case Property.BasicType.Float: writer.Write(value == null ? 0f : (float)value); break;
                        case Property.BasicType.Integer: writer.Write(value == null ? 0 : (int)value); break;
                        case Property.BasicType.String:
                            {
                                if (property.isFunctor)
                                {
                                    var bytes = (byte[])value;
                                    if (bytes == null || bytes.Length == 0) writer.Write(0);
                                    else
                                    {
                                        WriteVContent(() =>
                                        {
                                            writer.Write((ushort)bytes.Length);
                                            writer.Write(bytes);
                                        });
                                    }
                                }
                                else HandleString((string)value, property.bSplitString, property.splitType);
                            }
                            break;
                        case Property.BasicType.UInt: writer.Write(value == null ? 0 : (uint)value); break;
                        case Property.BasicType.UShort: writer.Write(value == null ? 0 : Convert.ToUInt32(value)); break;
                        case Property.BasicType.Short: writer.Write(value == null ? 0 : Convert.ToInt32(value)); break;
                        case Property.BasicType.SByte: writer.Write(value == null ? 0 : Convert.ToInt32(value)); break;
                        case Property.BasicType.Byte: writer.Write(value == null ? 0 : Convert.ToUInt32(value)); break;
                    }
                }
            }
            else if (property.refKlass.type == Klass.KlassType.Struct)
            {
                WriteVContent(() =>
                {
                    EnsureLength(property.refKlass.binarySize, () =>
                    {
                        int pIndex = index;
                        foreach (var subProperty in property.refKlass)
                        {
                            pIndex = HandleProperty(rowData, ownerKlass, subProperty, pIndex);
                        }
                        ret = pIndex;
                    });
                });
            }
            else
            {
                object value = rowData[index];

                if (property.refKlass.type == Klass.KlassType.Enum) writer.Write(value == null ? 0 : (int)value);
                else
                {
                    //暂时写入id而不是index
                    writer.Write(value == null ? 0 : (int)value);

                    //int listIndex = property.refKlass.GetDataIndexByID(value == null ? 0 : (int)value);
                    //writer.Write(listIndex);
                }

            }

            return ret;
        }
        private static void WriteVContent(Action action)
        {
            writer.Write(vPos);
            long currentPosition = stream.Position;
            stream.Position = vPos;
            action();

            int cvPos = (int)stream.Position;
            //4byte对齐
            if (cvPos % 4 != 0)
            {
                cvPos = ((cvPos / 4) + 1) * 4;
                stream.Position = cvPos;
            }
            if (cvPos > vPos) vPos = cvPos;

            stream.Position = currentPosition;
        }

        private static void ExportGroupKeyTable(Klass klass)
        {
            var property = klass.groupKeyProperty;

            if (property == null)
            {
                header.groupKeyOffset = 0;
                return;
            }

            int index = klass.GetPropertyValueIndex(property);

            var groups = new Dictionary<int, List<int>>();
            for (int it = 0; it < klass.data.Count; it++)
            {
                var rowData = klass.data[it];
                var value = rowData[index];
                int iValue;

                if (property.isBasicType)
                {
                    if (property.basicType == Property.BasicType.Integer)
                    {
                        iValue = value == null ? default : (int)value;
                    }
                    else throw new Exception("should not reach here");
                }
                else if (property.refKlass != null && property.refKlass.type == Klass.KlassType.Enum)
                {
                    iValue = value == null ? default : (int)value;
                }
                else throw new Exception("should not reach here");

                List<int> list;
                if (!groups.TryGetValue(iValue, out list))
                {
                    list = new List<int>();
                    groups[iValue] = list;
                }
                list.Add((int)rowData[klass.idPropertyIndex]);
            }

            var groupAddress = new Dictionary<int, int>();
            foreach (var pair in groups)
            {
                int iValue = pair.Key;
                var list = pair.Value;
                int address = (int)stream.Position;
                groupAddress[iValue] = address;
                writer.Write(list.Count);
                for (int it = 0; it < list.Count; it++) writer.Write(list[it]);
            }

            header.groupKeyOffset = (int)stream.Position;
            Util.WriteIDTable(writer, groupAddress);
        }

        private static void ExportIDTable(Klass klass)
        {
            if (klass.type != Klass.KlassType.Config) return;
            if (klass.listType == Klass.ListType.List) return;

            header.idTableOffset = (int)stream.Position;
            Util.WriteIDTable(writer, GetKlassIDEnumerator(klass), klass.data.Count);
        }
        private static IEnumerator<Util.TableItem> GetKlassIDEnumerator(Klass klass)
        {
            int column = klass.idPropertyIndex;

            int count = klass.data.Count;
            for (int it = 0; it < count; it++)
            {
                object idValue = klass.data[it][column];
                int id = (int)idValue;

                yield return new Util.TableItem { hash = id, key = id, value = header.dataOffset + klass.binarySize * it };
            }
        }

        private static void ExportNameTable(Klass klass)
        {
            if (klass.type != Klass.KlassType.Config) return;
            if (klass.listType != Klass.ListType.CodeNameKeyMap) return;

            header.nameTableOffset = (int)stream.Position;
            Util.WriteIDTable(writer, GetKlassNameEnumerator(klass), klass.data.Count);
        }
        private static IEnumerator<Util.TableItem> GetKlassNameEnumerator(Klass klass)
        {
            int column = klass.codeNamePropertyIndex;

            int count = klass.data.Count;
            for (int it = 0; it < count; it++)
            {
                string codeName = klass.data[it][column] as string;

                yield return new Util.TableItem { hash = codeName.AscIIHash(), key = stringCache[codeName], value = header.dataOffset + klass.binarySize * it };
            }
        }


        private static void HandleString(string strValue, bool bSplitString = false, int splitType = 0)
        {
            if (bSplitString)
            {
                if (strValue == null || strValue == "")
                {
                    writer.Write(0);
                    return;
                }
                var splits = strValue.Split(':');
                int count = splits.Length;
                if (splits[count - 1] == null || splits[count - 1] == "")
                {
                    count--;
                }
                if (count <= 0)
                {
                    writer.Write(0);
                    return;
                }

                WriteVContent(() =>
                {
                    writer.Write(count);

                    EnsureLength(4 * count, () =>
                    {
                        var type = (Config.ExcelConfigAttribute.SplitSingleType)splitType;
                        switch (type)
                        {
                            case Config.ExcelConfigAttribute.SplitSingleType.Float:
                                for (int i = 0; i < count; i++)
                                {
                                    float fVal;
                                    if (float.TryParse(splits[i], out fVal))
                                    {
                                        writer.Write(fVal);
                                    }
                                    else throw new ConfigException(string.Format("MarkSplitString Parse float val failed souce data:{0},Error Position:{1}", strValue, splits[i]));

                                }
                                break;
                            case Config.ExcelConfigAttribute.SplitSingleType.Int:
                                for (int i = 0; i < count; i++)
                                {
                                    int iVal;
                                    if (int.TryParse(splits[i], out iVal))
                                    {
                                        writer.Write(iVal);
                                    }
                                    else throw new ConfigException(string.Format("MarkSplitString Parse int val failed souce data:{0},Error Position: {1}", strValue, splits[i]));

                                }
                                break;
                            case Config.ExcelConfigAttribute.SplitSingleType.String:
                                for (int i = 0; i < count; i++)
                                {
                                    HandleString(splits[i]);
                                }
                                break;
                        }
                    });
                });

                return;
            }

            int cacheIndex;
            if (string.IsNullOrEmpty(strValue)) writer.Write(0);
            else if (stringCache.TryGetValue(strValue, out cacheIndex)) writer.Write(cacheIndex);
            else
            {
                int byteCount = Encoding.UTF8.GetByteCount(strValue);
                if (byteCount > stringValueBuffer.Length) throw new ConfigException("corner case: string too long");
                Encoding.UTF8.GetBytes(strValue, 0, strValue.Length, stringValueBuffer, 0);
                stringCache[strValue] = vPos;

                WriteVContent(() =>
                {
                    writer.Write((ushort)byteCount);
                    writer.Write(stringValueBuffer, 0, byteCount);
                });

            }
        }
        private static void HandleBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) writer.Write(0);
            else
            {
                WriteVContent(() =>
                {
                    writer.Write((ushort)bytes.Length);
                    writer.Write(bytes);
                });
            }
        }

        private static string TranslateString(string input)
        {
            return input;
        }

        //加上头部2字节长度保持4字节对齐
        private static int GetStringByteCount(string content)
        {
            return (((Encoding.UTF8.GetByteCount(content) + 2 + 3) >> 2) << 2) - 2;
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

        private static void WriteFile(Klass klass)
        {
            using (var fileStream = new FileStream(Path.Combine(Entry.BinaryFolderPath, klass.name + ".bytes"), FileMode.Create))
            {
                stream.WriteTo(fileStream);
            }
        }
    }
}
