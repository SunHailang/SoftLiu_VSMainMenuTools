﻿using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.Data.GUI;
using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private MainMenuForm m_parent = null;

        public SplashLoader()
        {
            InitializeComponent();

            close = true;
        }

        public void InitConfiger(MainMenuForm parent)
        {
            Console.WriteLine("InitConfiger");
            //MessageBox.Show("InitConfiger");
            m_parent = parent;
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
                                m_parent.MainMenuForm_Init();
                                close = false;
                                this.Close();
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
        }

        private void progressBarSplash_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void progressBarSplash_ChangeUICues(object sender, UICuesEventArgs e)
        {

        }
    }
}
