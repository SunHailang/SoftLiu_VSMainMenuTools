using MySql.Data.MySqlClient;
using SoftLiu_VSMainMenuTools.HelpMenu;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowTime();
        }

        private void ShowTime()
        {
            Thread th = new Thread(() =>
            {
                while (!m_formClosing)
                {
                    try
                    {
                        Thread.Sleep(200);
                        if (m_formClosing) break;
                        //labelShowTime.Text = string.Format("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        if (m_timeSpanPuse) continue;

                        //labelShowTime.Text = string.Format("{0}", (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
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

        private void MainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        private void openMySqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form mySql = new MySqlBasedataForm();
            mySql.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadChinaInfo();
        }

        private void ReadChinaInfo()
        {

            string path = Environment.CurrentDirectory;
            textBox1.AppendText(string.Format("path: {0}\n", path));
            string onePath = Directory.GetParent(path).FullName;
            textBox1.AppendText(string.Format("onePath: {0}\n", onePath));
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
    }
}
