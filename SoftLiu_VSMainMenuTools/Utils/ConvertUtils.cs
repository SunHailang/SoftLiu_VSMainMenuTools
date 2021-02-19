using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public static class ConvertUtils
    {

        public static List<int> To16thList(int value)
        {
            List<int> list = new List<int>();
            if (value / 16 > 0)
            {
                list.AddRange(To16thList(value / 16));
            }
            list.Add(value % 16);

            return list;
        }

        public static string To16thString(int value)
        {
            StringBuilder sb16 = new StringBuilder();
            List<int> list = ConvertUtils.To16thList(value);
            for (int i = 0; i < list.Count; i++)
            {
                int index = list[i];
                sb16.Append(ConvertUtils.To16thChar(list[i]));
            }
            int len = sb16.ToString().Length % 2;
            string head = "";
            if (len != 0)
            {
                head = "0";
            }
            return (head + sb16.ToString());
        }

        public static List<int> To2thList(int value)
        {
            List<int> list = new List<int>();
            if (value / 2 > 0)
            {
                list.AddRange(To2thList(value / 2));
            }
            list.Add(value % 2);
            return list;
        }

        public static int ToHexToInt(char value)
        {
            int ch = 0;
            switch (value)
            {
                case 'A':
                    ch = 10;
                    break;
                case 'B':
                    ch = 11;
                    break;
                case 'C':
                    ch = 12;
                    break;
                case 'D':
                    ch = 13;
                    break;
                case 'E':
                    ch = 14;
                    break;
                case 'F':
                    ch = 15;
                    break;
                default:
                    if (!int.TryParse(value.ToString(), out ch))
                    {
                        ch = -1;
                    }
                    break;
            }
            return ch;
        }

        public static char To16thChar(int value)
        {
            char ch = '0';
            switch (value)
            {
                case 10:
                    ch = 'A';
                    break;
                case 11:
                    ch = 'B';
                    break;
                case 12:
                    ch = 'C';
                    break;
                case 13:
                    ch = 'D';
                    break;
                case 14:
                    ch = 'E';
                    break;
                case 15:
                    ch = 'F';
                    break;
                default:
                    ch = value.ToString().ToCharArray(0, 1)[0];
                    break;
            }
            return ch;
        }


    }
}
