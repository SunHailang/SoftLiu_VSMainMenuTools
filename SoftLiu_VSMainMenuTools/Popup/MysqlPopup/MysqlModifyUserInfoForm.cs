using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.Popup.MysqlPopup
{
    public partial class MysqlModifyUserInfoForm : Form
    {

        private Student m_currentDatabaseStudent = null;

        private List<ChinaInfo> m_chinaList = null;
        private Dictionary<string, Dictionary<string, List<string>>> cityDic = new Dictionary<string, Dictionary<string, List<string>>>();

        private DateTime m_curBrithday = default(DateTime);

        public MysqlModifyUserInfoForm()
        {
            InitializeComponent();
        }

        private void MysqlModifyUserInfoForm_Load(object sender, EventArgs e)
        {

        }

        public void MysqlModifyUserInfoFormInit(Student student)
        {
            this.m_currentDatabaseStudent = student;
            m_chinaList = DatabaseManager.Instance.chinaList;
            for (int i = 0; i < m_chinaList.Count; i++)
            {
                string merName = m_chinaList[i].mername;
                string[] name = merName.Split(',');
                if (name.Length == 2)
                {
                    Dictionary<string, List<string>> dataDic = new Dictionary<string, List<string>>();
                    cityDic.Add(name[1], dataDic);

                }
                else if (name.Length == 3)
                {
                    List<string> dataList = new List<string>();
                    cityDic[name[1]].Add(name[2], dataList);
                }
                else if (name.Length == 4)
                {
                    cityDic[name[1]][name[2]].Add(name[3]);
                }
            }
            foreach (var item in cityDic.Keys)
            {
                if (!comboBox1.Items.Contains(item))
                {
                    comboBox1.Items.Add(item);
                }
            }

            List<int> gradeIDlist = GetComboBoxItems("select distinct gradeid from class;");
            this.comboBoxGrade.Items.Clear();
            for (int i = 0; i < gradeIDlist.Count; i++)
            {
                this.comboBoxGrade.Items.Add(gradeIDlist[i]);
            }
            this.comboBoxGrade.SelectedItem = student.GradeID;

            List<int> classIDlist = GetComboBoxItems($"select classid from class where gradeid={student.GradeID};");
            this.comboBoxClass.Items.Clear();
            for (int i = 0; i < classIDlist.Count; i++)
            {
                this.comboBoxClass.Items.Add(classIDlist[i]);
            }
            this.comboBoxClass.SelectedItem = student.ClassID;

            this.textBoxStuNum.Text = student.StuNum;

            this.textBoxName.Text = student.Name;
            this.textBoxAge.Text = string.Format("{0}", student.Age);
            string[] address = student.GetAddressFromDatabase().Split('$');
            if (address.Length <= 3)
            {
                this.textBoxAddress.Text = student.Address;
            }
            else
            {
                this.comboBox1.SelectedItem = address[0];
                this.comboBox2.SelectedItem = address[1];
                this.comboBox3.SelectedItem = address[2];
                this.textBoxAddress.Text = address[3];
            }
            this.textBoxEmai.Text = student.Email;
            this.textBoxPhone.Text = student.PhoneNum;
            this.comboBoxGender.SelectedItem = student.Gender;
            this.textBoxCardID.Text = student.CardID;
            this.textBoxCardID.ReadOnly = true;

            int gender = 0;
            m_curBrithday = default(DateTime);
            string area = "";
            string errMsg = "";
            RegexUtils.CheckCardID(student.CardID, out gender, out m_curBrithday, out area, out errMsg);
            Console.WriteLine($"{errMsg}");
            DateTime now = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                int offsetYear = i * -1;
                int curYear = now.AddYears(offsetYear).Year;
                if (curYear < 1974)
                {
                    break;
                }
                this.comboBoxYear.Items.Add(curYear);
            }
            this.comboBoxYear.SelectedItem = m_curBrithday.Year;
        }

        private List<int> GetComboBoxItems(string sql)
        {
            List<int> list = new List<int>();
            try
            {
                string gradesql = sql;// "select gradeid from grade;";
                DataSet gradeDataset = MysqlManager.Instance.SelectTables(gradesql);
                DataTable gradeTable = gradeDataset.Tables[0];
                DataRow[] gradeRows = gradeTable.Select();

                for (int i = 0; i < gradeRows.Length; i++)
                {
                    DataRow row = gradeRows[i];
                    list.Add(Convert.ToInt32(row[0]));
                }
                list.Sort((x, y) => { return x - y; });
            }
            catch (Exception error)
            {
                Console.WriteLine($"GetComboBoxItems Error: {error.Message}");
            }


            return list;
        }

        private void buttonSure_Click(object sender, EventArgs e)
        {
            if (this.m_currentDatabaseStudent == null)
            {
                MessageBox.Show("选择需要修改的数据.");
                return;
            }
            StringBuilder sb = new StringBuilder();
            //update 表名 set 列1=值1, 列2=值2, ... where 条件;
            Student student = this.m_currentDatabaseStudent;
            string befor = string.Format("cardID=\"{0}\" name=\"{1}\" age={2} gender={3} phoneNum=\"{4}\" Email=\"{5}\" address=\"{6}\"",
                    student.CardID, student.Name, student.Age, student.GetGenderFromDatabase(), student.PhoneNum, student.Email, student.GetAddressFromDatabase());
            string cardID = textBoxCardID.Text.Trim();
            //if (!cardID.Equals(student.CardID))
            //{
            //    sb.Append(string.Format("cardID=\"{0}\" ", cardID));
            //}
            string name = textBoxName.Text.Trim();
            if (!name.Equals(student.Name))
            {
                sb.Append(string.Format("name=\"{0}\", ", name));
            }
            int age = 0;
            int.TryParse(textBoxAge.Text, out age);
            if (!age.Equals(student.Age))
            {
                sb.Append(string.Format("age={0}, ", age));
            }
            int gender = comboBoxGender.SelectedIndex;
            if (!gender.Equals(student.GetGenderFromDatabase()))
            {
                sb.Append(string.Format("gender={0}, ", gender));
            }
            string phoneNum = textBoxPhone.Text.Trim();
            if (!phoneNum.Equals(student.PhoneNum))
            {
                sb.Append(string.Format("phoneNum=\"{0}\", ", phoneNum));
            }
            string email = textBoxEmai.Text.Trim();
            if (!email.Equals(student.Email))
            {
                sb.Append(string.Format("Email=\"{0}\", ", email));
            }
            string address = string.Format("{0}${1}${2}${3}", comboBox1.SelectedItem, comboBox2.SelectedItem, comboBox3.SelectedItem, textBoxAddress.Text.Trim());
            if (!address.Equals(student.GetAddressFromDatabase()))
            {
                sb.Append(string.Format("address=\"{0}\", ", address));
            }
            string after = string.Format("{0}", sb.ToString().Trim().Trim(','));

            if (befor.CompareTo(after) == 0 || string.IsNullOrEmpty(after))
            {
                MessageBox.Show("没有改变任何数据.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string sql = string.Format("update student set {0} where cardID=\"{1}\";", after, cardID);
                int result = MysqlManager.Instance.UpdateData(sql);
                if (result > 0)
                {
                    MessageBox.Show("修改成功.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改失败.", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.m_currentDatabaseStudent = null;
        }


        private void textBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBoxPhone.Text.Length >= 11 && e.KeyChar != 8 &&
                    e.KeyChar != 1 && e.KeyChar != 3 && e.KeyChar != 22)
            {
                e.Handled = true;
                return;
            }
            // 允许输入:数字、退格键(8)、全选(1)、复制(3)、粘贴(22)
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 &&
                    e.KeyChar != 1 && e.KeyChar != 3 && e.KeyChar != 22)
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = (string)comboBox1.SelectedItem;
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    text = comboBox1.Text.ToString();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("comboBox1: " + exc.Message);
            }
            finally
            {
                comboBox2.Items.Clear();
                foreach (var item in cityDic[text].Keys)
                {
                    if (!comboBox2.Items.Contains(item))
                    {
                        comboBox2.Items.Add(item);
                    }
                }
                if (comboBox2.Items.Count <= 0)
                {
                    comboBox2.Items.Add(comboBox1.SelectedItem);
                }
                comboBox2.SelectedIndex = 0;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            try
            {
                if (cityDic[comboBox1.SelectedItem.ToString()].ContainsKey(comboBox2.SelectedItem.ToString()))
                {
                    foreach (var item in cityDic[comboBox1.SelectedItem.ToString()][comboBox2.SelectedItem.ToString()])
                    {
                        if (!comboBox3.Items.Contains(item))
                        {
                            comboBox3.Items.Add(item);
                        }
                    }
                }
                else
                {
                    if (comboBox3.Items.Count <= 0)
                    {
                        comboBox3.Items.Add(comboBox2.SelectedItem);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("comboBox2_SelectedIndexChanged : " + exc.Message);
                comboBox3.Items.Clear();
                if (comboBox3.Items.Count <= 0)
                {
                    comboBox3.Items.Add(comboBox2.SelectedItem);
                }
            }
            comboBox3.SelectedIndex = 0;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxAddress.Text = string.Empty;
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxMonth.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                this.comboBoxMonth.Items.Add(i);
            }
            this.comboBoxMonth.SelectedItem = m_curBrithday.Month;
        }

        private void comboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yearItem = this.comboBoxYear.SelectedItem.ToString();
            int year = 0;
            int.TryParse(yearItem, out year);

            string monthItem = this.comboBoxMonth.SelectedItem.ToString();
            int month = 0;
            int.TryParse(monthItem, out month);

            int[] a = { 31, (year % 4 == 0 && year % 100 != 0 || year % 400 == 0) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int days = a[month - 1];

            this.comboBoxDay.Items.Clear();
            for (int i = 1; i <= days; i++)
            {
                this.comboBoxDay.Items.Add(i);
            }
            this.comboBoxDay.SelectedItem = m_curBrithday.Day;

        }
    }
}
