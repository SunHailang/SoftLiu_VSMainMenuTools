using Config.ExcelConfigAttribute;
using ExcelTConfig.Base;
using Mono.Cecil;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExcelTConfig
{

    public struct KlassSingleDataInfo
    {
        public string name;
        public DataRangeInfo range;
    }

    public class KlassDataInfo
    {
        public int rowBegin;
        public List<KlassSingleDataInfo> listDatas = new List<KlassSingleDataInfo>();
    }

    public class Entry
    {
        private static HashSet<string> dllNamespace = new HashSet<string>()
        {
            "Config.ClientOnly", "Config.ClientAndServer", "Config.ServerOnly", "Config.ClassLibriy"
        };

        #region 路径配置
        public static List<string> DLLPath { get; private set; } = new List<string>();
        public static string ExcelFolderPath { get; private set; }
        public static string I18NExcelFolderPath { get; private set; }
        public static string CSharpFolderPath { get; private set; }
        public static string BinaryFolderPath { get; private set; }
        public static string InsertSqlFolderPath { get; private set; }
        public static string SqlDataAndTableFolderPath { get; private set; }
        public static string[] Lang { get; private set; }
        #endregion

        public static List<string> DLLKlasses { get; private set; } = new List<string>();

        public static Dictionary<string, Klass> klasses;

        public static Dictionary<string, I18NKlass> i18NKlasses;

        public static Dictionary<string, int> klassOrders;

        public static Dictionary<string, int> klassStoredMetaHashes;

        public static Dictionary<string, int> i18nKlassOrders;

        public static Dictionary<string, KlassDataInfo> classPropertiesInfo;


        public void LoadInit()
        {
            // 1. 加载Json配置
            LoadJsonConfigData();
            // 2. 加载DLL文件
            LoadDLLKlassesData();
        }


        private void LoadJsonConfigData()
        {
            string jsonPath = $"{typeof(Entry).Namespace}.json";
            string jsonText = "";
            using (FileStream fs = new FileStream(jsonPath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                int totalBytes = (int)fs.Length;
                byte[] bytes = new byte[totalBytes];

                int byteRead = 0;
                while (byteRead < totalBytes)
                {
                    int len = fs.Read(bytes, byteRead, totalBytes);
                    byteRead += len;
                }
                jsonText = Encoding.UTF8.GetString(bytes);
            }

            JObject config = null;
            if (string.IsNullOrEmpty(jsonText))
            {
                config = new JObject();
            }
            else
            {
                config = JObject.Parse(jsonText);
            }

            JToken dllToken;
            if (!config.TryGetValue(nameof(DLLPath), out dllToken) || !(dllToken is JArray)) throw new ConfigException(ErrorMessage.JsonConfigFileNoDllFileField);
            DLLPath.Clear();
            foreach (var v in dllToken as JArray)
            {
                // "../TableDefine/Config.ClientAndServer.dll"
                var path = v.ToString();
                DLLPath.Add(path);
                if (!File.Exists(path)) throw new ConfigException(ErrorMessage.FileNotExist, path);
            }

            string getValue(string key)
            {
                bool suc = config.TryGetValue(key, out JToken jToken);
                return suc ? jToken.ToString() : throw new Exception($"Path:{jsonPath}, Key:{key} config not exist.");
            }

            ExcelFolderPath = getValue(nameof(ExcelFolderPath));
            I18NExcelFolderPath = getValue(nameof(I18NExcelFolderPath));
            CSharpFolderPath = getValue(nameof(CSharpFolderPath));
            BinaryFolderPath = getValue(nameof(BinaryFolderPath));
            InsertSqlFolderPath = getValue(nameof(InsertSqlFolderPath));
            SqlDataAndTableFolderPath = getValue(nameof(SqlDataAndTableFolderPath));


            JToken langToken;
            if (!config.TryGetValue(nameof(Lang), out langToken) || !(langToken is JArray))
            {
                throw new Exception($"Path:{jsonPath}, Key:{nameof(Lang)} Error.");
            }
            JArray langArray = langToken as JArray;
            Lang = new string[langArray.Count];
            for (int i = 0; i < langArray.Count; i++)
            {
                Lang[i] = langArray[i].ToString();
            }
        }

        
        private void LoadDLLKlassesData()
        {
            DLLKlasses.Clear();
            string usedInConfigFullName = typeof(UsedInConfig).FullName;
            foreach (var dllPath in DLLPath)
            {
                if (!File.Exists(dllPath))
                {
                    throw new Exception($"Load DLL Path:{dllPath} error file not exists");
                }
                byte[] bytes = File.ReadAllBytes(dllPath);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    using (AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(stream))
                    {
                        foreach (var type in assembly.MainModule.Types)
                        {
                            if (dllNamespace.Contains(type.Namespace))
                            {
                                DLLKlasses.Add(type.FullName);
                                continue;
                            }

                            if (!type.HasCustomAttributes) continue;
                            var usedInConfigAttribute = type.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName == usedInConfigFullName);
                            if (usedInConfigAttribute == null) continue;

                            string ns = usedInConfigAttribute.ConstructorArguments.First().Value as string;
                            if (dllNamespace.Contains(ns)) DLLKlasses.Add(type.FullName);
                        }
                    }
                }
            }
        }

        
    }
}
