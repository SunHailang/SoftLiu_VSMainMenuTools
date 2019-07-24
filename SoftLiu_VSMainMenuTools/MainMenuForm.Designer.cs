namespace SoftLiu_VSMainMenuTools
{
    partial class MainMenuForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excelToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMySqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControlMainTools = new System.Windows.Forms.TabControl();
            this.tabPageTimeTools = new System.Windows.Forms.TabPage();
            this.buttonCountdown = new System.Windows.Forms.Button();
            this.buttonTimeSpan = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxTime = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTimeAfter = new System.Windows.Forms.TextBox();
            this.textBoxTimeShowCount = new System.Windows.Forms.TextBox();
            this.textBoxTimeCount = new System.Windows.Forms.TextBox();
            this.textBoxTimeBefor = new System.Windows.Forms.TextBox();
            this.textBoxTimeCountdown = new System.Windows.Forms.TextBox();
            this.textBoxDesc = new System.Windows.Forms.TextBox();
            this.richTextBoxTimeSpan = new System.Windows.Forms.RichTextBox();
            this.labelTimeSpan = new System.Windows.Forms.Label();
            this.richTextBoxLocalTime = new System.Windows.Forms.RichTextBox();
            this.labelLocalTime = new System.Windows.Forms.Label();
            this.tabPageMD5Tools = new System.Windows.Forms.TabPage();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.comboBoxMD5 = new System.Windows.Forms.ComboBox();
            this.buttonMD5Sure = new System.Windows.Forms.Button();
            this.textBoxMD5Str = new System.Windows.Forms.TextBox();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.textBoxFileStr = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelFile = new System.Windows.Forms.Label();
            this.labelStr = new System.Windows.Forms.Label();
            this.tabPageHex = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.tabControlMainTools.SuspendLayout();
            this.tabPageTimeTools.SuspendLayout();
            this.tabPageMD5Tools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1043, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelToXmlToolStripMenuItem,
            this.excelToCSVToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // excelToXmlToolStripMenuItem
            // 
            this.excelToXmlToolStripMenuItem.Name = "excelToXmlToolStripMenuItem";
            this.excelToXmlToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.excelToXmlToolStripMenuItem.Text = "Excel To Xml";
            this.excelToXmlToolStripMenuItem.Click += new System.EventHandler(this.excelToXmlToolStripMenuItem_Click);
            // 
            // excelToCSVToolStripMenuItem
            // 
            this.excelToCSVToolStripMenuItem.Name = "excelToCSVToolStripMenuItem";
            this.excelToCSVToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.excelToCSVToolStripMenuItem.Text = "Excel To CSV";
            this.excelToCSVToolStripMenuItem.Click += new System.EventHandler(this.excelToCSVToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMySqlToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // openMySqlToolStripMenuItem
            // 
            this.openMySqlToolStripMenuItem.Name = "openMySqlToolStripMenuItem";
            this.openMySqlToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openMySqlToolStripMenuItem.Text = "Open mySql";
            this.openMySqlToolStripMenuItem.Click += new System.EventHandler(this.openMySqlToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControlMainTools
            // 
            this.tabControlMainTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMainTools.Controls.Add(this.tabPageTimeTools);
            this.tabControlMainTools.Controls.Add(this.tabPageMD5Tools);
            this.tabControlMainTools.Controls.Add(this.tabPageHex);
            this.tabControlMainTools.Location = new System.Drawing.Point(743, 28);
            this.tabControlMainTools.Name = "tabControlMainTools";
            this.tabControlMainTools.SelectedIndex = 0;
            this.tabControlMainTools.Size = new System.Drawing.Size(288, 566);
            this.tabControlMainTools.TabIndex = 2;
            // 
            // tabPageTimeTools
            // 
            this.tabPageTimeTools.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageTimeTools.Controls.Add(this.buttonCountdown);
            this.tabPageTimeTools.Controls.Add(this.buttonTimeSpan);
            this.tabPageTimeTools.Controls.Add(this.label3);
            this.tabPageTimeTools.Controls.Add(this.comboBoxTime);
            this.tabPageTimeTools.Controls.Add(this.label2);
            this.tabPageTimeTools.Controls.Add(this.label5);
            this.tabPageTimeTools.Controls.Add(this.label4);
            this.tabPageTimeTools.Controls.Add(this.label1);
            this.tabPageTimeTools.Controls.Add(this.textBoxTimeAfter);
            this.tabPageTimeTools.Controls.Add(this.textBoxTimeShowCount);
            this.tabPageTimeTools.Controls.Add(this.textBoxTimeCount);
            this.tabPageTimeTools.Controls.Add(this.textBoxTimeBefor);
            this.tabPageTimeTools.Controls.Add(this.textBoxTimeCountdown);
            this.tabPageTimeTools.Controls.Add(this.textBoxDesc);
            this.tabPageTimeTools.Controls.Add(this.richTextBoxTimeSpan);
            this.tabPageTimeTools.Controls.Add(this.labelTimeSpan);
            this.tabPageTimeTools.Controls.Add(this.richTextBoxLocalTime);
            this.tabPageTimeTools.Controls.Add(this.labelLocalTime);
            this.tabPageTimeTools.Location = new System.Drawing.Point(4, 22);
            this.tabPageTimeTools.Name = "tabPageTimeTools";
            this.tabPageTimeTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTimeTools.Size = new System.Drawing.Size(280, 503);
            this.tabPageTimeTools.TabIndex = 0;
            this.tabPageTimeTools.Text = "时间转换";
            this.tabPageTimeTools.UseVisualStyleBackColor = true;
            // 
            // buttonCountdown
            // 
            this.buttonCountdown.Location = new System.Drawing.Point(195, 354);
            this.buttonCountdown.Name = "buttonCountdown";
            this.buttonCountdown.Size = new System.Drawing.Size(75, 32);
            this.buttonCountdown.TabIndex = 7;
            this.buttonCountdown.Text = "确定";
            this.buttonCountdown.UseVisualStyleBackColor = true;
            this.buttonCountdown.Click += new System.EventHandler(this.buttonCountdown_Click);
            // 
            // buttonTimeSpan
            // 
            this.buttonTimeSpan.Location = new System.Drawing.Point(195, 205);
            this.buttonTimeSpan.Name = "buttonTimeSpan";
            this.buttonTimeSpan.Size = new System.Drawing.Size(75, 32);
            this.buttonTimeSpan.TabIndex = 7;
            this.buttonTimeSpan.Text = "转换";
            this.buttonTimeSpan.UseVisualStyleBackColor = true;
            this.buttonTimeSpan.Click += new System.EventHandler(this.buttonTimeSpan_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "转换类型：";
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.FormattingEnabled = true;
            this.comboBoxTime.Items.AddRange(new object[] {
            "时间转成时间戳",
            "时间戳转成时间"});
            this.comboBoxTime.Location = new System.Drawing.Point(141, 124);
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.Size = new System.Drawing.Size(129, 21);
            this.comboBoxTime.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "转换后：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 331);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "剩余时间：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 305);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "时间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "转换前：";
            // 
            // textBoxTimeAfter
            // 
            this.textBoxTimeAfter.Location = new System.Drawing.Point(75, 179);
            this.textBoxTimeAfter.Name = "textBoxTimeAfter";
            this.textBoxTimeAfter.ReadOnly = true;
            this.textBoxTimeAfter.Size = new System.Drawing.Size(195, 20);
            this.textBoxTimeAfter.TabIndex = 3;
            // 
            // textBoxTimeShowCount
            // 
            this.textBoxTimeShowCount.Location = new System.Drawing.Point(75, 328);
            this.textBoxTimeShowCount.Name = "textBoxTimeShowCount";
            this.textBoxTimeShowCount.ReadOnly = true;
            this.textBoxTimeShowCount.Size = new System.Drawing.Size(195, 20);
            this.textBoxTimeShowCount.TabIndex = 3;
            // 
            // textBoxTimeCount
            // 
            this.textBoxTimeCount.Location = new System.Drawing.Point(75, 302);
            this.textBoxTimeCount.Name = "textBoxTimeCount";
            this.textBoxTimeCount.Size = new System.Drawing.Size(195, 20);
            this.textBoxTimeCount.TabIndex = 3;
            // 
            // textBoxTimeBefor
            // 
            this.textBoxTimeBefor.Location = new System.Drawing.Point(75, 151);
            this.textBoxTimeBefor.Name = "textBoxTimeBefor";
            this.textBoxTimeBefor.Size = new System.Drawing.Size(195, 20);
            this.textBoxTimeBefor.TabIndex = 3;
            // 
            // textBoxTimeCountdown
            // 
            this.textBoxTimeCountdown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTimeCountdown.Location = new System.Drawing.Point(11, 256);
            this.textBoxTimeCountdown.Multiline = true;
            this.textBoxTimeCountdown.Name = "textBoxTimeCountdown";
            this.textBoxTimeCountdown.ReadOnly = true;
            this.textBoxTimeCountdown.Size = new System.Drawing.Size(259, 30);
            this.textBoxTimeCountdown.TabIndex = 2;
            this.textBoxTimeCountdown.Text = "倒计时\r\n时间格式：yyyy-MM-dd HH:mm:ss";
            this.textBoxTimeCountdown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxDesc
            // 
            this.textBoxDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDesc.Location = new System.Drawing.Point(11, 82);
            this.textBoxDesc.Multiline = true;
            this.textBoxDesc.Name = "textBoxDesc";
            this.textBoxDesc.ReadOnly = true;
            this.textBoxDesc.Size = new System.Drawing.Size(259, 30);
            this.textBoxDesc.TabIndex = 2;
            this.textBoxDesc.Text = "时间和时间戳互相转换\r\n时间格式：yyyy-MM-dd HH:mm:ss";
            this.textBoxDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // richTextBoxTimeSpan
            // 
            this.richTextBoxTimeSpan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxTimeSpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxTimeSpan.Location = new System.Drawing.Point(75, 46);
            this.richTextBoxTimeSpan.Name = "richTextBoxTimeSpan";
            this.richTextBoxTimeSpan.Size = new System.Drawing.Size(195, 23);
            this.richTextBoxTimeSpan.TabIndex = 1;
            this.richTextBoxTimeSpan.Text = "";
            // 
            // labelTimeSpan
            // 
            this.labelTimeSpan.AutoSize = true;
            this.labelTimeSpan.Location = new System.Drawing.Point(20, 50);
            this.labelTimeSpan.Name = "labelTimeSpan";
            this.labelTimeSpan.Size = new System.Drawing.Size(49, 13);
            this.labelTimeSpan.TabIndex = 0;
            this.labelTimeSpan.Text = "时间戳：";
            // 
            // richTextBoxLocalTime
            // 
            this.richTextBoxLocalTime.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLocalTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxLocalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLocalTime.Location = new System.Drawing.Point(75, 12);
            this.richTextBoxLocalTime.Name = "richTextBoxLocalTime";
            this.richTextBoxLocalTime.Size = new System.Drawing.Size(195, 23);
            this.richTextBoxLocalTime.TabIndex = 1;
            this.richTextBoxLocalTime.Text = "";
            // 
            // labelLocalTime
            // 
            this.labelLocalTime.AutoSize = true;
            this.labelLocalTime.Location = new System.Drawing.Point(11, 16);
            this.labelLocalTime.Name = "labelLocalTime";
            this.labelLocalTime.Size = new System.Drawing.Size(61, 13);
            this.labelLocalTime.TabIndex = 0;
            this.labelLocalTime.Text = "本地时间：";
            // 
            // tabPageMD5Tools
            // 
            this.tabPageMD5Tools.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageMD5Tools.Controls.Add(this.buttonSelectFile);
            this.tabPageMD5Tools.Controls.Add(this.comboBoxMD5);
            this.tabPageMD5Tools.Controls.Add(this.buttonMD5Sure);
            this.tabPageMD5Tools.Controls.Add(this.textBoxMD5Str);
            this.tabPageMD5Tools.Controls.Add(this.textBoxFilePath);
            this.tabPageMD5Tools.Controls.Add(this.textBoxFileStr);
            this.tabPageMD5Tools.Controls.Add(this.label6);
            this.tabPageMD5Tools.Controls.Add(this.labelFile);
            this.tabPageMD5Tools.Controls.Add(this.labelStr);
            this.tabPageMD5Tools.Location = new System.Drawing.Point(4, 22);
            this.tabPageMD5Tools.Name = "tabPageMD5Tools";
            this.tabPageMD5Tools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMD5Tools.Size = new System.Drawing.Size(280, 540);
            this.tabPageMD5Tools.TabIndex = 1;
            this.tabPageMD5Tools.Text = "MD5转换";
            this.tabPageMD5Tools.UseVisualStyleBackColor = true;
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(194, 164);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectFile.TabIndex = 4;
            this.buttonSelectFile.Text = "选择文件";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // comboBoxMD5
            // 
            this.comboBoxMD5.FormattingEnabled = true;
            this.comboBoxMD5.Items.AddRange(new object[] {
            "字符串",
            "文件"});
            this.comboBoxMD5.Location = new System.Drawing.Point(149, 194);
            this.comboBoxMD5.Name = "comboBoxMD5";
            this.comboBoxMD5.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMD5.TabIndex = 3;
            // 
            // buttonMD5Sure
            // 
            this.buttonMD5Sure.Location = new System.Drawing.Point(194, 249);
            this.buttonMD5Sure.Name = "buttonMD5Sure";
            this.buttonMD5Sure.Size = new System.Drawing.Size(75, 32);
            this.buttonMD5Sure.TabIndex = 2;
            this.buttonMD5Sure.Text = "确定";
            this.buttonMD5Sure.UseVisualStyleBackColor = true;
            this.buttonMD5Sure.Click += new System.EventHandler(this.buttonMD5Sure_Click);
            // 
            // textBoxMD5Str
            // 
            this.textBoxMD5Str.Location = new System.Drawing.Point(9, 223);
            this.textBoxMD5Str.Name = "textBoxMD5Str";
            this.textBoxMD5Str.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMD5Str.Size = new System.Drawing.Size(261, 20);
            this.textBoxMD5Str.TabIndex = 1;
            this.textBoxMD5Str.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragDrop);
            this.textBoxMD5Str.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragEnter);
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.AllowDrop = true;
            this.textBoxFilePath.Location = new System.Drawing.Point(60, 103);
            this.textBoxFilePath.Multiline = true;
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.ReadOnly = true;
            this.textBoxFilePath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFilePath.Size = new System.Drawing.Size(210, 54);
            this.textBoxFilePath.TabIndex = 1;
            this.textBoxFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragDrop);
            this.textBoxFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragEnter);
            // 
            // textBoxFileStr
            // 
            this.textBoxFileStr.Location = new System.Drawing.Point(60, 7);
            this.textBoxFileStr.Multiline = true;
            this.textBoxFileStr.Name = "textBoxFileStr";
            this.textBoxFileStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFileStr.Size = new System.Drawing.Size(210, 90);
            this.textBoxFileStr.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "MD5字符串：";
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(17, 124);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(37, 13);
            this.labelFile.TabIndex = 0;
            this.labelFile.Text = "文件：";
            // 
            // labelStr
            // 
            this.labelStr.AutoSize = true;
            this.labelStr.Location = new System.Drawing.Point(6, 45);
            this.labelStr.Name = "labelStr";
            this.labelStr.Size = new System.Drawing.Size(49, 13);
            this.labelStr.TabIndex = 0;
            this.labelStr.Text = "字符串：";
            // 
            // tabPageHex
            // 
            this.tabPageHex.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageHex.Location = new System.Drawing.Point(4, 22);
            this.tabPageHex.Name = "tabPageHex";
            this.tabPageHex.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHex.Size = new System.Drawing.Size(280, 503);
            this.tabPageHex.TabIndex = 2;
            this.tabPageHex.Text = "进制转换";
            this.tabPageHex.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 97);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(724, 497);
            this.dataGridView1.TabIndex = 3;
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 606);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tabControlMainTools);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainMenuForm";
            this.Text = "MainMenu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMenuForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMenu_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlMainTools.ResumeLayout(false);
            this.tabPageTimeTools.ResumeLayout(false);
            this.tabPageTimeTools.PerformLayout();
            this.tabPageMD5Tools.ResumeLayout(false);
            this.tabPageMD5Tools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToXmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMySqlToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControlMainTools;
        private System.Windows.Forms.TabPage tabPageTimeTools;
        private System.Windows.Forms.TabPage tabPageMD5Tools;
        private System.Windows.Forms.TabPage tabPageHex;
        private System.Windows.Forms.RichTextBox richTextBoxLocalTime;
        private System.Windows.Forms.Label labelLocalTime;
        private System.Windows.Forms.RichTextBox richTextBoxTimeSpan;
        private System.Windows.Forms.Label labelTimeSpan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTimeAfter;
        private System.Windows.Forms.TextBox textBoxTimeBefor;
        private System.Windows.Forms.TextBox textBoxDesc;
        private System.Windows.Forms.Button buttonTimeSpan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTimeShowCount;
        private System.Windows.Forms.TextBox textBoxTimeCount;
        private System.Windows.Forms.TextBox textBoxTimeCountdown;
        private System.Windows.Forms.Button buttonCountdown;
        private System.Windows.Forms.TextBox textBoxFileStr;
        private System.Windows.Forms.Label labelStr;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.TextBox textBoxMD5Str;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonMD5Sure;
        private System.Windows.Forms.ComboBox comboBoxMD5;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

