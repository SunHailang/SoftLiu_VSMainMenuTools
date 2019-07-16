//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace SoftLiu_VSMainMenuTools.ExcelToXml
{
    public class ExcelOrXmlManager
    {
        public static DataTable ReadExcelToDataTable(string path, string sheet)
        {
            //此连接只能操作Excel2007之前(.xls)文件
            //string connstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
            string sheetName = sheet + "$";
            //此连接可以操作.xls与.xlsx文件
            string connstring = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            using (OleDbConnection conn = new OleDbConnection(connstring))
            {
                conn.Open();
                DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                string sql = string.Format("SELECT * FROM [{0}]", sheetName); //查询字符串
                //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串

                OleDbDataAdapter odda = new OleDbDataAdapter(sql, conn);
                DataSet set = new DataSet();
                odda.Fill(set, sheetName);
                DataTable dt = set.Tables[0];
                //dt.WriteXml(@"C:\Users\hlsun\Desktop\excel.xml");

                //WriteDataTableToExcel(dt, string.Empty, string.Empty);
                return dt;
            }
        }

        public static void WriteDataTableToExcel(DataTable dataTable, string path, string sheet)
        {
            if (dataTable != null)
            {
                //此连接只能操作Excel2007之前(.xls)文件
                //string connstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
                string sheetName = sheet + "$";
                //此连接可以操作.xls与.xlsx文件
                string connstring = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                DataTable dataTable1 = dataTable;//获取表格2

                //Microsoft.Office.Interop.Excel.Application excelApp;
                Microsoft.Office.Interop.Excel._Workbook workBook;
                Microsoft.Office.Interop.Excel._Worksheet workSheet;
                Microsoft.Office.Interop.Excel._Worksheet workSheet1;
                object misValue = System.Reflection.Missing.Value;
                workBook = excelApp.Workbooks.Add(misValue);//加载模型

                workSheet = (Microsoft.Office.Interop.Excel._Worksheet)workBook.Sheets.get_Item(1);//第一个工作薄。

                //workSheet1 = (Microsoft.Office.Interop.Excel._Worksheet)workBook.Sheets.get_Item(2);
                int rowIndex = 0;
                int colIndex = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    rowIndex++;
                    colIndex = 0;
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        colIndex++;
                        workSheet.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString().Trim();

                    }
                }

                //rowIndex = 0;
                //colIndex = 0;
                //foreach (DataRow row in dataTable1.Rows)
                //{
                //    rowIndex++;
                //    colIndex = 0;
                //    foreach (DataColumn col in dataTable1.Columns)
                //    {
                //        colIndex++;
                //        workSheet1.Cells[rowIndex, colIndex] = row[col.ColumnName].ToString().Trim();

                //    }
                //}


                workSheet.Protect("MyPassword", Type.Missing, Type.Missing, Type.Missing,
                                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                            Type.Missing, true, Type.Missing, Type.Missing);

                //保护工作表
                //workSheet1.Protect("MyPassword", Type.Missing, Type.Missing, Type.Missing,
                //                  Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                //                  Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                //                  Type.Missing, true, Type.Missing, Type.Missing);

                /**/

                excelApp.Visible = false;

                workBook.SaveAs(new FileInfo(@"D:\outputFormDataBase2.xlsx"));/*, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue,
                    misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                    misValue, misValue, misValue, misValue, misValue);
                     */
                dataTable = null;

                workBook.Close(true, misValue, misValue);

                excelApp.Quit();

                //PublicMethod.Kill(excelApp);//调用kill当前excel进程  
            }
        }

        public static void ReadXml(string xmlPath, string rootNodeName)
        {
            //初始化一个xml实例
            XmlDocument myXmlDoc = new XmlDocument();
            //加载xml文件（参数为xml文件的路径）
            myXmlDoc.Load(xmlPath);
            //获得第一个姓名匹配的节点（SelectSingleNode）：此xml文件的根节点
            XmlNode rootNode = myXmlDoc.SelectSingleNode(rootNodeName);
            //分别获得该节点的InnerXml和OuterXml信息
            string innerXmlInfo = rootNode.InnerXml.ToString();
            string outerXmlInfo = rootNode.OuterXml.ToString();
            //获得该节点的子节点（即：该节点的第一层子节点）
            XmlNodeList levelNodeList = rootNode.ChildNodes;
            foreach (XmlNode node in levelNodeList)
            {
                //获得该节点的属性集合
                XmlAttributeCollection attributeCol = node.Attributes;
                foreach (XmlAttribute attri in attributeCol)
                {
                    //获取属性名称与属性值
                    string name = attri.Name;
                    string value = attri.Value;
                    Console.WriteLine("{0} = {1}", name, value);
                }

                //判断此节点是否还有子节点
                if (node.HasChildNodes)
                {
                    //获取该节点的第一个子节点
                    XmlNode secondLevelNode1 = node.FirstChild;
                    //获取该节点的名字
                    string name = secondLevelNode1.Name;
                    //获取该节点的值（即：InnerText）
                    string innerText = secondLevelNode1.InnerText;
                    Console.WriteLine("{0} = {1}", name, innerText);

                    //获取该节点的第二个子节点（用数组下标获取）
                    XmlNode secondLevelNode2 = node.ChildNodes[1];
                    name = secondLevelNode2.Name;
                    innerText = secondLevelNode2.InnerText;
                    Console.WriteLine("{0} = {1}", name, innerText);
                }
            }
        }

        public static void WriteXml(DataTable dt, string sheetName, string savePath)
        {

            //初始化一个xml实例
            XmlDocument myXmlDoc = new XmlDocument();
            //创建xml的根节点
            XmlElement rootElement = myXmlDoc.CreateElement(sheetName);
            //将根节点加入到xml文件中（AppendChild）
            myXmlDoc.AppendChild(rootElement);

            DataRow[] drArray = dt.Select();
            DataRow drArrayFirst = drArray[0];
            for (int i = 1; i < drArray.Length; i++)
            {
                DataRow dr = drArray[i];
                string dc0 = dr[0] as string;
                if (dc0 == null)
                {
                    MessageBox.Show(string.Format("{0}行  {1}列  出错！", i + 1, 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                XmlElement firstLevelElement1 = myXmlDoc.CreateElement(dc0);
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    string dc2 = dr[j] as string;
                    if (dc2 == null)
                    {
                        MessageBox.Show(string.Format("{0}行  {1}列  出错！", i + 1, j + 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    string dc1 = drArrayFirst[j] as string;
                    if (dc1 == null)
                    {
                        MessageBox.Show(string.Format("{0}行  {1}列  出错！", i + 1, j + 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //填充第一层的第一个子节点的属性值（SetAttribute）
                    firstLevelElement1.SetAttribute(dc1 == null ? "Not Fond" : dc1, dc2 == null ? "Not Fond" : dc2);
                }
                //将第一层的第一个子节点加入到根节点下
                rootElement.AppendChild(firstLevelElement1);
            }

            //将xml文件保存到指定的路径下
            myXmlDoc.Save(savePath);
        }


        public static void WriteJson(DataTable dt, string sheetName, string savePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            sb.Append(string.Format("\"{0}\": [\n", sheetName));

            DataRow[] drArray = dt.Select();
            DataRow drArrayFirst = drArray[0];
            for (int i = 1; i < drArray.Length; i++)
            {
                sb.Append("{\n");
                DataRow dr = drArray[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string dc2 = dr[j] as string;
                    if (dc2 == null)
                    {
                        MessageBox.Show(string.Format("{0}行  {1}列  出错！", i + 1, j + 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    string dc1 = drArrayFirst[j] as string;
                    if (dc1 == null)
                    {
                        MessageBox.Show(string.Format("{0}行  {1}列  出错！", i + 1, j + 1), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    sb.Append(string.Format("\"{0}\": {1}", dc1, dc2));
                    if (j < dt.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                    sb.Append("\n");
                }
                sb.Append("}");
                if (i < drArray.Length - 1)
                {
                    sb.Append(",");
                }
                sb.Append("\n");
            }
            sb.Append("]\n}\n");
            
            string contents = Regex.Replace(sb.ToString(), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            File.WriteAllText(savePath, contents, System.Text.Encoding.UTF8);
        }
    }
}
