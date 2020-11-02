﻿using SoftLiu_VSMainMenuTools.Singleton;
using SoftLiu_VSMainMenuTools.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftLiu_VSMainMenuTools
{
    public class GameDBManager : AutoGeneratedSingleton<GameDBManager>
    {
        private List<ChinaInfo> m_chinaList = null;
        public List<ChinaInfo> chinaList { get { return m_chinaList; } }


        public GameDBManager()
        {
            ReadChinaInfo();
        }

        public void Init() { }

        private void ReadChinaInfo()
        {
            string pathRoot = FileUtils.GetProjectRootPath();
            string path = @"Resources\ChinaInfo.txt";//pathRoot + @"\ChinaInfo.txt";
            string[] data = File.ReadAllLines(path);
            m_chinaList = new List<ChinaInfo>();
            for (int i = 0; i < data.Length; i++)
            {
                string[] data1 = data[i].Split('(', ')');
                string[] data2 = data1[1].Trim().Split('\'');

                ChinaInfo china = new ChinaInfo();
                china.id = data2[1].Trim();
                china.name = data2[3].Trim();
                china.pid = data2[5].Trim();
                china.sname = data2[7].Trim();
                china.level = data2[9].Trim();
                china.citycode = data2[11].Trim();
                china.yzcode = data2[13].Trim();
                china.mername = data2[15].Trim();
                china.lng = data2[17].Trim();
                china.lat = data2[19].Trim();
                china.pinyin = data2[21].Trim();

                m_chinaList.Add(china);
            }
        }

    }
}
