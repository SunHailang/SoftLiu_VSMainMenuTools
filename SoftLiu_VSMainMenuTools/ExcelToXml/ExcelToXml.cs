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
    public partial class ExcelToXml : Form
    {
        public ExcelToXml()
        {
            InitializeComponent();
        }

        private void ExcelToXml_Load(object sender, EventArgs e)
        {
            string excelPath = "D:\\VS2015Program\\SF45.xlsx";
            string sheetName = "Sheet1";
            DataTable dt = ExcelOrXmlManager.ReadExcelToDataTable(excelPath, sheetName);
            //ExcelOrXmlManager.WriteXml(dt, sheetName, "D:\\VS2015Program\\SF2.xml");
            ExcelOrXmlManager.WriteJson(dt, sheetName, "D:\\VS2015Program\\SF2.json");
        }

        


    }
}
