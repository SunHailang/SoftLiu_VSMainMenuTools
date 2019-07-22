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

        private List<Student> m_currentStudentList = null;

        private List<ChinaInfo> m_chinaList = null;

        private Dictionary<string, Dictionary<string, List<string>>> cityDic = new Dictionary<string, Dictionary<string, List<string>>>();

        public MySqlBasedataForm()
        {
            InitializeComponent();
        }

        private void MySqlBasedataForm_Load(object sender, EventArgs e)
        {
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
            }

            comboBox1.SelectedIndex = 0;
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
            string sql = "select * from student";
            DataSet data = MysqlManager.Instance.SelectTables(sql);

            this.m_currentDataTable = data.Tables[0];
            //dataGridView1.DataSource = this.m_currentDataTable.DefaultView;
            List<Student> studentList = new List<Student>();
            DataRow[] dataRows = this.m_currentDataTable.Select();
            for (int i = 0; i < dataRows.Length; i++)
            {
                DataRow dataRow = dataRows[i];
                Student student = new Student(i + 1, dataRow[1].ToString(), (int)dataRow[3], (int)dataRow[2], dataRow[4].ToString(), dataRow[6].ToString(), dataRow[5].ToString());
                studentList.Add(student);
            }
            this.m_currentStudentList = studentList;
            ShowDataSource(this.m_currentStudentList);
        }

        private void ShowDataSource(List<Student> studentList)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = new BindingList<Student>(studentList);

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
            string sql = "insert into student values({0}, \"{1}\", {2}, {3}, \"{4}\", \"{5}\", \"{6}\", {7})";
            string name = textBoxName.Text.Trim();
            int age = 0;
            int.TryParse(textBoxAge.Text.Trim(), out age);
            int gender = comboBoxGender.SelectedIndex;
            string phone = textBoxPhone.Text.Trim();
            string email = textBoxEmai.Text.Trim();
            string address = string.Format("{0}{1}{2}{3}", comboBox1.SelectedItem, comboBox2.SelectedItem, comboBox3.SelectedItem, textBoxAddress.Text.Trim());
            sql = string.Format(sql, 0, name, age, gender, phone, email, address, 0);

            int insert = MysqlManager.Instance.InsesetData(sql);
            if (insert > 0)
            {
                MessageBox.Show("insert success.");
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControlBasedata.SelectedTab == this.tabPageAddData)
            {
                this.textBoxName.Text = string.Empty;
                this.textBoxAge.Text = string.Format("{0}", 18);
                this.textBoxAddress.Text = string.Empty;
                this.textBoxEmai.Text = string.Empty;
                this.textBoxPhone.Text = string.Empty;
                this.comboBoxGender.SelectedIndex = 0;
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
                        }
                        break;
                    case "btnDelete":
                        result = MessageBox.Show("确认删除选择的信息！", "删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if(result == DialogResult.OK)
                        {
                            //TODO
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void textBoxPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
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
                    comboBox2.Items.Add("市区");
                }
                comboBox2.SelectedIndex = 0;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            try
            {
                foreach (var item in cityDic[comboBox1.SelectedItem.ToString()][comboBox2.SelectedItem.ToString()])
                {
                    if (!comboBox3.Items.Contains(item))
                    {
                        comboBox3.Items.Add(item);
                    }
                }
            }
            catch (Exception exc)
            {
                if (comboBox3.Items.Count <= 0)
                {
                    comboBox3.Items.Add("市区");
                }
            }

            comboBox3.SelectedIndex = 0;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
