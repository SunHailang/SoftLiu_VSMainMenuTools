using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTConfig.Base
{
    public struct DataRangeInfo
    {
        public int columnFrom;
        public int columnTo;
    }

    public class Property
    {
        public enum BasicType
        {
            Integer = 1,
            Float,
            Boolean,
            String,
            UInt,
            Short,
            UShort,
            SByte,
            Byte,
        }
        private static readonly Dictionary<string, BasicType> basicTypeMap = new Dictionary<string, BasicType>()
        {
            {"int", BasicType.Integer },
            {"float", BasicType.Float },
            {"bool", BasicType.Boolean },
            {"string", BasicType.String },
            {"uint", BasicType.UInt},
            {"short", BasicType.Short},
            {"ushort", BasicType.UShort},
            {"sbyte", BasicType.SByte},
            {"byte", BasicType.Byte},
        };

        public static readonly Property idProperty = new Property("int", "id", "ID");
        public static readonly Property codeNameProperty = new Property("string", "codeName", "Code Name");
        public static readonly Property designNameProperty = new Property("string", "designName", "Design Name");

        #region 属性定义
        public string type { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }
        public bool isBasicType { get; private set; }
        public BasicType basicType { get; private set; }
        public bool isArray { get; private set; }
        public int arrayLength { get; private set; }
        public bool isDictionary { get; private set; }
        public string arrayDescription { get; private set; }
        public bool isI18N { get { return i18NKlass != null; } }
        public I18NKlass i18NKlass { get; private set; }
        public Klass refKlass { get; private set; }
        public Attribute[] attributes { get; private set; }
        public bool commentOnly { get; private set; }

        public bool isBasicArray, isBasic1DArray, isBasic2DArray;
        public bool isClassifyKey { get; private set; }

        public bool isFunctor { get; private set; }
        public string[] functorArgNames { get; private set; }

        public DataRangeInfo dataRange;

        public bool bClientOnly { get; private set; }
        public bool bServerOnly { get; private set; }

        public bool bMarkAsProperty;

        public bool bSplitString;
        public int splitType;

        public int specificType = -1;
        public int vLen;
        public bool bNegative;

        public bool markDelete = false;
        #endregion
        public Property(string type, string name, string description)
        {
            this.type = type;
            this.name = name;
            this.description = description;

            CheckBasicType();
        }
        public Property(PropertyInfo propertyInfo)
        {
            type = propertyInfo.type;
            name = propertyInfo.name;
            description = propertyInfo.description;

            isArray = propertyInfo.isArray;
            arrayLength = propertyInfo.arrayLength;
            arrayDescription = propertyInfo.arrayDescription;
            isDictionary = propertyInfo.isDictionary;

            i18NKlass = propertyInfo.i18NKlass;

            attributes = propertyInfo.attrbutes?.ToArray();

            commentOnly = propertyInfo.commentOnly;

            isBasic1DArray = propertyInfo.isBasic1DArray;
            isBasic2DArray = propertyInfo.isBasic2DArray;
            isBasicArray = isBasic1DArray || isBasic2DArray;

            bServerOnly = propertyInfo.bServerOnly;
            bClientOnly = propertyInfo.bClientOnly;

            bSplitString = propertyInfo.bSplitString;

            splitType = propertyInfo.spliteType;

            specificType = propertyInfo.specificType;
            vLen = propertyInfo.vLen;
            bNegative = propertyInfo.bNegative;

            markDelete = propertyInfo.markDelete;

            isClassifyKey = HasAttribute<Config.ExcelConfigAttribute.ClassifyKey>();

            CheckBasicType();

            var functor = GetAttribute<Config.ExcelConfigAttribute.Functor>();
            if (functor != null)
            {
                if (isI18N)
                {
                    throw new ConfigException("functor property must not be i18n string");
                }

                if (isArray || isBasicArray || !isBasicType || basicType != BasicType.String)
                {
                    throw new ConfigException("functor property type must be [string]");
                }
                isFunctor = true;
                functorArgNames = functor.argNames;
            }
        }

        private void CheckBasicType()
        {
            BasicType basicType;
            isBasicType = basicTypeMap.TryGetValue(type, out basicType);
            this.basicType = basicType;
        }

        public void LinkKlass(Klass myKlass)
        {
            if (isBasicType) return;

            Klass klass;
            if (Entry.klasses.TryGetValue(type, out klass)) refKlass = klass;
            else throw new ConfigException(ErrorMessage.CouldNotFindRefKlass, type, myKlass.name, name);

            if (isClassifyKey) clonedClassifyKeyProperty.refKlass = klass;
            if (isI18N && commentOnly) throw new ConfigException($"CommentOnly不能用于I18N类型属性, klass {myKlass.name} property {name}");
        }

        private Attribute dirtyAttribute;
        public T GetAttribute<T>() where T : Attribute
        {
            if (dirtyAttribute is T tt) return tt;

            if (attributes != null)
            {
                foreach (var a in attributes)
                {
                    if (a is T t)
                    {
                        dirtyAttribute = a;
                        return t;
                    }
                }
            }
            return null;
        }
        public bool HasAttribute<T>() where T : Attribute
        {
            return GetAttribute<T>() != null;
        }

        private Property _clonedClassifyKeyProperty;
        public Property clonedClassifyKeyProperty
        {
            get
            {
                return _clonedClassifyKeyProperty ?? (_clonedClassifyKeyProperty = new Property(new PropertyInfo
                {
                    type = type,
                    name = name,
                    description = description,
                    commentOnly = true,
                }));
            }
        }
    }

    public class PropertyInfo
    {
        public string type;
        public string name;
        public string description;
        public string arrayDescription;

        public int arrayLength = -1;
        public bool isArray;
        public bool isDictionary;
        public List<Attribute> attrbutes = new List<Attribute>();
        public I18NKlass i18NKlass;
        public bool commentOnly;
        public bool isBasic1DArray, isBasic2DArray;
        public bool bClientOnly;
        public bool bServerOnly;
        public bool bSplitString;
        public int spliteType;

        public int specificType = -1;
        public int vLen = -1;
        public bool bNegative;

        public bool markDelete = false;

    }
}
