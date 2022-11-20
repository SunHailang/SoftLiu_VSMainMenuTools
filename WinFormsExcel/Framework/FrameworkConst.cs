using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public static class FrameworkConst
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        public const string DLLNAME = "__Internal";
#else
        public const string DLLNAME = "xlua";
#endif

        public static string FILE_INFO = "FileInfo.json";

        public static string VERSION_FILE_NAME = "version.json";

#if UNITY_EDITOR
        public static string GetEditorPath()
        {
                return $"{Application.dataPath}/../Tableex/Tools";
        }
        
        public static string GetEditorBytesPath(bool bPackOne = false)
        {
                if (!bPackOne)
                {
                        return $"{GetEditorPath()}/Binary";
                }
                
                return $"{GetEditorPath()}/BinaryPack";
        }

        public static string GetEditorVersionFile()
        {
                return $"{GetEditorPath()}/{VERSION_FILE_NAME}"; 
        }

        public static string GetBuildConfigFile()
        {
                return $"{GetEditorPath()}/BuildConfig.json";
        }
        
#endif

        public static string GetLocalPath()
        {
            return $"{WinFormsExcel.configPath}/TableData";
        }

        public static string GetPackagePath()
        {
            return $"{WinFormsExcel.configPath}/TableData/";
        }

        public static string GetPackageVersionFile()
        {
            return $"{GetPackagePath()}{VERSION_FILE_NAME}";
        }
    }
}
