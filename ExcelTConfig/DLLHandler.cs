using Config.ExcelConfigAttribute;
using ExcelTConfig.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExcelTConfig
{
    public class DLLHandler
    {
        private static HashSet<string> illegalNames = new HashSet<string>()
        {
            //valid variable name in C#, but not in Lua
            "end","local","and","or","not","repeat",
            "nil","elseif","until","then","function",

            "p", "_Data", "ByIndex", "ByID", "ByCodeName", "pVersion", "Count",
        };

        private static StringBuilder sb = new StringBuilder();

        private const string OrderKey = "order";
        private const string MetaHashKey = "metaHash";
        private const string I18NOrderKey = "i18nOrder";
        private const string ClassInfoKey = "classInfo";

        public static void LoadDLL()
        {
            var klassMap = new Dictionary<string, KlassInfo>();
            var internalKlassMap = new Dictionary<string, KlassInfo>();
            var i18NKlasses = new Dictionary<string, I18NKlass>();
            var i18NHashCodes = new Dictionary<int, string>();

            var assembies = Entry.DLLPath.Select(path => Assembly.LoadFrom(Path.GetFullPath(path))).ToArray();
            {
                foreach (var typeName in Entry.DLLKlasses)
                {
                    Type type = null;
                    foreach (var assembly in assembies)
                    {
                        type = assembly.GetType(typeName, false);
                        if (type != null) break;
                    }
                    if (type == null)
                    {
                        throw new ConfigException($"Load Type {typeName} fail");
                    }
                    var targetExcelAttributes = type.GetCustomAttributes<TargetExcel>();

                    var klassInfo = new KlassInfo();

                    Klass.KlassType klassType;

                    if (type.IsEnum)
                    {
                        klassType = Klass.KlassType.Enum;
                        klassInfo.isPersistentEnum = type.GetCustomAttribute<PersistentEnum>() != null;
                        var customDataValidate = type.GetCustomAttribute<CustomDescEnum>();
                        if (customDataValidate != null)
                        {
                            klassInfo.EnumCustomDataValidation = customDataValidate.dataValidation;
                        }
                    }
                    else if (type.IsValueType) klassType = Klass.KlassType.Struct;
                    else if (type.IsAbstract && type.IsSealed) klassType = Klass.KlassType.Static;
                    else klassType = Klass.KlassType.Config;

                    klassInfo.type = klassType;
                    klassInfo.name = type.Name;

                    Dictionary<string, string> fixHashInfo = new Dictionary<string, string>();

                    klassInfo.metaHash = CalculateKlassMetaHash(type, ref fixHashInfo);

                    klassInfo.fixHashInfo = fixHashInfo;

                    if (targetExcelAttributes != null)
                    {
                        klassInfo.excels = targetExcelAttributes.Select(a => a.excel).ToArray();
                        klassInfo.sheets = targetExcelAttributes.Select(a => a.sheet).ToArray();
                    }

                    var usedInConfigAttribute = type.GetCustomAttribute<UsedInConfig>();
                    if (usedInConfigAttribute != null)
                    {
                        klassInfo.externalFullname = type.FullName;
                        klassInfo.isPersistentEnum = true;
                    }

                    foreach (var attribute in type.GetCustomAttributes()) klassInfo.attrbutes.Add(attribute);

                    var listAsAttribute = type.GetCustomAttribute<ListAs>();
                    if (listAsAttribute != null) klassInfo.listType = (Klass.ListType)(int)listAsAttribute.listType;

                    if (klassMap.ContainsKey(klassInfo.name)) throw new ConfigException(ErrorMessage.KlassNameCollide, klassInfo.name);
                    if (illegalNames.Contains(klassInfo.name)) throw new ConfigException(ErrorMessage.KlassNameIllegal, klassInfo.name);

                    klassMap[klassInfo.name] = klassInfo;

                    if (klassInfo.type == Klass.KlassType.Enum)
                    {
                        if (klassInfo.isPersistentEnum)
                        {
                            var data = new List<object[]>();

                            foreach (var field in type.GetFields())
                            {
                                if (field.IsSpecialName) continue;

                                if (illegalNames.Contains(field.Name))
                                {
                                    throw new ConfigException(ErrorMessage.PropertyNameIllegal, field.Name, klassInfo.name);
                                }

                                var tipAttribute = field.GetCustomAttribute<Tips>();

                                data.Add(new object[]
                                {
                                    Convert.ToInt32(field.GetRawConstantValue()),
                                    field.Name,
                                    tipAttribute?.description == null ? Util.GetToolTipsName(field.Name) : tipAttribute.description,
                                });
                            }
                            klassInfo.data = data;
                        }

                        continue;
                    }
                    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        if (field.IsLiteral) continue;
                        if (klassType != Klass.KlassType.Static && field.IsStatic) continue;

                        var tipAttribute = field.GetCustomAttribute<Tips>();

                        var propertyInfo = new Base.PropertyInfo();
                        propertyInfo.description = tipAttribute?.description == null ? Util.GetToolTipsName(field.Name) : tipAttribute.description;
                        propertyInfo.name = field.Name;
                        propertyInfo.arrayDescription = tipAttribute?.collectionTip;

                        if (illegalNames.Contains(propertyInfo.name))
                        {
                            throw new ConfigException(ErrorMessage.PropertyNameIllegal, propertyInfo.name, klassInfo.name);
                        }

                        GetPropertyType(klassInfo, propertyInfo, field.FieldType, internalKlassMap);

                        foreach (var attribute in field.GetCustomAttributes())
                        {
                            if (attribute is Capacity capacity)
                            {
                                // 是数组
                                propertyInfo.arrayLength = capacity.capacity;
                            }
                            else if (attribute is IntegerRange integerRange)
                            {
                                if (integerRange.min > integerRange.max)
                                {
                                    throw new ConfigException(ErrorMessage.RangeAttributeError, propertyInfo.name, klassInfo.name);
                                }
                            }
                            else if (attribute is FloatRange floatRange)
                            {
                                if (float.IsNaN(floatRange.min) || float.IsNaN(floatRange.max) || floatRange.min > floatRange.max)
                                {
                                    throw new ConfigException(ErrorMessage.RangeAttributeError, propertyInfo.name, klassInfo.name);
                                }
                            }
                            else if (attribute is MarkI18N)
                            {
                                if (klassInfo.type == Klass.KlassType.Config && propertyInfo.type == typeMap[typeof(string)] && !propertyInfo.isArray)
                                {
                                    var i18nKlass = new I18NKlass(klassInfo.name, propertyInfo.name);
                                    if (i18NHashCodes.ContainsKey(i18nKlass.hashCode))
                                    {
                                        throw new ConfigException(ErrorMessage.I18NKlassConflict, i18nKlass.name, i18NHashCodes[i18nKlass.hashCode]);
                                    }
                                    i18NKlasses[i18nKlass.name] = i18nKlass;
                                    i18NHashCodes[i18nKlass.hashCode] = i18nKlass.name;
                                    propertyInfo.i18NKlass = i18nKlass;
                                }
                                else throw new ConfigException($"MarkI18N 标签只能用于ConfigType的string类型属性, klass {klassInfo.name} property {field.Name}");
                            }
                            else if (attribute is CommentOnly)
                            {
                                if ((klassInfo.type != Klass.KlassType.Config && klassInfo.type != Klass.KlassType.Struct)
                                    || propertyInfo.isArray || propertyInfo.isBasic1DArray || propertyInfo.isBasic2DArray
                                    || propertyInfo.type != typeMap[typeof(string)])
                                {
                                    throw new ConfigException($"CommentOnly只能用于ConfigType或者Struct的string类型属性, klass {klassInfo.name} property {field.Name}");
                                }
                                propertyInfo.commentOnly = true;
                            }
                            else if (attribute is FieldAttribution)
                            {
                                var attribution = attribute as FieldAttribution;

                                switch (attribution.type)
                                {
                                    case AttributionType.ClientOnly:
                                        {
                                            propertyInfo.bClientOnly = true;
                                            propertyInfo.bServerOnly = false;
                                            break;
                                        }
                                    case AttributionType.ServerOnly:
                                        {
                                            propertyInfo.bClientOnly = false;
                                            propertyInfo.bServerOnly = true;
                                            break;
                                        }
                                    default:
                                        {
                                            propertyInfo.bClientOnly = false;
                                            propertyInfo.bServerOnly = false;
                                        }
                                        break;
                                }
                            }
                            else if (attribute is MarkSplitString)
                            {
                                if (propertyInfo.type == "string")
                                {
                                    var attr = attribute as MarkSplitString;

                                    propertyInfo.bSplitString = true;
                                    propertyInfo.spliteType = (int)attr.singleType;
                                }
                                else
                                {
                                    throw new ConfigException($"分割串必须是String类型 klass {klassInfo.name} property {field.Name}");
                                }
                            }
                            else if (attribute is MarkSpecificType)
                            {
                                var attr = attribute as MarkSpecificType;
                                propertyInfo.specificType = (int)attr.specificType;
                                propertyInfo.vLen = attr.len;
                                propertyInfo.bNegative = attr.bNegative;
                            }
                            else if (attribute is MarkDeleteType)
                            {
                                propertyInfo.markDelete = true;
                            }

                            propertyInfo.attrbutes.Add(attribute);
                        }

                        if (propertyInfo.isArray && propertyInfo.arrayLength <= 0) propertyInfo.arrayLength = 3;

                        klassInfo.properties.Add(propertyInfo);
                    }
                }
            }

            var kMap = new Dictionary<string, Klass>();
            foreach (var kv in klassMap)
            {
                kMap[kv.Key] = new Klass(kv.Value);
            }
            foreach (var kv in internalKlassMap)
            {
                kMap[kv.Key] = new Klass(kv.Value);
            }
            Entry.klasses = kMap;

            foreach (var kv in kMap)
            {
                kv.Value.Link();
            }
            foreach (var kv in kMap)
            {
                kv.Value.CalculateSize();
                kv.Value.CalculateBaseTypes();
                kv.Value.SetOtherNames();
            }
            foreach (var kv in kMap) kv.Value.CalculateMetaHash();

            Entry.i18NKlasses = i18NKlasses;

            FlushKlassOrders();
            CheckMetaHashDirtyCount();
        }
        private static void FlushKlassOrders()
        {
            var klassOrders = new Dictionary<string, int>();
            Entry.klassOrders = klassOrders;
            var prevOrders = new Dictionary<string, int>();
            Entry.klassStoredMetaHashes = new Dictionary<string, int>();
            var prevMetaHash = Entry.klassStoredMetaHashes;
            var i18nKlassOrders = new Dictionary<string, int>();
            Entry.i18nKlassOrders = i18nKlassOrders;
            var prevI18NOrders = new Dictionary<string, int>();
            Entry.classPropertiesInfo = new Dictionary<string, KlassDataInfo>();

            var orderFilePath = typeof(Entry).Namespace.ToString() + "Order.json";
            JObject config = null;
            if (File.Exists(orderFilePath))
            {
                string data = File.ReadAllText(orderFilePath);
                if (!string.IsNullOrEmpty(data))
                {
                    config = JObject.Parse(data);

                    JToken jToken;
                    if (config.TryGetValue(OrderKey, out jToken))
                    {
                        var orderConfig = jToken as JObject;
                        foreach (var kv in orderConfig) prevOrders[kv.Key] = (int)kv.Value;
                    }
                    if (config.TryGetValue(MetaHashKey, out jToken))
                    {
                        var metaHashConfig = jToken as JObject;
                        foreach (var kv in metaHashConfig) prevMetaHash[kv.Key] = (int)kv.Value;
                    }
                    if (config.TryGetValue(I18NOrderKey, out jToken))
                    {
                        var i18nOrderConfig = jToken as JObject;
                        foreach (var kv in i18nOrderConfig) prevI18NOrders[kv.Key] = (int)kv.Value;
                    }
                }
            }

            var classInfoPath = typeof(Entry).Namespace.ToString() + "ClassInfo.json";
            if (File.Exists(classInfoPath))
            {
                string data = File.ReadAllText(classInfoPath);
                if (!string.IsNullOrEmpty(data))
                {
                    var classInfoJson = JObject.Parse(data);
                    JToken jToken;
                    if (classInfoJson.TryGetValue(ClassInfoKey, out jToken))
                    {
                        var classInfoConfig = jToken as JObject;
                        foreach (var kv in classInfoConfig) Entry.classPropertiesInfo[kv.Key] = kv.Value.ToObject<KlassDataInfo>();
                    }
                }
            }

            int orderGen = prevOrders.Count == 0 ? 1 : (prevOrders.Max(kv => kv.Value) + 1);
            bool dirty = false;
            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;
                if (klass.type != Klass.KlassType.Config && klass.type != Klass.KlassType.Static) continue;

                int order;
                if (!prevOrders.TryGetValue(klass.name, out order))
                {
                    order = orderGen++;
                    dirty = true;
                }

                klassOrders[kv.Key] = order;
            }
            int i18nOrderGen = prevI18NOrders.Count == 0 ? 1 : (prevI18NOrders.Max(kv => kv.Value) + 1);
            foreach (var kv in Entry.i18NKlasses)
            {
                var i18nKlass = kv.Value;

                int order;
                if (!prevI18NOrders.TryGetValue(i18nKlass.name, out order))
                {
                    order = i18nOrderGen++;
                    dirty = true;
                }

                i18nKlassOrders[kv.Key] = order;
            }
            if (dirty)
            {
                if (config == null) config = new JObject();

                var orderConfig = new JObject();
                config[OrderKey] = orderConfig;
                foreach (var kv in Entry.klasses.Where(kv => kv.Value.type == Klass.KlassType.Config || kv.Value.type == Klass.KlassType.Static).OrderBy(kv => klassOrders[kv.Key]))
                {
                    orderConfig[kv.Key] = klassOrders[kv.Key];
                }
                var i18nOrderConfig = new JObject();
                config[I18NOrderKey] = i18nOrderConfig;
                foreach (var kv in Entry.i18NKlasses.OrderBy(kv => i18nKlassOrders[kv.Key])) i18nOrderConfig[kv.Key] = i18nKlassOrders[kv.Key];

                File.WriteAllText(orderFilePath, config.ToString());
            }
        }


        public static void GetPropertyType(KlassInfo klass, Base.PropertyInfo propertyInfo, Type type, Dictionary<string, KlassInfo> internalKlassMap)
        {
            if (type.IsArray)
            {
                var elementType = GetPropertyTypeRaw(type.GetElementType());
                propertyInfo.type = elementType ?? throw new ConfigException(ErrorMessage.UnSupportType, type.Name, klass.name, propertyInfo.name);
                propertyInfo.isArray = true;
            }
            else if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                var genericArguments = type.GetGenericArguments();
                if (genericTypeDefinition == typeof(Array1D<>) || genericTypeDefinition == typeof(Array2D<>))
                {
                    var elementRawType = genericArguments[0];

                    if (elementRawType != typeof(int) && elementRawType != typeof(float) && elementRawType != typeof(string))
                    {
                        throw new ConfigException(ErrorMessage.UnSupportType, type.FullName, klass.name, propertyInfo.name);
                    }

                    var elementType = GetPropertyTypeRaw(elementRawType);
                    propertyInfo.type = elementType;

                    if (genericTypeDefinition == typeof(Array1D<>)) propertyInfo.isBasic1DArray = true;
                    else propertyInfo.isBasic2DArray = true;
                }
                else if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    var keyRawType = genericArguments[0];

                    if (keyRawType != typeof(int) && keyRawType != typeof(string) && keyRawType != typeof(float) && !keyRawType.IsEnum)
                    {
                        throw new ConfigException(ErrorMessage.UnSupportType, type.FullName, klass.name, propertyInfo.name);
                    }

                    string keyType = GetPropertyTypeRaw(keyRawType);
                    string valType = GetPropertyTypeRaw(genericArguments[1]);

                    if (keyType == null || valType == null)
                    {
                        throw new ConfigException(ErrorMessage.UnSupportType, type.FullName, klass.name, propertyInfo.name);
                    }

                    var pairName = KlassInfo.GetKeyValuePairKlassName(keyType, valType);
                    if (!internalKlassMap.ContainsKey(pairName)) internalKlassMap[pairName] = KlassInfo.GetKeyValuePairKlass(keyType, valType);

                    propertyInfo.type = pairName;
                    propertyInfo.isArray = true;
                    propertyInfo.isDictionary = true;
                }
                else throw new ConfigException(ErrorMessage.UnSupportType, type.FullName, klass.name, propertyInfo.name);
            }
            else
            {
                var pType = GetPropertyTypeRaw(type);
                propertyInfo.type = pType ?? throw new ConfigException(ErrorMessage.UnSupportType, type.Name, klass.name, propertyInfo.name);
            }
        }

        private static Dictionary<Type, string> typeMap = new Dictionary<Type, string>
        {
            {typeof(string), "string" },
            {typeof(int), "int" },
            {typeof(float), "float" },
            {typeof(bool), "bool" },
            {typeof(uint), "uint" },
            {typeof(short), "short" },
            {typeof(ushort), "ushort" },
            {typeof(sbyte), "sbyte" },
            {typeof(byte), "byte" },
        };

        private static string GetPropertyTypeRaw(Type type)
        {
            if (type.IsGenericType || type.IsArray)
                return null;

            string ret;
            if (typeMap.TryGetValue(type, out ret))
                return ret;

            return type.Name;
        }

        private static void CheckMetaHashDirtyCount()
        {
            Klass dirtyKlass = null;
            int dirtyCount = 0;
            int metaHash;
            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;
                if (!Entry.klassStoredMetaHashes.TryGetValue(klass.name, out metaHash) || metaHash != klass.metaHash)
                {
                    dirtyCount++;
                    dirtyKlass = klass;
                }
            }
            //if (dirtyCount == 0) Entry.UpdateTooltip(string.Empty);
            //else if (dirtyCount == 1) Entry.UpdateTooltip($"[{dirtyKlass.name}] was dirty");
            //else Entry.UpdateTooltip($"{dirtyCount} klasses ({dirtyKlass.name}...) was dirty");
        }

        private static int CalculateKlassMetaHash(Type type, ref Dictionary<string, string> fixHashInfo)
        {
            sb.Clear();

            sb.Append(type.FullName);
            AppendAttributes(type);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.IsSpecialName) continue;

                sb.Append(field.FieldType.FullName);
                sb.Append(field.Name);
                AppendAttributes(field);

                fixHashInfo.Add(field.Name, field.FieldType.FullName);

            }

            int ret = sb.ToString().AscIIHash();

            sb.Clear();
            return ret;
        }

        private static void AppendAttributes(ICustomAttributeProvider provider)
        {
            foreach (var attribute in provider.GetCustomAttributes(false))
            {
                AppendAttribute(attribute);
            }
        }
        private static void AppendAttribute(object obj)
        {
            var type = obj.GetType();
            sb.Append(type.FullName);

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsSpecialName) continue;

                sb.Append(field.FieldType.FullName);
                sb.Append(field.Name);
                var fieldValue = field.GetValue(obj);
                if (fieldValue == null) sb.Append("null");
                else if (fieldValue is Array array)
                {
                    foreach (var elementValue in array) sb.Append(elementValue == null ? "null" : elementValue.ToString());
                }
                else
                {
                    sb.Append(fieldValue.ToString());
                }
            }
        }
    }
}
