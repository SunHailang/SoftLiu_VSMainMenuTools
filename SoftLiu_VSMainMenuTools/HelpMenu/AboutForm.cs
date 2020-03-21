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

        private void OnUpdaVersiononCompleteEvent(Events eventType, object[] arg2)
        {
            m_version = VersionUtils.Instance.version;
            labelVer.Text = m_version.ToString();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            //TODU
            //DialogResult result = MessageBox.Show("在线更新", "更新", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //if(result == DialogResult.Yes)
            //{
            //    if(ConfigurationUtils.Instance.existItem("versionName"))
            //    {
            //        ConfigurationUtils.Instance.modifyItem("versionName", "1.0.182356");//string.Format("{0}", m_version.MinorRevision + 1));
            //        EventManager<Events>.Instance.TriggerEvent(Utils.EventsManager.Events.UpdateAppConfigEvent, null);
            //    }
            //}
            try
            {
                string url = "http://localhost:8080/AssetBundles/";
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
    }
}
