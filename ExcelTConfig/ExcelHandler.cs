using ExcelTConfig.Base;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

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
                        if(!bNewSheet)
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
                                    Console.WriteLine($"gry: {sheet.Name}, column:{i}");
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


        static int AddRowForDescription(ExcelWorksheet sheet, Property property, int row, int column, int arrayIndex)
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
            if (property.isArray && arrayIndex == 1)
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
            var nameTypeCell = nameTypeCellCrossCell ? sheet.Cells[row + 1, column, lastRow, ret - 1] : sheet.Cells[row + 1, column];
            if (nameTypeCellCrossCell) nameTypeCell.Merge = true;
            //if (hyperLink != null) nameTypeCell.Hyperlink = hyperLink;
            nameTypeCell.Style.Border.BorderAround(ExcelBorderStyle.Medium, TitleBorderColor);
            nameTypeCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            nameTypeCell.Style.Fill.BackgroundColor.SetColor(NameTypeBackgroundColor);

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
    }
}
