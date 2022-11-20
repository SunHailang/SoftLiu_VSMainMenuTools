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

namespace WinFormsExcel
{
    public partial class WinFormsExcel : Form
    {
        public static readonly string configPath = Path.GetFullPath($"{Environment.CurrentDirectory}/../../../");

        public WinFormsExcel()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ExcelTConfig.Entry.LogInfoEvent += updateLogInfo;

            updateLogInfo("Info Type ...", ExcelTConfig.LogLevelType.InfoType);
            updateLogInfo("Warnning Type ...", ExcelTConfig.LogLevelType.WarnningType);
            updateLogInfo("Error Type ...", ExcelTConfig.LogLevelType.ErrorType);

            //string configPath = Path.GetFullPath($"{Environment.CurrentDirectory}/../../../");
            ExcelTConfig.Entry.LoadInit(configPath);


            //TableLoader.LoadMain();
        }

        private void updateLogInfo(string msg, ExcelTConfig.LogLevelType level)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            if (richTextLog.TextLength > 0)
            {
                richTextLog.AppendText(Environment.NewLine);
            }

            richTextLog.SelectionStart = richTextLog.TextLength;
            richTextLog.SelectionLength = 0;
            richTextLog.SelectionColor = ((uint)level >= 10) ? Color.Red : (uint)level>= 5 ? Color.Yellow : Color.Black;     // 设置输入字体颜色
            richTextLog.AppendText(DateTime.Now.ToString("[HH:mm:ss:ffff] ") + msg);
            richTextLog.SelectionColor = richTextLog.ForeColor;

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            //ExcelTConfig.Entry.ExportAll();
            ExcelTConfig.ExcelHandler.Export();
        }

        private void btnCSharp_Click(object sender, EventArgs e)
        {
            ExcelTConfig.CSharpHandler.Export();
        }
    }
}
