using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ExcelTConfig.Base
{
    public class Klass : IEnumerable<Property>
    {
        public enum KlassType
        {
            Config,
            Struct,
            Enum,
            Static,
        }

        public enum ListType
        {
            IDKeyMap,
            List,
            CodeNameKeyMap,
        }


        public string name { get; private set; }
        public KlassType type { get; private set; }
        public string description { get; private set; }
        public string[] excels { get; private set; }
        public string[] sheets { get; private set; }
        public ListType listType { get; private set; }

        public bool hasClassifyKey { get { return classifyKeyProperty != null; } }
        public bool isKeyValuePair { get; private set; }
        public bool isPersistentEnum { get; private set; }
        public string enumCustomDataValidation { get; private set; }
        public Attribute[] attributes { get; private set; }
        public bool isExternal => externalFullname != null;
        public string externalFullname { get; private set; }

        private bool everCalculateMetaHash;
        public int metaHash { get; private set; }

        public Dictionary<string, string> fixHashInfo;

        public Config.ExcelConfigAttribute.ScriptType exportScriptType;

        public Property idProperty { get; private set; }
        public Property designNameProperty { get; private set; }
        public Property codeNameProperty { get; private set; }
        public Property classifyKeyProperty { get; private set; }
        public Property groupKeyProperty { get; private set; }
        public Property markAsIdProperty { get; private set; }
        public int idPropertyIndex { get; private set; } = -1;
        public int codeNamePropertyIndex { get; private set; } = -1;
        public int designNamePropertyIndex { get; private set; } = -1;
        public int classifyKeyPropertyIndex { get; private set; } = -1;
        public int groupKeyPropertyIndex { get; private set; } = -1;

        public List<Property> baseTypes { get; private set; } = new List<Property>();

        public List<string> otherNames { get; private set; } = new List<string>();
        public int rawWidth { get => baseTypes.Count; }

        public int width { get; private set; }
        public int depth { get; private set; }

        public List<Property> rawProperties { get; private set; } = new List<Property>();
        public List<Property> properties { get; private set; } = new List<Property>();
        public List<Property> serverOnlyProperties { get; private set; } = new List<Property>();
        public List<Property> clientOnlyProperties { get; private set; } = new List<Property>();


        public List<object[]> data { get; private set; }
        public LineInfo[] lineInfos { get; private set; }

        private Dictionary<int, int> id2IndexMap;

        public List<KeyValuePair<string, string>> dbPropertyInfo { get; private set; }


        public Klass(KlassInfo klassInfo)
        {
            name = klassInfo.name;
            type = klassInfo.type;
            description = klassInfo.description;
            if (klassInfo.excels == null || klassInfo.excels.Length == 0)
            {
                excels = new string[] { name };
                sheets = new string[] { name };
            }
            else
            {
                excels = klassInfo.excels;
                sheets = klassInfo.sheets;
                for (int it = 0; it < excels.Length; it++)
                {
                    if (string.IsNullOrEmpty(excels[it])) excels[it] = name;
                    if (string.IsNullOrEmpty(sheets[it])) sheets[it] = name;
                }
            }
            listType = klassInfo.listType;
            isKeyValuePair = klassInfo.isKeyValuePair;
            isPersistentEnum = klassInfo.isPersistentEnum;
            enumCustomDataValidation = klassInfo.EnumCustomDataValidation;

            attributes = klassInfo.attrbutes.ToArray();
            externalFullname = klassInfo.externalFullname;
            metaHash = klassInfo.metaHash;
            fixHashInfo = klassInfo.fixHashInfo;

            var exportScripts = GetAttribute<Config.ExcelConfigAttribute.ExportScripts>();
            exportScriptType = exportScripts == null ? Config.ExcelConfigAttribute.ScriptType.ALL : exportScripts.type;

            foreach (var propertyInfo in klassInfo.properties)
            {
                var property = new Property(propertyInfo);
                properties.Add(property);
            }

            if (type == KlassType.Config)
            {
                int tIndex;

                tIndex = properties.FindIndex(p => p.name == Property.designNameProperty.name || p.HasAttribute<Config.ExcelConfigAttribute.MarkAsDesignName>());
                if (tIndex >= 0)
                {
                    designNameProperty = properties[tIndex];
                    properties.RemoveAt(tIndex);
                    properties.Insert(0, designNameProperty);
                    AssertTypeEqual(designNameProperty, Property.designNameProperty);
                    if (designNameProperty.commentOnly) throw new Exception($"{nameof(designNameProperty)} should not be CommentOnly! klass: {name}");
                }

                tIndex = properties.FindIndex(p => p.name == Property.codeNameProperty.name || p.HasAttribute<Config.ExcelConfigAttribute.MarkAsCodeName>());
                if (tIndex >= 0)
                {
                    codeNameProperty = properties[tIndex];
                    properties.RemoveAt(tIndex);
                    properties.Insert(0, codeNameProperty);
                    AssertTypeEqual(codeNameProperty, Property.codeNameProperty);
                    if (codeNameProperty.commentOnly) throw new Exception($"{nameof(codeNameProperty)} should not be CommentOnly! klass: {name}");
                }

                tIndex = properties.FindIndex(p => p.name == Property.idProperty.name);
                if (tIndex < 0)
                {
                    tIndex = properties.FindIndex(p => p.HasAttribute<Config.ExcelConfigAttribute.MarkAsID>());
                    if (tIndex >= 0)
                    {
                        markAsIdProperty = properties[tIndex];
                        markAsIdProperty.bMarkAsProperty = true;
                    }
                }

                tIndex = properties.FindIndex(p => p.name == Property.idProperty.name || p.HasAttribute<Config.ExcelConfigAttribute.MarkAsID>());
                if (tIndex >= 0)
                {
                    idProperty = properties[tIndex];
                    properties.RemoveAt(tIndex);
                }
                else idProperty = Property.idProperty;

                AssertTypeEqual(idProperty, Property.idProperty);
                properties.Insert(0, idProperty);

                classifyKeyProperty = properties.Find(p => p.isClassifyKey);
                groupKeyProperty = properties.Find(p => p.HasAttribute<Config.ExcelConfigAttribute.GroupKey>());
                if (groupKeyProperty != null)
                {
                    if (groupKeyProperty.name.Equals("id", StringComparison.OrdinalIgnoreCase)
                        || groupKeyProperty.name.Equals("index", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new ConfigException($"GroupKey Property's Name Must Not Be [id] or [index], klass: {name}, property: {groupKeyProperty.name}");
                    }

                    if (groupKeyProperty.isArray
                        || groupKeyProperty.isBasicArray
                        || (groupKeyProperty.refKlass != null && groupKeyProperty.refKlass.type != KlassType.Enum)
                        || (groupKeyProperty.isBasicType && groupKeyProperty.basicType != Property.BasicType.Integer))
                    {
                        throw new ConfigException($"GroupKey Property's Type Must Be (int or enum), klass: {name}, property: {groupKeyProperty.name}");
                    }
                }
            }
            else if (type == KlassType.Enum)
            {
                idProperty = Property.idProperty;
                codeNameProperty = Property.codeNameProperty;
                designNameProperty = Property.designNameProperty;

                properties.Add(idProperty);
                properties.Add(codeNameProperty);
                properties.Add(designNameProperty);

                if (klassInfo.isPersistentEnum) ResetData(klassInfo.data);
            }

            if (type == KlassType.Config && listType == ListType.CodeNameKeyMap && codeNameProperty == null)
            {
                throw new ConfigException(ErrorMessage.ListAsNameKeyMapButHasNoNameProperty, name);
            }

            rawProperties.AddRange(properties);

            for (int it = properties.Count - 1; it >= 0; it--)
            {
                var property = properties[it];
                if (property.bServerOnly)
                {
                    serverOnlyProperties.Add(property);
                }

                if (property.bClientOnly && !(property == idProperty || property == codeNameProperty || property == designNameProperty))
                {
                    clientOnlyProperties.Add(property);
                }
            }

            for (int it = properties.Count - 1; it >= 0; it--)
            {
                var property = properties[it];

                if ((property.commentOnly || property.bServerOnly)
                    && !(property == idProperty || property == codeNameProperty || property == designNameProperty))
                {
                    properties.RemoveAt(it);
                }
            }
        }

        public struct LineInfo
        {
            public int from; //[
            public int to;   //)
            public string excel;
            public string sheet;
        }

        public void ResetData(List<object[]> data)
        {
            this.data = data;
            ClearID2IndexMap();
            lineInfos = data == null ? null : new LineInfo[excels.Length];
        }

        public void ClearID2IndexMap()
        {
            id2IndexMap = null;
        }

        public void CalculateMetaHash()
        {
            if (everCalculateMetaHash) return;
            everCalculateMetaHash = true;
            StringBuilder sb = null;
            foreach (var property in rawProperties)
            {
                var refKlass = property.refKlass;
                if (refKlass == null) continue;
                if (sb == null) sb = new StringBuilder();
                sb.Append(refKlass.type);
                if (refKlass.type != KlassType.Struct && !refKlass.isPersistentEnum) continue;
                refKlass.CalculateMetaHash();
                sb.Append(refKlass.metaHash);
            }
            if (sb != null) metaHash ^= sb.ToString().AscIIHash();
        }

        public void ResetDbData(List<KeyValuePair<string, string>> dbPropertyInfo)
        {
            this.dbPropertyInfo = dbPropertyInfo;
        }


        private void AssertTypeEqual(Property property, Property target)
        {
            if (property.type != target.type)
            {
                throw new ConfigException(ErrorMessage.PropertyTypeNotCorrect, property.name, name, target.type);
            }
        }

        private Attribute dirtyAttribute;
        public T GetAttribute<T>() where T : Attribute
        {
            if (dirtyAttribute is T tt) return tt;

            foreach (var a in attributes)
            {
                if (a is T t)
                {
                    dirtyAttribute = a;
                    return t;
                }
            }
            return null;
        }

        public IEnumerator<Property> GetEnumerator() => properties.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => properties.GetEnumerator();

        public void Link()
        {
            foreach (var property in rawProperties)
            {
                property.LinkKlass(this);
            }
        }

        private bool everCalculateSize;
        public void CalculateSize()
        {
            if (everCalculateSize) return;
            everCalculateSize = true;

            int width = 0;
            int depth = 1;
            foreach (var property in properties)
            {
                int pWidth = 1;
                int pDepth = 1;

                if (property.isBasicType) pWidth = pDepth = 1;
                else
                {
                    var klassType = property.refKlass.type;

                    if (klassType == KlassType.Enum || klassType == KlassType.Config)
                    {
                        pWidth = pDepth = 1;
                        if (property.refKlass.hasClassifyKey) pDepth = 2;
                    }
                    else if (klassType == KlassType.Struct)
                    {
                        property.refKlass.CalculateSize();

                        pWidth = property.refKlass.width;
                        pDepth = property.refKlass.depth + 1;
                    }
                }

                if (property.isArray)
                {
                    pWidth *= property.arrayLength;
                    pDepth += 1;
                }

                width += pWidth;
                if (pDepth > depth) depth = pDepth;
            }

            this.width = width;
            this.depth = depth;
        }

        public void CalculateBaseTypes()
        {
            foreach (var property in rawProperties) CalculateBaseTypesOnProperty(property);

            if (type == KlassType.Config || type == KlassType.Enum)
            {
                if (idProperty != null) idPropertyIndex = baseTypes.IndexOf(idProperty);
                if (codeNameProperty != null) codeNamePropertyIndex = baseTypes.IndexOf(codeNameProperty);
                if (designNameProperty != null) designNamePropertyIndex = baseTypes.IndexOf(designNameProperty);
                if (classifyKeyProperty != null) classifyKeyPropertyIndex = baseTypes.IndexOf(classifyKeyProperty);
                if (groupKeyProperty != null) groupKeyPropertyIndex = baseTypes.IndexOf(groupKeyProperty);
            }
        }

        private void CalculateBaseTypesOnProperty(Property property, bool inArray = false)
        {
            if (property.isArray && !inArray)
            {
                for (int it = 0; it < property.arrayLength; it++)
                {
                    CalculateBaseTypesOnProperty(property, true);
                }
            }
            else if (property.isBasicType) baseTypes.Add(property);
            else if (property.refKlass.type == KlassType.Enum) baseTypes.Add(property);
            else if (property.refKlass.type == KlassType.Config)
            {
                if (property.refKlass.hasClassifyKey) baseTypes.Add(property.refKlass.classifyKeyProperty.clonedClassifyKeyProperty);
                baseTypes.Add(property);
            }
            else
            {
                foreach (var subProperty in property.refKlass.rawProperties) CalculateBaseTypesOnProperty(subProperty);
            }
        }

        public void SetOtherNames()
        {
            foreach (var property in rawProperties) SetOtherNameOnProperty(property, property.name);
        }
        private void SetOtherNameOnProperty(Property property, string name, int arraryIndex = -1)
        {
            if (arraryIndex != -1)
            {
                name = string.Format("{0}_{1}", name, arraryIndex);
            }

            if (property.isArray && arraryIndex == -1)
            {
                for (int it = 0; it < property.arrayLength; it++)
                {
                    SetOtherNameOnProperty(property, name, it);
                }
            }

            else if (property.isBasicType) otherNames.Add(name);
            else if (property.refKlass.type == KlassType.Enum) otherNames.Add(name);
            else if (property.refKlass.type == KlassType.Config)
            {
                throw new ConfigException("No define referece config current !!!");
            }
            else
            {
                foreach (var subProperty in property.refKlass.rawProperties)
                {
                    var newName = string.Format("{0}_{1}", name, subProperty.name);
                    SetOtherNameOnProperty(subProperty, newName);
                }
            }
        }

    }

    public class KlassInfo
    {
        public Klass.KlassType type;
        public string name;
        public string description;
        public string[] excels;
        public string[] sheets;
        public string group;
        public Klass.ListType listType;

        public bool isKeyValuePair;

        public List<PropertyInfo> properties = new List<PropertyInfo>();
        public List<object[]> data;
        public bool isPersistentEnum;
        public string EnumCustomDataValidation;

        public List<Attribute> attrbutes = new List<Attribute>();

        public string externalFullname;

        public int metaHash;

        public Dictionary<string, string> fixHashInfo;

        public static string GetKeyValuePairKlassName(string keyType, string valueType)
        {
            return $"<{keyType},{valueType}>";
        }

        public static KlassInfo GetKeyValuePairKlass(string keyType, string valueType)
        {
            var klass = new KlassInfo();

            klass.isKeyValuePair = true;
            klass.name = GetKeyValuePairKlassName(keyType, valueType);
            klass.type = Klass.KlassType.Struct;

            klass.properties.Add(new PropertyInfo() { type = keyType, name = "key", description = "Key" });
            klass.properties.Add(new PropertyInfo() { type = valueType, name = "value", description = "Value" });

            return klass;
        }
    }

    public class I18NKlass
    {
        public string declareKlassName { get; private set; }
        public string declarePropertyName { get; private set; }
        public string name { get; private set; }
        public int hashCode { get; private set; }

        public Dictionary<int, string[]> data;

        public I18NKlass(string declareKlassName, string declarePropertyName)
        {
            this.declareKlassName = declareKlassName;
            this.declarePropertyName = declarePropertyName;
            name = (declareKlassName + Util.GetToolTipsName(declarePropertyName)).Replace(" ", "");
            hashCode = name.AscIIHash();
            //Console.WriteLine($"name: {name}, hashcode: {hashCode}");
        }
    }
}
