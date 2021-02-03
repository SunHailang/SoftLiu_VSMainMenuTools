using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public class RegexUtils
    {

        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber.Replace(" ", ""), @"^1(3[0-9]|5[0-9]|7[6-8]|8[0-9])[0-9]{8}$");
        }
        public static bool IPCheck(string str_IP)
        {
            //string num = "(25[0-5]|2[0-4]//d|[0-1]//d{2}|[1-9]?//d)";
            string num = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            return Regex.IsMatch(str_IP, num);
        }

        public bool IsEmail(string str_Email)
        {
            return Regex.IsMatch(str_Email, @"^([/w-/.]+)@((/[[0-9]{1,3}/.[0-9] {1,3}/.[0-9]{1,3}/.)|(([/w-]+/.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(/)?]$");
        }

        public bool IsUrl(string str_url)
        {
            return Regex.IsMatch(str_url, @"http(s)?://([/w-]+/.)+[/w-]+(/[/w- ./?%&=]*)?");
        }

        public static bool IsContainsInteger(string msgString)
        {
            return Regex.IsMatch(msgString, @"^[0-9]*$");
        }

        public static bool IsContainsLetter(string msgString)
        {
            return Regex.IsMatch(msgString, @"^[a-zA-Z]*$");
        }

        public static bool IsAllInteger(string msgString)
        {
            return Regex.IsMatch(msgString, @"^[0-9]+$");
        }

        public static bool IsAllLetter(string msgString)
        {
            return Regex.IsMatch(msgString, @"^[a-zA-Z]+$");
        }

        public static bool CheckCardID(string cardID, out int gender, out DateTime brithday, out string area, out string errorMsg)
        {
            cardID = cardID.ToUpper();
            errorMsg = "";
            gender = -1;
            brithday = DateTime.Now;
            area = "";
            if (cardID.Length != 18)
            {
                errorMsg = $"身份证号码长度错误，长度：{cardID.Length}";
                return false;
            }
            bool allInt = RegexUtils.IsAllInteger(cardID.Substring(0, 17));
            if (!allInt)
            {
                errorMsg = "身份证号码含有无效位数";
                return false;
            }

            // 第一、二位表示省（自治区、直辖市、特别行政区）。
            // 第三、四位表示市（地级市、自治州、盟及国家直辖市所属市辖区和县的汇总码）。其中，01-20，51-70表示省直辖市；21-50表示地区（自治州、盟）。
            // 第五、六位表示县（市辖区、县级市、旗）。01-18表示市辖区或地区（自治州、盟）辖县级市；21-80表示县（旗）；81-99表示省直辖县级市。
            /*
                身份证号码前六位表示编码对象常住户口所在县（市、镇、区）的行政区划代码。
                北京市|110000，天津市|120000，河北省|130000，山西省|140000，内蒙古自治区|150000，
                辽宁省|210000，吉林省|220000，黑龙江省|230000，
                上海市|310000，江苏省|320000，浙江省|330000，安徽省|340000，福建省|350000，江西省|360000，山东省|370000，
                河南省|410000，湖北省|420000，湖南省|430000，广东省|440000，广西壮族自治区|450000，海南省|460000，
                重庆市|500000，四川省|510000，贵州省|520000，云南省|530000，西藏自治区|540000，
                陕西省|610000，甘肃省|620000，青海省|630000，宁夏回族自治区|640000，新疆维吾尔自治区|650000，
                香港特别行政区|810000，澳门特别行政区|820000，台湾省|830000
             */
            string startSix = cardID.Substring(0, 6);
            string startTwo = cardID.Substring(0, 2);

            Dictionary<string, string> m_ProvinceID = new Dictionary<string, string>()
                {
                    { "11", "北京" },
                    { "12", "天津" },
                    { "13", "河北省" },
                    { "14", "山西省" },
                    { "15", "内蒙古自治区" },
                    { "21", "辽宁省" },
                    { "22", "吉林省" },
                    { "23", "黑龙江省" },
                    { "31", "上海" },
                    { "32", "江苏省" },
                    { "33", "浙江省" },
                    { "34", "安徽省" },
                    { "35", "福建省" },
                    { "36", "江西省" },
                    { "37", "山东省" },
                    { "41", "河南省" },
                    { "42", "湖北省" },
                    { "43", "湖南省" },
                    { "44", "广东省" },
                    { "45", "广西壮族自治区" },
                    { "46", "海南省" },
                    { "50", "重庆" },
                    { "51", "四川省" },
                    { "52", "贵州省" },
                    { "53", "云南省" },
                    { "54", "西藏自治区" },
                    { "61", "陕西省" },
                    { "62", "甘肃省" },
                    { "63", "青海省" },
                    { "64", "宁夏回族自治区" },
                    { "65", "新疆维吾尔自治区" },
                    { "81", "香港特别行政区" },
                    { "82", "澳门特别行政区" },
                    { "83", "台湾" },
                };
            if (!m_ProvinceID.ContainsKey(startTwo))
            {
                errorMsg = $"无效的地区序号, {startTwo}不存在.";
                return false;
            }
            area = m_ProvinceID[startTwo];

            int[] letterInt = new int[17];

            for (int i = 0; i < cardID.Length - 1; i++)
            {
                int.TryParse(cardID.Substring(i, 1), out letterInt[i]);
            }

            //int.TryParse(cardID.Substring(0, 1), out letterInt[0]);
            //int.TryParse(cardID.Substring(1, 1), out letterInt[1]);
            //int.TryParse(cardID.Substring(2, 1), out letterInt[2]);
            //int.TryParse(cardID.Substring(3, 1), out letterInt[3]);
            //int.TryParse(cardID.Substring(4, 1), out letterInt[4]);
            //int.TryParse(cardID.Substring(5, 1), out letterInt[5]);
            //int.TryParse(cardID.Substring(6, 1), out letterInt[6]);
            //int.TryParse(cardID.Substring(7, 1), out letterInt[7]);
            //int.TryParse(cardID.Substring(8, 1), out letterInt[8]);
            //int.TryParse(cardID.Substring(9, 1), out letterInt[9]);
            //int.TryParse(cardID.Substring(10, 1), out letterInt[10]);
            //int.TryParse(cardID.Substring(11, 1), out letterInt[11]);
            //int.TryParse(cardID.Substring(12, 1), out letterInt[12]);
            //int.TryParse(cardID.Substring(13, 1), out letterInt[13]);
            //int.TryParse(cardID.Substring(14, 1), out letterInt[14]);
            //int.TryParse(cardID.Substring(15, 1), out letterInt[15]);
            //int.TryParse(cardID.Substring(16, 1), out letterInt[16]);

            /*
                中国居民身份证校验码算法
                步骤如下：

                将身份证号码前面的17位数分别乘以不同的系数。从第一位到第十七位的系数分别为：7－9－10－5－8－4－2－1－6－3－7－9－10－5－8－4－2。
                将这17位数字和系数相乘的结果相加。
                用加出来和除以11，取余数。
                余数只可能有0－1－2－3－4－5－6－7－8－9－10这11个数字。其分别对应的最后一位身份证的号码为1－0－X－9－8－7－6－5－4－3－2。
                通过上面计算得知如果余数是3，第18位的校验码就是9。如果余数是2那么对应的校验码就是X，X实际是罗马数字10。
             */

            // 0~16 位的系数
            int[] modulus = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;
            for (int i = 0; i < modulus.Length; i++)
            {
                sum += letterInt[i] * modulus[i];
            }
            int endStarIndex = sum % 11;
            // 余数对应的校验位
            string[] endStrArray = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            string endStrChar = endStrArray[endStarIndex];

            string endStr = cardID.Substring(17, 1);
            if (endStrChar != endStr)
            {
                errorMsg = $"无效的身份证号， 应该是：{ endStrChar}， 不是：{ endStr}";
                return false;
            }
            // 第十五、十七位表示顺序码。对同地区、同年、月、日出生的人员编定的顺序号。其中第十七位奇数分给男性，偶数分给女性。
            gender = letterInt[16] % 2;

            // 第七、十四位表示出生年月日（单数字月日左侧用0补齐）。其中年份用四位数字表示，年、月、日之间不用分隔符。例如：1981年05月11日就用19810511表示。
            // 计算出生日期和年龄
            string birthday = cardID.Substring(6, 8);

            string maxDay = DateTime.Now.ToString("yyyyMMdd");
            string minDay = DateTime.Now.AddYears(-100).ToString("yyyyMMdd");

            if (birthday.CompareTo(minDay) < 0 || birthday.CompareTo(maxDay) > 0)
            {
                errorMsg = $"无效的身份证号,日期无效(年龄太大或出生日期大于当前日期), 日期：{birthday}";
                return false;
            }
            string year = birthday.Substring(0, 4);
            int yearInt = 0;
            int.TryParse(year, out yearInt);
            string month = birthday.Substring(4, 2);
            int monthInt = 0;
            int.TryParse(month, out monthInt);
            string day = birthday.Substring(6, 2);
            int dayInt = 0;
            int.TryParse(day, out dayInt);
            if (!ValidateDate(yearInt, monthInt, dayInt))
            {
                errorMsg = $"无效的出生日期, 年：{year}, 月：{month}, 日：{day}";
                return false;
            }
            brithday = new DateTime(yearInt, monthInt, dayInt);
            //age = CalculateAgeCorrect(birthdayDate, DateTime.Now);

            return true;
        }

        public static bool CreateCardID(string address, DateTime brithday)
        {

            return true;
        }

        private static bool ValidateDate(int year, int month, int day)
        {
            int[] a = { 31, (year % 4 == 0 && year % 100 != 0 || year % 400 == 0) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return month >= 1 && month <= 12 && day >= 1 && day <= a[month - 1];
        }

        public static int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }
    }
}
