﻿using SoftLiu_VSMainMenuTools.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public class JsonUtils : AutoGeneratedSingleton<JsonUtils>
    {
        private JavaScriptSerializer m_jsonScript = null;
        //Dictionary<string, object> dic = jss.Deserialize<Dictionary<string, object>>(js);

        public JsonUtils()
        {
            //实例化JavaScriptSerializer类的新实例
            m_jsonScript = new JavaScriptSerializer();
        }

        ~JsonUtils()
        {
            m_jsonScript = null;
        }

        public Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return m_jsonScript.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;
            }
        }

        public string DictionaryToJson(Dictionary<string, object> dicData)
        {
            try
            {
                return m_jsonScript.Serialize(dicData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                return null;
            }
        }

        public T JsonToObject<T>(string jsonData)
        {
            return m_jsonScript.Deserialize<T>(jsonData);
        }

        public string ObjectToJson(object objData)
        {
            return m_jsonScript.Serialize(objData);
        }
    }
}
