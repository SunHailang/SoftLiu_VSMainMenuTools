﻿/// <summary>
/// 
/// __author__ = "sun hai lang"
/// __date__ 2019-07-16
/// 
/// </summary>

using SoftLiu_VSMainMenuTools.Singleton;
using SoftLiu_VSMainMenuTools.Utils.EventsManager;
using System;
using System.Reflection;

namespace SoftLiu_VSMainMenuTools.Utils
{
    public sealed class VersionUtils : AutoGeneratedSingleton<VersionUtils>
    {
        /// <summary>
        /// 获取当前程序的版本号
        /// </summary>
        public Version version
        {
            private set;
            get;
        }

        public string fileVersion
        {
            private set;
            get;
        }

        public string product
        {
            private set;
            get;
        }

        public string copyRight
        {
            private set;
            get;
        }

        public string title
        {
            private set;
            get;
        }

        public string company
        {
            private set;
            get;
        }

        public VersionUtils()
        {
            Update();
            EventManager<Events>.Instance.RegisterEvent(Events.UpdateVersionEvent, OnUpdateVersionEvent);

        }

        private void Update()
        {
            string ver = ReadAssemblyVersion();
            version = new Version(ver);
            fileVersion = ReadAssemblyFileVersion();
            product = ReadAssemblyProduct();
            copyRight = ReadAssemblyCopyright();
            title = ReadAssemblyTitle();
            company = ReadAssemblyCompany();
        }

        private void OnUpdateVersionEvent(Events eventType, object[] arg2)
        {
            Update();
            EventManager<Events>.Instance.TriggerEvent(Events.UpdateVersionCompleteEvent, null);
        }

        ~VersionUtils()
        {
            if (version != null)
                version = null;

            EventManager<Events>.Instance.DeregisterEvent(Events.UpdateVersionEvent, OnUpdateVersionEvent);
        }

        public string ReadAssemblyProduct()
        {
            Type t = typeof(Program);
            AssemblyProductAttribute productAttr = t.Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true)[0] as AssemblyProductAttribute;
            return productAttr.Product;
        }

        public string ReadAssemblyCopyright()
        {
            Type t = typeof(Program);
            AssemblyCopyrightAttribute productAttr = t.Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true)[0] as AssemblyCopyrightAttribute;
            return productAttr.Copyright;
        }

        public string ReadAssemblyTitle()
        {
            Type t = typeof(Program);
            AssemblyTitleAttribute productAttr = t.Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), true)[0] as AssemblyTitleAttribute;
            return productAttr.Title;
        }

        public string ReadAssemblyCompany()
        {
            Type t = typeof(Program);
            AssemblyCompanyAttribute productAttr = t.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true)[0] as AssemblyCompanyAttribute;
            return productAttr.Company;
        }

        public string ReadAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //Type t = typeof(Program);
            //AssemblyVersionAttribute productAttr = t.Assembly.GetCustomAttributes(typeof(AssemblyVersionAttribute), true)[0] as AssemblyVersionAttribute;
            //return productAttr.Version;
        }

        public string ReadAssemblyFileVersion()
        {
            Type t = typeof(Program);
            AssemblyFileVersionAttribute productAttr = t.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true)[0] as AssemblyFileVersionAttribute;
            return productAttr.Version;
        }

    }
}
