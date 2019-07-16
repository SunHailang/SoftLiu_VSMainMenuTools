using System;
using System.Threading;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools
{
    public partial class MainMenu : Form
    {
        private bool m_timeSpanPuse = false;
        private bool m_formClosing = false;

        public MainMenu()
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
            Form excelToXml = new ExcelToXml.ExcelToXml();
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
    }
}
