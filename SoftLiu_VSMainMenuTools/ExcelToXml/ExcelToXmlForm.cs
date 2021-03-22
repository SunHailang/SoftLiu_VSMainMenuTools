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

        private void ExcelToXml_Load(object sender, EventArgs e)
        {

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

        public class ExcelCellData
        {
            public string columnName;
            public string rowData;
        }

        private void WriteExcel(string excelPath, string sheetName)
        {
            List<string> columns = new List<string>();

            DataTable dt = new DataTable();
            dt.Clear();

            for (int i = 0; i < columns.Count; i++)
            {
                dt.Columns.Add(columns[i]);
            }

            ExcelOrXmlManager.WriteDataTableToExcel(dt, excelPath, sheetName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExcelCompare.ReadExcel("", "");
        }
    }
}
