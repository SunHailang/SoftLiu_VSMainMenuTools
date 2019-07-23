namespace SoftLiu_VSMainMenuTools
{
    partial class MySqlBasedataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPhoneNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectDatabase = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelAge = new System.Windows.Forms.Label();
            this.textBoxAge = new System.Windows.Forms.TextBox();
            this.labelPhone = new System.Windows.Forms.Label();
            this.textBoxPhone = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBoxEmai = new System.Windows.Forms.TextBox();
            this.comboBoxGender = new System.Windows.Forms.ComboBox();
            this.labelGender = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.tabControlBasedata = new System.Windows.Forms.TabControl();
            this.tabPageShowData = new System.Windows.Forms.TabPage();
            this.buttonFind = new System.Windows.Forms.Button();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.tabPageAddData = new System.Windows.Forms.TabPage();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.comboBoxFind = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControlBasedata.SuspendLayout();
            this.tabPageShowData.SuspendLayout();
            this.tabPageAddData.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIndex,
            this.ColumnName,
            this.ColumnAge,
            this.ColumnGender,
            this.ColumnPhoneNum,
            this.ColumnAddress});
            this.dataGridView1.Location = new System.Drawing.Point(6, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(910, 478);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ColumnIndex
            // 
            this.ColumnIndex.DataPropertyName = "Index";
            this.ColumnIndex.HeaderText = "序号";
            this.ColumnIndex.Name = "ColumnIndex";
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.HeaderText = "姓名";
            this.ColumnName.Name = "ColumnName";
            // 
            // ColumnAge
            // 
            this.ColumnAge.DataPropertyName = "Age";
            this.ColumnAge.HeaderText = "年龄";
            this.ColumnAge.Name = "ColumnAge";
            // 
            // ColumnGender
            // 
            this.ColumnGender.DataPropertyName = "Gender";
            this.ColumnGender.HeaderText = "性别";
            this.ColumnGender.Name = "ColumnGender";
            // 
            // ColumnPhoneNum
            // 
            this.ColumnPhoneNum.DataPropertyName = "PhoneNum";
            this.ColumnPhoneNum.HeaderText = "手机号码";
            this.ColumnPhoneNum.Name = "ColumnPhoneNum";
            // 
            // ColumnAddress
            // 
            this.ColumnAddress.DataPropertyName = "Address";
            this.ColumnAddress.HeaderText = "联系地址";
            this.ColumnAddress.Name = "ColumnAddress";
            this.ColumnAddress.Width = 80;
            // 
            // selectDatabase
            // 
            this.selectDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectDatabase.Location = new System.Drawing.Point(828, 6);
            this.selectDatabase.Name = "selectDatabase";
            this.selectDatabase.Size = new System.Drawing.Size(88, 33);
            this.selectDatabase.TabIndex = 1;
            this.selectDatabase.Text = "刷新所有数据";
            this.selectDatabase.UseVisualStyleBackColor = true;
            this.selectDatabase.Click += new System.EventHandler(this.secectDatabase_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(109, 32);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(112, 20);
            this.textBoxName.TabIndex = 3;
            // 
            // labelAge
            // 
            this.labelAge.AutoSize = true;
            this.labelAge.Location = new System.Drawing.Point(38, 75);
            this.labelAge.Name = "labelAge";
            this.labelAge.Size = new System.Drawing.Size(37, 13);
            this.labelAge.TabIndex = 2;
            this.labelAge.Text = "年龄：";
            // 
            // textBoxAge
            // 
            this.textBoxAge.Location = new System.Drawing.Point(109, 72);
            this.textBoxAge.Name = "textBoxAge";
            this.textBoxAge.Size = new System.Drawing.Size(112, 20);
            this.textBoxAge.TabIndex = 3;
            // 
            // labelPhone
            // 
            this.labelPhone.AutoSize = true;
            this.labelPhone.Location = new System.Drawing.Point(14, 155);
            this.labelPhone.Name = "labelPhone";
            this.labelPhone.Size = new System.Drawing.Size(61, 13);
            this.labelPhone.TabIndex = 2;
            this.labelPhone.Text = "手机号码：";
            // 
            // textBoxPhone
            // 
            this.textBoxPhone.Location = new System.Drawing.Point(109, 152);
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.Size = new System.Drawing.Size(112, 20);
            this.textBoxPhone.TabIndex = 3;
            this.textBoxPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPhone_KeyPress);
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(35, 195);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(37, 13);
            this.labelEmail.TabIndex = 2;
            this.labelEmail.Text = "邮箱：";
            // 
            // textBoxEmai
            // 
            this.textBoxEmai.Location = new System.Drawing.Point(109, 192);
            this.textBoxEmai.Name = "textBoxEmai";
            this.textBoxEmai.Size = new System.Drawing.Size(112, 20);
            this.textBoxEmai.TabIndex = 3;
            this.textBoxEmai.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEmai_KeyPress);
            // 
            // comboBoxGender
            // 
            this.comboBoxGender.FormattingEnabled = true;
            this.comboBoxGender.Items.AddRange(new object[] {
            "女",
            "男",
            "保密"});
            this.comboBoxGender.Location = new System.Drawing.Point(109, 112);
            this.comboBoxGender.Name = "comboBoxGender";
            this.comboBoxGender.Size = new System.Drawing.Size(112, 21);
            this.comboBoxGender.TabIndex = 4;
            // 
            // labelGender
            // 
            this.labelGender.AutoSize = true;
            this.labelGender.Location = new System.Drawing.Point(38, 115);
            this.labelGender.Name = "labelGender";
            this.labelGender.Size = new System.Drawing.Size(34, 13);
            this.labelGender.TabIndex = 2;
            this.labelGender.Text = "性别：";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(14, 235);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(61, 13);
            this.labelAddress.TabIndex = 2;
            this.labelAddress.Text = "联系地址：";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(463, 227);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(207, 20);
            this.textBoxAddress.TabIndex = 3;
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(56, 284);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(88, 33);
            this.buttonInsert.TabIndex = 1;
            this.buttonInsert.Text = "插入数据";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // tabControlBasedata
            // 
            this.tabControlBasedata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlBasedata.Controls.Add(this.tabPageShowData);
            this.tabControlBasedata.Controls.Add(this.tabPageAddData);
            this.tabControlBasedata.Location = new System.Drawing.Point(12, 12);
            this.tabControlBasedata.Name = "tabControlBasedata";
            this.tabControlBasedata.SelectedIndex = 0;
            this.tabControlBasedata.Size = new System.Drawing.Size(930, 555);
            this.tabControlBasedata.TabIndex = 5;
            this.tabControlBasedata.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageShowData
            // 
            this.tabPageShowData.Controls.Add(this.comboBoxFind);
            this.tabPageShowData.Controls.Add(this.buttonFind);
            this.tabPageShowData.Controls.Add(this.textBoxFind);
            this.tabPageShowData.Controls.Add(this.dataGridView1);
            this.tabPageShowData.Controls.Add(this.selectDatabase);
            this.tabPageShowData.Location = new System.Drawing.Point(4, 22);
            this.tabPageShowData.Name = "tabPageShowData";
            this.tabPageShowData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShowData.Size = new System.Drawing.Size(922, 529);
            this.tabPageShowData.TabIndex = 0;
            this.tabPageShowData.Text = "查看所有数据";
            this.tabPageShowData.UseVisualStyleBackColor = true;
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(186, 11);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(52, 23);
            this.buttonFind.TabIndex = 4;
            this.buttonFind.Text = "搜索";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // textBoxFind
            // 
            this.textBoxFind.Location = new System.Drawing.Point(75, 13);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(105, 20);
            this.textBoxFind.TabIndex = 3;
            // 
            // tabPageAddData
            // 
            this.tabPageAddData.Controls.Add(this.comboBox3);
            this.tabPageAddData.Controls.Add(this.comboBox2);
            this.tabPageAddData.Controls.Add(this.comboBox1);
            this.tabPageAddData.Controls.Add(this.buttonInsert);
            this.tabPageAddData.Controls.Add(this.textBoxAddress);
            this.tabPageAddData.Controls.Add(this.labelAddress);
            this.tabPageAddData.Controls.Add(this.comboBoxGender);
            this.tabPageAddData.Controls.Add(this.textBoxName);
            this.tabPageAddData.Controls.Add(this.textBoxEmai);
            this.tabPageAddData.Controls.Add(this.labelEmail);
            this.tabPageAddData.Controls.Add(this.label5);
            this.tabPageAddData.Controls.Add(this.label4);
            this.tabPageAddData.Controls.Add(this.label3);
            this.tabPageAddData.Controls.Add(this.label2);
            this.tabPageAddData.Controls.Add(this.label1);
            this.tabPageAddData.Controls.Add(this.labelName);
            this.tabPageAddData.Controls.Add(this.textBoxAge);
            this.tabPageAddData.Controls.Add(this.textBoxPhone);
            this.tabPageAddData.Controls.Add(this.labelPhone);
            this.tabPageAddData.Controls.Add(this.labelAge);
            this.tabPageAddData.Controls.Add(this.labelGender);
            this.tabPageAddData.Location = new System.Drawing.Point(4, 22);
            this.tabPageAddData.Name = "tabPageAddData";
            this.tabPageAddData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAddData.Size = new System.Drawing.Size(922, 529);
            this.tabPageAddData.TabIndex = 1;
            this.tabPageAddData.Text = "添加用户信息";
            this.tabPageAddData.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(345, 227);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(112, 21);
            this.comboBox3.TabIndex = 5;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(227, 227);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(112, 21);
            this.comboBox2.TabIndex = 5;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(109, 227);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(112, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(676, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(227, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(227, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(227, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(227, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "*";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(38, 35);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(37, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "姓名：";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxFind
            // 
            this.comboBoxFind.FormattingEnabled = true;
            this.comboBoxFind.Items.AddRange(new object[] {
            "序号",
            "姓名",
            "手机号"});
            this.comboBoxFind.Location = new System.Drawing.Point(7, 13);
            this.comboBoxFind.Name = "comboBoxFind";
            this.comboBoxFind.Size = new System.Drawing.Size(62, 21);
            this.comboBoxFind.TabIndex = 5;
            // 
            // MySqlBasedataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 579);
            this.Controls.Add(this.tabControlBasedata);
            this.Name = "MySqlBasedataForm";
            this.Text = "MySqlBasedata";
            this.Load += new System.EventHandler(this.MySqlBasedataForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControlBasedata.ResumeLayout(false);
            this.tabPageShowData.ResumeLayout(false);
            this.tabPageShowData.PerformLayout();
            this.tabPageAddData.ResumeLayout(false);
            this.tabPageAddData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button selectDatabase;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelAge;
        private System.Windows.Forms.TextBox textBoxAge;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.TextBox textBoxPhone;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textBoxEmai;
        private System.Windows.Forms.ComboBox comboBoxGender;
        private System.Windows.Forms.Label labelGender;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Button buttonInsert;
        private System.Windows.Forms.TabControl tabControlBasedata;
        private System.Windows.Forms.TabPage tabPageShowData;
        private System.Windows.Forms.TabPage tabPageAddData;
        private System.Windows.Forms.TextBox textBoxFind;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPhoneNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboBoxFind;
    }
}