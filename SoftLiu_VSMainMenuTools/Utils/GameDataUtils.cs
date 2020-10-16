using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public static class GameDataUtils
    {
        public static List<T> CreateInstances<T>(List<object> objects)
        {
            List<T> list = new List<T>(objects.Count);
            foreach (object instance in objects)
            {
                Dictionary<string, object> data = instance as Dictionary<string, object>;
                T obj = (T)System.Activator.CreateInstance<T>();
                ApplyData(obj, data, "", "");
                list.Add(obj);
            }
            return list;
        }

        // TODO: We may want to flag non serialisable (or use the Serialise tag) for variables we don't want saved out.
        private static Dictionary<string, object> GetData(object item)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            FieldInfo[] fieldList = GetFields(item.GetType());
            for (int i = 0; i < fieldList.Length; i++)
            {
                FieldInfo field = fieldList[i];
                string fieldName = field.Name;
                if (fieldName.StartsWith("m_"))
                {
                    fieldName = fieldName.Substring(2);
                }
                object value = field.GetValue(item);
                System.Type valueType = value.GetType();
                if (value is IEnumerable && valueType != typeof(string))
                {
                    System.Type listItemType;
                    if (valueType.IsArray)
                    {
                        listItemType = valueType.GetElementType();
                    }
                    else
                    {
                        listItemType = valueType.GetGenericArguments()[0];
                    }
                    bool isClassType = listItemType.IsClass && listItemType != typeof(string);
                    bool isGeneric = listItemType.IsGenericType;
                    bool isArray = listItemType.IsArray;
                    List<object> list = new List<object>();
                    IEnumerator enumerator = (value as IEnumerable).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        object listItem = enumerator.Current;
                        if (isClassType || isGeneric || isArray)
                        {
                            listItem = GetData(listItem);
                        }
                        list.Add(listItem);
                    }
                    value = list;
                }
                else if (valueType.IsClass && valueType != typeof(string))
                {
                    value = GetData(value);
                }
                else if (valueType.IsEnum)
                {
                    // Lets save Enums as integers. May want an option to save as strings?
                    value = (int)value;
                }
                data[fieldName] = value;
            }
            return data;
        }

        private static void ApplyData(object item, Dictionary<string, object> data, string baseName, string baseNameAlt)
        {
            FieldInfo[] fieldList = GetFields(item.GetType());
            for (int i = 0; i < fieldList.Length; i++)
            {
                FieldInfo field = fieldList[i];
                object dataValue = null;
                if (data.TryGetValue(baseName + field.Name, out dataValue) || (field.Name.StartsWith("m_") && data.TryGetValue(baseName + field.Name.Substring(2), out dataValue))
                   || data.TryGetValue(baseNameAlt + field.Name, out dataValue) || (field.Name.StartsWith("m_") && data.TryGetValue(baseNameAlt + field.Name.Substring(2), out dataValue)))
                {
                    object fieldValue = GetValue(field.FieldType, dataValue);
                    if (fieldValue != null)
                    {
                        field.SetValue(item, fieldValue);
                    }
                }
            }
        }

        private static object GetValue(System.Type fieldType, object data)
        {
            object value = null;
            // Generics
            if (fieldType.IsGenericType)
            {
                //List<>
                if (fieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (data is IList)
                    {
                        // Create list
                        IList newList = System.Activator.CreateInstance(fieldType) as IList;
                        // If we are a list of Classes, 2d array/list, not primitive types
                        System.Type listItemType = fieldType.GetGenericArguments()[0];
                        bool isClassType = listItemType.IsClass && listItemType != typeof(string);
                        bool isGeneric = listItemType.IsGenericType;
                        bool isArray = listItemType.IsArray;
                        // Add from list
                        foreach (object listValue in (data as IList))
                        {
                            object v = null;
                            if (isClassType || isGeneric || isArray)
                            {
                                v = GetValue(listItemType, listValue);
                            }
                            else
                            {
                                v = ChangeType(listValue, listItemType);
                            }
                            newList.Add(v);
                        }
                        value = newList;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect data format for a List<>. {0} but should be IList" + data.GetType().Name);
                    }
                }
                else
                {
                    Console.WriteLine("No support to read in the type " + fieldType.Name);
                }
                // No support for any other generics yet
            }
            // Array
            else if (fieldType.IsArray)
            {
                if (data is IList)
                {
                    // Create array
                    System.Array array = System.Array.CreateInstance(fieldType.GetElementType(), (data as IList).Count);
                    // If we are a list of Classes, 2d array/list, not primitive types
                    System.Type arrayItemType = fieldType.GetElementType();
                    bool isClassType = arrayItemType.IsClass && arrayItemType != typeof(string);
                    bool isGeneric = arrayItemType.IsGenericType;
                    bool isArray = arrayItemType.IsArray;
                    // Add from list
                    int i = 0;
                    foreach (object listValue in (data as IList))
                    {
                        object v = null;
                        if (isClassType || isGeneric || isArray)
                        {
                            v = GetValue(arrayItemType, listValue);
                        }
                        else
                        {
                            v = ChangeType(listValue, arrayItemType);
                        }
                        array.SetValue(v, i++);
                    }
                    value = array;
                }
                else
                {
                    value = null; // Empty arrays are now set to null
                                  //Debug.LogError("Incorrect data format for an array. {0} but should be IList" + data.GetType().Name);
                }
            }
            // Object field
            else if (fieldType.IsClass && fieldType != typeof(string))
            {
                Dictionary<string, object> objectData = data as Dictionary<string, object>;
                if (objectData != null)
                {
                    value = System.Activator.CreateInstance(fieldType);
                    ApplyData(value, objectData, "", "");
                }
                else
                {
                    Console.WriteLine("Incorrect data format for a class. {0} but should be Dictionary<string, object>" + data.GetType().Name);
                }
            }
            else if (fieldType == typeof(DateTime))
            {
                if (data != null)
                {
                    string strData = (string)data;
                    if (strData.Equals("")) //If it's null it's an error, if it's empty means default value.
                    {
                        value = DateTime.MinValue; //If field was left empty on DB, use a default value.
                    }
                    else if (strData != null)
                    {
                        DateTime dt = DateTime.Now;
                        if (DateTime.TryParse(strData, out dt))
                        {
                            value = (object)dt;
                        }
                        else
                        {
                            value = (object)DateTime.Now;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect data format for a Date. {0} but should be a valid DateTime using format DD/MM/YYYY hh:mm:ss" + data.GetType().Name);
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect data format for a Date. {0} but should be a valid DateTime using format DD/MM/YYYY hh:mm:ss" + data.GetType().Name);
                }
            }
            // Individual field
            else
            {
                try
                {
                    System.Type valueType = data.GetType();
                    if (fieldType.IsEnum && (valueType == typeof(int) || valueType == typeof(long)))
                    {
                        value = System.Enum.ToObject(fieldType, data);
                    }
                    else
                    {
                        value = ChangeType(data, fieldType);
                        if (fieldType.IsEnum)
                        {
                            if (value == null)
                            {
                                Console.WriteLine(string.Format("Unable to convert {0} to type {1}", data, fieldType));
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(string.Format("Unable to convert {0} to type {1}", data.GetType(), fieldType));
                }
            }
            return value;
        }

        private static FieldInfo[] GetFields(System.Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            /*if(type.BaseType != typeof(System.Object))
            {
                GetFields(fields, type.BaseType);
            }*/
        }

        private static object ChangeType(object value, System.Type type)
        {
            if (type.IsEnum)
                return System.Convert.ChangeType(System.Enum.Parse(type, value as string), type);
            else
                return System.Convert.ChangeType(value, type);
        }
    }
}