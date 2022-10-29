using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SoftLiu_VSMainMenuTools.ExcelToXml
{
    public static class ExcelCompare
    {
        const string pathAll = "Students/";
        public static Dictionary<string, StudentCompaer> ReadExcel(string path, string sheetName)
        {
            //string path1 = pathAll + "武汉工程大学2020年硕士研究生拟录取名单.pdf";
            string path1 = pathAll + "计算机学院2020研究生复试成绩公示（一志愿）.pdf";
            Dictionary<string, StudentCompaer> studentAlls = new Dictionary<string, StudentCompaer>();
            List<string> subjects = new List<string>();
            subjects.Add("电子信息");
            subjects.Add("信息与通信工程");
            subjects.Add("光学工程");
            ConvertPdfToExcel(path1, studentAlls, 0, 5, 4, 2, subjects);

            //string path3 = pathAll + "山西大学2020年硕士研究生一志愿考生复试名单.pdf";
            //Dictionary<string, StudentCompaer> studentAlls1 = new Dictionary<string, StudentCompaer>();
            //ConvertPdfToExcel(path3, studentAlls1, 1, 12, 6, 4, subjects);


            //DataTable dtStudent = new DataTable();
            //dtStudent.Clear();
            //dtStudent.Columns.Add("姓名");
            //dtStudent.Columns.Add("学院");
            //dtStudent.Columns.Add("专业");
            //dtStudent.Columns.Add("初试总分");
            List<StudentCompaer> residueList = new List<StudentCompaer>();
            foreach (KeyValuePair<string, StudentCompaer> item in studentAlls)
            {
                //if (!studentAlls1.ContainsKey(item.Key))
                residueList.Add(item.Value);
            }

            residueList.Sort();
            StringBuilder sbS = new StringBuilder();
            for (int i = 0; i < residueList.Count; i++)
            {
                string line = $"姓名：{residueList[i].m_name}\t\t学院：{residueList[i].m_college}\t\t专业：{residueList[i].m_subject}\t\t初试总分：{residueList[i].m_score}";
                sbS.Append(line);
                sbS.Append(Environment.NewLine);
                //DataRow dr = dtStudent.NewRow();
                //dr["姓名"] = residueList[i].m_name;
                //dr["学院"] = residueList[i].m_college;
                //dr["专业"] = residueList[i].m_subject;
                //dr["初试总分"] = residueList[i].m_score;
                //dtStudent.Rows.Add(dr);
            }
            File.WriteAllText(pathAll + "武汉工程大学.txt", sbS.ToString());
            //ExcelOrXmlManager.WriteDataTableToExcel(dtStudent, pathAll + "山西大学.xls", "Sheet1");

            return null;
#if false

            path = "宁夏大学_1.xls";
            sheetName = "Sheet1";

            DataTable dt = ExcelOrXmlManager.ReadExcelToDataTable(path, sheetName);

            if (dt.Rows.Count <= 1 || dt.Columns.Count <= 1)
            {
                Console.WriteLine("None Item Information.");
                return null;
            }

            Dictionary<string, StudentCompaer> students = new Dictionary<string, StudentCompaer>();

            for (int j = 1; j < dt.Rows.Count; j++)
            {
                string name = string.Empty;
                string score = string.Empty;
                string subject = string.Empty;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    string columnsName = dt.Rows[0][i].ToString().Trim();
                    switch (columnsName)
                    {
                        case "姓名":
                            name = dt.Rows[j][i].ToString().Trim();
                            break;
                        case "专业名称":
                            subject = dt.Rows[j][i].ToString().Trim();
                            break;
                        case "初试":
                            score = dt.Rows[j][i].ToString().Trim();
                            break;
                    }
                }
                StudentCompaer student = new StudentCompaer() { m_name = name, m_score = score, m_subject = subject };
                if (!students.ContainsKey(name))
                {
                    students.Add(name, student);
                }
                else
                {
                    Console.WriteLine($"Has Name: {name}");
                }
            }
            Console.WriteLine($"Complete: student count: {students.Count}");
            return students;
#endif
        }


        public static void ConvertPdfToExcel(string pdfPath, Dictionary<string, StudentCompaer> students, int nameIndex, int scoreIndex, int subjectIndex, int collegeIndex, List<string> screenList)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfPath);

            // 实例化一个StringBuilder 对象
            StringBuilder content = new StringBuilder();

            //提取PDF所有页面的文本
            int indexImage = 0;
            foreach (PdfPageBase page in doc.Pages)
            {
                content.Append(page.ExtractText());

                //Image[] image = page.ExtractImages();
                //Console.WriteLine("image Length: " + image.Length);
                //foreach (var item in image)
                //{
                //    item.Save(pathAll + $"{indexImage++}.png");
                //}
            }

            //将提取到的文本写为.txt格式并保存到本地路径
            String fileName = pathAll + "获取文本.txt";
            File.WriteAllText(fileName, content.ToString());

            string[] dataLines = File.ReadAllLines(fileName);
            for (int i = 0; i < dataLines.Length; i++)
            {
                string data1 = dataLines[i].Trim().Replace(" ", "$");
                int index = 0;
                List<char> dataList = new List<char>();
                StringBuilder data = new StringBuilder();
                while (index < data1.Length)
                {
                    char dataC = data1[index];
                    if (dataC != '$') dataList.Add(dataC);
                    if (dataList[dataList.Count - 1] == '$')
                    {
                        index++;
                        continue;
                    }
                    dataList.Add(dataC);
                    data.Append(dataC);
                    index++;
                }

                string[] datas = data.ToString().Split('$');
                long num = 0;
                //if (long.TryParse(datas[0], out num))
                {
                    if (datas.Length > subjectIndex && datas.Length > nameIndex && datas.Length > scoreIndex)
                    {
                        string name = datas[nameIndex];
                        string score = datas[scoreIndex];
                        string subject = datas[subjectIndex];
                        string college = datas[collegeIndex];
                        if (!students.ContainsKey(name) && screenList.Contains(subject))
                            students.Add(datas[nameIndex], new StudentCompaer() { m_name = name, m_score = score, m_subject = subject, m_college = college });
                    }
                    else
                    {
                        Console.WriteLine($"Error:: Line:{num}, datas.Length: {datas.Length}, nameIndex: {nameIndex}, scoreIndex: {scoreIndex}, subjectIndex: {subjectIndex}");
                    }
                }
            }

            Console.WriteLine("ConvertPdfToExcel");
        }
    }

}

public class StudentCompaer : IComparable
{
    public string m_name;
    public string m_score;
    public string m_subject;
    public string m_college;

    public int CompareTo(object obj)
    {
        if (obj is StudentCompaer)
        {
            int ret1 = this.m_college.CompareTo(((StudentCompaer)obj).m_college);
            if (ret1 == 0)
            {
                int ret = this.m_subject.CompareTo(((StudentCompaer)obj).m_subject);
                if (ret == 0)
                {
                    return this.m_score.CompareTo(((StudentCompaer)obj).m_score);
                }
                return ret;
            }
            return ret1;
        }

        return 0;
    }
}
