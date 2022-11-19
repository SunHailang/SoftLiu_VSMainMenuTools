using ExcelTConfig.Base;
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
    public static class I18NHandlerExcel
    {
        public static readonly Color TitleBorderColor = Color.FromArgb(32, 32, 32);
        public static readonly Color DescriptionBackgroundColor = Color.FromArgb(70, 130, 180);
        public static readonly Color DescriptionFontColor = Color.White;

        public static void ExportI18N()
        {
            var excelFiles = ExcelHandler.CollectExportsInfo().GroupBy(p => p.excel);

            var listTask = new List<Task>();

            foreach (var excelPair in excelFiles)
            {
                //表中所有类，不存在任何property为I18N
                if (excelPair.All(g => !g.klass.properties.Any(p => p.isI18N))) continue;

                string excel = excelPair.Key;
                //   Console.WriteLine($"{excel}");
                var tempPair = excelPair;

                listTask.Add(Task.Run(() =>
                {
                    var file = Path.Combine(Entry.I18NExcelFolderPath, excel + "I18N.xlsx");

                    bool fileExists = File.Exists(file);
                    if (!fileExists)
                    {
                        var directory = Path.GetDirectoryName(file);
                        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    }
                    using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        using (ExcelPackage excelPackage = new ExcelPackage(fs))
                        {
                            bool dirty = !fileExists;
                            var workbook = excelPackage.Workbook;

                            foreach (var group in tempPair)
                            {
                                var klass = group.klass;

                                var hashExist = new HashSet<string>();

                                foreach (var property in klass.properties)
                                {
                                    if (!property.isI18N) continue;
                                    var i18NKlass = property.i18NKlass;

                                    // Sheet表的名称限制31个字符以内
                                    if (i18NKlass.name.Length > 31)
                                    {
                                        throw new ConfigException($"i18n sheet Exist in:{i18NKlass.name},orgin name:{i18NKlass.declarePropertyName} Sheet表名长度大于31个字符");
                                    }

                                    var sheet = workbook.Worksheets.SingleOrDefault(s => s.Name == i18NKlass.name);

                                    bool newSheet = sheet == null;

                                    Dictionary<int, string[]> prevData = null;

                                    if (!newSheet)
                                    {
                                        dirty |= IsDirtyI18NExcel(sheet);
                                        prevData = ExtractI18NData(sheet);
                                        workbook.Worksheets.Delete(sheet);
                                    }

                                    try
                                    {
                                        sheet = workbook.Worksheets.Add(i18NKlass.name);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ConfigException($"i18n sheet Exist in:{i18NKlass.name},orgin name:{i18NKlass.declarePropertyName}, Error:{e.Message}");
                                    }

                                    SetupI18NSheetDefaultStyle(sheet, i18NKlass, klass);

                                    int valueIndex = klass.GetPropertyValueIndex(property);
                                    int specificIndex = -1;
                                    for (int it = 0; it < klass.excels.Length; it++)
                                    {
                                        if (klass.excels[it] == excel && klass.sheets[it] == group.sheet)
                                        {
                                            specificIndex = it;
                                            break;
                                        }
                                    }
                                    int rowFrom = klass.lineInfos[specificIndex].from;
                                    int rowTo = klass.lineInfos[specificIndex].to;
                                    if (rowTo - rowFrom != (prevData == null ? 0 : prevData.Count)) dirty = true;

                                    int count = 0;
                                    for (int it = rowFrom; it < rowTo; it++)
                                    {
                                        var rowData = klass.data[it];
                                        int row = 2 + count;
                                        count++;
                                        int id = (int)rowData[klass.idPropertyIndex];
                                        sheet.Cells[row, 1].Value = rowData[klass.idPropertyIndex];

                                        var zhValue = rowData[valueIndex] as string;
                                        if (zhValue == null) zhValue = string.Empty;

                                        sheet.Cells[row, 2].Value = zhValue;

                                        string[] line;
                                        if (prevData != null && prevData.TryGetValue(id, out line))
                                        {
                                            if (line[0] != zhValue) dirty = true;
                                            for (int jt = 1; jt < Entry.LangCount; jt++)
                                            {
                                                if (!string.IsNullOrEmpty(line[jt])) sheet.Cells[row, jt + 2].Value = line[jt];
                                            }
                                        }
                                        else dirty = true;
                                    }
                                }
                            }

                            if (dirty)
                            {
                                Console.WriteLine($"{file} dirty ");
                                excelPackage.Save();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(listTask.ToArray());

        }

        static object lockObject = new object();

        public static void ExtractI18NData()
        {
            var excelFiles = ExcelHandler.CollectExportsInfo().GroupBy(p => p.excel);
            var listTask = new List<Task>();

            HashSet<Property> existProps = new HashSet<Property>();
            foreach (var excelFile in excelFiles)
            {
                //表中所有类，不存在任何property为I18N
                if (excelFile.All(g => !g.klass.properties.Any(p => p.isI18N))) continue;

                var group = excelFile;
                listTask.Add(Task.Run(() =>
                {
                    string excel = group.Key;

                    var file = Path.Combine(Entry.I18NExcelFolderPath, excel + "I18N.xlsx");

                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (ExcelPackage excelPackage = new ExcelPackage(fs))
                        {
                            var workbook = excelPackage.Workbook;

                            foreach (var exportInfo in group)
                            {
                                var klass = exportInfo.klass;

                                foreach (var property in klass.properties)
                                {
                                    if (!property.isI18N) continue;

                                    var i18NKlass = property.i18NKlass;

                                    ExcelWorksheet sheet;

                                    try
                                    {
                                        sheet = workbook.Worksheets.Single(s => s.Name == i18NKlass.name);
                                    }
                                    catch (Exception e)
                                    {
                                        throw new ConfigException("single error:" + i18NKlass.name);
                                    }

                                    var data = ExtractI18NData(sheet);

                                    //操作同一个i18nKlass
                                    lock (lockObject)
                                    {
                                        if (!existProps.Contains(property))
                                        {
                                            i18NKlass.data = null;
                                            existProps.Add(property);
                                        }

                                        if (i18NKlass.data == null) i18NKlass.data = data;
                                        else i18NKlass.data = i18NKlass.data.Union(data).ToDictionary(p => p.Key, p => p.Value);
                                    }
                                }
                            }
                        }
                    }

                }));
            }

            Task.WaitAll(listTask.ToArray());
        }
        public static Dictionary<int, string[]> ExtractI18NData(ExcelWorksheet sheet)
        {
            var ret = new Dictionary<int, string[]>();

            for (int it = 2; ; it++)
            {
                string idString = sheet.Cells[it, 1].Value?.ToString();
                if (string.IsNullOrEmpty(idString)) break;

                var id = int.Parse(idString);

                var rowData = new string[Entry.LangCount];
                for (int jt = 0; jt < Entry.LangCount; jt++)
                {
                    var str = sheet.Cells[it, jt + 2].GetValue<string>();
                    rowData[jt] = str == null ? string.Empty : str;
                }
                ret[id] = rowData;
            }

            return ret;
        }

        private static bool IsDirtyI18NExcel(ExcelWorksheet sheet)
        {
            for (int it = 2, index = 0; index < Entry.LangCount; it++, index++)
            {
                string i18NString = sheet.Cells[1, it].Value?.ToString();
                string i18NName = Entry.Lang[index];
                if (string.IsNullOrEmpty(i18NString) || !i18NString.Equals(i18NName, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }
        private static void SetupI18NSheetDefaultStyle(ExcelWorksheet sheet, I18NKlass i18NKlass, Klass klass)
        {
            sheet.DefaultColWidth = 50f;
            sheet.Cells.Style.WrapText = true;
            sheet.DefaultRowHeight = 16f;

            for (int jt = 1; jt <= Entry.LangCount + 1; jt++)
            {
                var c = sheet.Column(jt);

                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.Fill.PatternType = ExcelFillStyle.Solid;
                c.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(245, 245, 245));
                c.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                c.Style.Border.Bottom.Color.SetColor(Color.FromArgb(192, 192, 192));
                c.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                c.Style.Border.Right.Color.SetColor(Color.FromArgb(192, 192, 192));

                //var relativePath = Util.CalculateRelativePath(Path.Combine(Entry.I18NExcelFolderPath, "abc"), Path.Combine(Entry.ExcelFolderPath, klass.excel));
                var descriptionCell = sheet.Cells[1, jt];
                //descriptionCell.Hyperlink = new ExcelHyperLink($"{relativePath}.xlsx#{i18NKlass.declareKlassName}!A1", UriKind.Relative);
                descriptionCell.Value = jt == 1 ? Property.idProperty.name : Entry.Lang[jt - 2];
                descriptionCell.Style.Border.BorderAround(ExcelBorderStyle.Medium, TitleBorderColor);
                descriptionCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                descriptionCell.Style.Fill.BackgroundColor.SetColor(DescriptionBackgroundColor);
                descriptionCell.Style.Font.Color.SetColor(DescriptionFontColor);
                descriptionCell.Style.Font.Bold = true;
            }

            sheet.Column(1).Width = 10;
            sheet.View.FreezePanes(2, 1);
        }
    }
}
