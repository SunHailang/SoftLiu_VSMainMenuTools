using SoftLiu_VSMainMenuTools.Data;
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

namespace SoftLiu_VSMainMenuTools
{
    public partial class MySqlBasedataForm : Form
    {
        private DataTable m_currentDataTable = null;

        private List<Student> m_currentStudentList = null;

        public MySqlBasedataForm()
        {
            InitializeComponent();
        }

        private void MySqlBasedataForm_Load(object sender, EventArgs e)
        {

            this.tabControlBasedata.SelectTab(this.tabPageShowData);
            //初始化拉取数据
            this.secectDatabase_Click(null, new EventArgs());
        }

        private void secectDatabase_Click(object sender, EventArgs e)
        {
            string sql = "select * from student";
            DataSet data = MysqlManager.Instance.SelectTables(sql);
            //textBox1.Text = data.GetXml();
            /*
            for (int i = 0; i < data.Tables.Count; i++)
            {
                DataTable dt = data.Tables[i];
                DataRow[] drRowArray = dt.Select();
                for (int j = 0; j < drRowArray.Length; j++)
                {
                    DataRow dr = drRowArray[j];
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        //textBox1.AppendText(dr[k].ToString());
                        //textBox1.AppendText("\n"); 
                    }
                }
            }
            */
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
            string address = textBoxAddress.Text.Trim();

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
            if (tabControlBasedata.SelectedTab == tabPageAddData)
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

        
    }
}
