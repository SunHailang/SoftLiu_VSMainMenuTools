using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelTConfig.Base
{
    public static class ErrorMessage
    {
        public const string JsonConfigFileNotExist = "Json Config File Not Exist";
        public const string JsonConfigFileFormatError = "Json Config File Format Error";
        public const string JsonConfigFileNoExcelFolderField = "Json Config File No Excel Folder Filed";
        public const string JsonConfigFileNoDllFileField = "Json Config File No DLL File Field or DLL File Field is not Array";
        public const string JsonConfigFileNoXXXXField = "Json Config File No {0} Field";

        public const string SqlConfigFileNoXXXXField = "Sql Config File No {0} Field";

        public const string SqlDataLengthNotEqualPropertyLen = "data length not correct table:{0} row {1}";

        public const string CreateSqlConfigFileNoXXXXField = "Create Sql Config File No {0} Field";
        public const string CreateSqlAndDataConfigFileNoXXXXField = "Create Sql&data Config File No {0} Field";

        public const string FolderNotExist = "{0} Folder Not Exist";
        public const string FileNotExist = "{0} File Not Exist";

        public const string UnSupportType = "Unsupport type [{0}] in class {1}, property {2}";

        public const string KlassNameCollide = "Exists More than 1 Class Named [{0}]";
        public const string KlassNameIllegal = "Class Name [{0}] Illegal";
        public const string PropertyNameIllegal = "Property Name [{0}] Illegal, Class [{1}]";

        public const string ListAsNameKeyMapButHasNoNameProperty = "Class [{0}] list as code name key map, but has no codeName property";

        public const string CouldNotFindRefKlass = "could not find ref class for type [{0}] in klass {1} property {2}";

        public const string PropertyTypeNotCorrect = "property named with {0} in class {1}, it's type should be {2}";

        public const string RangeAttributeError = "property named with {0} in class {1}, range attribute is error";

        public const string I18NKlassConflict = "i18n klass hash code conflict: [{0}] with [{1}]";

        public const string OpenExcelFail = "open excel [{0}] fail";

        public const string SheetTitleDamaged = "sheet [{0}]'s title was damaged";

        public const string SheetNeedRe_Export = "sheet [{0}] was dirty, need re-export";

        public const string BasicTypeFormatError = "excel [{0}.xlsx!{1}], cell[{2},{3}] [{4}] format error, reason: [{5}]";

        public const string RangeExceed = "range should limit to [{0}, {1}]";

        public const string KlassDataNull = "class [{0}]'s data is null";

        public const string RefTypeFormatError = "excel [{0}.xlsx!{1}], cell[{2},{3}] [{4}] format error, ref not find in class {5}";

        public const string LinkI18NWithNameKeyType = "class [{0}] ref I18N Type [{1}], key type should not be name-key";

        public const string Unknown = "Unknown Exception";

        public const string WorkBookParseError = "WorkBook is null check it";

    }

    class ConfigException : Exception
    {
        public ConfigException(string message) : base(message)
        {

        }

        public ConfigException(string message, params object[] args) : base(string.Format(message, args))
        {

        }
    }
}
