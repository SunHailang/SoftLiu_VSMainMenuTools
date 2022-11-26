using Config.ExcelConfigAttribute;
using ExcelTConfig.Base;
using Mono.Cecil;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

    public enum LogLevelType : uint
    {
        InfoType = 0,
        WarnningType = 5,
        ErrorType = 10,
    }

    public class Entry
    {
        public static event Action<string, LogLevelType> LogInfoEvent;
        public static void UpdateLogInfo(string msg, LogLevelType level = LogLevelType.InfoType) => LogInfoEvent?.Invoke(msg, level);

        private static HashSet<string> dllNamespace = new HashSet<string>()
        {
            "Config.ClientOnly", "Config.ClientAndServer", "Config.ServerOnly", "Config.ClassLibriy"
        };

        #region 路径配置
        public static List<string> DLLPath { get; private set; } = new List<string>();
        public static string ExcelFolderPath { get; private set; }
        public static string I18NExcelFolderPath { get; private set; }
        public static string VExcelFolderPath { get; private set; }
        public static string JsonFolderPath { get; private set; }
        public static string CSharpFolderPath { get; private set; }
        public static string BinaryFolderPath { get; private set; }
        public static string BinaryPackOnePath { get; private set; }
        public static string JavaFolderPath { get; private set; }
        public static string PythonFolderPath { get; private set; }
        public static string InsertSqlFolderPath { get; private set; }
        public static string SqlDataAndTableFolderPath { get; private set; }
        public static string LangPrefix { get; private set; }
        public static string[] Lang { get; private set; }
        public static int LangCount { get; private set; }
        #endregion

        internal static string RootPath { get; private set; }

        public static List<string> DLLKlasses { get; private set; } = new List<string>();        

        public static Dictionary<string, Klass> klasses;

        public static Dictionary<string, I18NKlass> i18NKlasses;

        public static Dictionary<string, int> klassOrders;

        public static Dictionary<string, int> klassStoredMetaHashes;

        public static Dictionary<string, int> i18nKlassOrders;

        public static Dictionary<string, KlassDataInfo> classPropertiesInfo;

        public static bool ExportPack = false;

        public static void LoadInit(string configDirectory)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            RootPath = Path.GetFullPath(configDirectory);

            // 1. 加载Json配置
            LoadJsonConfigData();

            // 2. 加载DLL文件
            LoadDLLKlassesData();

            DLLHandler.LoadDLL();

            //ExcelHandler.Export();
        }


        private static void LoadJsonConfigData()
        {
            string jsonPath = $"{RootPath}/Config.json";
            bool rewrite = !File.Exists(jsonPath);
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

            JObject config;
            if (string.IsNullOrEmpty(jsonText))
            {
                config = new JObject();
                rewrite = true;
            }
            else
            {
                config = JObject.Parse(jsonText);
            }

            JToken dllToken;
            if (!config.TryGetValue(nameof(DLLPath), out dllToken) || !(dllToken is JArray))
            {
                dllToken = new JArray();
                config[nameof(DLLPath)] = dllToken;
                rewrite = true;
            }
            DLLPath.Clear();
            foreach (var v in dllToken as JArray)
            {
                var path = $"{RootPath}/{v}";
                DLLPath.Add(path);
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            string getValue(string key)
            {
                bool suc = config.TryGetValue(key, out JToken jToken);
                string value;
                if (!suc)
                {
                    config[key] = $"{key}";
                    rewrite = true;
                    value = key;
                }
                else
                {
                    value = jToken.ToString();
                }
                string path = Path.GetFullPath($"{RootPath}/{value}");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }

            ExcelFolderPath = getValue(nameof(ExcelFolderPath));
            I18NExcelFolderPath = getValue(nameof(I18NExcelFolderPath));
            CSharpFolderPath = getValue(nameof(CSharpFolderPath));
            BinaryFolderPath = getValue(nameof(BinaryFolderPath));
            InsertSqlFolderPath = getValue(nameof(InsertSqlFolderPath));
            SqlDataAndTableFolderPath = getValue(nameof(SqlDataAndTableFolderPath));
            LangPrefix = getValue(nameof(LangPrefix));

            JToken langToken;
            if (!config.TryGetValue(nameof(Lang), out langToken) || !(langToken is JArray))
            {
                langToken = new JArray();
                config[nameof(Lang)] = langToken;
                rewrite = true;
            }
            JArray langArray = langToken as JArray;
            Lang = new string[langArray.Count];
            for (int i = 0; i < langArray.Count; i++)
            {
                Lang[i] = langArray[i].ToString();
            }
            if (rewrite)
            {
                using (FileStream fs = new FileStream(jsonPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.SetLength(0);
                    byte[] writes = Encoding.UTF8.GetBytes(config.ToString());
                    fs.Write(writes, 0, writes.Length);
                }
            }
        }

        private static void LoadDLLKlassesData()
        {
            DLLKlasses.Clear();
            string usedInConfigFullName = typeof(UsedInConfig).FullName;
            foreach (var dllPath in DLLPath)
            {
                if (!File.Exists(dllPath))
                {
                    Entry.UpdateLogInfo($"Load DLL Path:{Path.GetFullPath(dllPath)} error file not exists.", LogLevelType.ErrorType);
                    continue;
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

        public static void ExportAll()
        {
            System.Diagnostics.Stopwatch swAll = new System.Diagnostics.Stopwatch();
            swAll.Restart();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();

            ExcelHandler.Export();
            ExcelHandler.ExtractData(true);

            sw.Stop();
            Entry.UpdateLogInfo(string.Format("ExcelHandler总共花费{0}ms.", sw.Elapsed.TotalMilliseconds));
            Console.WriteLine("ExcelHandler总共花费{0}ms.", sw.Elapsed.TotalMilliseconds);

            sw.Restart();
            var task1 = Task.Run(() => { CSharpHandler.Export(); });
            var task2 = Task.Run(() => { BinaryHandler.Export(); });
            var task3 = Task.Run(() =>
            {
                I18NHandlerExcel.ExportI18N();
                I18NHandlerExcel.ExtractI18NData();
                I18NHandlerBinary.ExportBinary();
            });

            var task4 = Task.Run(() => { VHandler.Export(); });

            var tasks = new Task[] { task1, task2, task3, task4 };
            Task.WaitAll(tasks);

            sw.Stop();
            Entry.UpdateLogInfo(string.Format("AllExport总共花费{0}ms.", sw.Elapsed.TotalMilliseconds));
            //Console.WriteLine("AllExport总共花费{0}ms.", sw.Elapsed.TotalMilliseconds);

            sw.Restart();

            ExcelHandler.ExtractData(false, true);

            sw.Stop();
            //Console.WriteLine("ExtractData总共花费{0}ms.", sw.Elapsed.TotalMilliseconds);
            Entry.UpdateLogInfo(string.Format("ExtractData总共花费{0}ms.", sw.Elapsed.TotalMilliseconds));
            sw.Restart();
            //ExcelHandler.ExtractCreateSqlData();

            sw.Stop();
            //Console.WriteLine("ExtractCreateSqlData{0}ms.", sw.Elapsed.TotalMilliseconds);
            Entry.UpdateLogInfo(string.Format("ExtractCreateSqlData{0}ms.", sw.Elapsed.TotalMilliseconds));
            sw.Restart();

            //SqlHandler.Export(false);

            sw.Stop();
            //Console.WriteLine("SqlHandler{0}ms.", sw.Elapsed.TotalMilliseconds);
            Entry.UpdateLogInfo(string.Format("SqlHandler{0}ms.", sw.Elapsed.TotalMilliseconds));
            sw.Restart();
            VersionHandler.WriteVersion(false);

            sw.Stop();
            //Console.WriteLine("VersionHandler{0}ms.", sw.Elapsed.TotalMilliseconds);
            Entry.UpdateLogInfo(string.Format("VersionHandler{0}ms.", sw.Elapsed.TotalMilliseconds));
            GC.Collect();
            swAll.Stop();
            //Console.WriteLine("doall_client_Click总共花费{0}ms.", swAll.Elapsed.TotalMilliseconds);
            Entry.UpdateLogInfo(string.Format("doall_client_Click总共花费{0}ms.", swAll.Elapsed.TotalMilliseconds));

        }
    }
}
