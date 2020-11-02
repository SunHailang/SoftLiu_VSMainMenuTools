using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.Data.GUI;
using SoftLiu_VSMainMenuTools.UGUI;
using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools
{
    public partial class SplashLoader : Form
    {
        private ProgressBarExtension m_progressBarSplash;

        private bool close = false;

        public SplashLoader()
        {
            InitializeComponent();

            close = true;

            //this.Visible = false;
        }

        //导入dll
        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        //判断网络状况的方法,返回值true为连接，false为未连接
        public extern static bool InternetGetConnectedState(out int conState, int reder);


        //在你的button事件中写下如下代码就行
        private void RefreshX()
        {
            while (true)
            {
                int n = 0;
                if (!InternetGetConnectedState(out n, 0))
                {
                    MessageBox.Show("网络处于未连接状态");

                }
            }
        }

        private void SplashLoader_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            m_progressBarSplash = new ProgressBarExtension(progressBarSplash);
            m_progressBarSplash.Value = 0;
            m_progressBarSplash.onChanged += (value) =>
            {
                if (value >= 100)
                {
                    Thread th = new Thread(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                Thread.Sleep(800);
                                //m_parent.MainMenuForm_Init();

                                close = false;
                                this.Close();
                                this.DialogResult = DialogResult.OK;
                                Form main = new MainMenuForm();
                                FormManager.Instance.OpenForm(main);
                                //Application.Run(new MainMenuForm());
                               
                                break;
                            }
                            catch (Exception msg)
                            {
                                Console.WriteLine("onChanged : " + msg.Message);
                            }
                        }
                    });
                    th.Start();
                }
            };
            //获取UI线程同步上下文
            App.Instance.InitSyncContext(SynchronizationContext.Current);
            m_progressBarSplash.Value = 30;

            // read csv file
            Localization.Instance.Init();

            GameDBManager.Instance.Init();

            //m_progressBarSplash.Value = 100;
            init();
        }

        private void init()
        {
            Thread th = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(100);
                        if (m_progressBarSplash.Value >= 100)
                        {
                            break;
                        }
                        m_progressBarSplash.Value++;

                    }
                    catch (Exception msg)
                    {
                        Console.WriteLine("SplashLoader init : " + msg.Message);
                    }
                }
            });
            th.Start();
        }

        private void SplashLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = close;
            //Application.Exit();
        }

        private void progressBarSplash_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void progressBarSplash_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }
    }
}
