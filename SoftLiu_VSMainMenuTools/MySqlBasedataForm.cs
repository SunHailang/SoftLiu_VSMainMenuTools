using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.UGUI;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools
{
    public partial class MySqlBasedataForm : Form
    {
        private DataTable m_currentDataTable = null;

        private List<Student> m_currentStudentList = null;

        private List<Student> m_currentStudentIsDeleteList = null;

        private List<ChinaInfo> m_chinaList = null;

        private Dictionary<string, Dictionary<string, List<string>>> cityDic = new Dictionary<string, Dictionary<string, List<string>>>();

        //private delegate void StudentDatabaseDelegate(Student student); // 委托的声明

        //private StudentDatabaseDelegate ModifyDataFunc;
        //private StudentDatabaseDelegate DeletaDataFunc;

        private Student m_currentDatabaseStudent = null;

        private bool m_autoInsertData = false;

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
                if (!comboBoxModiftAddress1.Items.Contains(item))
                {
                    comboBoxModiftAddress1.Items.Add(item);
                }
            }
            comboBoxModiftAddress1.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            comboBoxFind.SelectedItem = "姓名";
        }

        private void ReadChinaInfo()
        {
            string pathRoot = FileUtils.GetProjectRootPath();

            string path = @"Resources\ChinaInfo.txt";//pathRoot + @"\ChinaInfo.txt";
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

            string sql = "select * from student where isDelete=0";
            DataSet data = MysqlManager.Instance.SelectTables(sql);
            this.m_currentDataTable = data.Tables[0];

            //dataGridView1.DataSource = this.m_currentDataTable.DefaultView;
            List<Student> studentList = new List<Student>();
            DataRow[] dataRows = this.m_currentDataTable.Select();
            for (int i = 0; i < dataRows.Length; i++)
            {
                DataRow dataRow = dataRows[i];

                Student student = new Student(i + 1, Convert.ToInt32(dataRow[1]), Convert.ToInt32(dataRow[2]), dataRow[3].ToString(),
                                            dataRow[5].ToString(), Convert.ToInt32((int)dataRow[7]),
                                            Convert.ToInt32(dataRow[6]), dataRow[8].ToString(), dataRow[10].ToString(),
                                            dataRow[9].ToString(), dataRow[4].ToString(), Convert.ToInt32(dataRow[11]));
                studentList.Add(student);
            }
            this.m_currentStudentList = studentList;


            string tableJson = JsonUtils.Instance.ObjectToJson(this.m_currentStudentList);


            ShowDataSource(studentList);
        }

        private void ShowDataSource(List<Student> studentList)
        {
            //dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ClearSelection();
            //dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new BindingList<Student>(studentList);

            //DataGridViewCell cell = dataGridView1.Rows[1].Cells[2];
            //cell.ReadOnly = false;
            //cell.Value = "Hello";
            //Console.WriteLine(cell.Value.ToString());

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

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            //是否可以进行编辑的条件检查
            if (dgv.Columns[e.ColumnIndex].Name == "Column1" && !(bool)dgv["Column2", e.RowIndex].Value)
            {
                //取消编辑
                e.Cancel = true;
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e)
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

        private void buttonFind_Click(object sender, EventArgs e)
        {
            string findCondition = comboBoxFind.SelectedItem.ToString();

            string findStr = textBoxFind.Text.Trim();
            List<Student> list = new List<Student>();
            IEnumerable<Student> listPair = null;
            switch (findCondition)
            {
                case "序号":
                    listPair = this.m_currentStudentList.Where((dic) =>
                    {
                        return dic.Index.ToString().CompareTo(findStr) == 0;
                    });
                    foreach (var item in listPair)
                    {
                        list.Add(item);
                    }
                    break;
                case "手机号":
                    listPair = this.m_currentStudentList.Where((dic) =>
                    {
                        return dic.PhoneNum.ToString().CompareTo(findStr) == 0;
                    });
                    foreach (var item in listPair)
                    {
                        list.Add(item);
                    }
                    break;
                case "姓名":
                    listPair = this.m_currentStudentList.Where((dic) =>
                    {
                        return dic.Name.ToString().CompareTo(findStr) == 0;
                    });
                    foreach (var item in listPair)
                    {
                        list.Add(item);
                    }
                    break;
            }
            ShowDataSource(list);
        }

        private List<int> GetComboBoxItems(string sql)
        {
            string gradesql = sql;// "select gradeid from grade;";
            DataSet gradeDataset = MysqlManager.Instance.SelectTables(gradesql);
            DataTable gradeTable = gradeDataset.Tables[0];
            DataRow[] gradeRows = gradeTable.Select();
            List<int> list = new List<int>();
            for (int i = 0; i < gradeRows.Length; i++)
            {
                DataRow row = gradeRows[i];
                list.Add(Convert.ToInt32(row[0]));
            }
            list.Sort((x, y) => { return x - y; });

            return list;
        }

        private void tabControlBasedata_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControlBasedata.SelectedTab == this.tabPageIsDelete)
            {
                if (this.m_currentStudentIsDeleteList == null)
                {
                    this.comboBoxIsDelete.SelectedIndex = 0;
                    buttonRushIsDelete_Click(null, new EventArgs());
                }
                else
                {
                    ShowIsDeleteDataSource(this.m_currentStudentIsDeleteList);
                }
            }
            else if (this.tabControlBasedata.SelectedTab == this.tabPageModifyData && this.m_currentDatabaseStudent != null)
            {
                Student student = this.m_currentDatabaseStudent;
                List<int> gradeIDlist = GetComboBoxItems("select gradeid from grade;");
                this.comboBoxModifyGrade.Items.Clear();
                for (int i = 0; i < gradeIDlist.Count; i++)
                {
                    this.comboBoxModifyGrade.Items.Add(gradeIDlist[i]);
                }
                this.comboBoxModifyGrade.SelectedItem = student.GradeID;

                List<int> classIDlist = GetComboBoxItems("select classid from class;");
                this.comboBoxModifyClass.Items.Clear();
                for (int i = 0; i < classIDlist.Count; i++)
                {
                    this.comboBoxModifyClass.Items.Add(classIDlist[i]);
                }
                this.comboBoxModifyClass.SelectedItem = student.ClassID;

                this.textBoxModifyStuNum.Text = student.StuNum;

                this.textBoxModifyName.Text = student.Name;
                this.textBoxModifyAge.Text = string.Format("{0}", student.Age);
                string[] address = student.GetAddressFromDatabase().Split('$');
                if (address.Length <= 3)
                {
                    this.textBoxModifyAddress.Text = student.Address;
                }
                else
                {
                    this.comboBoxModiftAddress1.SelectedItem = address[0];
                    this.comboBoxModiftAddress2.SelectedItem = address[1];
                    this.comboBoxModiftAddress3.SelectedItem = address[2];
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
                if (m_autoInsertData)
                {
                    return;
                }
                List<int> gradeIDlist = GetComboBoxItems("select gradeid from grade;");
                this.comboBoxAddGrade.Items.Clear();
                for (int i = 0; i < gradeIDlist.Count; i++)
                {
                    this.comboBoxAddGrade.Items.Add(gradeIDlist[i]);
                }
                this.comboBoxAddGrade.SelectedIndex = 0;

                List<int> classIDlist = GetComboBoxItems("select classid from class;");
                this.comboBoxAddClass.Items.Clear();
                for (int i = 0; i < classIDlist.Count; i++)
                {
                    this.comboBoxAddClass.Items.Add(classIDlist[i]);
                }
                this.comboBoxAddClass.SelectedIndex = 0;
                // 100102020110004
                string addStunum = $"100102020{this.comboBoxAddGrade.SelectedItem}{this.comboBoxAddClass.SelectedItem}0001";
                this.textBoxAddStuNum.Text = addStunum;
                this.textBoxAddName.Text = string.Empty;
                this.textBoxAddAge.Text = string.Format("{0}", 18);
                this.textBoxAddress.Text = string.Empty;
                this.textBoxEmai.Text = string.Empty;
                this.textBoxPhone.Text = string.Empty;
                this.comboBoxGender.SelectedItem = "男";
                this.textBoxCardID.ReadOnly = false;
                // 342201200201204892
                this.textBoxCardID.Text = "342201200201200001";
                
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (rowIndex >= 0)
            {
                string stuNum = dataGridView1[3, rowIndex].Value.ToString();
                switch (dataGridView1.Columns[columnIndex].Name)
                {
                    case "btnModify":
                        DialogResult modifyResult = MessageBox.Show("确认修改选择的信息！", "修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (modifyResult == DialogResult.OK)
                        {
                            //TODO
                            IEnumerable<Student> students = this.m_currentStudentList.Where((stu) =>
                            {
                                return stu.StuNum == stuNum;
                            });
                            Student student = students.FirstOrDefault();
                            if (student != null)
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
                        DialogResult deleteResult = MessageBox.Show("确认删除选择的信息！", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (deleteResult == DialogResult.OK)
                        {
                            //TODO
                            IEnumerable<Student> students = this.m_currentStudentList.Where((stu) =>
                            {
                                return stu.StuNum == stuNum;
                            });
                            Student student = students.FirstOrDefault();
                            if (student != null)
                            {
                                DeleteData(student, rowIndex);
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
            string address = string.Format("{0}${1}${2}${3}", comboBoxModiftAddress1.SelectedItem, comboBoxModiftAddress2.SelectedItem, comboBoxModiftAddress3.SelectedItem, textBoxModifyAddress.Text.Trim());
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
                    textBoxModifyAge.Text = string.Empty;
                    textBoxModifyCardID.Text = string.Empty;
                    textBoxModifyAddress.Text = string.Empty;
                    textBoxModifyEmai.Text = string.Empty;
                    textBoxModifyName.Text = string.Empty;
                    textBoxModifyPhone.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("修改失败.");
                }
            }
            this.m_currentDatabaseStudent = null;
        }
        private void DeleteData(Student student, int index)
        {
            //TODO
            string sql = string.Format("update student set {0} where stunum=\"{1}\";", "isDelete=1", student.StuNum);
            int resultSql = MysqlManager.Instance.UpdateData(sql, true);
            if (resultSql > 0)
            {
                MessageBox.Show("删除成功.");
                dataGridView1.Rows.RemoveAt(index);
                // 更新学生字典
                this.m_currentStudentList.Remove(student);
            }
            else
            {
                MessageBox.Show("删除失败.");
            }
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

        private void comboBoxModiftAddress1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = (string)this.comboBoxModiftAddress1.SelectedItem;
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    text = comboBoxModiftAddress1.Text.ToString();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("comboBox1: " + exc.Message);
            }
            finally
            {
                comboBoxModiftAddress2.Items.Clear();
                foreach (var item in cityDic[text].Keys)
                {
                    if (!comboBoxModiftAddress2.Items.Contains(item))
                    {
                        comboBoxModiftAddress2.Items.Add(item);
                    }
                }
                if (comboBoxModiftAddress2.Items.Count <= 0)
                {
                    comboBoxModiftAddress2.Items.Add(comboBoxModiftAddress1.SelectedItem);
                }
                comboBoxModiftAddress2.SelectedIndex = 0;
            }
        }

        private void comboBoxModiftAddress2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxModiftAddress3.Items.Clear();
            try
            {
                if (cityDic[comboBoxModiftAddress1.SelectedItem.ToString()].ContainsKey(comboBoxModiftAddress2.SelectedItem.ToString()))
                {
                    foreach (var item in cityDic[comboBoxModiftAddress1.SelectedItem.ToString()][comboBoxModiftAddress2.SelectedItem.ToString()])
                    {
                        if (!comboBoxModiftAddress3.Items.Contains(item))
                        {
                            comboBoxModiftAddress3.Items.Add(item);
                        }
                    }
                }
                else
                {
                    if (comboBoxModiftAddress3.Items.Count <= 0)
                    {
                        comboBoxModiftAddress3.Items.Add(comboBoxModiftAddress2.SelectedItem);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("comboBox2Modify_SelectedIndexChanged : " + exc.Message);
                comboBoxModiftAddress3.Items.Clear();
                if (comboBoxModiftAddress3.Items.Count <= 0)
                {
                    comboBoxModiftAddress3.Items.Add(comboBoxModiftAddress2.SelectedItem);
                }
            }
            comboBoxModiftAddress3.SelectedIndex = 0;
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

        private void buttonRushIsDelete_Click(object sender, EventArgs e)
        {
            this.textBoxIsDelete.Text = string.Empty;

            string sql = "select * from student where isDelete=1";
            DataSet data = MysqlManager.Instance.SelectTables(sql);

            this.m_currentDataTable = data.Tables[0];
            //dataGridView1.DataSource = this.m_currentDataTable.DefaultView;
            List<Student> studentList = new List<Student>();
            DataRow[] dataRows = this.m_currentDataTable.Select();
            for (int i = 0; i < dataRows.Length; i++)
            {
                DataRow dataRow = dataRows[i];

                Student student = new Student(i + 1, Convert.ToInt32(dataRow[1]), Convert.ToInt32(dataRow[2]), dataRow[3].ToString(),
                                            dataRow[5].ToString(), Convert.ToInt32((int)dataRow[7]),
                                            Convert.ToInt32(dataRow[6]), dataRow[8].ToString(), dataRow[10].ToString(),
                                            dataRow[9].ToString(), dataRow[4].ToString(), Convert.ToInt32(dataRow[11]));
                studentList.Add(student);
            }
            this.m_currentStudentIsDeleteList = studentList;
            ShowIsDeleteDataSource(studentList);
        }
        private void ShowIsDeleteDataSource(List<Student> studentList)
        {
            dataGridViewIsDelete.ClearSelection();
            dataGridViewIsDelete.AutoGenerateColumns = false;
            dataGridViewIsDelete.DataSource = new BindingList<Student>(studentList);


            if (dataGridViewIsDelete.Columns.Contains("btnRestore"))
            {
                return;
            }
            DataGridViewButtonColumn btnModify = new DataGridViewButtonColumn();
            btnModify.Name = "btnRestore";
            btnModify.HeaderText = "";
            btnModify.DefaultCellStyle.NullValue = "恢复";
            btnModify.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            btnModify.DefaultCellStyle.BackColor = Color.Green;
            dataGridViewIsDelete.Columns.Add(btnModify);
        }

        private void buttonFindIsDelete_Click(object sender, EventArgs e)
        {
            string findCondition = comboBoxIsDelete.SelectedItem.ToString();

            string findStr = textBoxIsDelete.Text.Trim();
            List<Student> list = new List<Student>();
            foreach (Student item in this.m_currentStudentIsDeleteList)
            {
                switch (findCondition)
                {
                    case "序号":
                        if (item.Index.ToString().Equals(findStr))
                        {
                            list.Add(item);
                        }
                        break;
                    case "手机号":
                        if (item.PhoneNum.Equals(findStr))
                        {
                            list.Add(item);
                        }
                        break;
                    default:
                        if (item.Name.Equals(findStr))
                        {
                            list.Add(item);
                        }
                        break;
                }
            }
            ShowIsDeleteDataSource(list);
        }

        private void dataGridViewIsDelete_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (rowIndex >= 0)
            {
                string stuNum = this.dataGridViewIsDelete[3, rowIndex].Value.ToString();
                DialogResult result = DialogResult.None;
                switch (dataGridViewIsDelete.Columns[columnIndex].Name)
                {
                    case "btnRestore":
                        result = MessageBox.Show("确认恢复的信息！", "恢复", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            //TODO
                            IEnumerable<Student> students = this.m_currentStudentIsDeleteList.Where((stu) =>
                            {
                                return stu.StuNum == stuNum;
                            });
                            Student student = students.FirstOrDefault();
                            if (student != null)
                            {
                                string sql = string.Format("update student set {0} where cardID=\"{1}\";", "isDelete=0", student.CardID);
                                int resultSql = MysqlManager.Instance.UpdateData(sql);
                                if (resultSql > 0)
                                {
                                    MessageBox.Show("恢复成功.");
                                    dataGridViewIsDelete.Rows.RemoveAt(rowIndex);
                                    this.m_currentStudentIsDeleteList.Remove(student);
                                }
                                else
                                {
                                    MessageBox.Show("恢复失败.");
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void buttonAutoAddUser_Click(object sender, EventArgs e)
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
                    sutunum += 1;
                    textBoxCardID.Text = string.Format("{0}", sutunum);
                    StringBuilder sb = new StringBuilder();
                    Random ra = new Random();
                    int key = ra.Next(97, 123);
                    for (int i = 0; i < 6; i++)
                    {
                        sb.Append((char)key);
                    }
                    textBoxAddName.Text = string.Format("{0}", sb.ToString());

                    textBoxAddAge.Text = ra.Next(0, 100).ToString();
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
                    string name = textBoxAddName.Text.Trim();
                    string cardID = textBoxCardID.Text.Trim();
                    int age = 0;
                    int.TryParse(textBoxAddAge.Text.Trim(), out age);
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

        private int InsesetDataToBasedata()
        {
            int result = -1;
            // 对应 sql 语句 如果是字符串类型 需要加双引号
            string tableName = "student";
            string sql = "{0}, \"{1}\", {2}, {3}, \"{4}\", \"{5}\", \"{6}\", \"{7}\", {8}";
            string name = textBoxAddName.Text.Trim();
            string cardID = textBoxCardID.Text.Trim();
            int age = 0;
            int.TryParse(textBoxAddAge.Text.Trim(), out age);
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

        private void dataGridViewIsDelete_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || dataGridViewIsDelete.Rows.Count <= 0) return;
            dataGridViewIsDelete.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = (dataGridViewIsDelete.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? string.Empty).ToString();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPageShowData_Click(object sender, EventArgs e)
        {

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void MySqlBasedataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instance.BackClose();
        }

        private void comboBoxModifyGrade_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
