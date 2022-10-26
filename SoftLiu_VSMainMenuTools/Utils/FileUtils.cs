
using Microsoft.Win32;
/// <summary>
/// 
/// __author__ = "sun hai lang"
/// __date__ 2019-07-22
/// 
/// </summary>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public partial class FileUtils
    {

        /// <summary>
        /// 获得项目的根路径
        /// </summary>
        /// <returns></returns>
        public static string GetProjectRootPath()
        {
            // exe 运行文件夹目录
            //string environmentPath = Environment.CurrentDirectory;
            // 获取一个文件夹的父对象文件夹信息
            //string parentPath = Directory.GetParent(environmentPath).Parent.FullName;

            string rootPath = "";
            string BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory; // F:\project\WPF\AstroATE-PDR\04. 程序\01. 源代码\AstroATE\AstroATE\bin\Debug
            // 向上回退三级，得到需要的目录
            rootPath = BaseDirectoryPath.Substring(0, BaseDirectoryPath.LastIndexOf("\\")); // 第一个\是转义符，所以要写两个
            rootPath = rootPath.Substring(0, rootPath.LastIndexOf(@"\"));   // 或者写成这种格式
            rootPath = rootPath.Substring(0, rootPath.LastIndexOf("\\")); // @"F:\project\WPF\AstroATE-PDR\04. 程序\01. 源代码\AstroATE\AstroATE
            return rootPath;
        }

        /// <summary>
        /// 获取 一个文件夹下的所有文件  默认包含子文件夹
        /// </summary>
        /// <param name="dir">文件夹信息</param>
        /// <param name="subfile">是否包含子文件夹  默认包含</param>
        /// <returns></returns>
        public static FileInfo[] GetDirectorAllFiles(string directory, bool subfile = true)
        {
            DirectoryInfo dir = new DirectoryInfo(directory);
            if (dir == null)
            {
                return null;
            }
            List<FileInfo> fileList = new List<FileInfo>();
            if (subfile)
            {
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    fileList.AddRange(GetDirectorAllFiles(item.FullName, subfile));
                }
            }
            foreach (FileInfo item in dir.GetFiles())
            {
                fileList.Add(item);
            }
            return fileList.ToArray<FileInfo>();
        }

        /// <summary>
        /// 删除一个文件夹下的所有空文件夹
        /// </summary>
        /// <param name="directory">文件夹路径</param>
        /// <returns></returns>
        public static bool DeleteEmptyDirs(string directory)
        {
            bool didDelete = false;
            string[] directoriesdirectories = Directory.GetDirectories(directory);
            for (int i = 0; i < directoriesdirectories.Length; i++)
            {
                string dir = directoriesdirectories[i];
                int filecount = Directory.GetFiles(dir).Length + Directory.GetDirectories(dir).Length;
                if (filecount > 0)
                {
                    if (DeleteEmptyDirs(dir))
                    {
                        i--;
                    }
                }
                else
                {
                    Directory.Delete(dir);
                    didDelete = true;
                }
            }
            return didDelete;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_path"></param>
        /// <returns></returns>
        public static byte[] ReadFileBytes(string m_path, bool isCreate = false)
        {
            if (!File.Exists(m_path) && !isCreate)
            {
                return null;
            }
            byte[] bytes = null;
            try
            {
                using (FileStream stream = new FileStream(m_path, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    long leftLength = stream.Length;
                    bytes = new byte[leftLength];
                    int buffersize = 1024;
                    byte[] buffer = new byte[buffersize];

                    int rNum = 0;
                    int fileStart = 0;
                    while (leftLength > 0)
                    {
                        stream.Position = fileStart;
                        if (leftLength < buffersize)
                        {
                            rNum = stream.Read(buffer, 0, Convert.ToInt32(leftLength));
                        }
                        else
                        {
                            rNum = stream.Read(buffer, 0, buffersize);
                        }
                        if (rNum == 0)
                        {
                            break;
                        }
                        Array.Copy(buffer, 0, bytes, fileStart, rNum);
                        fileStart += rNum;
                        leftLength -= rNum;
                    }

                    //int len = (int)stream.Length;
                    //bytes = new byte[len];
                    //int readLend = stream.Read(bytes, 0, len);
                }
            }
            catch (Exception msg)
            {
                bytes = null;
                Console.WriteLine("Error: " + msg.Message);
            }

            return bytes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_bytes"></param>
        /// <param name="m_path"></param>
        /// <param name="create">如果没有 是否创建，默认创建 </param>
        /// <returns></returns>
        public static bool WriteFileBytes(byte[] m_bytes, string m_path, bool create = true)
        {
            FileMode f_Mode = FileMode.OpenOrCreate;
            if (!create)
            {
                f_Mode = FileMode.Open;
            }
            if (f_Mode == FileMode.Open)
            {
                if (!File.Exists(m_path))
                {
                    return false;
                }
            }
            try
            {
                using (FileStream stream = new FileStream(m_path, f_Mode, FileAccess.Write))
                {
                    stream.Write(m_bytes, 0, m_bytes.Length);
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine("Write File Bytes Error: " + msg.Message);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 设置开机自启动
        /// </summary>
        /// <param name="appName">名字</param>
        /// <param name="appPath">程序的路径</param>
        /// <param name="isCurrentUser">是否设置为当前用户</param>
        private static void AddStartSoft(string appName, string appPath, bool isCurrentUser = false)
        {
            if (isCurrentUser)
            {
                RegistryKey rKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (rKey.GetValue(appName) == null)
                {
                    rKey.SetValue(appName, appPath);
                }
            }
            else
            {
                RegistryKey rKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (rKey.GetValue(appName) == null)
                {
                    rKey.SetValue(appName, appPath);
                }
            }
        }
    }
}
