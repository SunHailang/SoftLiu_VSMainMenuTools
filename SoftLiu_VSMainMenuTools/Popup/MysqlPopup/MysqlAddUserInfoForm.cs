using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.DatabaseManager;
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

namespace SoftLiu_VSMainMenuTools.Popup.MysqlPopup
{
    public partial class MysqlAddUserInfoForm : Form
    {

        private bool m_autoInsertData = false;

        private List<ChinaInfo> m_chinaList = null;

        private Dictionary<string, Dictionary<string, List<string>>> cityDic = new Dictionary<string, Dictionary<string, List<string>>>();


        public MysqlAddUserInfoForm()
        {
            InitializeComponent();
        }

        private void MysqlAddUserInfoForm_Load(object sender, EventArgs e)
        {
            
        }

        public void Init()
        {
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
            this.comboBoxGrade.SelectedIndex = 0;

            string gradeID = this.comboBoxGrade.SelectedItem.ToString();

            List<int> classIDlist = GetComboBoxItems($"select classid from class where gradeid={gradeID};");
            this.comboBoxClass.Items.Clear();
            for (int i = 0; i < classIDlist.Count; i++)
            {
                this.comboBoxClass.Items.Add(classIDlist[i]);
            }
            this.comboBoxClass.SelectedIndex = 0;
            // 100102020110004
            string addStunum = $"100102020{this.comboBoxGrade.SelectedItem}{this.comboBoxClass.SelectedItem}0001";
            this.textBoxStuNum.Text = addStunum;
            this.textBoxName.Text = string.Empty;
            this.textBoxAge.Text = string.Format("{0}", 18);
            this.textBoxAddress.Text = string.Empty;
            this.textBoxEmai.Text = string.Empty;
            this.textBoxPhone.Text = string.Empty;
            this.comboBoxGender.SelectedItem = "男";
            this.textBoxCardID.ReadOnly = false;
            // 342201200201204892
            this.textBoxCardID.Text = "342201200201200001";
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int insert = InsesetDataToBasedata();
            if (insert > 0)
            {
                MessageBox.Show("insert success.");
            }
            else if (insert == -2)
            {
                MessageBox.Show("insert data has exists.");
            }
            else
            {
                MessageBox.Show("insert Failed.");
            }
        }

        private int InsesetDataToBasedata()
        {
            int result = -1;
            // 对应 sql 语句 如果是字符串类型 需要加双引号
            string tableName = "student";
            string sql = "{0}, \"{1}\", {2}, {3}, \"{4}\", \"{5}\", \"{6}\", \"{7}\", {8}";
            string name = textBoxName.Text.Trim();
            string cardID = textBoxCardID.Text.Trim();
            int age = 0;
            int.TryParse(textBoxAge.Text.Trim(), out age);
            int gender = comboBoxGender.SelectedIndex;
            string phone = textBoxPhone.Text.Trim();
            if (!RegexUtils.IsPhoneNumber(phone))
            {
                // 判断是否是手机号码
                MessageBox.Show("不是手机号码，重新输入.");
                return result;
            }
            string email = textBoxEmai.Text.Trim();
            string address = string.Format("{0}${1}${2}${3}", comboBox1.SelectedItem, comboBox2.SelectedItem, comboBox3.SelectedItem, textBoxAddress.Text.Trim());
            sql = string.Format(sql, 0, name, age, gender, phone, email, cardID, address, 0);

            bool exists = MysqlManager.Instance.ExistsData(tableName, string.Format("CardID=\"{0}\"", cardID));
            if (exists)
            {
                //MessageBox.Show("insert data has exists.");
                result = -2;
            }
            else
            {
                result = MysqlManager.Instance.InsesetData(tableName, sql);
            }
            return result;
        }

        private void buttonAutoAdd_Click(object sender, EventArgs e)
        {
            StartStep();
        }

        private void StartStep()
        {
            string tableName = "student";
            bool back = false;
            int index = 0;
            List<string> sqlList = new List<string>();
            List<string> cardIdList = new List<string>();
            Thread th = new Thread(() =>
            {
                m_autoInsertData = true;
                int sutunum = 1;
                //insert into student(classid, gradeid, stunum, cardid, name, age, gender, phonenum, email, address, isdelete) 
                //       values(1, 1, '100102020110001', '342201200201204892', 'zhansan', 18, 0, '1587920110', 'unknow@emil', '', 0);
                object obj = MysqlManager.Instance.GetMaxData(tableName, "stunum");
                string maxStuNum = obj.ToString();
                string stunumSub = maxStuNum.Substring(11, 4);
                int.TryParse(stunumSub.ToString(), out sutunum);
                if (sutunum <= 0)
                {
                    back = true;
                }
                while (true)
                {
                    if (++index > 100)
                    {
                        back = true;
                    }
                    if (back)
                    {
                        break;
                    }
                    m_autoInsertData = true;
                    sutunum += 1;
                    textBoxCardID.Text = string.Format("{0}", sutunum);
                    StringBuilder sb = new StringBuilder();
                    Random ra = new Random();
                    int key = ra.Next(97, 123);
                    for (int i = 0; i < 6; i++)
                    {
                        sb.Append((char)key);
                    }
                    textBoxName.Text = string.Format("{0}", sb.ToString());

                    textBoxAge.Text = ra.Next(0, 100).ToString();
                    comboBoxGender.SelectedIndex = ra.Next(0, 3);
                    //@"^1(3[0-9]|5[0-9]|7[6-8]|8[0-9])[0-9]{8}$"
                    char[] charS = { '3', '5', '7', '8' };
                    string num = string.Format("1{0}{1}{2}{3}", charS[ra.Next(0, charS.Length)], ra.Next(0, 10), ra.Next(1000, 9999), ra.Next(1000, 9999));
                    textBoxPhone.Text = num;
                    textBoxEmai.Text = num + "@qq.com";
                    comboBox1.SelectedIndex = ra.Next(0, comboBox1.Items.Count);
                    Thread.Sleep(200);
                    comboBox2.SelectedIndex = ra.Next(0, comboBox2.Items.Count);
                    Thread.Sleep(200);
                    comboBox3.SelectedIndex = ra.Next(0, comboBox3.Items.Count);
                    textBoxAddress.Text = string.Format("{0}村{1}", comboBox1.SelectedItem.ToString(), ra.Next(1, 100));

                    Thread.Sleep(1000);

                    // 对应 sql 语句 如果是字符串类型 需要加双引号

                    string sql = "{0}, \"{1}\", {2}, {3}, \"{4}\", \"{5}\", \"{6}\", \"{7}\", {8}";
                    string name = textBoxName.Text.Trim();
                    string cardID = textBoxCardID.Text.Trim();
                    int age = 0;
                    int.TryParse(textBoxAge.Text.Trim(), out age);
                    int gender = comboBoxGender.SelectedIndex;
                    string phone = textBoxPhone.Text.Trim();
                    string email = textBoxEmai.Text.Trim();
                    string address = string.Format("{0}${1}${2}${3}", comboBox1.SelectedItem, comboBox2.SelectedItem, comboBox3.SelectedItem, textBoxAddress.Text.Trim());

                    if (!cardIdList.Contains(cardID.ToString()))
                    {
                        sql = string.Format(sql, 0, name, age, gender, phone, email, cardID, address, 0);
                        sqlList.Add(sql);
                        cardIdList.Add(cardID.ToString());
                    }
                }

                int insert = MysqlManager.Instance.InsesetBigData(tableName, sqlList);
                if (insert > 0)
                {
                    MessageBox.Show("insert success.");
                }
                else
                {
                    MessageBox.Show("insert Failed.");
                }
                m_autoInsertData = false;
            });
            th.Start();


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

        private void textBoxAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许输入:数字、退格键(8)、全选(1)、复制(3)、粘贴(22)
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 &&
                    e.KeyChar != 1 && e.KeyChar != 3 && e.KeyChar != 22)
            {
                e.Handled = true;
            }
        }

        private void comboBoxGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gradeid = this.comboBoxGrade.SelectedItem.ToString();
            string sql = $"select classid from class where gradeid={gradeid}";
            List<int> list = GetComboBoxItems(sql);
            this.comboBoxClass.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                this.comboBoxClass.Items.Add(list[i]);
            }
            this.comboBoxClass.SelectedIndex = 0;
        }
    }
}
