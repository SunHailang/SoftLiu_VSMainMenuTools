using MySql.Data.MySqlClient;
using SoftLiu_VSMainMenuTools.HelpMenu;
using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools
{
    public partial class MainMenuForm : Form
    {
        private bool m_timeSpanPuse = false;
        private bool m_formClosing = false;

        public MainMenuForm()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowTimeAndTimeSpan();
            this.textBoxTimeCount.Text = string.Format("{0}-01-01 00:00:00", DateTime.Now.Year + 1);
            this.textBoxTimeBefor.Text = string.Format("{0}-01-01 00:00:00", DateTime.Now.Year + 1);
            comboBoxTime.SelectedIndex = 0;
            comboBoxMD5.SelectedIndex = 0;

            // load csv file to language

            textBox1.AppendText(Localization.Instance.Get("STRING_FISHQUIP_COD_01") + "\n");
            textBox1.AppendText(Localization.Instance.Format("STRING_FISHQUIP_SMALLTURTLE_01", "One", "Two") + "\n");
        }

        private void ShowTimeAndTimeSpan()
        {
            Thread th = new Thread(() =>
            {
                while (!m_formClosing)
                {
                    try
                    {
                        Thread.Sleep(200);
                        if (m_formClosing) break;
                        richTextBoxLocalTime.Text = string.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (m_timeSpanPuse) continue;

                        richTextBoxTimeSpan.Text = string.Format("{0}", (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
                    }
                    catch (Exception msg)
                    {
                        if (m_formClosing) break;
                        else
                        {
                            MessageBox.Show(msg.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }
            });
            th.Start();
        }

        private void excelToXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form excelToXml = new ExcelToXml.ExcelToXmlForm();
            excelToXml.Show();
        }

        private void excelToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openMySqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form mySql = new MySqlBasedataForm();
            mySql.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ReadChinaInfo();
            string sql = "select * from region where level=1;";
            DataSet dataSet = MysqlManager.Instance.SelectTables(sql);
            DataTable dataTable = dataSet.Tables[0];

            dataGridView1.DataSource = dataTable;
        }

        private void ReadChinaInfo()
        {

            string path = Environment.CurrentDirectory;
            //textBox1.AppendText(string.Format("path: {0}\n", path));
            string onePath = Directory.GetParent(path).FullName;
            //textBox1.AppendText(string.Format("onePath: {0}\n", onePath));
            DirectoryInfo dir = new DirectoryInfo(path);
            //textBox1.AppendText(string.Format("{0}\n", dir.Ge);

            // Path.


            //string path = @"C:\Users\hlsun\Desktop\Document.txt";
            //List<ChinaInfo> chinaList = new List<ChinaInfo>();
            //string[] data = File.ReadAllLines(path);
            //StringBuilder sb = new StringBuilder();

            /*string[] data1 = data[2].Split('(', ')');
            string[] data2 = data1[1].Trim().Split('\'');

            textBox1.AppendText(data2.Length.ToString() + "\n");            

            for (int i = 0; i < data2.Length; i++)
            {
                string vinfo = string.Format("index: {0} , value: {1}\n", i, data2[i].ToString());
                textBox1.AppendText(vinfo);
                sb.Append(vinfo);
            }
            */
            //for (int i = 0; i < data.Length; i++)
            //{
            //    //textBox1.AppendText(data[i] + "\n");


            //    string[] data1 = data[i].Split('(', ')');
            //    string[] data2 = data1[1].Trim().Split('\'');
            //    //for (int j = 0; j < data2.Length; j++)
            //    //{
            //    //    string info = data2[j].Replace('\'', ' ').Trim();
            //    //}
            //    ChinaInfo china = new ChinaInfo();
            //    china.id = data2[1].Trim();
            //    china.name = data2[3].Trim();
            //    china.pid = data2[5].Trim();
            //    china.sname = data2[7].Trim();
            //    china.level = data2[9].Replace('\'', ' ').Trim();
            //    china.citycode = data2[11].Replace('\'', ' ').Trim();
            //    china.yzcode = data2[13].Replace('\'', ' ').Trim();
            //    china.mername = data2[15].Replace('\'', ' ').Trim();
            //    china.lng = data2[17].Replace('\'', ' ').Trim();
            //    china.lat = data2[19].Replace('\'', ' ').Trim();
            //    china.pinyin = data2[21].Replace('\'', ' ').Trim();

            //    chinaList.Add(china);
            //}

            //ExcelToXml.ExcelOrXmlManager.m_chinaList = chinaList;

            //for (int i = 0; i < chinaList.Count; i++)
            //{
            //    sb.Append(string.Format("省：{0} , 市：{1} , 全称：{2}{3}",chinaList[i].name, chinaList[i].sname, chinaList[i].mername, "\n"));
            //}
            //string path1 = @"C:\Users\hlsun\Desktop\1.txt";
            //File.WriteAllText(path1, sb.ToString(), Encoding.UTF8);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new AboutForm();
            about.Show();
        }

        private void buttonTimeSpan_Click(object sender, EventArgs e)
        {
            string type = comboBoxTime.SelectedItem.ToString();
            try
            {
                string info = textBoxTimeBefor.Text.Trim();
                switch (type)
                {
                    case "时间转成时间戳":
                        DateTime dt1 = Convert.ToDateTime(info);
                        int ts = TimeUtils.ConvertDateTimeInt(dt1);
                        textBoxTimeAfter.Text = string.Format("{0}", ts);
                        break;
                    case "时间戳转成时间":
                        DateTime dt2 = TimeUtils.GetTime(info);
                        textBoxTimeAfter.Text = string.Format("{0}", dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                        break;
                }
            }
            catch (Exception msg)
            {
                MessageBox.Show(string.Format("{0}\n{1}", type, msg.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private Thread m_CountTimeTh = null;
        private DateTime m_CountTimeDt;
        private bool m_cancelCountTime = false;
        private void buttonCountdown_Click(object sender, EventArgs e)
        {
            if (buttonCountdown.Text == "确定")
            {
                this.textBoxTimeCount.ReadOnly = true;
                this.m_cancelCountTime = false;
                buttonCountdown.Text = "取消";
            }
            else if (buttonCountdown.Text == "取消")
            {
                this.textBoxTimeCount.ReadOnly = false;
                this.m_cancelCountTime = true;
                buttonCountdown.Text = "确定";
                return;
            }
            try
            {
                string timeinfo = this.textBoxTimeCount.Text.Trim();
                m_CountTimeDt = Convert.ToDateTime(timeinfo);

                if (m_CountTimeTh != null)
                {
                    //Monitor mo
                    m_cancelCountTime = true;
                    m_CountTimeTh.Abort();
                    m_CountTimeTh = null;
                }

                if (m_CountTimeTh == null)
                {
                    m_cancelCountTime = false;
                    m_CountTimeTh = new Thread(() =>
                    {
                        bool timeOut = false;
                        while (!timeOut && !m_formClosing && !m_cancelCountTime)
                        {
                            //Thread.Sleep(100);
                            try
                            {
                                double ts = (m_CountTimeDt - DateTime.Now).TotalSeconds;
                                //textBox16.Text = (DateTime.Now - dt).ToString("yyyy - MM - dd HH: mm:ss");
                                string time = (m_CountTimeDt - DateTime.Now).ToString();
                                string tm = time.Substring(0, time.LastIndexOf('.'));
                                tm = tm.Replace(".", "天");
                                textBoxTimeShowCount.Text = string.Format("{0}", tm);
                                if (ts <= 0)
                                {
                                    timeOut = true;
                                    textBoxTimeShowCount.Text = string.Format("{0}", 0);
                                }
                            }
                            catch (Exception msg)
                            {
                                //if (m_formClosing || m_startDealLineTime)
                                //{
                                //    break;
                                //}
                                //else
                                //{
                                //    MessageBox.Show(msg.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                                //}
                            }
                        }
                        textBoxTimeShowCount.Text = string.Empty;
                    });
                    m_CountTimeTh.Start();
                }

            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_formClosing = true;
        }
        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        private void textBoxFilePath_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            if (this.textBoxFilePath.Text.CompareTo(path) != 0)
            {
                this.textBoxFilePath.Text = path; //由一个textBox显示路径
                this.textBoxMD5Str.Text = string.Empty;
            }
        }

        private void textBoxFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private Thread m_MD5ExchargeThread = null;
        private void buttonMD5Sure_Click(object sender, EventArgs e)
        {
            this.textBoxMD5Str.Text = string.Empty;
            bool fsChecked = false;
            bool strChecked = false;
            switch (comboBoxMD5.SelectedItem.ToString())
            {
                case "文件":
                    fsChecked = true;
                    break;
                default:
                    strChecked = true;
                    break;
            }

            if (strChecked)
            {
                string change = this.textBoxFileStr.Text.Trim();

                if (!string.IsNullOrEmpty(change))
                {
                    this.textBoxMD5Str.Text = MD5Utils.GetMD5String(change);
                }
                else
                {
                    MessageBox.Show("Exchange Text can't null or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (fsChecked)
            {
                string m_MD5SelectFilePath = this.textBoxFilePath.Text;
                if (File.Exists(m_MD5SelectFilePath))
                {
                    if (m_MD5ExchargeThread == null || m_MD5ExchargeThread.ThreadState != ThreadState.Running)
                    {
                        m_MD5ExchargeThread = null;
                        m_MD5ExchargeThread = new Thread(() =>
                        {
                            using (FileStream fsStream = new FileStream(m_MD5SelectFilePath, FileMode.Open, FileAccess.Read))
                            {
                                long length = fsStream.Length;
                                byte[] bs = new byte[length];
                                int r = fsStream.Read(bs, 0, bs.Length);
                                string md5Str = MD5Utils.GetMD5String(bs);
                                if (!string.IsNullOrEmpty(md5Str))
                                {
                                    this.textBoxMD5Str.Text = md5Str;
                                }
                                else
                                {
                                    MessageBox.Show("Exchange File String is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        });
                        m_MD5ExchargeThread.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Exchange MD5 Select File not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Exchange Check Box don't select.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.textBoxFilePath.Text.CompareTo(dialog.FileName) != 0)
                {
                    this.textBoxFilePath.Text = dialog.FileName;
                    this.textBoxMD5Str.Text = string.Empty;
                }
            }
        }

        private void textBoxTenExchange_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许输入:数字、退格键(8)、全选(1)、复制(3)、粘贴(22)
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 &&
                    e.KeyChar != 1 && e.KeyChar != 3 && e.KeyChar != 22)
            {
                e.Handled = true;
            }
        }

        private void buttonTenExchange_Click(object sender, EventArgs e)
        {
            string str = textBoxTenExchange.Text.Trim();
            int result = 0;
            if (int.TryParse(str, out result))
            {
                StringBuilder sb16 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                List<int> list = Get16thList(result);
                for (int i = 0; i < list.Count; i++)
                {
                    int index = list[i];
                    sb16.Append(Get16thString(list[i]));
                    List<int> list2 = Get2thString(index);
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < list2.Count; j++)
                    {
                        sb.Append(list2[j]);
                    }
                    string str2 = sb.ToString();
                    for (int k = str2.Length; k < 4; k++)
                    {
                        str2 = string.Format("{0}{1}", 0, str2);
                    }
                    sb2.Append(str2);
                    if (i < list.Count - 1)
                    {
                        sb2.Append(" ");
                    }
                }
                int len = sb16.ToString().Length % 2;
                string head = "";
                if (len != 0)
                {
                    head = "0";
                }
                textBoxSixExchange.Text = "0x" + head + sb16.ToString();
                textBoxTwoExchange.Text = sb2.ToString();
            }
            else
            {
                MessageBox.Show("输入有误.");
            }
        }

        private List<int> Get16thList(int value)
        {
            List<int> list = new List<int>();

            if (value / 16 > 0)
            {
                list.AddRange(Get16thList(value / 16));
            }
            list.Add(value % 16);

            return list;
        }
        private List<int> Get2thString(int index)
        {
            List<int> list = new List<int>();
            if (index / 2 > 0)
            {
                list.AddRange(Get2thString(index / 2));
            }
            list.Add(index % 2);
            return list;
        }

        private char Get16thString(int index)
        {
            char ch = '0';
            switch (index)
            {
                case 10:
                    ch = 'A';
                    break;
                case 11:
                    ch = 'B';
                    break;
                case 12:
                    ch = 'C';
                    break;
                case 13:
                    ch = 'D';
                    break;
                case 14:
                    ch = 'E';
                    break;
                case 15:
                    ch = 'F';
                    break;
                default:
                    ch = index.ToString().ToCharArray(0, 1)[0];
                    break;
            }
            return ch;
        }

        private int GetHexInt(char hex)
        {
            int ch = 0;
            switch (hex)
            {
                case 'A':
                    ch = 10;
                    break;
                case 'B':
                    ch = 11;
                    break;
                case 'C':
                    ch = 12;
                    break;
                case 'D':
                    ch = 13;
                    break;
                case 'E':
                    ch = 14;
                    break;
                case 'F':
                    ch = 15;
                    break;
                default:
                    int.TryParse(hex.ToString(), out ch);
                    break;
            }
            return ch;
        }

        private void textBoxSix_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxTwo_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void buttonSixExchange_Click(object sender, EventArgs e)
        {
            string input = textBoxSix.Text.Trim();
            input = input.ToUpper().Replace("0X", "");

            // 12FF
            StringBuilder sb2 = new StringBuilder();
            List<int> hexList = new List<int>();
            List<int> list2 = null;
            for (int i = 0; i < input.Length; i++)
            {
                int hex = GetHexInt(input[input.Length - i - 1]);
                hexList.Add(hex);
                list2 = Get2thString(GetHexInt(input[i]));
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < list2.Count; j++)
                {
                    sb.Append(list2[j]);
                }
                string str2 = sb.ToString();
                for (int k = str2.Length; k < 4; k++)
                {
                    str2 = string.Format("{0}{1}", 0, str2);
                }
                sb2.Append(str2);
                if (i < input.Length - 1)
                {
                    sb2.Append(" ");
                }
            }

            int hexInt = 0;
            for (int i = 0; i < hexList.Count; i++)
            {
                int val = hexList[i];
                hexInt += (int)Math.Pow(16, i) * val;
            }
            // 因为0的0次方是1 所以结果要减1
            textBoxTenToSix.Text = hexInt.ToString();
            textBoxTwoToSix.Text = sb2.ToString();
        }

        private void buttonTwoExchange_Click(object sender, EventArgs e)
        {
            string input = textBoxTwo.Text.Trim();
            List<string> list = new List<string>();
            input = input.Replace(" ", "");
            int len = input.Length % 4;
            if (len != 0)
            {
                for (int i = 0; i < 4 - len; i++)
                {
                    input = string.Format("{0}{1}", 0, input);
                }
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (i % 4 == 0)
                {
                    string s = input.Substring(i, 4);
                    list.Add(s);
                }
            }
            StringBuilder sb = new StringBuilder();
            List<int> hexList = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                string item = list[i];
                int hex = 0;
                for (int j = item.Length - 1; j >= 0; j--)
                {
                    if (item[item.Length - j - 1] != '0')
                    {
                        hex += (int)Math.Pow(2, j);
                    }
                }
                sb.Append(Get16thString(hex));
                hexList.Add(hex);
            }

            int hexInt = 0;
            for (int i = hexList.Count - 1; i >= 0; i--)
            {
                int val = (int)Math.Pow(16, i) * hexList[hexList.Count - i - 1];
                hexInt += val;
            }
            string outStr = sb.ToString();
            len = outStr.Length % 2;
            if (len != 0)
            {
                for (int i = 0; i < len; i++)
                {
                    outStr = "0" + outStr;
                }
            }

            textBoxSixToTwo.Text = "0X" + outStr;
            textBoxTenToTwo.Text = hexInt.ToString();
        }
    }
}
