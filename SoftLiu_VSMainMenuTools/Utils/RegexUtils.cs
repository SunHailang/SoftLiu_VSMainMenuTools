using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public class RegexUtils
    {

        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^1(3[0-9]|5[0-9]|7[6-8]|8[0-9])[0-9]{8}$");
        }
        public static bool IPCheck(string str_IP)
        {
            string num = "(25[0-5]|2[0-4]//d|[0-1]//d{2}|[1-9]?//d)";
            return Regex.IsMatch(str_IP, ("^" + num + "//." + num + "//." + num + "//." + num + "$"));
        }

        public bool IsEmail(string str_Email)
        {
            return Regex.IsMatch(str_Email, @"^([/w-/.]+)@((/[[0-9]{1,3}/.[0-9] {1,3}/.[0-9]{1,3}/.)|(([/w-]+/.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(/)?]$");
        }

        public bool IsUrl(string str_url)
        {
            return Regex.IsMatch(str_url, @"http(s)?://([/w-]+/.)+[/w-]+(/[/w- ./?%&=]*)?");
        }

    }
}
