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
    }
}
