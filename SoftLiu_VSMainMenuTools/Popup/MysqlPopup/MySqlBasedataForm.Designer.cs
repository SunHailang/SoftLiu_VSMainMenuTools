﻿namespace SoftLiu_VSMainMenuTools
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabPageIsDelete = new System.Windows.Forms.TabPage();
            this.comboBoxIsDelete = new System.Windows.Forms.ComboBox();
            this.buttonFindIsDelete = new System.Windows.Forms.Button();
            this.textBoxIsDelete = new System.Windows.Forms.TextBox();
            this.dataGridViewIsDelete = new System.Windows.Forms.DataGridView();
            this.ColumnIsDeleteIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeleteGradeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeleteClassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDeleteStuNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteCardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeletePhoneNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteEmil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnIsDeleteInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonRushIsDelete = new System.Windows.Forms.Button();
            this.tabPageShowData = new System.Windows.Forms.TabPage();
            this.comboBoxFind = new System.Windows.Forms.ComboBox();
            this.buttonFind = new System.Windows.Forms.Button();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGradeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnClassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStuNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCardID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPhoneNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAddUser = new System.Windows.Forms.Button();
            this.selectDatabase = new System.Windows.Forms.Button();
            this.tabControlBasedata = new System.Windows.Forms.TabControl();
            this.tabPageIsDelete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIsDelete)).BeginInit();
            this.tabPageShowData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControlBasedata.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageIsDelete
            // 
            this.tabPageIsDelete.Controls.Add(this.comboBoxIsDelete);
            this.tabPageIsDelete.Controls.Add(this.buttonFindIsDelete);
            this.tabPageIsDelete.Controls.Add(this.textBoxIsDelete);
            this.tabPageIsDelete.Controls.Add(this.dataGridViewIsDelete);
            this.tabPageIsDelete.Controls.Add(this.buttonRushIsDelete);
            this.tabPageIsDelete.Location = new System.Drawing.Point(4, 22);
            this.tabPageIsDelete.Name = "tabPageIsDelete";
            this.tabPageIsDelete.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIsDelete.Size = new System.Drawing.Size(1036, 486);
            this.tabPageIsDelete.TabIndex = 3;
            this.tabPageIsDelete.Text = "查看已删除数据";
            this.tabPageIsDelete.UseVisualStyleBackColor = true;
            // 
            // comboBoxIsDelete
            // 
            this.comboBoxIsDelete.FormattingEnabled = true;
            this.comboBoxIsDelete.Items.AddRange(new object[] {
            "序号",
            "姓名",
            "手机号"});
            this.comboBoxIsDelete.Location = new System.Drawing.Point(7, 12);
            this.comboBoxIsDelete.Name = "comboBoxIsDelete";
            this.comboBoxIsDelete.Size = new System.Drawing.Size(62, 20);
            this.comboBoxIsDelete.TabIndex = 10;
            // 
            // buttonFindIsDelete
            // 
            this.buttonFindIsDelete.Location = new System.Drawing.Point(186, 10);
            this.buttonFindIsDelete.Name = "buttonFindIsDelete";
            this.buttonFindIsDelete.Size = new System.Drawing.Size(52, 21);
            this.buttonFindIsDelete.TabIndex = 9;
            this.buttonFindIsDelete.Text = "搜索";
            this.buttonFindIsDelete.UseVisualStyleBackColor = true;
            this.buttonFindIsDelete.Click += new System.EventHandler(this.buttonFindIsDelete_Click);
            // 
            // textBoxIsDelete
            // 
            this.textBoxIsDelete.Location = new System.Drawing.Point(75, 12);
            this.textBoxIsDelete.Name = "textBoxIsDelete";
            this.textBoxIsDelete.Size = new System.Drawing.Size(105, 21);
            this.textBoxIsDelete.TabIndex = 8;
            // 
            // dataGridViewIsDelete
            // 
            this.dataGridViewIsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewIsDelete.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIsDelete.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewIsDelete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewIsDelete.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIsDeleteIndex,
            this.ColumnDeleteGradeID,
            this.ColumnDeleteClassID,
            this.ColumnDeleteStuNum,
            this.ColumnIsDeleteName,
            this.ColumnIsDeleteAge,
            this.ColumnIsDeleteGender,
            this.ColumnIsDeleteCardID,
            this.ColumnIsDeletePhoneNum,
            this.ColumnIsDeleteEmil,
            this.ColumnIsDeleteAddress,
            this.ColumnIsDeleteInfo});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewIsDelete.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewIsDelete.Location = new System.Drawing.Point(6, 42);
            this.dataGridViewIsDelete.Name = "dataGridViewIsDelete";
            this.dataGridViewIsDelete.Size = new System.Drawing.Size(1024, 441);
            this.dataGridViewIsDelete.TabIndex = 6;
            this.dataGridViewIsDelete.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIsDelete_CellContentClick);
            this.dataGridViewIsDelete.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIsDelete_CellMouseEnter);
            // 
            // ColumnIsDeleteIndex
            // 
            this.ColumnIsDeleteIndex.DataPropertyName = "Index";
            this.ColumnIsDeleteIndex.HeaderText = "序号";
            this.ColumnIsDeleteIndex.Name = "ColumnIsDeleteIndex";
            this.ColumnIsDeleteIndex.ReadOnly = true;
            // 
            // ColumnDeleteGradeID
            // 
            this.ColumnDeleteGradeID.DataPropertyName = "GradeID";
            this.ColumnDeleteGradeID.HeaderText = "年级";
            this.ColumnDeleteGradeID.Name = "ColumnDeleteGradeID";
            this.ColumnDeleteGradeID.ReadOnly = true;
            // 
            // ColumnDeleteClassID
            // 
            this.ColumnDeleteClassID.DataPropertyName = "ClassID";
            this.ColumnDeleteClassID.HeaderText = "班级";
            this.ColumnDeleteClassID.Name = "ColumnDeleteClassID";
            this.ColumnDeleteClassID.ReadOnly = true;
            // 
            // ColumnDeleteStuNum
            // 
            this.ColumnDeleteStuNum.DataPropertyName = "StuNum";
            this.ColumnDeleteStuNum.HeaderText = "学号";
            this.ColumnDeleteStuNum.Name = "ColumnDeleteStuNum";
            this.ColumnDeleteStuNum.ReadOnly = true;
            // 
            // ColumnIsDeleteName
            // 
            this.ColumnIsDeleteName.DataPropertyName = "Name";
            this.ColumnIsDeleteName.HeaderText = "姓名";
            this.ColumnIsDeleteName.Name = "ColumnIsDeleteName";
            this.ColumnIsDeleteName.ReadOnly = true;
            // 
            // ColumnIsDeleteAge
            // 
            this.ColumnIsDeleteAge.DataPropertyName = "Age";
            this.ColumnIsDeleteAge.HeaderText = "年龄";
            this.ColumnIsDeleteAge.Name = "ColumnIsDeleteAge";
            this.ColumnIsDeleteAge.ReadOnly = true;
            // 
            // ColumnIsDeleteGender
            // 
            this.ColumnIsDeleteGender.DataPropertyName = "Gender";
            this.ColumnIsDeleteGender.HeaderText = "性别";
            this.ColumnIsDeleteGender.Name = "ColumnIsDeleteGender";
            this.ColumnIsDeleteGender.ReadOnly = true;
            // 
            // ColumnIsDeleteCardID
            // 
            this.ColumnIsDeleteCardID.DataPropertyName = "CardID";
            this.ColumnIsDeleteCardID.HeaderText = "身份证号";
            this.ColumnIsDeleteCardID.Name = "ColumnIsDeleteCardID";
            this.ColumnIsDeleteCardID.ReadOnly = true;
            // 
            // ColumnIsDeletePhoneNum
            // 
            this.ColumnIsDeletePhoneNum.DataPropertyName = "PhoneNum";
            this.ColumnIsDeletePhoneNum.HeaderText = "手机号码";
            this.ColumnIsDeletePhoneNum.Name = "ColumnIsDeletePhoneNum";
            this.ColumnIsDeletePhoneNum.ReadOnly = true;
            // 
            // ColumnIsDeleteEmil
            // 
            this.ColumnIsDeleteEmil.DataPropertyName = "Email";
            this.ColumnIsDeleteEmil.HeaderText = "邮箱";
            this.ColumnIsDeleteEmil.Name = "ColumnIsDeleteEmil";
            this.ColumnIsDeleteEmil.ReadOnly = true;
            // 
            // ColumnIsDeleteAddress
            // 
            this.ColumnIsDeleteAddress.DataPropertyName = "Address";
            this.ColumnIsDeleteAddress.HeaderText = "联系地址";
            this.ColumnIsDeleteAddress.Name = "ColumnIsDeleteAddress";
            this.ColumnIsDeleteAddress.ReadOnly = true;
            // 
            // ColumnIsDeleteInfo
            // 
            this.ColumnIsDeleteInfo.DataPropertyName = "IsDelete";
            this.ColumnIsDeleteInfo.HeaderText = "逻辑删除";
            this.ColumnIsDeleteInfo.Name = "ColumnIsDeleteInfo";
            this.ColumnIsDeleteInfo.ReadOnly = true;
            // 
            // buttonRushIsDelete
            // 
            this.buttonRushIsDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRushIsDelete.Location = new System.Drawing.Point(942, 6);
            this.buttonRushIsDelete.Name = "buttonRushIsDelete";
            this.buttonRushIsDelete.Size = new System.Drawing.Size(88, 30);
            this.buttonRushIsDelete.TabIndex = 7;
            this.buttonRushIsDelete.Text = "刷新所有数据";
            this.buttonRushIsDelete.UseVisualStyleBackColor = true;
            this.buttonRushIsDelete.Click += new System.EventHandler(this.buttonRushIsDelete_Click);
            // 
            // tabPageShowData
            // 
            this.tabPageShowData.Controls.Add(this.comboBoxFind);
            this.tabPageShowData.Controls.Add(this.buttonFind);
            this.tabPageShowData.Controls.Add(this.textBoxFind);
            this.tabPageShowData.Controls.Add(this.dataGridView1);
            this.tabPageShowData.Controls.Add(this.buttonAddUser);
            this.tabPageShowData.Controls.Add(this.selectDatabase);
            this.tabPageShowData.Location = new System.Drawing.Point(4, 22);
            this.tabPageShowData.Name = "tabPageShowData";
            this.tabPageShowData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShowData.Size = new System.Drawing.Size(1036, 486);
            this.tabPageShowData.TabIndex = 0;
            this.tabPageShowData.Text = "查看所有数据";
            this.tabPageShowData.UseVisualStyleBackColor = true;
            // 
            // comboBoxFind
            // 
            this.comboBoxFind.FormattingEnabled = true;
            this.comboBoxFind.Items.AddRange(new object[] {
            "序号",
            "姓名",
            "手机号"});
            this.comboBoxFind.Location = new System.Drawing.Point(7, 12);
            this.comboBoxFind.Name = "comboBoxFind";
            this.comboBoxFind.Size = new System.Drawing.Size(62, 20);
            this.comboBoxFind.TabIndex = 5;
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(186, 10);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(52, 21);
            this.buttonFind.TabIndex = 4;
            this.buttonFind.Text = "搜索";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // textBoxFind
            // 
            this.textBoxFind.Location = new System.Drawing.Point(75, 12);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(105, 21);
            this.textBoxFind.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnIndex,
            this.ColumnGradeID,
            this.ColumnClassID,
            this.ColumnStuNum,
            this.ColumnName,
            this.ColumnAge,
            this.ColumnGender,
            this.ColumnCardID,
            this.ColumnPhoneNum,
            this.ColumnEmail,
            this.ColumnAddress});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView1.Location = new System.Drawing.Point(6, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1024, 441);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridView1_CellBeginEdit);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewIsDelete_CellContentClick);
            // 
            // ColumnIndex
            // 
            this.ColumnIndex.DataPropertyName = "Index";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnIndex.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnIndex.HeaderText = "序号";
            this.ColumnIndex.Name = "ColumnIndex";
            this.ColumnIndex.ReadOnly = true;
            // 
            // ColumnGradeID
            // 
            this.ColumnGradeID.DataPropertyName = "GradeID";
            this.ColumnGradeID.HeaderText = "年级";
            this.ColumnGradeID.Name = "ColumnGradeID";
            this.ColumnGradeID.ReadOnly = true;
            // 
            // ColumnClassID
            // 
            this.ColumnClassID.DataPropertyName = "ClassID";
            this.ColumnClassID.HeaderText = "班级";
            this.ColumnClassID.Name = "ColumnClassID";
            this.ColumnClassID.ReadOnly = true;
            // 
            // ColumnStuNum
            // 
            this.ColumnStuNum.DataPropertyName = "StuNum";
            this.ColumnStuNum.HeaderText = "学号";
            this.ColumnStuNum.Name = "ColumnStuNum";
            this.ColumnStuNum.ReadOnly = true;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnName.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnName.HeaderText = "姓名";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnAge
            // 
            this.ColumnAge.DataPropertyName = "Age";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnAge.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnAge.HeaderText = "年龄";
            this.ColumnAge.Name = "ColumnAge";
            this.ColumnAge.ReadOnly = true;
            // 
            // ColumnGender
            // 
            this.ColumnGender.DataPropertyName = "Gender";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnGender.DefaultCellStyle = dataGridViewCellStyle7;
            this.ColumnGender.HeaderText = "性别";
            this.ColumnGender.Name = "ColumnGender";
            this.ColumnGender.ReadOnly = true;
            // 
            // ColumnCardID
            // 
            this.ColumnCardID.DataPropertyName = "CardID";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnCardID.DefaultCellStyle = dataGridViewCellStyle8;
            this.ColumnCardID.HeaderText = "身份证号";
            this.ColumnCardID.Name = "ColumnCardID";
            this.ColumnCardID.ReadOnly = true;
            // 
            // ColumnPhoneNum
            // 
            this.ColumnPhoneNum.DataPropertyName = "PhoneNum";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnPhoneNum.DefaultCellStyle = dataGridViewCellStyle9;
            this.ColumnPhoneNum.HeaderText = "手机号码";
            this.ColumnPhoneNum.Name = "ColumnPhoneNum";
            this.ColumnPhoneNum.ReadOnly = true;
            // 
            // ColumnEmail
            // 
            this.ColumnEmail.DataPropertyName = "Email";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnEmail.DefaultCellStyle = dataGridViewCellStyle10;
            this.ColumnEmail.HeaderText = "邮箱";
            this.ColumnEmail.Name = "ColumnEmail";
            this.ColumnEmail.ReadOnly = true;
            // 
            // ColumnAddress
            // 
            this.ColumnAddress.DataPropertyName = "Address";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnAddress.DefaultCellStyle = dataGridViewCellStyle11;
            this.ColumnAddress.HeaderText = "联系地址";
            this.ColumnAddress.Name = "ColumnAddress";
            this.ColumnAddress.ReadOnly = true;
            // 
            // buttonAddUser
            // 
            this.buttonAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddUser.Location = new System.Drawing.Point(838, 6);
            this.buttonAddUser.Name = "buttonAddUser";
            this.buttonAddUser.Size = new System.Drawing.Size(88, 30);
            this.buttonAddUser.TabIndex = 1;
            this.buttonAddUser.Text = "添加数据";
            this.buttonAddUser.UseVisualStyleBackColor = true;
            this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
            // 
            // selectDatabase
            // 
            this.selectDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectDatabase.Location = new System.Drawing.Point(942, 6);
            this.selectDatabase.Name = "selectDatabase";
            this.selectDatabase.Size = new System.Drawing.Size(88, 30);
            this.selectDatabase.TabIndex = 1;
            this.selectDatabase.Text = "刷新所有数据";
            this.selectDatabase.UseVisualStyleBackColor = true;
            this.selectDatabase.Click += new System.EventHandler(this.secectDatabase_Click);
            // 
            // tabControlBasedata
            // 
            this.tabControlBasedata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlBasedata.Controls.Add(this.tabPageShowData);
            this.tabControlBasedata.Controls.Add(this.tabPageIsDelete);
            this.tabControlBasedata.Location = new System.Drawing.Point(12, 11);
            this.tabControlBasedata.Name = "tabControlBasedata";
            this.tabControlBasedata.SelectedIndex = 0;
            this.tabControlBasedata.Size = new System.Drawing.Size(1044, 512);
            this.tabControlBasedata.TabIndex = 5;
            this.tabControlBasedata.SelectedIndexChanged += new System.EventHandler(this.tabControlBasedata_SelectedIndexChanged);
            // 
            // MySqlBasedataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 534);
            this.Controls.Add(this.tabControlBasedata);
            this.Name = "MySqlBasedataForm";
            this.Text = "MySqlBasedata";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MySqlBasedataForm_FormClosing);
            this.Load += new System.EventHandler(this.MySqlBasedataForm_Load);
            this.tabPageIsDelete.ResumeLayout(false);
            this.tabPageIsDelete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIsDelete)).EndInit();
            this.tabPageShowData.ResumeLayout(false);
            this.tabPageShowData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControlBasedata.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageIsDelete;
        private System.Windows.Forms.ComboBox comboBoxIsDelete;
        private System.Windows.Forms.Button buttonFindIsDelete;
        private System.Windows.Forms.TextBox textBoxIsDelete;
        private System.Windows.Forms.DataGridView dataGridViewIsDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeleteGradeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeleteClassID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDeleteStuNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteCardID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeletePhoneNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteEmil;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIsDeleteInfo;
        private System.Windows.Forms.Button buttonRushIsDelete;
        private System.Windows.Forms.TabPage tabPageShowData;
        private System.Windows.Forms.ComboBox comboBoxFind;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.TextBox textBoxFind;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button selectDatabase;
        private System.Windows.Forms.TabControl tabControlBasedata;
        private System.Windows.Forms.Button buttonAddUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGradeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnClassID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStuNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCardID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPhoneNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAddress;
    }
}