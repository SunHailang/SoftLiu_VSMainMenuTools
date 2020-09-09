using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.EventsManager;
using SoftLiu_VSMainMenuTools.Utils.ServerManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Net;
using System.IO;

namespace SoftLiu_VSMainMenuTools.HelpMenu
{
    public partial class AboutForm : Form
    {

        private Version m_version = null;

        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            EventManager<Events>.Instance.RegisterEvent(Utils.EventsManager.Events.UpdateVersionCompleteEvent, OnUpdaVersiononCompleteEvent);
            m_version = VersionUtils.Instance.version;
            labelVer.Text = m_version.ToString();
        }
        private void AboutForm_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //Color FColor = Color.Blue;
            //Color TColor = Color.Yellow;


            //Brush b = new LinearGradientBrush(this.ClientRectangle, FColor, TColor, LinearGradientMode.ForwardDiagonal);


            //g.FillRectangle(b, this.ClientRectangle);
        }
        private void AboutForm_Resize(object sender, EventArgs e)
        {
            this.Invalidate();//重绘窗体
        }
        private void OnUpdaVersiononCompleteEvent(Events eventType, object[] arg2)
        {
            m_version = VersionUtils.Instance.version;
            labelVer.Text = m_version.ToString();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://localhost:8080/";
                RequestManager.Instance.DownloadFile(url, progressBarDownload, labelProcess);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

        }

        ~AboutForm()
        {
            EventManager<Events>.Instance.DeregisterEvent(Utils.EventsManager.Events.UpdateVersionCompleteEvent, OnUpdaVersiononCompleteEvent);
        }

        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


    }
}
