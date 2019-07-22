using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    class FileUtils
    {

        /// <summary>
        /// 获得项目的根路径
        /// </summary>
        /// <returns></returns>
        public static string GetProjectRootPath()
        {
            string rootPath = "";
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory; // F:\project\WPF\AstroATE-PDR\04. 程序\01. 源代码\AstroATE\AstroATE\bin\Debug
            // 向上回退三级，得到需要的目录
            rootPath = BaseDirectoryPath.Substring(0, BaseDirectoryPath.LastIndexOf("\\")); // 第一个\是转义符，所以要写两个
            rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));   // 或者写成这种格式
            rootPath = rootPath.Substring(0, rootPath.LastIndexOf("\\")); // @"F:\project\WPF\AstroATE-PDR\04. 程序\01. 源代码\AstroATE\AstroATE
            return rootPath;
        }
    }
}
