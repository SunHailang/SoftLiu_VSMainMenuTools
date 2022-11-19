using ExcelTConfig.Base;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelTConfig
{
    public class ExcelHandler
    {
        public const string DotXLSX = ".xlsx";

        public const string TrueString = "✔", FalseString = "✘";
        public const string TrueString2 = "TRUE", FalseString2 = "FALSE";

        public static readonly Color TitleBorderColor = Color.FromArgb(32, 32, 32);
        public static readonly Color DescriptionBackgroundColor = Color.FromArgb(70, 130, 180);
        public static readonly Color DescriptionFontColor = Color.White;

        public static readonly Color MarkDeleteColumBgColor = Color.FromArgb(100, 100, 100);

        public static readonly Color NameTypeBackgroundColor = Color.FromArgb(0, 139, 139);
        public static readonly Color NameTypeFontColor = Color.White;

        public const int InfoRow = 1, InfoColumn = 1;
        public const char BlankChar = (char)58800, BlankCharUpperLimit = (char)59200;

        public static string GetColumnLetter(int column) { var str = ExcelCellBase.GetAddress(1, column); return str.Substring(0, str.Length - 1); }
        public struct ExportInfo
        {
            public Klass klass;
            public string excel;
            public string sheet;
            public int dataLineIndex;
        }

        public static List<ExportInfo> CollectExportsInfo()
        {
            var ret = new List<ExportInfo>();
            foreach (var item in Entry.klasses)
            {
                var klass = item.Value;
                if (klass.type == Klass.KlassType.Struct || klass.isPersistentEnum) continue;

                for (int it = 0; it < klass.excels.Length; it++)
                {
                    var excel = klass.excels[it];
                    var sheet = klass.sheets[it];
                    ret.Add(new ExportInfo() { klass = klass, excel = excel, sheet = sheet, dataLineIndex = it });
                }
            }
            return ret;
        }

        public static void Export(HashSet<string> exportFiles = null)
        {
            var excelFiles = CollectExportsInfo().GroupBy(p => p.excel);
            bool reflushMetaHash = false;

            List<string> refreshKlass = new List<string>();
            var dictkalssDataInfo = new Dictionary<string, List<KlassSingleDataInfo>>();
            foreach (var excelPair in excelFiles)
            {
                // same group excel name
                var excelName = excelPair.Key as string;
                string filePath = Path.GetFullPath(Path.Combine(Entry.ExcelFolderPath, excelName + DotXLSX));
                if (exportFiles != null)
                {
                    if (!exportFiles.Contains(filePath)) continue;
                }
                else
                {
                    // for loop sheets
                    bool skip = true;
                    foreach (var group in excelPair)
                    {
                        var klass = group.klass;
                        int storedHash;
                        if (Entry.klassStoredMetaHashes.TryGetValue(klass.name, out storedHash) && storedHash == klass.metaHash)
                        {
                            // Do Nothing
                        }
                        else
                        {
                            reflushMetaHash = true;
                            skip = false;
                        }
                    }
                    if (skip) continue;
                }
                bool fileExists = File.Exists(filePath);
                using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    ExcelPackage excelPackage = new ExcelPackage(stream);
                    ExcelWorkbook workbook = excelPackage.Workbook;
                    List<int> hiddens = new List<int>();
                    foreach (var group in excelPair)
                    {
                        var klass = group.klass;
                        string sheetName = group.sheet;
                        ExcelWorksheet sheet = null;
                        sheet = workbook.Worksheets.SingleOrDefault(s => s.Name == sheetName);

                        bool bNewSheet = sheet == null;
                        ExcelWorksheet tempSheet = null;
                        if (bNewSheet)
                        {
                            sheet = workbook.Worksheets.Add(sheetName);
                        }
                        else
                        {
                            string tempName = "_&";
                            tempSheet = workbook.Worksheets.Copy(sheetName, tempName);

                            workbook.Worksheets.Delete(sheetName);

                            sheet = workbook.Worksheets.Add(sheetName);
                        }
                        if (sheet == null)
                        {
                            Entry.UpdateLogInfo($"Sheet Create failed: {sheetName}, Len:{sheetName.Length} (Max Length < 32 char).", LogLevelType.ErrorType);
                            continue;
                        }
                        SetoupSheetDefaultStyle(sheet, klass, bNewSheet);
                        int rowMax = klass.depth * 2;
                        int column = 1;
                        List<KlassSingleDataInfo> klassSingleDataInfos = new List<KlassSingleDataInfo>();
                        foreach (var property in klass.rawProperties)
                        {
                            column = HandleProperty(excelName, sheet, klass, property, 1, column, rowMax, klassSingleDataInfos, "");
                        }
                        if (!bNewSheet)
                        {
                            int dataRowMax = GetDataRowMax(rowMax, tempSheet);
                            HandleData(rowMax, dataRowMax, column - 1, klass, tempSheet, sheet, klassSingleDataInfos);

                            workbook.Worksheets.Delete(tempSheet);
                        }
                        int descColumn = 1;
                        int startColumn = descColumn;
                        hiddens.Clear();
                        foreach (var property in klass.rawProperties)
                        {
                            if (property.markDelete)
                            {
                                startColumn = descColumn;
                            }
                            descColumn = AddRowForDescription(sheet, property, rowMax + 1, descColumn, -1);
                            if (property.markDelete)
                            {
                                // 将开始~结束的列置灰
                                for (int i = startColumn; i < descColumn; i++)
                                {
                                    hiddens.Add(i);
                                }
                            }
                        }
                        int dbColumn = 1;
                        foreach (var property in klass.rawProperties)
                        {
                            dbColumn = AddRowForDescription(sheet, property, rowMax + 2, dbColumn, -1);
                        }
                        AddDataValidation(sheet, klass);
                        WriteSheetInfo(sheet, klass);

                        refreshKlass.Add(klass.name);

                        if (!dictkalssDataInfo.ContainsKey(klass.name))
                            dictkalssDataInfo.Add(klass.name, klassSingleDataInfos);

                        foreach (var item in hiddens)
                        {
                            // 隐藏当前列 但是用户可以主动取消隐藏
                            ExcelColumn eccelColumn = sheet.Column(item);
                            eccelColumn.Style.Hidden = true;
                            // 将隐藏的列填充背景色RGB(100,100,100)
                            sheet.Cells[1, item, ExcelPackage.MaxRows, item].Style.Fill.BackgroundColor.SetColor(MarkDeleteColumBgColor);
                        }
                    }
                    excelPackage.Save();
                }
            }
            if (reflushMetaHash)
            {
                DLLHandler.ReflushMetaHash();
                DLLHandler.RefreshClassProperticesInfo(refreshKlass, dictkalssDataInfo);
            }
        }

        private static void WriteSheetInfo(ExcelWorksheet sheet, Klass klass)
        {
            int info = klass.depth * 2;
            if (info == 0) return;

            var cell = sheet.Cells[InfoRow, InfoColumn];
            var content = cell.Value?.ToString();
            if (content == null) return;

            cell.Value = BlankChar + content + (char)(info + BlankChar);
        }

        private static void AddDataValidation(ExcelWorksheet ws, Klass klass)
        {
            ws.DataValidations.Clear();
            for (int jt = klass.rawWidth - 1; jt >= 0; jt--)
            {
                Property baseType = klass.baseTypes[jt];

                var columnString = GetColumnLetter(jt + 1);

                int startRow = 2 * klass.depth + 1 + 1 + 1;//一行供描述使用，一行供database使用
                int endRow = startRow + 20000;

                var range = ws.Cells[$"{columnString}{startRow}:{columnString}{endRow}"];
                if (baseType.isBasicType)
                {
                    if (baseType.isBasicArray)
                    {
                        //Do Nothing
                    }
                    else if (jt == klass.idPropertyIndex)
                    {
                        int minValue = int.MinValue;
                        int maxValue = int.MaxValue;

                        var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                        if (integerRange != null)
                        {
                            minValue = integerRange.min;
                            maxValue = integerRange.max;
                        }

                        string formula = string.Format("=AND(INT({0}{1})={0}{1},AND({0}{1}>={3},{0}{1}<={4}))",
                            columnString, startRow, endRow, minValue, maxValue
                        );

                        var dv = range.DataValidation.AddCustomDataValidation();
                        dv.Formula.ExcelFormula = formula;

                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"ID字段范围须介于{minValue}至{maxValue}";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";
                    }
                    else if (jt == klass.codeNamePropertyIndex)
                    {
                        string formula = string.Format(
                        "=AND(NOT(ISNUMBER(SEARCH(LEFT({0}{1}), \"0123456789\"))),ISNUMBER(SUMPRODUCT(SEARCH(MID({0}{1},ROW(INDIRECT(\"1:\"&LEN({0}{1}))),1),\"0123456789abcdefghijklmnopqrstuvwxyz_\"))))",
                        columnString,
                        startRow,
                        endRow
                        );

                        var dv = range.DataValidation.AddCustomDataValidation();
                        dv.Formula.ExcelFormula = formula;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = "CodeName字段必须符合命名规范";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";
                    }
                    else if (jt == klass.designNamePropertyIndex)
                    {
                        string formula = string.Format(
                        "=NOT(ISNUMBER({0}{1}))",
                        columnString,
                        startRow,
                        endRow
                        );

                        var dv = range.DataValidation.AddCustomDataValidation();
                        dv.Formula.ExcelFormula = formula;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = "DesignName字段必须不能为数字形式";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";
                    }
                    else if (baseType.basicType == Property.BasicType.Integer)
                    {
                        if (baseType.HasAttribute<Config.ExcelConfigAttribute.AsTimeStamp>())
                        {
                            var dv = range.DataValidation.AddDateTimeDataValidation();
                            dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.greaterThanOrEqual;
                            dv.Formula.Value = Util.unixTimeStampStartLocalDateTime;
                            dv.AllowBlank = true;
                            dv.ShowErrorMessage = true;
                            dv.Error = string.Format("请输入合法的时间格式： yyyy/MM/dd (hh:mm:ss)");
                            dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                            dv.ErrorTitle = "错误";
                        }
                        else
                        {
                            int minValue = int.MinValue;
                            int maxValue = int.MaxValue;

                            var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                            if (integerRange != null)
                            {
                                minValue = integerRange.min;
                                maxValue = integerRange.max;
                            }

                            var dv = range.DataValidation.AddIntegerDataValidation();
                            dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                            dv.Formula.Value = minValue;
                            dv.Formula2.Value = maxValue;
                            dv.AllowBlank = true;
                            dv.ShowErrorMessage = true;
                            dv.Error = $"请输入介于{minValue}与{maxValue}之间的整数";
                            dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                            dv.ErrorTitle = "错误";
                        }
                    }
                    else if (baseType.basicType == Property.BasicType.Float)
                    {
                        float minValue = float.MinValue;
                        float maxValue = float.MaxValue;

                        var floatRange = baseType.GetAttribute<Config.ExcelConfigAttribute.FloatRange>();
                        if (floatRange != null)
                        {
                            minValue = floatRange.min;
                            maxValue = floatRange.max;
                        }

                        var dv = range.DataValidation.AddDecimalDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = minValue;
                        dv.Formula2.Value = maxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入介于{minValue}与{maxValue}之间的数";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";
                    }
                    else if (baseType.basicType == Property.BasicType.Boolean)
                    {
                        var dv = range.DataValidation.AddListDataValidation();
                        dv.Formula.Values.Add(TrueString);
                        dv.Formula.Values.Add(FalseString);
                        //dv.Formula.Values.Add(TrueString2);
                        //dv.Formula.Values.Add(FalseString2);
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入{TrueString}/{FalseString}或{TrueString2}/{FalseString2}";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";
                    }
                    else if (baseType.basicType == Property.BasicType.UInt)
                    {
                        int minValue = int.MinValue;
                        int maxValue = int.MaxValue;

                        var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                        if (integerRange != null)
                        {
                            minValue = integerRange.min;
                            maxValue = integerRange.max;
                        }

                        var dv = range.DataValidation.AddIntegerDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = minValue;
                        dv.Formula2.Value = maxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入介于{minValue}与{maxValue}之间的整数";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";

                    }
                    else if (baseType.basicType == Property.BasicType.Short)
                    {
                        int minValue = int.MinValue;
                        int maxValue = int.MaxValue;

                        var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                        if (integerRange != null)
                        {
                            minValue = integerRange.min;
                            maxValue = integerRange.max;
                        }

                        var dv = range.DataValidation.AddIntegerDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = minValue;
                        dv.Formula2.Value = maxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入介于{minValue}与{maxValue}之间的整数";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";

                    }
                    else if (baseType.basicType == Property.BasicType.UShort)
                    {
                        int minValue = int.MinValue;
                        int maxValue = int.MaxValue;

                        var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                        if (integerRange != null)
                        {
                            minValue = integerRange.min;
                            maxValue = integerRange.max;
                        }

                        var dv = range.DataValidation.AddIntegerDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = minValue;
                        dv.Formula2.Value = maxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入介于{minValue}与{maxValue}之间的整数";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";

                    }
                    else if (baseType.basicType == Property.BasicType.SByte)
                    {
                        int minValue = int.MinValue;
                        int maxValue = int.MaxValue;

                        var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                        if (integerRange != null)
                        {
                            minValue = integerRange.min;
                            maxValue = integerRange.max;
                        }

                        var dv = range.DataValidation.AddIntegerDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = minValue;
                        dv.Formula2.Value = maxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入介于{minValue}与{maxValue}之间的整数";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";

                    }
                    else if (baseType.basicType == Property.BasicType.Byte)
                    {
                        int minValue = int.MinValue;
                        int maxValue = int.MaxValue;

                        var integerRange = baseType.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                        if (integerRange != null)
                        {
                            minValue = integerRange.min;
                            maxValue = integerRange.max;
                        }

                        var dv = range.DataValidation.AddIntegerDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = minValue;
                        dv.Formula2.Value = maxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = $"请输入介于{minValue}与{maxValue}之间的整数";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";

                    }
                }
                else
                {
                    var refKlass = baseType.refKlass;

                    //只允许id引用
                    if (refKlass.designNameProperty == null)
                    {
                        var dv = range.DataValidation.AddIntegerDataValidation();
                        dv.Operator = OfficeOpenXml.DataValidation.ExcelDataValidationOperator.between;
                        dv.Formula.Value = int.MinValue;
                        dv.Formula2.Value = int.MaxValue;
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = true;
                        dv.Error = "请输入合法的ID值";
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        dv.ErrorTitle = "错误";
                    }
                    else if (refKlass.isPersistentEnum)
                    {
                        if (!String.IsNullOrEmpty(refKlass.enumCustomDataValidation))
                        {
                            var dv = range.DataValidation.AddListDataValidation();
                            dv.Formula.ExcelFormula = refKlass.enumCustomDataValidation;
                            dv.AllowBlank = true;
                            dv.ShowErrorMessage = false;
                            dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                        }
                        else
                        {
                            var dv = range.DataValidation.AddListDataValidation();

                            foreach (var row in refKlass.data)
                            {
                                dv.Formula.Values.Add(row[2] as string);
                            }

                            dv.AllowBlank = true;
                            dv.ShowErrorMessage = false;
                            dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.undefined;
                        }

                    }
                    //级联引用
                    else if (refKlass.hasClassifyKey)
                    {
                        int irowDelta = -2 * refKlass.depth - 1;
                        string rowDelta = irowDelta == 0 ? "" : irowDelta < 0 ? irowDelta.ToString() : "+" + irowDelta;

                        var excelFile = refKlass.excels[0];
                        int splashIndex = excelFile.LastIndexOf('/');
                        if (splashIndex >= 0) excelFile = excelFile.Substring(splashIndex + 1);

                        string cCol = GetColumnLetter(refKlass.classifyKeyPropertyIndex + 1);
                        int cRow = refKlass.depth * 2 + 1 + 1 + 1;//一行供描述使用,一行供dataBase使用
                        string lCol = GetColumnLetter(jt - 1 + 1);
                        int row = startRow;
                        int colDelta = refKlass.designNamePropertyIndex - refKlass.classifyKeyPropertyIndex;

                        var columnStr = GetColumnLetter(refKlass.designNamePropertyIndex + 1);

                        string classifyKeyFormula = $"OFFSET(INDIRECT(\"[{excelFile}.xlsx]{refKlass.name}!${cCol}{cRow}\"),MATCH({lCol}{row},INDIRECT(\"[{excelFile}.xlsx]{refKlass.name}!${cCol}:${cCol}\"),0){rowDelta},{colDelta},COUNTIF(INDIRECT(\"[{excelFile}.xlsx]{refKlass.name}!${cCol}:${cCol}\"),{lCol}{row}))";
                        string rawFormula = $"INDIRECT(\"[{excelFile}{DotXLSX}]{refKlass.name}!${columnStr}${2 * refKlass.depth + 1 + 1}:${columnStr}${2 * refKlass.depth + 1 + 1 + 20000}\")";
                        string formula = $"=IF(ISBLANK({lCol}{row}),{rawFormula},{classifyKeyFormula})";

                        var dv = range.DataValidation.AddListDataValidation();
                        dv.Formula.ExcelFormula = formula;

                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = false;
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.undefined;
                    }
                    //允许id引用与下拉引用
                    else
                    {
                        var dv = range.DataValidation.AddListDataValidation();
                        var columnStr = GetColumnLetter(refKlass.designNamePropertyIndex + 1);

                        string excelFile;
                        string sheetName;
                        FindRefExcel(baseType, out excelFile, out sheetName);

                        int splashIndex = excelFile.LastIndexOf('/');
                        if (splashIndex >= 0) excelFile = excelFile.Substring(splashIndex + 1);

                        dv.Formula.ExcelFormula =
                            $"=INDIRECT(\"[{excelFile}{DotXLSX}]{sheetName}!${columnStr}${2 * refKlass.depth + 1 + 1}:${columnStr}${2 * refKlass.depth + 1 + 1 + 20000}\")";
                        dv.AllowBlank = true;
                        dv.ShowErrorMessage = false;
                        dv.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.undefined;
                    }
                }
            }
        }


        private static int AddRowForDescription(ExcelWorksheet sheet, Property property, int row, int column, int arrayIndex)
        {
            int ret = column + 1;

            if (property.isArray && arrayIndex == -1)
            {
                int pColumn = column;
                for (int it = 0; it < property.arrayLength; it++)
                {
                    pColumn = AddRowForDescription(sheet, property, row, pColumn, it);
                }

                ret = pColumn;
            }
            else if (property.refKlass != null && property.refKlass.type == Klass.KlassType.Struct)
            {
                Klass klass = property.refKlass;

                int pColumn = column;
                foreach (var pProperty in klass.rawProperties)
                {
                    pColumn = AddRowForDescription(sheet, pProperty, row, pColumn, -1);
                }

                ret = pColumn;
            }
            else
            {
                FillDescriptionInfo(sheet, property, row, column);
            }

            return ret;
        }

        static void FillDescriptionInfo(ExcelWorksheet sheet, Property property, int row, int column)
        {
            var descCell = sheet.Cells[row, column];
            descCell.Style.Border.BorderAround(ExcelBorderStyle.Medium, TitleBorderColor);
            descCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            descCell.Style.Fill.BackgroundColor.SetColor(DescriptionBackgroundColor);
        }

        static void HandleData(int titleRowMax, int dataRowMax, int colMax, Klass klass, ExcelWorksheet tempSheet, ExcelWorksheet targetSheet,
            List<KlassSingleDataInfo> newklassSingleDataInfos)
        {
            KlassDataInfo info;

            if (!Entry.classPropertiesInfo.TryGetValue(klass.name, out info))
            {
                //return;
                // 如果是拷贝进来的新表，则将表的本身内容拷贝一份
                info = new KlassDataInfo();
                info.rowBegin = titleRowMax + 1;
                info.listDatas = newklassSingleDataInfos;
            }

            int dataRowBegin = titleRowMax + 1;
            int prevRowBegin = info.rowBegin;
            if (prevRowBegin == 0)
            {
                prevRowBegin = dataRowBegin;
            }
            Dictionary<string, DataRangeInfo> propOldDataRange = new Dictionary<string, DataRangeInfo>();

            int oldMaxCol = -1;

            foreach (var propInfo in info.listDatas)
            {
                if (propInfo.range.columnTo > oldMaxCol)
                    oldMaxCol = propInfo.range.columnTo;

                if (propInfo.name == null)
                    continue;

                if (propOldDataRange.ContainsKey(propInfo.name))
                {
                    throw new ConfigException("Old Data description has the same key");
                }
                propOldDataRange.Add(propInfo.name, propInfo.range);
            }

            foreach (var newDataInfo in newklassSingleDataInfos)
            {
                if (propOldDataRange.TryGetValue(newDataInfo.name, out DataRangeInfo oldInfo))
                {
                    int diffRow = dataRowBegin - prevRowBegin;

                    if (diffRow != 0)
                    {
                        Console.WriteLine($"Copy Error {diffRow}, {targetSheet.Name}");
                    }
                    var targetCells = targetSheet.Cells[dataRowBegin, newDataInfo.range.columnFrom, dataRowMax, newDataInfo.range.columnTo];
                    var orginCells = tempSheet.Cells[prevRowBegin, oldInfo.columnFrom, dataRowMax - diffRow, oldInfo.columnTo];
                    orginCells.Copy(targetCells);
                }
                else
                {
                    targetSheet.Cells[dataRowBegin, newDataInfo.range.columnFrom, dataRowMax, newDataInfo.range.columnTo].Value = null;
                }
            }
        }

        static int GetDataRowMax(int titleRowMax, ExcelWorksheet sheet)
        {
            int emptyLine = 0;
            int dataRowBegin = titleRowMax + 1;
            int dataRowEnd = dataRowBegin;

            for (int row = dataRowBegin; ; row++)
            {
                string firstValue = sheet.Cells[row, 1].Value?.ToString();
                if (string.IsNullOrEmpty(firstValue))
                {
                    emptyLine++;

                    if (emptyLine > 20) break;
                    continue;
                }

                emptyLine = 0;

                dataRowEnd = row;
            }

            return dataRowEnd;
        }

        private static void SetoupSheetDefaultStyle(ExcelWorksheet sheet, Klass klass, bool newSheet)
        {
            sheet.DefaultColWidth = 16f;
            sheet.Cells.Style.WrapText = true;
            sheet.DefaultRowHeight = 16f;

            for (int i = 1; i < klass.rawWidth; i++)
            {
                var style = sheet.Column(i).Style;

                style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                style.VerticalAlignment = ExcelVerticalAlignment.Center;
                style.Fill.PatternType = ExcelFillStyle.Solid;
                style.Fill.BackgroundColor.SetColor(Color.FromArgb(245, 245, 245));
                style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                style.Border.Bottom.Color.SetColor(Color.FromArgb(192, 192, 192));
                style.Border.Right.Style = ExcelBorderStyle.Thin;
                style.Border.Right.Color.SetColor(Color.FromArgb(192, 192, 192));
            }
            if (newSheet)
            {
                sheet.View.FreezePanes(klass.depth * 2 + 1, 1);
            }
        }

        private static int HandleProperty(string excelPath, ExcelWorksheet sheet, Klass ownerKlass, Property property, int row, int column, int rowMax,
            List<KlassSingleDataInfo> klassSingleDataInfos, string storageName, int arrayIndex = -1, bool expandClassifyKey = true)
        {
            int ret = column + 1;
            int width = 1;
            int lastRow = rowMax;

            string description = property.description;
            string name = property.name;
            string type = property.type;

            if (arrayIndex >= 0)
            {
                name = $"{name}_{arrayIndex}";
                description = $"{description} {arrayIndex}";
            }

            storageName = $"{storageName}@{name}";
            ExcelHyperLink hyperLink = null;
            bool bTail = true;
            if (property.isArray && arrayIndex == -1)
            {
                lastRow = row + 1;
                if (property.isDictionary) type = $"Dictionary{type}";
                else type += "[]";

                if (property.arrayDescription != null) description = property.arrayDescription;
                width = property.arrayLength;
                if (property.refKlass != null)
                {
                    var refKlass = property.refKlass;
                    if (refKlass.type == Klass.KlassType.Struct) width *= refKlass.rawWidth;
                    else if (refKlass.type == Klass.KlassType.Config)
                    {
                        if (refKlass.hasClassifyKey) width *= 2;
                    }
                }
                int pColumn = column;
                for (int it = 0; it < property.arrayLength; it++)
                {
                    pColumn = HandleProperty(excelPath, sheet, ownerKlass, property, row + 2, pColumn, rowMax, klassSingleDataInfos, storageName, it);
                }
                ret = column + width;
                bTail = false;
            }
            else if (property.isBasicArray)
            {
                if (property.isBasic1DArray) type = $"Array<{property.type}>";
                else type = $"Array2D<{property.type}>";
            }
            else if (property.isBasicType)
            {
                if (property.basicType == Property.BasicType.Integer)
                {
                    if (property.HasAttribute<Config.ExcelConfigAttribute.AsTimeStamp>()) type += Config.ExcelConfigAttribute.AsTimeStamp.TypeName;
                }
                else if (property.basicType == Property.BasicType.String)
                {
                    if (property.isI18N)
                    {
                        type = Config.ExcelConfigAttribute.MarkI18N.TypeName;
                        var relativePath = Util.CalculateRelativePath(Path.Combine(Entry.ExcelFolderPath, excelPath), Path.Combine(Entry.I18NExcelFolderPath, $"{ownerKlass.excels[0]}I18N"));
                        hyperLink = new ExcelHyperLink($"{relativePath}.xlsx#{property.i18NKlass.name}!A1", UriKind.Relative);
                    }
                }
            }
            else if (property.refKlass.hasClassifyKey && expandClassifyKey && property.refKlass.type == Klass.KlassType.Config)
            {
                width = 2;
                lastRow = row + 1;
                HandleProperty(excelPath, sheet, ownerKlass, property.refKlass.classifyKeyProperty.clonedClassifyKeyProperty, row + 2, column, rowMax, klassSingleDataInfos, storageName, arrayIndex, false);
                HandleProperty(excelPath, sheet, ownerKlass, property, row + 2, column + 1, rowMax, klassSingleDataInfos, storageName, arrayIndex, false);
                ret = column + 2;
                bTail = false;
            }
            else if (property.refKlass.type == Klass.KlassType.Config || property.refKlass.type == Klass.KlassType.Enum)
            {
                if (!property.refKlass.isPersistentEnum)
                {
                    string refExcel, refSheet;
                    FindRefExcel(property, out refExcel, out refSheet);
                    var relativePath = Util.CalculateRelativePath(Path.Combine(Entry.ExcelFolderPath, excelPath), Path.Combine(Entry.ExcelFolderPath, refExcel));
                    hyperLink = new ExcelHyperLink($"{relativePath}.xlsx#{refSheet}!A1", UriKind.Relative);
                }
            }
            else if (property.refKlass.type == Klass.KlassType.Struct)
            {
                Klass klass = property.refKlass;

                width = klass.rawWidth;
                lastRow = row + 1;

                int pColumn = column;
                foreach (var pProperty in klass.rawProperties)
                {
                    pColumn = HandleProperty(excelPath, sheet, ownerKlass, pProperty, row + 2, pColumn, rowMax, klassSingleDataInfos, storageName);
                }
                ret = column + width;
                bTail = false;
            }
            if (property.commentOnly) type += Config.ExcelConfigAttribute.CommentOnly.TypeName;

            if (bTail)
            {
                KlassSingleDataInfo dataInfo;
                dataInfo.name = storageName;
                dataInfo.range.columnFrom = column;
                dataInfo.range.columnTo = ret - 1;
                klassSingleDataInfos.Add(dataInfo);
            }
            property.dataRange.columnFrom = column;
            property.dataRange.columnTo = ret - 1;

            bool descriptionCellCrossCell = ret > column + 1;
            var descriptionCell = descriptionCellCrossCell ? sheet.Cells[row, column, row, ret - 1] : sheet.Cells[row, column];
            if (descriptionCellCrossCell) descriptionCell.Merge = true;
            if (hyperLink != null) descriptionCell.Hyperlink = hyperLink;
            descriptionCell.Value = description;
            descriptionCell.Style.Border.BorderAround(ExcelBorderStyle.Medium, TitleBorderColor);
            descriptionCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            descriptionCell.Style.Fill.BackgroundColor.SetColor(DescriptionBackgroundColor);
            descriptionCell.Style.Font.Color.SetColor(DescriptionFontColor);
            descriptionCell.Style.Font.Bold = true;

            bool nameTypeCellCrossCell = ret > column + 1 || lastRow > row + 1;
            ExcelRange nameTypeCell = nameTypeCellCrossCell ? sheet.Cells[row + 1, column, lastRow, ret - 1] : sheet.Cells[row + 1, column];
            if (nameTypeCellCrossCell) nameTypeCell.Merge = true;
            //if (hyperLink != null) nameTypeCell.Hyperlink = hyperLink;
            // 先设置文本
            nameTypeCell.RichText.Clear();
            var t = nameTypeCell.RichText.Add(name);
            t.Bold = true;
            t.Color = NameTypeFontColor;
            t.UnderLine = false;

            t = nameTypeCell.RichText.Add(" ");
            t.UnderLine = false;

            t = nameTypeCell.RichText.Add(type);
            t.Color = NameTypeFontColor;
            t.Italic = true;
            t.Bold = false;
            t.UnderLine = true;
            t.Color = NameTypeFontColor;
            nameTypeCell.IsRichText = true;
            // 设置单元格的配置
            nameTypeCell.Style.Border.BorderAround(ExcelBorderStyle.Medium, TitleBorderColor);
            nameTypeCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            nameTypeCell.Style.Fill.BackgroundColor.SetColor(NameTypeBackgroundColor);

            var prompt = property.GetAttribute<Config.ExcelConfigAttribute.Prompt>();
            if (prompt != null)
            {
                if (!property.isArray || arrayIndex >= 0)
                {
                    AddComment(descriptionCell, prompt.content, prompt.title);
                    AddPrompt(nameTypeCell, prompt.content, prompt.title);
                }
            }
            else if ((!property.isArray || arrayIndex >= 0) && property.refKlass != null && property.refKlass.isPersistentEnum)
            {
                var persistenceEnumTooltip = property.refKlass.persistenceEnumToolTip;
                AddComment(descriptionCell, persistenceEnumTooltip, null);
                //enum不用tooltip,因为数据太多会导致验证出错  AddPrompt(nameTypeCell, persistenceEnumTooltip, null);
            }

            return ret;
        }
        private static void AddComment(ExcelRange range, string promptContent, string promptTitle)
        {
            range.AddComment(promptContent == null ? string.Empty : promptContent, promptTitle == null ? Config.ExcelConfigAttribute.Prompt.DefaultTitle : promptTitle);
        }
        private static void AddPrompt(ExcelRange range, string promptContent, string promptTitle)
        {
            var dv = range.DataValidation.AddAnyDataValidation();
            dv.ShowInputMessage = true;
            var content = promptContent == null ? string.Empty : promptContent;
            int nCount = 0; foreach (var c in content) if (c == '\n') nCount++;
            if (nCount > 6) content = content.Replace("\n", "  ");
            dv.Prompt = content;
            dv.PromptTitle = promptTitle == null ? Config.ExcelConfigAttribute.Prompt.DefaultTitle : promptTitle;
        }
        private static void FindRefExcel(Property property, out string excel, out string sheet)
        {
            var refKlass = property.refKlass;
            excel = "";
            sheet = "";
            if (refKlass == null)
            {
                Entry.UpdateLogInfo($"FindRefExcel Not Find RefKlass:{property.name}");
                return;
            }
            if (refKlass.excels.Length > 1)
            {
                var specificAttribute = property.GetAttribute<Config.ExcelConfigAttribute.SpecificExcel>();
                if (specificAttribute != null && (!string.IsNullOrEmpty(specificAttribute.excel) || !string.IsNullOrEmpty(specificAttribute.sheet)))
                {
                    string expectedExcel = specificAttribute.excel;
                    string expectedSheet = specificAttribute.sheet;

                    for (int it = 0; it < refKlass.excels.Length; it++)
                    {
                        if (refKlass.excels[it] != expectedExcel && !string.IsNullOrEmpty(expectedExcel)) continue;
                        if (refKlass.sheets[it] != expectedSheet && !string.IsNullOrEmpty(expectedSheet)) continue;
                        excel = refKlass.excels[it];
                        sheet = refKlass.sheets[it];
                        return;
                    }
                }
            }
            excel = refKlass.excels[0];
            sheet = refKlass.sheets[0];
        }

        public static void ExtractData(bool bOnlyForClient = false, bool bOnlyForServer = false)
        {
            if (bOnlyForClient && bOnlyForServer)
            {
                Entry.UpdateLogInfo("ExtraData bOnlyForServer && bOnlyForClient Error", LogLevelType.ErrorType);
                return;
            }
            var allKlass = Entry.klasses.Values.Where(k => k.type != Klass.KlassType.Struct && !k.isPersistentEnum);
            foreach (var klass in allKlass)
            {
                klass.ResetData(null);
            }
            var excelFiles = CollectExportsInfo().GroupBy(p => p.excel);
            BuildCached();
            List<Task> tasks = new List<Task>();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Restart();
            foreach (var excelPair in excelFiles)
            {
                var tempPair = excelPair;
                Task task = Task.Run(() =>
                {
                    var excel = tempPair.Key;
                    var file = Path.Combine(Entry.ExcelFolderPath, excel as string + DotXLSX);
                    if (!File.Exists(file)) return;

                    foreach (var group in tempPair)
                    {
                        var klass = group.klass;
                        var sheetName = group.sheet;
                        var dataLineIndex = group.dataLineIndex;

                        ExcelWorksheet cacheData;
                        if (epplusCached.TryGetValue($"{excel}_{sheetName}", out cacheData))
                        {
                            if (cacheData == null)
                                continue;

                            ExtractKlassData(cacheData, klass, excel, sheetName, dataLineIndex, bOnlyForClient, bOnlyForServer);
                        }
                        else
                        {
                            throw new Exception("Can't find epplusCached");
                        }
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var klass in allKlass)
            {
                var lineInfos = klass.lineInfos;
                var LineCount = 0;
                foreach (var line in lineInfos)
                {
                    if (line.to > LineCount)
                    {
                        LineCount = line.to;
                    }
                }

                Klass.LineInfo[] finnalLineInfo = new Klass.LineInfo[lineInfos.Length];
                var finalData = new object[LineCount][];

                var datas = klass.data.ToArray();
                int destOffset = 0;
                for (int i = 0; i < lineInfos.Length; i++)
                {
                    var line = lineInfos[i];
                    var count = line.to - line.from;
                    Array.Copy(datas, line.from, finalData, destOffset, count);

                    finnalLineInfo[i] = new Klass.LineInfo { from = destOffset, to = destOffset + count, excel = line.excel, sheet = line.sheet };
                    destOffset += count;
                }

                klass.ResetData(finalData.ToList());

                for (int i = 0; i < finnalLineInfo.Length; i++)
                {
                    klass.lineInfos[i] = finnalLineInfo[i];
                }
            }
            sw.Stop();
            Entry.UpdateLogInfo($"ExtractKlassData总共花费{sw.Elapsed.TotalMilliseconds}ms.");
            var idSet = new HashSet<int>();
            var nameSet = new Dictionary<int, string>();
            var designNameSet = new HashSet<string>();

            //将引用类型转为ID
            foreach (var kv in Entry.klasses)
            {
                var klass = kv.Value;

                if (klass.type == Klass.KlassType.Struct || klass.isPersistentEnum) continue;

                if (klass.data == null) throw new ConfigException(ErrorMessage.KlassDataNull, klass.name);

                var data = klass.data;

                var types = klass.baseTypes;

                int jt = -1;
                for (int column = 1; column <= klass.rawWidth; column++)
                {
                    var property = types[column - 1];
                    if (property.commentOnly) continue;
                    if ((bOnlyForClient && property.bServerOnly) ||
                        (bOnlyForServer && property.bClientOnly)) continue;

                    jt++;

                    if (property.isBasicType)
                    {
                        if (property.isBasicArray) continue;
                        if (jt == klass.idPropertyIndex)
                        {
                            idSet.Clear();
                            for (int it = data.Count - 1; it >= 0; it--)
                            {
                                var value = data[it][jt];
                                if (value == null) throw new ConfigException($"find empty id in [{klass.name}]");

                                int intValue = (int)value;
                                if (intValue < 0) throw new ConfigException($"find negative id [{intValue}] in [{klass.name}]");

                                if (!idSet.Add(intValue)) throw new ConfigException($"find conflict id [{intValue}] in [{klass.name}]");
                            }
                        }
                        else if (jt == klass.codeNamePropertyIndex)
                        {
                            nameSet.Clear();

                            for (int it = data.Count - 1; it >= 0; it--)
                            {
                                var value = data[it][jt];
                                if (value == null) throw new ConfigException($"find empty name in [{klass.name}]");

                                var stringValue = value as string;
                                if (!Util.IsValidName(stringValue)) throw new ConfigException($"invalid name [{stringValue}] in [{klass.name}]");

                                int hash = stringValue.AscIIHash();
                                if (nameSet.ContainsKey(hash)) throw new ConfigException($"find name hash conflict in [{klass.name}]: [{nameSet[hash]}] with [{stringValue}]");

                                nameSet[hash] = stringValue;
                            }
                        }
                        else if (jt == klass.designNamePropertyIndex)
                        {
                            designNameSet.Clear();

                            for (int it = data.Count - 1; it >= 0; it--)
                            {
                                var value = data[it][jt];
                                if (value == null) throw new ConfigException($"find empty designName in [{klass.name}]");

                                if (!designNameSet.Add(value as string)) throw new ConfigException($"find conflict designName [{value as string}] in [{klass.name}]");
                            }
                        }
                        continue;
                    }

                    for (int it = data.Count - 1; it >= 0; it--)
                    {
                        string stringVal = data[it][jt] as string;

                        if (string.IsNullOrEmpty(stringVal))
                        {
                            data[it][jt] = null;
                            continue;
                        }

                        int refID;
                        var refKlass = property.refKlass;
                        var refData = refKlass.data;
                        if (refData == null) throw new ConfigException(ErrorMessage.KlassDataNull, property.refKlass.name);
                        bool find = false;
                        if (int.TryParse(stringVal, out refID))
                        {
                            for (int index = refData.Count - 1; index >= 0; index--)
                            {
                                if ((int)refData[index][refKlass.idPropertyIndex] == refID)
                                {
                                    find = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (refKlass.designNameProperty == null)
                            {
                                var lineInfo = klass.GetLineInfo(it);
                                throw new ConfigException(ErrorMessage.RefTypeFormatError, lineInfo.excel, lineInfo.sheet, GetColumnLetter(column), it - lineInfo.from + 1 + 2 * klass.depth + 1, stringVal, refKlass.name);
                            }
                            for (int index = refData.Count - 1; index >= 0; index--)
                            {
                                if (refData[index][refKlass.designNamePropertyIndex] as string == stringVal)
                                {
                                    find = true;
                                    refID = (int)refData[index][refKlass.idPropertyIndex];
                                    break;
                                }
                            }
                        }

                        if (!find)
                        {
                            var lineInfo = klass.GetLineInfo(it);
                            throw new ConfigException(ErrorMessage.RefTypeFormatError, lineInfo.excel, lineInfo.sheet, GetColumnLetter(column), it - lineInfo.from + 1 + 2 * klass.depth + 1, stringVal, refKlass.name);
                        }

                        data[it][jt] = refID;
                    }
                }
            }
        }

        private static int ReadSheetInfo(ExcelWorksheet sheet, Klass klass)
        {
            var content = sheet.Cells[InfoRow, InfoColumn].Value?.ToString();
            if (string.IsNullOrEmpty(content)) return 0;

            //表头被修改
            if (content.Length < 2 || content[0] != BlankChar || content[content.Length - 1] <= BlankChar || content[content.Length - 1] > BlankCharUpperLimit)
            {
                throw new ConfigException(ErrorMessage.SheetTitleDamaged, klass.name);
            }

            return content[content.Length - 1] - BlankChar;
        }

        static System.Collections.Concurrent.ConcurrentDictionary<string, ExcelWorksheet> epplusCached = new System.Collections.Concurrent.ConcurrentDictionary<string, ExcelWorksheet>();
        private static void BuildCached()
        {
            if (epplusCached.IsEmpty)
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Restart();

                List<Task> tasks = new List<Task>();
                var excelFiles = CollectExportsInfo().GroupBy(p => p.excel);
                foreach (var excelPair in excelFiles)
                {
                    var tempPair = excelPair;
                    Task task = Task.Run(() =>
                    {
                        var excel = tempPair.Key;
                        var file = Path.Combine(Entry.ExcelFolderPath, excel as string + DotXLSX);
                        if (!File.Exists(file)) return;

                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var excelPackage = new ExcelPackage(fs))
                            {
                                var workbook = excelPackage.Workbook;
                                foreach (var group in tempPair)
                                {
                                    var klass = group.klass;
                                    var sheetName = group.sheet;
                                    try
                                    {
                                        var sheet = workbook.Worksheets.SingleOrDefault(s => s.Name == sheetName);
                                        if (!epplusCached.TryAdd($"{excel}_{sheetName}", sheet))
                                        {
                                            Entry.UpdateLogInfo($"{excel}_{sheetName} alreay exist", LogLevelType.WarnningType);
                                            continue;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new Exception(string.Format("exception：{0} ExtractData file: {1} sheetName: {2}", e.ToString(), file, sheetName));
                                    }
                                }
                            }
                        }
                    });

                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());

                sw.Stop();
                Entry.UpdateLogInfo($"BuildCached总共花费{sw.Elapsed.TotalMilliseconds}ms.", LogLevelType.InfoType);
            }
        }
        private delegate object CellValueParser(string content);

        private static object ParseDateTime(string content) => DateTime.Parse(content);
        private static object ParseInteger(string content) => int.Parse(content);
        private static object ParseFloat(string content) => float.Parse(content);
        private static object ParseString(string content) => content;
        private static object ParseBoolean(string content)
        {
            if (TrueString.Equals(content, StringComparison.OrdinalIgnoreCase)) return true;
            if (TrueString2.Equals(content, StringComparison.OrdinalIgnoreCase)) return true;

            if (FalseString.Equals(content, StringComparison.OrdinalIgnoreCase)) return false;
            if (FalseString2.Equals(content, StringComparison.OrdinalIgnoreCase)) return false;

            throw new ConfigException($"bool value must be ({TrueString}/{FalseString}) or ({TrueString2}/{FalseString2})");
        }
        private static object ParseUInt(string content) => uint.Parse(content);
        private static object ParseShort(string content) => short.Parse(content);
        private static object ParseUShort(string content) => ushort.Parse(content);
        private static object ParseSbyte(string content) => sbyte.Parse(content);
        private static object ParseByte(string content) => byte.Parse(content);

        private static PEG.Pattern functorPattern;
        //解析返回bytes数组
        private static CellValueParser GetFunctorParser(Config.ExcelConfigAttribute.Functor functor)
        {
            if (functorPattern == null) functorPattern = MiniScript.Parser.BuildPattern();

            var localVarNames = functor.argNames;

            return new CellValueParser(content =>
            {
                try
                {
                    var result = functorPattern.Match(content);
                    var engine = new MiniScript.OpCodes.OpCodeEngine(MiniScript.GlobalFunctions.globalFunctionNames, localVarNames);
                    return engine.Emit(content, result.Select(o => (MiniScript.Nodes.Node)o).ToArray());
                }
                catch (Exception e)
                {
                    throw new ConfigException(e.Message);
                }
            });
        }

        private static object ParseBasicIntegerArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new int[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer) throw new ConfigException("None Integer Element");
                array[it] = (int)token;
            }
            return array;
        }
        private static object ParseBasicFloatArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new float[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer && token.Type != JTokenType.Float) throw new ConfigException("None Float Element");
                array[it] = (float)token;
            }
            return array;
        }
        private static object ParseBasicStringArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new string[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.String) throw new ConfigException("None String Element");
                array[it] = (string)token;
            }
            return array;
        }

        private static object ParseBasicUintArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new uint[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer) throw new ConfigException("None Integer Element");
                array[it] = (uint)token;
            }
            return array;
        }

        private static object ParseBasicShortArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new short[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer) throw new ConfigException("None Integer Element");
                array[it] = (short)token;
            }
            return array;
        }

        private static object ParseBasicUShortArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new ushort[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer) throw new ConfigException("None Integer Element");
                array[it] = (ushort)token;
            }
            return array;
        }

        private static object ParseBasicSbyteArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new sbyte[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer) throw new ConfigException("None Integer Element");
                array[it] = (sbyte)token;
            }
            return array;
        }

        private static object ParseBasicByteArray(string content)
        {
            var contentArray = JArray.Parse(content);

            var array = new byte[contentArray.Count];
            for (int it = 0; it < array.Length; it++)
            {
                var token = contentArray[it];
                if (token.Type != JTokenType.Integer) throw new ConfigException("None Integer Element");
                array[it] = (byte)token;
            }
            return array;
        }

        private static object ParseBasic2DIntegerArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new int[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new int[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (int)pContentArray[jt];
                }
            }
            return array;
        }
        private static object ParseBasic2DFloatArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new float[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new float[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (float)pContentArray[jt];
                }
            }
            return array;
        }
        private static object ParseBasic2DStringArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new string[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new string[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (string)pContentArray[jt];
                }
            }
            return array;
        }

        private static object ParseBasic2DUIntArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new uint[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new uint[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (uint)pContentArray[jt];
                }
            }
            return array;
        }


        private static object ParseBasic2DShortArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new short[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new short[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (short)pContentArray[jt];
                }
            }
            return array;
        }

        private static object ParseBasic2DUShortArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new ushort[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new ushort[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (ushort)pContentArray[jt];
                }
            }
            return array;
        }

        private static object ParseBasic2DSByteArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new sbyte[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new sbyte[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (sbyte)pContentArray[jt];
                }
            }
            return array;
        }

        private static object ParseBasic2DByteArray(string content)
        {
            var contentArray = JArray.Parse(content);
            var array = new byte[contentArray.Count][];
            for (int it = 0; it < array.Length; it++)
            {
                var pContentArray = contentArray[it] as JArray;
                var pArray = new byte[pContentArray.Count];
                array[it] = pArray;
                for (int jt = 0; jt < pArray.Length; jt++)
                {
                    pArray[jt] = (byte)pContentArray[jt];
                }
            }
            return array;
        }

        private static CellValueParser GetIntegerWithRangeParser(int min, int max)
        {
            return new CellValueParser(content =>
            {
                int value = int.Parse(content);
                if (value < min || value > max) throw new ConfigException(ErrorMessage.RangeExceed, min, max);
                return value;
            });
        }
        private static CellValueParser GetFloatWithRangeParser(float min, float max)
        {
            return new CellValueParser(content =>
            {
                float value = float.Parse(content);
                if (value < min || value > max) throw new ConfigException(ErrorMessage.RangeExceed, min, max);
                return value;
            });
        }

        private struct DataLine
        {
            public int row;
            public string value;
        }

        static object lockObject = new object();
        private static void ExtractKlassData(ExcelWorksheet sheet, Klass klass, string excelName, string sheetName, int dataLineIndex, bool bOnlyForClient = false, bool bOnlyForServer = false)
        {
            var cells = sheet.Cells;

            var baseTypes = klass.baseTypes;

            int info = ReadSheetInfo(sheet, klass);

            if (info != 2 * klass.depth) throw new ConfigException(ErrorMessage.SheetNeedRe_Export, klass.name);
            List<DataLine> dataLines = new List<DataLine>();
            int firstContentRow = 2 * klass.depth + 1 + 1 + 1;//一行供描述使用,还有一行供database类型使用
            int emptyLine = 0;
            if (klass.type == Klass.KlassType.Static)
            {
                int row = firstContentRow;
                string firstValue = cells[row, 1].Value?.ToString();
                dataLines.Add(new DataLine { row = row, value = firstValue });
            }
            else
            {
                for (int row = firstContentRow; ; row++)
                {
                    string firstValue = cells[row, 1].Value?.ToString();
                    if (string.IsNullOrEmpty(firstValue))
                    {
                        emptyLine++;

                        if (emptyLine > 20) break;
                        continue;
                    }

                    emptyLine = 0;
                    dataLines.Add(new DataLine { row = row, value = firstValue });
                }
            }
            var data = new object[dataLines.Count][];
            int validDataColumns = klass.baseTypes.Where(p => !(p.commentOnly || (bOnlyForClient && p.bServerOnly) || (bOnlyForServer && p.bClientOnly))).Count();
            for (int it = dataLines.Count - 1; it >= 0; it--) data[it] = new object[validDataColumns];
            int dataColumn = -1;
            for (int column = 1; column <= klass.rawWidth; column++)
            {
                CellValueParser parser;
                var property = baseTypes[column - 1];

                if (property.commentOnly) continue;
                if ((bOnlyForClient && property.bServerOnly) ||
                    (bOnlyForServer && property.bClientOnly)) continue;

                dataColumn++;

                if (property.isBasicArray)
                {
                    if (property.isBasic1DArray)
                    {
                        switch (property.basicType)
                        {
                            case Property.BasicType.Integer: parser = ParseBasicIntegerArray; break;
                            case Property.BasicType.Float: parser = ParseBasicFloatArray; break;
                            case Property.BasicType.String: parser = ParseBasicStringArray; break;
                            case Property.BasicType.UInt: parser = ParseBasicUintArray; break;
                            case Property.BasicType.Short: parser = ParseBasicShortArray; break;
                            case Property.BasicType.UShort: parser = ParseBasicUShortArray; break;
                            case Property.BasicType.SByte: parser = ParseBasicSbyteArray; break;
                            case Property.BasicType.Byte: parser = ParseBasicByteArray; break;

                            default: throw new Exception("aha???");
                        }
                    }
                    else
                    {
                        switch (property.basicType)
                        {
                            case Property.BasicType.Integer: parser = ParseBasic2DIntegerArray; break;
                            case Property.BasicType.Float: parser = ParseBasic2DFloatArray; break;
                            case Property.BasicType.String: parser = ParseBasic2DStringArray; break;
                            case Property.BasicType.UInt: parser = ParseBasic2DUIntArray; break;
                            case Property.BasicType.Short: parser = ParseBasic2DShortArray; break;
                            case Property.BasicType.UShort: parser = ParseBasic2DUShortArray; break;
                            case Property.BasicType.SByte: parser = ParseBasic2DSByteArray; break;
                            case Property.BasicType.Byte: parser = ParseBasic2DByteArray; break;
                            default: throw new Exception("aha???");
                        }
                    }
                }
                else if (property.isBasicType)
                {
                    switch (property.basicType)
                    {
                        case Property.BasicType.Integer:
                            {
                                if (property.HasAttribute<Config.ExcelConfigAttribute.AsTimeStamp>()) parser = ParseDateTime;
                                else
                                {
                                    var integerRange = property.GetAttribute<Config.ExcelConfigAttribute.IntegerRange>();
                                    if (integerRange != null) parser = GetIntegerWithRangeParser(integerRange.min, integerRange.max);
                                    else parser = ParseInteger;
                                }
                            }
                            break;

                        case Property.BasicType.Float:
                            {
                                var floatRange = property.GetAttribute<Config.ExcelConfigAttribute.FloatRange>();
                                if (floatRange != null) parser = GetFloatWithRangeParser(floatRange.min, floatRange.max);
                                else parser = ParseFloat;
                            }
                            break;

                        case Property.BasicType.String:
                            {
                                var functor = property.GetAttribute<Config.ExcelConfigAttribute.Functor>();
                                if (functor != null) parser = GetFunctorParser(functor);
                                else parser = ParseString;
                            }
                            break;
                        case Property.BasicType.Boolean: parser = ParseBoolean; break;
                        case Property.BasicType.UInt: parser = ParseUInt; break;
                        case Property.BasicType.Short: parser = ParseShort; break;
                        case Property.BasicType.UShort: parser = ParseUShort; break;
                        case Property.BasicType.SByte: parser = ParseSbyte; break;
                        case Property.BasicType.Byte: parser = ParseByte; break;

                        default: throw new Exception("aha??");
                    }
                }
                else parser = ParseString;
                if (property.type == "TeamPublicType")
                {
                    //Entry.UpdateLogText();
                    Console.WriteLine($"Parser Type: {property.type}");
                }
                for (int dIndex = 0; dIndex < dataLines.Count; dIndex++)
                {
                    var dataLine = dataLines[dIndex];
                    var row = dataLine.row;
                    var rowData = data[dIndex];
                    var content = column == 1 ? dataLine.value : cells[row, column].Value?.ToString();

                    try
                    {
                        object value;
                        if (string.IsNullOrEmpty(content))
                        {
                            value = null;
                        }
                        else
                        {
                            value = parser(content);
                        }
                        rowData[dataColumn] = value;
                    }
                    catch (Exception e)
                    {
                        throw new ConfigException(ErrorMessage.BasicTypeFormatError, excelName, sheetName, GetColumnLetter(column), row, content, e.Message);
                    }
                }
            }

            dataLines.Clear();

            //Excel下面同样的sheet会处理导相同的klass，所以需要加锁保证原子操作
            lock (lockObject)
            {
                if (klass.data == null)
                {
                    klass.ResetData(new List<object[]>(data));
                    klass.lineInfos[dataLineIndex] = new Klass.LineInfo { from = 0, to = klass.data.Count, excel = excelName, sheet = sheetName };
                }
                else
                {
                    klass.lineInfos[dataLineIndex] = new Klass.LineInfo { from = klass.data.Count, to = klass.data.Count + data.Length, excel = excelName, sheet = sheetName };
                    klass.data.AddRange(data);
                }
            }
        }

    }

}
