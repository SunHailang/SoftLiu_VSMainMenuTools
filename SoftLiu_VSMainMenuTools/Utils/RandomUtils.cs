using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public class RandomUtils
    {
        /// <summary>
        /// 获取 一个随机 int 值
        /// 左闭右开
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
        public static int Next()
        {
            Random rd = new Random(m_RandomSeed);
            return rd.Next();
        }
        /// <summary>
        /// 获取一个指定随机种子的 int 随机值
        /// 左闭右开
        /// </summary>
        /// <param name="seed">随机种子</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
        public static int Next(int seed)
        {
            Random rd = new Random(seed);
            return rd.Next();
        }
        /// <summary>
        /// 获取一个指定范围的 int 随机值
        /// 左闭右开
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>the range of return values includes minValue but not maxValue.</returns>
        public static int Next(int minValue, int maxValue)
        {
            Random rd = new Random(m_RandomSeed);
            return rd.Next(minValue, maxValue);
        }
        /// <summary>
        /// 获取一个指定范围的 int 随机值
        /// 左闭右开
        /// </summary>
        /// <param name="seed">随机种子</param>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>the range of return values includes minValue but not maxValue.</returns>
        public static int Next(int seed, int minValue, int maxValue)
        {
            Random rd = new Random(seed);
            return rd.Next(minValue, maxValue);
        }
        /// <summary>
        /// 获取一个随机种子
        /// </summary>
        public static int m_RandomSeed
        {
            get
            {
                byte[] bytes = new byte[4];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(bytes);
                return BitConverter.ToInt32(bytes, 0);
            }
        }
    }
}
