using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SoftLiu_VSMainMenuTools.ExcelToXml
{
    public partial class ExcelToXmlForm : Form
    {
        public ExcelToXmlForm()
        {
            InitializeComponent();
        }

        private Dictionary<string, string> m_language = new Dictionary<string, string>()
        {
            {"English","en"},
            {"Italian","it"},
            {"Spanish","es" },
            {"Chinese","zh" },
            {"Portuguese","pt" },
            {"Russian","ru"},
            {"French","fr"},
            {"Polish","pl" },
            {"Arabic","ar" },
            {"Japanese","ja" },
            {"Indonesian","id"},
            {"Dutch","nl"},
            {"Vietnamese","vi" },
            {"Swedish","sv" },
            {"Thai","th" },
            {"Hungarian","hu"},
            {"Turkish","tr"},
            {"Greek","el" },
            {"German","de" },
            {"Czech","cs" },
        };

        private void ExcelToXml_Load(object sender, EventArgs e)
        {
            //string excelPath = "D:\\VS2015Program\\SF45.xlsx";
            //string sheetName = "Sheet1";
            //DataTable dt = ExcelOrXmlManager.ReadExcelToDataTable(excelPath, sheetName);
            ////ExcelOrXmlManager.WriteXml(dt, sheetName, "D:\\VS2015Program\\SF2.xml");
            //ExcelOrXmlManager.WriteJson(dt, sheetName, "D:\\VS2015Program\\SF2.json");

            foreach (KeyValuePair<string, string> item in m_language)
            {
                comboBoxLanguage.Items.Add(item.Key);
            }
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void textBoxPath_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            if (this.textBoxPath.Text.CompareTo(path) != 0)
            {
                this.textBoxPath.Text = path; //由一个textBox显示路径
            }
        }

        private void textBoxPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void buttonSure_Click(object sender, EventArgs e)
        {
            string excelPath = textBoxPath.Text;
            string sheetName = "FB_Dunkshot";
            DataTable dt = ExcelOrXmlManager.ReadExcelToDataTable(excelPath, sheetName);

            if (dt.Rows.Count <= 1 || dt.Columns.Count <= 1)
            {
                Console.WriteLine("None Item Information.");
                return;
            }
            Dictionary<string, Dictionary<string, object>> dic = new Dictionary<string, Dictionary<string, object>>();
            dic.Clear();

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                if (dt.Rows[1][i] == null || string.IsNullOrEmpty(dt.Rows[1][i].ToString().Trim()))
                {
                    continue;
                }
                string language = dt.Rows[0][i].ToString().Trim();
                Dictionary<string, object> dicItem = new Dictionary<string, object>();
                dicItem.Clear();
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    string key = dt.Rows[j][0].ToString().Trim();
                    string value = dt.Rows[j][i] == null ? string.Empty : dt.Rows[j][i].ToString().Trim();

                    dicItem.Add(key, value);
                }
                dic.Add(language, dicItem);
            }



            foreach (KeyValuePair<string, Dictionary<string, object>> item in dic)
            {
                if (!m_language.ContainsKey(item.Key))
                {
                    Console.WriteLine(string.Format("not {0} language.", item.Key));
                    continue;
                }
                string path = string.Format("language_{0}.ts", m_language[item.Key]);

                string json = JsonUtils.Instance.DictionaryToJson(item.Value);

                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("export const language_{0} = \n", m_language[item.Key]));
                string[] data = json.Split(',');
                for (int i = 0; i < data.Length; i++)
                {
                    int index = data[i].IndexOf(':');
                    string str1 = data[i].Substring(0, index);
                    string str2 = data[i].Substring(index, data[i].Length - index);
                    str1 = str1.Replace("\"", "");
                    sb.Append(string.Format("{0}{1}", str1, str2));
                    if (i < data.Length - 1)
                    {
                        sb.Append(",\n");
                    }
                }

                byte[] bt = Encoding.UTF8.GetBytes(sb.ToString());

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Write(bt, 0, bt.Length);
                }
            }
        }

        private void readContent()
        {
            string path = @"Resources\enFb.ts";

            byte[] bytes = FileUtils.ReadFileBytes(path);
            Console.WriteLine(bytes.Length);
            string data = Encoding.UTF8.GetString(bytes).Trim().Trim('{', '}');
            string[] needData = data.Split('=')[1].Split('\n');

            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            dic.Clear();

            string cur_str = "normal";

            foreach (var str in needData)
            {
                string item = str.Replace("\r", "").Trim();
                if (string.IsNullOrEmpty(item) || item.Contains("//"))
                {
                    continue;
                }
                // TODO
                if (item.Contains("{") && !item.Contains("${"))
                {
                    //start
                    if (item.Contains(":"))
                    {
                        // new start
                        string[] data1 = item.Split(':');
                        cur_str = data1[0].Trim();

                        if (!string.IsNullOrEmpty(cur_str) && !dic.ContainsKey(cur_str))
                        {
                            Dictionary<string, string> curDic = new Dictionary<string, string>();
                            dic.Add(cur_str, curDic);
                        }
                    }
                    else
                    {
                        cur_str = "normal";
                        if (!dic.ContainsKey(cur_str))
                        {
                            Dictionary<string, string> curDic = new Dictionary<string, string>();
                            dic.Add(cur_str, curDic);
                        }
                    }
                }
                if (!item.Contains("{") && !item.Contains("}") || item.Contains("${"))
                {
                    string[] data1 = item.Split(':');
                    if (data1.Length == 2 && !dic[cur_str].ContainsKey(data1[0].Trim()))
                    {

                        dic[cur_str].Add(data1[0].Trim(), data1[1].Trim().Trim(',').Trim('\'', '"'));
                    }
                }
                if (item.Contains("},") && !item.Contains("${"))
                {
                    //end 添加
                    cur_str = "normal";
                }
            }
            Console.WriteLine("Dic Count: " + dic.Count);
            Console.WriteLine("read end. 结束");


            DataTable dt = null;
            dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Type");
            dt.Columns.Add("KEY");
            foreach (KeyValuePair<string, string> item in m_language)
            {
                dt.Columns.Add(item.Key);
            }
            DataRow dr_column = dt.NewRow();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dr_column[dt.Columns[i].ColumnName] = dt.Columns[i].ColumnName;
            }
            dt.Rows.Add(dr_column);
            bool writeType = true;
            foreach (KeyValuePair<string, Dictionary<string, string>> item in dic)
            {
                writeType = true;
                foreach (KeyValuePair<string, string> itemDic in item.Value)
                {
                    DataRow dr = dt.NewRow();
                    if (writeType)
                    {
                        dr["Type"] = item.Key;
                        writeType = false;
                    }
                    else
                    {
                        dr["Type"] = "";
                    }
                    dr["KEY"] = itemDic.Key;
                    dr["English"] = itemDic.Value;
                    dt.Rows.Add(dr);
                }
            }

            WriteStackLanguageExcel(dt);
            //WriteStackLanguageJson(dt);

            Console.WriteLine("DT Rows Count: " + dt.Rows.Count);
            Console.WriteLine("Create Excel End.");
        }

        private void WriteStackLanguageJson(DataTable dt)
        {

            Dictionary<string, Dictionary<string, Dictionary<string, object>>> dicData = new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();
            dicData.Clear();


            for (int i = 2; i < dt.Columns.Count; i++)
            {
                string column = dt.Rows[0][i].ToString().Trim();
                string column2 = dt.Rows[1][i].ToString().Trim();
                if (string.IsNullOrEmpty(column2))
                {
                    continue;
                }
                if (!dicData.ContainsKey(column))
                {
                    Dictionary<string, Dictionary<string, object>> dic = new Dictionary<string, Dictionary<string, object>>();
                    dic.Clear();
                    dicData.Add(column, dic);
                }
                string curType = string.Empty;
                for (int j = 1; j < dt.Rows.Count; j++)
                {
                    string type = dt.Rows[j][0].ToString().Trim();
                    if (!string.IsNullOrEmpty(type))
                    {
                        curType = type;
                        if (!dicData[column].ContainsKey(curType))
                        {
                            Dictionary<string, object> dicItem = new Dictionary<string, object>();
                            dicData[column].Add(curType, dicItem);
                        }
                    }
                    string key = dt.Rows[j][1].ToString().Trim();
                    string value = dt.Rows[j][i].ToString().Trim();
                    dicData[column][curType].Add(key, value);
                }
            }
            Console.WriteLine(dicData.Count);
            // write json


            foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, object>>> item in dicData)
            {
                StringBuilder sb = new StringBuilder();
                string path = string.Format("stack_{0}.ts", item.Key);
                sb.Append(string.Format("export const language_{0} = ", item.Key));
                sb.Append("{\n");
                int indexData = 0;
                foreach (KeyValuePair<string, Dictionary<string, object>> item1 in item.Value)
                {
                    if (item1.Key == "normal")
                    {
                        int index = 0;
                        foreach (KeyValuePair<string, object> item2 in item1.Value)
                        {
                            sb.Append(string.Format("{0}:\"{1}\"", item2.Key, item2.Value.ToString()));
                            if (index < item1.Value.Count - 1)
                            {
                                sb.Append(",\n");
                            }
                            index++;
                        }
                        if (indexData < item.Value.Count - 1)
                        {
                            sb.Append(",\n");
                        }
                    }
                    else
                    {
                        sb.Append(string.Format("{0}:{{\n", item1.Key));
                        int index = 0;
                        foreach (KeyValuePair<string, object> item2 in item1.Value)
                        {
                            sb.Append(string.Format("{0}:\"{1}\"", item2.Key, item2.Value.ToString()));
                            if (index < item1.Value.Count - 1)
                            {
                                sb.Append(",");
                            }
                            sb.Append("\n");
                            index++;
                        }
                        sb.Append("}");
                        if (indexData < item.Value.Count - 1)
                        {
                            sb.Append(",");
                        }
                        sb.Append("\n");
                    }
                    indexData++;
                }
                sb.Append("}\n");

                byte[] bt = Encoding.UTF8.GetBytes(sb.ToString());

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Write(bt, 0, bt.Length);
                }
            }
        }

        private void WriteStackLanguageExcel(DataTable dt)
        {
            string excelPath = textBoxPath.Text;
            string sheetName = "FB_Stack";

            bool result = ExcelOrXmlManager.WriteDataTableToExcel(dt, excelPath, sheetName);
            MessageBox.Show(string.Format("Write Finish , Result : {0}", result), "Noted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            readContent();
        }
    }
}
