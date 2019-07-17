using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public class MD5Utils
    {
        /// <summary>
        /// 获取一个 MD5 字符串
        /// </summary>
        /// <param name="key">字符串类型</param>
        /// <returns></returns>
        public static string GetMD5String(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            MD5 md5 = MD5.Create();
            byte[] bts = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bts.Length; i++)
            {
                sb.Append(bts[i].ToString("X2"));
                if (i < bts.Length - 1)
                {
                    sb.Append(":");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">二进制流类型</param>
        /// <returns>MD5 字符串</returns>
        public static string GetMD5String(byte[] key)
        {
            if (key != null)
            {
                return null;
            }
            MD5 md5 = MD5.Create();
            byte[] bts = md5.ComputeHash(key);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bts.Length; i++)
            {
                sb.Append(bts[i].ToString("X2"));
                if (i < bts.Length - 1)
                {
                    sb.Append(":");
                }
            }
            return sb.ToString();
        }
    }
}
