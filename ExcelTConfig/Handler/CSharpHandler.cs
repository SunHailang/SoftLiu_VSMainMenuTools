using ExcelTConfig.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TConfiger;

namespace ExcelTConfig
{
    public static class CSharpHandler
    {
        private static StringBuilder sb, fsb;
        private static Klass currentKlass;

        public const string DataKlassName = "_Data";
        public const string PName = "p";
        public const string ArrayName = "NArray";
        public const string NBoolArrayName = "NBoolArray";
        public const string UIntArraryName = "NUIntArray";
        public const string ShortArrayName = "NShortArray";
        public const string UShortArrayName = "NUShortArray";
        public const string SByteArrayName = "NSByteArray";
        public const string ByteArrayName = "NByteArray";
        public const string StructArrayName = "NStructArray";
        public const string ConfigArrayName = "NConfigArray";
        public const string StringName = "NString";
        public const string I18NStringName = "I18NString";
        public const string Namespace = "Framework";
        public const string DataPointer = "dataPointer";
        public const string TableIndex = "TableIndex";
        public const string PVersion = "pVersion";

        public static void Export()
        {
            if (string.IsNullOrEmpty(Entry.CSharpFolderPath)) throw new ConfigException(ErrorMessage.JsonConfigFileNoXXXXField, "CSharp Folder Path");
            if (!Directory.Exists(Entry.CSharpFolderPath)) throw new ConfigException(ErrorMessage.FolderNotExist, "CSharp");

            sb = new StringBuilder();
            fsb = new StringBuilder();

            ExportTableIndex();

            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;

                if ((klass.exportScriptType & Config.ExcelConfigAttribute.ScriptType.CSharp) == Config.ExcelConfigAttribute.ScriptType.None) continue;

                currentKlass = klass;

                sb.Clear();
                fsb.Clear();

                switch (klass.type)
                {
                    case Klass.KlassType.Config: ExportInstance(klass); break;
                    case Klass.KlassType.Enum:
                        {
                            if (klass.isExternal) continue;
                            ExportEnum(klass);
                        }
                        break;
                    case Klass.KlassType.Struct:
                        {
                            if (klass.isKeyValuePair) continue;
                            ExportStruct(klass);
                        }
                        break;
                    case Klass.KlassType.Static: ExportStatic(klass); break;
                }
            }

            currentKlass = null;
            sb = null;
            fsb = null;
        }

        private static void ExportTableIndex()
        {
            bool first = true;
            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;
                if (klass.type != Klass.KlassType.Config && klass.type != Klass.KlassType.Static) continue;

                if (first) first = false;
                else
                {
                    sb.AppendLine();
                    fsb.AppendLine();
                }

                sb.Append($"\t    public static int {klass.name} {{ get; private set;}}");
                fsb.Append($"\t\t    {klass.name} = Table.GetTableIndex(nameof({klass.name}));");
            }

            var fieldContent = sb.ToString();
            var setDataContent = fsb.ToString();
            sb.Clear();
            fsb.Clear();

            var content = CSharpTemplate.TableIndex
                .Replace("__fields__", fieldContent)
                .Replace("__setDataContents__", setDataContent)
                .Replace("__namespace__", Namespace)
                .Replace("__className__", "TableIndex");

            File.WriteAllText(Path.Combine(Entry.CSharpFolderPath, "TableIndex.cs"), content);
        }

        private static void ExportEnum(Klass klass)
        {
            bool first = true;
            foreach (var row in klass.data)
            {
                if (first) first = false;
                else sb.AppendLine();

                sb.Append("\t    ");
                sb.Append("// " + row[klass.designNamePropertyIndex]);
                sb.AppendLine();
                sb.Append("\t    ");
                sb.Append(row[klass.codeNamePropertyIndex]);
                sb.Append(" = ");
                sb.Append((int)row[klass.idPropertyIndex]);
                sb.Append(",");
            }

            var fieldContent = sb.ToString();
            sb.Clear();

            string content = CSharpTemplate.EnumTemplate
                .Replace("__namespace__", Namespace)
                .Replace("__className__", klass.name)
                .Replace("__fields__", fieldContent);

            Config.ExcelConfigAttribute.EnumExtraAttribute enumExtraAttribute = klass.GetAttribute<Config.ExcelConfigAttribute.EnumExtraAttribute>();
            if (enumExtraAttribute != null)
            {
                content = content.Replace("__extra_attrs__", enumExtraAttribute.ToString());
            }
            else
            {
                content = content.Replace("__extra_attrs__", "");
            }

            WriteFile(content, klass);
        }

        private static void ExportStruct(Klass klass)
        {
            foreach (var property in klass) HandleField(property);

            sb.Remove(sb.Length - 2, 2);
            fsb.Remove(fsb.Length - 2, 2);

            string dataFieldContent = sb.ToString();
            string fieldContent = fsb.ToString();

            string content = CSharpTemplate.StructTemplate
            .Replace("__namespace__", Namespace)
            .Replace("__className__", klass.name)
            .Replace("__dataName__", DataKlassName)
            .Replace("__dataFields__", dataFieldContent)
            .Replace("__fields__", fieldContent)
            ;

            WriteFile(content, klass);
        }

        private static void ExportStatic(Klass klass)
        {
            foreach (var property in klass)
            {
                HandleField(property);
                HandleMarkAsProperty(property);
            }

            sb.Remove(sb.Length - 2, 2);
            fsb.Remove(fsb.Length - 2, 2);

            string dataFieldContent = sb.ToString();
            string fieldContent = fsb.ToString();
            //sb.Clear();

            string content = CSharpTemplate.StaticTemplate
            .Replace("__namespace__", Namespace)
            .Replace("__dataName__", DataKlassName)
            .Replace("__dataFields__", dataFieldContent)
            .Replace("__fields__", fieldContent)
            .Replace("__className__", klass.name)
            ;

            WriteFile(content, klass);
        }

        private static void ExportInstance(Klass klass)
        {
            foreach (var property in klass)
            {
                HandleField(property);
                HandleMarkAsProperty(property);
            }

            sb.Remove(sb.Length - 2, 2);
            fsb.AppendLine();
            fsb.Append("\t\tpublic static __className__ ByIndex(int index) => Table.ByIndex<__className__>(TableIndex.__className__, index);");
            fsb.AppendLine();
            fsb.Append("\t\tpublic static __className__ Find(Predicate<__className__> match) { for (int i = 0; i < Count; i++) { __className__ data = ByIndex(i); if (match(data)) { return data;} } return default;}");
            if (klass.listType != Klass.ListType.List)
            {
                fsb.AppendLine();
                fsb.Append("\t\tpublic static __className__ ByID(int id) { __className__ data = Table.ByID<__className__>(TableIndex.__className__, id); return data;}");
            }

            if (klass.listType == Klass.ListType.CodeNameKeyMap)
            {
                fsb.AppendLine();
                var property = klass.codeNameProperty;
                fsb.Append($"\t\tpublic static __className__ By{Util.FirstCharToUpper(property.name)}(string {property.name}) => Table.ByID<__className__>(TableIndex.__className__, {property.name});");
            }

            if (klass.groupKeyProperty != null)
            {
                fsb.AppendLine();
                var property = klass.groupKeyProperty;
                fsb.Append($"\t\tpublic static NConfigArray<__className__> By{Util.FirstCharToUpper(property.name)}({property.type} {property.name}) => Table.ByGroupKey<__className__, {property.type}>(TableIndex.__className__, {property.name});");
            }

            string dataFieldContent = sb.ToString();
            string fieldContent = fsb.ToString();
            //sb.Clear();

            string content = CSharpTemplate.InstanceTemplate
            .Replace("__excelName__", klass.excels[0])
            .Replace("__namespace__", Namespace)
            .Replace("__dataName__", DataKlassName)
            .Replace("__dataFields__", dataFieldContent)
            .Replace("__fields__", fieldContent)
            .Replace("__className__", klass.name)
            ;

            WriteFile(content, klass);
        }

        private static void HandleField(Property property)
        {
            string fsbGetContent = string.Empty;

            sb.Append("\t\t\t// " + property.description);
            sb.AppendLine();
            sb.Append($"\t\t\tpublic ");
            fsb.Append("\t\tpublic ");

            if (currentKlass.type == Klass.KlassType.Static) fsb.Append("static ");

            if (property.isArray)
            {
                sb.Append("int");

                if (property.isBasicType)
                {
                    switch (property.basicType)
                    {
                        case Property.BasicType.Integer:
                        case Property.BasicType.Float:
                            fsb.Append(ArrayName).Append("<").Append(property.type).Append(">");
                            fsbGetContent = $"return new {ArrayName}<{property.type}>({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.Boolean:
                            fsb.Append(NBoolArrayName);
                            fsbGetContent = $"return new {NBoolArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.UInt:
                            fsb.Append(UIntArraryName);
                            fsbGetContent = $"return new {UIntArraryName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.Short:
                            fsb.Append(ShortArrayName);
                            fsbGetContent = $"return new {ShortArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.UShort:
                            fsb.Append(UShortArrayName);
                            fsbGetContent = $"return new {UShortArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.SByte:
                            fsb.Append(SByteArrayName);
                            fsbGetContent = $"return new {SByteArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.Byte:
                            fsb.Append(ByteArrayName);
                            fsbGetContent = $"return new {ByteArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;

                        case Property.BasicType.String:
                            fsb.Append(StructArrayName).Append("<").Append(StringName).Append(">");
                            fsbGetContent = $"return new {StructArrayName}<{StringName}>({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion});";
                            break;
                    }
                }
                else
                {
                    var refKlass = property.refKlass;
                    if (refKlass.type == Klass.KlassType.Enum)
                    {
                        fsb.Append(ArrayName).Append("<").Append(refKlass.externalFullname ?? property.type).Append(">");
                        fsbGetContent = $"return new {ArrayName}<{refKlass.externalFullname ?? property.type}>({DataPointer} + {PName}->{property.name}, {PVersion});";
                    }
                    else if (refKlass.type == Klass.KlassType.Struct)
                    {
                        {
                            string typeName = $"{StructArrayName}<{property.type}>";
                            fsb.Append(typeName);
                            fsbGetContent = $"return new {typeName}({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion});";
                        }
                    }
                    else if (refKlass.type == Klass.KlassType.Config)
                    {
                        string typeName = $"{ConfigArrayName}<{property.type}>";
                        fsb.Append(typeName);
                        fsbGetContent = $"return new {typeName}({DataPointer} + {PName}->{property.name}, {TableIndex}.{property.type}, {PVersion});";
                    }
                }
            }
            else if (property.isBasicArray)
            {
                sb.Append("int");

                if (property.isBasic1DArray)
                {
                    switch (property.basicType)
                    {
                        case Property.BasicType.Integer:
                        case Property.BasicType.Float:
                            fsb.Append(ArrayName).Append("<").Append(property.type).Append(">");
                            fsbGetContent = $"return new {ArrayName}<{property.type}>({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.Boolean:
                            fsb.Append(NBoolArrayName);
                            fsbGetContent = $"return new {NBoolArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.UInt:
                            fsb.Append(UIntArraryName);
                            fsbGetContent = $"return new {UIntArraryName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.Short:
                            fsb.Append(ShortArrayName);
                            fsbGetContent = $"return new {ShortArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.UShort:
                            fsb.Append(UShortArrayName);
                            fsbGetContent = $"return new {UShortArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.SByte:
                            fsb.Append(SByteArrayName);
                            fsbGetContent = $"return new {SByteArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;
                        case Property.BasicType.Byte:
                            fsb.Append(ByteArrayName);
                            fsbGetContent = $"return new {ByteArrayName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            break;

                        case Property.BasicType.String:
                            fsb.Append(StructArrayName).Append("<").Append(StringName).Append(">");
                            fsbGetContent = $"return new {StructArrayName}<{StringName}>({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion});";
                            break;
                    }
                }
                else
                {
                    switch (property.basicType)
                    {
                        case Property.BasicType.Integer:
                        case Property.BasicType.Float:
                        case Property.BasicType.Boolean:
                        case Property.BasicType.UInt:
                        case Property.BasicType.UShort:
                        case Property.BasicType.Short:
                        case Property.BasicType.SByte:
                        case Property.BasicType.Byte:
                            fsb.Append($"{StructArrayName}<{ArrayName}<{property.type}>>");
                            fsbGetContent = $"return new {StructArrayName}<{ArrayName}<{property.type}>>({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion});";
                            break;

                        case Property.BasicType.String:
                            fsb.Append($"{StructArrayName}<{StructArrayName}<{StringName}>>");
                            fsbGetContent = $"return new {StructArrayName}<{StructArrayName}<{StringName}>>({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion}, {PVersion});";
                            break;
                    }
                }
            }
            else if (property.isBasicType)
            {
                switch (property.basicType)
                {
                    case Property.BasicType.Integer:
                    case Property.BasicType.Float:
                        {
                            sb.Append(property.type);

                            fsb.Append(property.type);
                            fsbGetContent = $"return {PName}->{property.name};";
                        }
                        break;

                    case Property.BasicType.Boolean:
                        {
                            sb.Append("int");

                            fsb.Append(property.type);
                            fsbGetContent = $"return {PName}->{property.name} != 0;";
                        }
                        break;

                    case Property.BasicType.String:
                        {
                            sb.Append("int");

                            if (property.isI18N)
                            {
                                fsb.Append(I18NStringName);
                                fsbGetContent = $"return new {I18NStringName}({DataPointer} + {PName}->{property.name}, {PVersion}, \"{property.i18NKlass.name}\", {property.i18NKlass.hashCode}, {PName}->{currentKlass.idProperty.name});";
                            }
                            else if (property.bSplitString)
                            {
                                var type = (Config.ExcelConfigAttribute.SplitSingleType)property.splitType;
                                switch (type)
                                {
                                    case Config.ExcelConfigAttribute.SplitSingleType.Float:
                                        fsb.Append(ArrayName).Append("<").Append("float").Append(">");
                                        fsbGetContent = $"return new {ArrayName}<{"float"}>({DataPointer} + {PName}->{property.name}, {PVersion});";
                                        break;
                                    case Config.ExcelConfigAttribute.SplitSingleType.Int:
                                        fsb.Append(ArrayName).Append("<").Append("int").Append(">");
                                        fsbGetContent = $"return new {ArrayName}<{"int"}>({DataPointer} + {PName}->{property.name}, {PVersion});";
                                        break;
                                    case Config.ExcelConfigAttribute.SplitSingleType.String:
                                        fsb.Append(StructArrayName).Append("<").Append(StringName).Append(">");
                                        fsbGetContent = $"return new {StructArrayName}<{StringName}>({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion});";
                                        break;

                                }

                            }
                            else
                            {
                                fsb.Append(StringName);
                                fsbGetContent = $"return new {StringName}({DataPointer} + {PName}->{property.name}, {PVersion});";
                            }

                        }
                        break;
                    case Property.BasicType.UInt:
                        {
                            sb.Append("int");

                            fsb.Append(property.type);
                            fsbGetContent = $"return (uint)({PName}->{property.name});";
                        }
                        break;
                    case Property.BasicType.UShort:
                        {
                            sb.Append("int");

                            fsb.Append(property.type);
                            fsbGetContent = $"return (ushort)({PName}->{property.name});";
                        }
                        break;
                    case Property.BasicType.Short:
                        {
                            sb.Append("int");

                            fsb.Append(property.type);
                            fsbGetContent = $"return (short)({PName}->{property.name});";
                        }
                        break;
                    case Property.BasicType.SByte:
                        {
                            sb.Append("int");

                            fsb.Append(property.type);
                            fsbGetContent = $"return (sbyte)({PName}->{property.name});";
                        }
                        break;
                    case Property.BasicType.Byte:
                        {
                            sb.Append("int");

                            fsb.Append(property.type);
                            fsbGetContent = $"return (byte)({PName}->{property.name});";
                        }
                        break;
                }
            }
            else if (property.refKlass.type == Klass.KlassType.Struct)
            {
                //sb.Append(property.type).Append(".").Append(DataKlassName);
                sb.Append("int");

                fsb.AppendFormat(property.type);
                fsbGetContent = $"return new {property.type}({DataPointer} + {PName}->{property.name}, {DataPointer}, {PVersion});";
            }
            else if (property.refKlass.type == Klass.KlassType.Enum)
            {
                sb.Append("int");
                fsb.Append(property.refKlass.externalFullname ?? property.type);
                fsbGetContent = $"return ({property.refKlass.externalFullname ?? property.type}){PName}->{property.name};";
            }
            else
            {
                sb.Append("int");
                fsb.Append(property.type);
                //暂时使用ByID
                fsbGetContent = $"return {property.type}.ByID({PName}->{property.name});";
                //fsbGetContent = $"return {property.type}.ByIndex({PName}->{property.name});";
            }

            sb.Append(" ").Append(property.name).Append(";").AppendLine();
            fsb.Append(" ").Append(property.name).Append(" { get{ Table.PointerCheck(").Append(PName).Append(", ").Append(PVersion).Append("); ").Append(fsbGetContent).AppendLine("} }");
        }

        static void HandleMarkAsProperty(Property property)
        {
            if (!property.bMarkAsProperty)
                return;

            if (property.bMarkAsProperty)
            {
                fsb.Append("\t\tpublic ");

                fsb.Append(property.type);
                string fsbGetContent = $"return {PName}->{property.name};";

                fsb.Append(" ").Append("id").Append(" { get{ Table.PointerCheck(").Append(PName).Append(", ").Append(PVersion).Append("); ").Append(fsbGetContent).AppendLine("} }");

            }
        }

        private static void WriteFile(string content, Klass klass)
        {
            File.WriteAllText(Path.Combine(Entry.CSharpFolderPath, klass.name + ".cs"), content);
        }
    }
}
