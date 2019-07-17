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
        public MySqlBasedataForm()
        {
            InitializeComponent();
        }

        private void MySqlBasedataForm_Load(object sender, EventArgs e)
        {

        }

        private void secectDatabase_Click(object sender, EventArgs e)
        {
            string sql = "select * from student";
            DataSet data = MysqlManager.Instance.SelectTables(sql);
            //textBox1.Text = data.GetXml();

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

            dataGridView1.DataSource = data.Tables[0].DefaultView;
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            // 对应 sql 语句 如果是字符串类型 需要加双引号
            string sql = "insert into student values({0}, \"{1}\", {2}, {3}, \"{4}\", \"{5}\", \"{6}\", {7})";
            string name = textBoxName.Text;
            int age = 0;
            int.TryParse(textBoxAge.Text, out age);
            int gender = comboBoxGender.SelectedIndex;
            string phone = textBoxPhone.Text;
            string email = textBoxEmai.Text;
            string address = textBoxAddress.Text;

            sql = string.Format(sql, 0, name, age, gender, phone, email, address, 0);

            int insert = MysqlManager.Instance.InsesetData(sql);
            if(insert > 0)
            {
                MessageBox.Show("insert success.");
            }
        }
    }
}
