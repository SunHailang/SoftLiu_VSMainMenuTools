using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.Utils;
using SoftLiu_VSMainMenuTools.Utils.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools
{
    public partial class MySqlBasedataForm : Form
    {
        private DataTable m_currentDataTable = null;

        private Dictionary<int, Student> m_currentStudentDic = null;

        private List<ChinaInfo> m_chinaList = null;

        private Dictionary<string, Dictionary<string, List<string>>> cityDic = new Dictionary<string, Dictionary<string, List<string>>>();

        //private delegate void StudentDatabaseDelegate(Student student); // 委托的声明

        //private StudentDatabaseDelegate ModifyDataFunc;
        //private StudentDatabaseDelegate DeletaDataFunc;

        private Student m_currentDatabaseStudent = null;

        public MySqlBasedataForm()
        {
            InitializeComponent();
        }

        private void MySqlBasedataForm_Load(object sender, EventArgs e)
        {
            this.m_currentDatabaseStudent = null;
            this.tabControlBasedata.SelectTab(this.tabPageShowData);
            //初始化拉取数据
            this.secectDatabase_Click(null, new EventArgs());

            ReadChinaInfo();

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
                if (!comboBox1Modify.Items.Contains(item))
                {
                    comboBox1Modify.Items.Add(item);
                }
            }
            comboBox1Modify.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            comboBoxFind.SelectedItem = "姓名";
        }

        private void ReadChinaInfo()
        {
            string pathRoot = FileUtils.GetProjectRootPath();

            string path = pathRoot + @"\ChinaInfo.txt";
            List<ChinaInfo> chinaList = new List<ChinaInfo>();
            string[] data = File.ReadAllLines(path);
            StringBuilder sb = new StringBuilder();

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

                chinaList.Add(china);
            }
            m_chinaList = chinaList;
        }

        private void secectDatabase_Click(object sender, EventArgs e)
        {
            this.textBoxFind.Text = string.Empty;

            string sql = "select * from student";
            DataSet data = MysqlManager.Instance.SelectTables(sql);

            this.m_currentDataTable = data.Tables[0];
            //dataGridView1.DataSource = this.m_currentDataTable.DefaultView;
            List<Student> studentList = new List<Student>();
            Dictionary<int, Student> studentDic = new Dictionary<int, Student>();
            DataRow[] dataRows = this.m_currentDataTable.Select();
            for (int i = 0; i < dataRows.Length; i++)
            {
                DataRow dataRow = dataRows[i];
                Student student = new Student(i + 1, dataRow[1].ToString(), (int)dataRow[3], (int)dataRow[2], dataRow[4].ToString(), dataRow[7].ToString(), dataRow[5].ToString(), dataRow[6].ToString());
                studentList.Add(student);
                studentDic.Add(i, student);
            }
            this.m_currentStudentDic = studentDic;
            ShowDataSource(studentList);
        }

        private void ShowDataSource(List<Student> studentList)
        {
            dataGridView1.ClearSelection();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new BindingList<Student>(studentList);

            if (dataGridView1.Columns.Contains("btnModify"))
            {
                return;
            }
            DataGridViewButtonColumn btnModify = new DataGridViewButtonColumn();
            btnModify.Name = "btnModify";
            btnModify.HeaderText = "";
            btnModify.DefaultCellStyle.NullValue = "修改";
            btnModify.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            btnModify.DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns.Add(btnModify);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.Name = "btnDelete";
            btnDelete.HeaderText = "";
            btnDelete.DefaultCellStyle.NullValue = "删除";
            btnDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            btnDelete.DefaultCellStyle.BackColor = Color.Red;
            dataGridView1.Columns.Add(btnDelete);
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            // 对应 sql 语句 如果是字符串类型 需要加双引号
            string tableName = "student";
            string sql = "{0}, \"{1}\", {2}, {3}, \"{4}\", \"{5}\", \"{6}\", \"{7}\", {8}";
            string name = textBoxName.Text.Trim();
            string cardID = textBoxCardID.Text.Trim();
            int age = 0;
            int.TryParse(textBoxAge.Text.Trim(), out age);
            int gender = comboBoxGender.SelectedIndex;
            string phone = textBoxPhone.Text.Trim();
            string email = textBoxEmai.Text.Trim();
            string address = string.Format("{0}${1}${2}${3}", comboBox1.SelectedItem, comboBox2.SelectedItem, comboBox3.SelectedItem, textBoxAddress.Text.Trim());
            sql = string.Format(sql, 0, name, age, gender, phone, email, cardID, address, 0);

            bool exists = MysqlManager.Instance.ExistsData(tableName, string.Format("CardID=\"{0}\"", cardID));
            if (exists)
            {
                MessageBox.Show("insert data has exists.");
            }
            else
            {
                int insert = MysqlManager.Instance.InsesetData(tableName, sql);
                if (insert > 0)
                {
                    MessageBox.Show("insert success.");
                }
                else
                {
                    MessageBox.Show("insert Failed.");
                }
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            string findCondition = comboBoxFind.SelectedItem.ToString();

            string findStr = textBoxFind.Text.Trim();
            List<Student> list = new List<Student>();
            foreach (KeyValuePair<int, Student> item in this.m_currentStudentDic)
            {
                switch (findCondition)
                {
                    case "序号":
                        if (item.Value.Index.ToString().Equals(findStr))
                        {
                            list.Add(item.Value);
                        }
                        break;
                    case "手机号":
                        if (item.Value.PhoneNum.Equals(findStr))
                        {
                            list.Add(item.Value);
                        }
                        break;
                    default:
                        if (item.Value.Name.Equals(findStr))
                        {
                            list.Add(item.Value);
                        }
                        break;
                }
            }
            ShowDataSource(list);
        }
        private void tabControlBasedata_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControlBasedata.SelectedTab == this.tabPageModifyData && this.m_currentDatabaseStudent != null)
            {
                Student student = this.m_currentDatabaseStudent;
                this.textBoxModifyName.Text = student.Name;
                this.textBoxModifyAge.Text = string.Format("{0}", student.Age);
                string[] address = student.GetAddressFromDatabase().Split('$');
                if (address.Length <= 3)
                {
                    this.textBoxModifyAddress.Text = student.Address;
                }
                else
                {
                    this.comboBox1Modify.SelectedItem = address[0];
                    this.comboBox2Modify.SelectedItem = address[1];
                    this.comboBox3Modify.SelectedItem = address[2];
                    this.textBoxModifyAddress.Text = address[3];
                }
                this.textBoxModifyEmai.Text = student.Email;
                this.textBoxModifyPhone.Text = student.PhoneNum;
                this.comboBoxModifyGender.SelectedItem = student.Gender;
                this.textBoxModifyCardID.Text = student.CardID;
                this.textBoxModifyCardID.ReadOnly = true;
            }
            else if (this.tabControlBasedata.SelectedTab == this.tabPageAddData)
            {
                this.textBoxName.Text = string.Empty;
                this.textBoxAge.Text = string.Format("{0}", 18);
                this.textBoxAddress.Text = string.Empty;
                this.textBoxEmai.Text = string.Empty;
                this.textBoxPhone.Text = string.Empty;
                this.comboBoxGender.SelectedItem = "男";
                this.textBoxCardID.ReadOnly = false;
                this.textBoxCardID.Text = "10010190001";
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (rowIndex >= 0)
            {
                DialogResult result = DialogResult.None;
                switch (dataGridView1.Columns[columnIndex].Name)
                {
                    case "btnModify":
                        result = MessageBox.Show("确认修改选择的信息！", "修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            //TODO
                            Student student;
                            if (this.m_currentStudentDic.TryGetValue(rowIndex, out student))
                            {
                                ModifyData(student);
                            }
                            else
                            {
                                MessageBox.Show("未查到该用户数据，失败！", "修改", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        break;
                    case "btnDelete":
                        result = MessageBox.Show("确认删除选择的信息！", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            //TODO
                            Student student;
                            if (this.m_currentStudentDic.TryGetValue(rowIndex, out student))
                            {
                                DeleteData(student);
                            }
                            else
                            {
                                MessageBox.Show("未查到该用户数据，失败！", "删除", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void ModifyData(Student student)
        {
            //TODO
            this.m_currentDatabaseStudent = student;
            this.tabControlBasedata.SelectedTab = this.tabPageModifyData;
        }
        private void buttonModify_Click(object sender, EventArgs e)
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
            string cardID = textBoxModifyCardID.Text.Trim();
            //if (!cardID.Equals(student.CardID))
            //{
            //    sb.Append(string.Format("cardID=\"{0}\" ", cardID));
            //}
            string name = textBoxModifyName.Text.Trim();
            if (!name.Equals(student.Name))
            {
                sb.Append(string.Format("name=\"{0}\", ", name));
            }
            int age = 0;
            int.TryParse(textBoxModifyAge.Text, out age);
            if (!age.Equals(student.Age))
            {
                sb.Append(string.Format("age={0}, ", age));
            }
            int gender = comboBoxModifyGender.SelectedIndex;
            if (!gender.Equals(student.GetGenderFromDatabase()))
            {
                sb.Append(string.Format("gender={0}, ", gender));
            }
            string phoneNum = textBoxModifyPhone.Text.Trim();
            if (!phoneNum.Equals(student.PhoneNum))
            {
                sb.Append(string.Format("phoneNum=\"{0}\", ", phoneNum));
            }
            string email = textBoxModifyEmai.Text.Trim();
            if (!email.Equals(student.Email))
            {
                sb.Append(string.Format("Email=\"{0}\", ", email));
            }
            string address = string.Format("{0}${1}${2}${3}", comboBox1Modify.SelectedItem, comboBox2Modify.SelectedItem, comboBox3Modify.SelectedItem, textBoxModifyAddress.Text.Trim());
            if (!address.Equals(student.GetAddressFromDatabase()))
            {
                sb.Append(string.Format("address=\"{0}\", ", address));
            }
            string after = string.Format("{0}", sb.ToString().Trim().Trim(','));

            if (befor.CompareTo(after) == 0)
            {
                MessageBox.Show("没有改变任何数据.");
            }
            else
            {
                string sql = string.Format("update student set {0} where cardID=\"{1}\";", after, cardID);
                int result = MysqlManager.Instance.UpdateData(sql);
                if (result > 0)
                {
                    MessageBox.Show("修改成功.");
                }
                else
                {
                    MessageBox.Show("修改失败.");
                }
            }
            this.m_currentDatabaseStudent = null;
        }
        private void DeleteData(Student student)
        {
            //TODO
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

        private void textBoxEmai_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 允许输入:数字、退格键(8)、全选(1)、复制(3)、粘贴(22)、空格(32)
            if (e.KeyChar == 32)
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

        private void comboBox1Modify_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = (string)comboBox1Modify.SelectedItem;
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    text = comboBox1Modify.Text.ToString();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("comboBox1: " + exc.Message);
            }
            finally
            {
                comboBox2Modify.Items.Clear();
                foreach (var item in cityDic[text].Keys)
                {
                    if (!comboBox2Modify.Items.Contains(item))
                    {
                        comboBox2Modify.Items.Add(item);
                    }
                }
                if (comboBox2Modify.Items.Count <= 0)
                {
                    comboBox2Modify.Items.Add(comboBox1Modify.SelectedItem);
                }
                comboBox2Modify.SelectedIndex = 0;
            }
        }

        private void comboBox2Modify_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3Modify.Items.Clear();
            try
            {
                if (cityDic[comboBox1Modify.SelectedItem.ToString()].ContainsKey(comboBox2Modify.SelectedItem.ToString()))
                {
                    foreach (var item in cityDic[comboBox1Modify.SelectedItem.ToString()][comboBox2Modify.SelectedItem.ToString()])
                    {
                        if (!comboBox3Modify.Items.Contains(item))
                        {
                            comboBox3Modify.Items.Add(item);
                        }
                    }
                }
                else
                {
                    if (comboBox3Modify.Items.Count <= 0)
                    {
                        comboBox3Modify.Items.Add(comboBox2Modify.SelectedItem);
                    }
                }
            }
            catch (Exception exc)
            {
                comboBox3Modify.Items.Clear();
                if (comboBox3Modify.Items.Count <= 0)
                {
                    comboBox3Modify.Items.Add(comboBox2Modify.SelectedItem);
                }
            }
            comboBox3Modify.SelectedIndex = 0;
        }

        private void comboBox3Modify_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxModifyAddress.Text = string.Empty;
        }

        private void textBoxModifyPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBoxModifyPhone.Text.Length >= 11 && e.KeyChar != 8 &&
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
    }
}
