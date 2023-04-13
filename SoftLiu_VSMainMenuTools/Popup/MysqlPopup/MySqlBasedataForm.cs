using SoftLiu_VSMainMenuTools.Data;
using SoftLiu_VSMainMenuTools.Popup.MysqlPopup;
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

        

        public MySqlBasedataForm()
        {
            InitializeComponent();
        }

        private void MySqlBasedataForm_Load(object sender, EventArgs e)
        {
            this.tabControlBasedata.SelectTab(this.tabPageShowData);
            //初始化拉取数据
            this.secectDatabase_Click(null, new EventArgs());
            
            comboBoxFind.SelectedItem = "姓名";
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
        }

        private void ModifyData(Student student)
        {
            //TODO
            MysqlModifyUserInfoForm form = new MysqlModifyUserInfoForm();
            form.MysqlModifyUserInfoFormInit(student);
            form.ShowDialog();
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
        
        private void dataGridViewIsDelete_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || dataGridViewIsDelete.Rows.Count <= 0) return;
            dataGridViewIsDelete.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = (dataGridViewIsDelete.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? string.Empty).ToString();
        }

        private void MySqlBasedataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instance.BackClose();
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            MysqlAddUserInfoForm form = new MysqlAddUserInfoForm();
            form.Init();
            form.Show();
        }
    }
}
