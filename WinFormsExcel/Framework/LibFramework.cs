using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class LibFramework
    {
        private static LibFramework libFramework;
        private static Table table;
        private static VTables vTables;

        public static bool bInitOK = false;

        public string ComponentName => "LibFramework";

        public LibFramework()
        {
        }

        public void LibFrameworkInit(bool bEditorUsePacked)
        {
            string tablePath = $"{WinFormsExcel.WinFormsExcel.configPath}BinaryFolderPath";
            InitNative(tablePath);
        }

        void InitNative(string tablePath)
        {
            ExcelTConfig.Entry.UpdateLogInfo($"InitNative Path:{tablePath}");
            NativeFramework.LibFrameworkInit("", $"{tablePath}/");
        }

        internal void Disable()
        {
            NativeFramework.LibFrameworkDestroy();
        }

        public static void Init(bool bFromLauncher = false, bool bEditorUsePacked = false)
        {
            bool bValidInit = true;
            //非editor下只有从configtablelaucher处调用才是合法初始化
#if !UNITY_EDITOR
            bValidInit = bFromLauncher;
#endif

            if (!bValidInit)
            {
                Console.WriteLine("非法初始化");
                return;
            }

            if (bInitOK)
            {
                return;
            }



            libFramework = new LibFramework();
            libFramework.LibFrameworkInit(bEditorUsePacked);
            table = new Table();
            vTables = new VTables();

            bInitOK = true;
        }

        public static void Destroy()
        {
            bInitOK = false;
            vTables.Disable();
            table.Disable();
            libFramework.Disable();
        }


    }
}
