namespace SoftLiu_VSMainMenuTools.TrainTicket
{
    partial class TrainTicket
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
            this.btnQuery = new System.Windows.Forms.Button();
            this.ticketGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioOneWay = new System.Windows.Forms.RadioButton();
            this.radioRoundTrip = new System.Windows.Forms.RadioButton();
            this.radioStudent = new System.Windows.Forms.RadioButton();
            this.radioOrdinary = new System.Windows.Forms.RadioButton();
            this.dateTimeTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtToStation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFromStation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkSecondSeat = new System.Windows.Forms.CheckBox();
            this.checkFirstSeat = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkTrainZ = new System.Windows.Forms.CheckBox();
            this.checkTrainD = new System.Windows.Forms.CheckBox();
            this.checkTrainG = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ticketGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnQuery.Location = new System.Drawing.Point(813, 22);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(83, 33);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = false;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // ticketGridView
            // 
            this.ticketGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ticketGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ticketGridView.Location = new System.Drawing.Point(12, 245);
            this.ticketGridView.Name = "ticketGridView";
            this.ticketGridView.RowTemplate.Height = 23;
            this.ticketGridView.Size = new System.Drawing.Size(902, 435);
            this.ticketGridView.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.radioStudent);
            this.groupBox1.Controls.Add(this.radioOrdinary);
            this.groupBox1.Controls.Add(this.dateTimeTo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dateTimeFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtToStation);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFromStation);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(902, 73);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioOneWay);
            this.groupBox3.Controls.Add(this.radioRoundTrip);
            this.groupBox3.Location = new System.Drawing.Point(6, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(58, 58);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            // 
            // radioOneWay
            // 
            this.radioOneWay.AutoSize = true;
            this.radioOneWay.Location = new System.Drawing.Point(6, 13);
            this.radioOneWay.Name = "radioOneWay";
            this.radioOneWay.Size = new System.Drawing.Size(47, 16);
            this.radioOneWay.TabIndex = 1;
            this.radioOneWay.TabStop = true;
            this.radioOneWay.Text = "单程";
            this.radioOneWay.UseVisualStyleBackColor = true;
            // 
            // radioRoundTrip
            // 
            this.radioRoundTrip.AutoSize = true;
            this.radioRoundTrip.Location = new System.Drawing.Point(6, 35);
            this.radioRoundTrip.Name = "radioRoundTrip";
            this.radioRoundTrip.Size = new System.Drawing.Size(47, 16);
            this.radioRoundTrip.TabIndex = 3;
            this.radioRoundTrip.TabStop = true;
            this.radioRoundTrip.Text = "往返";
            this.radioRoundTrip.UseVisualStyleBackColor = true;
            // 
            // radioStudent
            // 
            this.radioStudent.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioStudent.AutoSize = true;
            this.radioStudent.Location = new System.Drawing.Point(740, 43);
            this.radioStudent.Name = "radioStudent";
            this.radioStudent.Size = new System.Drawing.Size(47, 16);
            this.radioStudent.TabIndex = 13;
            this.radioStudent.TabStop = true;
            this.radioStudent.Text = "学生";
            this.radioStudent.UseVisualStyleBackColor = true;
            // 
            // radioOrdinary
            // 
            this.radioOrdinary.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.radioOrdinary.AutoSize = true;
            this.radioOrdinary.Location = new System.Drawing.Point(740, 18);
            this.radioOrdinary.Name = "radioOrdinary";
            this.radioOrdinary.Size = new System.Drawing.Size(47, 16);
            this.radioOrdinary.TabIndex = 12;
            this.radioOrdinary.TabStop = true;
            this.radioOrdinary.Text = "普通";
            this.radioOrdinary.UseVisualStyleBackColor = true;
            // 
            // dateTimeTo
            // 
            this.dateTimeTo.Location = new System.Drawing.Point(525, 43);
            this.dateTimeTo.Name = "dateTimeTo";
            this.dateTimeTo.Size = new System.Drawing.Size(114, 21);
            this.dateTimeTo.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(523, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "返程日期：";
            // 
            // dateTimeFrom
            // 
            this.dateTimeFrom.Location = new System.Drawing.Point(383, 43);
            this.dateTimeFrom.Name = "dateTimeFrom";
            this.dateTimeFrom.Size = new System.Drawing.Size(122, 21);
            this.dateTimeFrom.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(381, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "出发日期：";
            // 
            // txtToStation
            // 
            this.txtToStation.Location = new System.Drawing.Point(265, 42);
            this.txtToStation.Name = "txtToStation";
            this.txtToStation.Size = new System.Drawing.Size(100, 21);
            this.txtToStation.TabIndex = 7;
            this.txtToStation.Text = "宿州东";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(263, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "目的地：";
            // 
            // txtFromStation
            // 
            this.txtFromStation.Location = new System.Drawing.Point(125, 42);
            this.txtFromStation.Name = "txtFromStation";
            this.txtFromStation.Size = new System.Drawing.Size(94, 21);
            this.txtFromStation.TabIndex = 5;
            this.txtFromStation.Text = "上海虹桥";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "出发地：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkSecondSeat);
            this.groupBox2.Controls.Add(this.checkFirstSeat);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkTrainZ);
            this.groupBox2.Controls.Add(this.checkTrainD);
            this.groupBox2.Controls.Add(this.checkTrainG);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(902, 125);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // checkSecondSeat
            // 
            this.checkSecondSeat.AutoSize = true;
            this.checkSecondSeat.Checked = true;
            this.checkSecondSeat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSecondSeat.Location = new System.Drawing.Point(270, 101);
            this.checkSecondSeat.Name = "checkSecondSeat";
            this.checkSecondSeat.Size = new System.Drawing.Size(60, 16);
            this.checkSecondSeat.TabIndex = 9;
            this.checkSecondSeat.Text = "二等座";
            this.checkSecondSeat.UseVisualStyleBackColor = true;
            // 
            // checkFirstSeat
            // 
            this.checkFirstSeat.AutoSize = true;
            this.checkFirstSeat.Location = new System.Drawing.Point(181, 102);
            this.checkFirstSeat.Name = "checkFirstSeat";
            this.checkFirstSeat.Size = new System.Drawing.Size(60, 16);
            this.checkFirstSeat.TabIndex = 8;
            this.checkFirstSeat.Text = "一等座";
            this.checkFirstSeat.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(78, 103);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(60, 16);
            this.checkBox4.TabIndex = 7;
            this.checkBox4.Text = "商务座";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkTrainZ
            // 
            this.checkTrainZ.AutoSize = true;
            this.checkTrainZ.Location = new System.Drawing.Point(270, 20);
            this.checkTrainZ.Name = "checkTrainZ";
            this.checkTrainZ.Size = new System.Drawing.Size(60, 16);
            this.checkTrainZ.TabIndex = 6;
            this.checkTrainZ.Text = "Z-直达";
            this.checkTrainZ.UseVisualStyleBackColor = true;
            // 
            // checkTrainD
            // 
            this.checkTrainD.AutoSize = true;
            this.checkTrainD.Location = new System.Drawing.Point(181, 19);
            this.checkTrainD.Name = "checkTrainD";
            this.checkTrainD.Size = new System.Drawing.Size(60, 16);
            this.checkTrainD.TabIndex = 5;
            this.checkTrainD.Text = "D-动车";
            this.checkTrainD.UseVisualStyleBackColor = true;
            // 
            // checkTrainG
            // 
            this.checkTrainG.AutoSize = true;
            this.checkTrainG.Checked = true;
            this.checkTrainG.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTrainG.Location = new System.Drawing.Point(78, 20);
            this.checkTrainG.Name = "checkTrainG";
            this.checkTrainG.Size = new System.Drawing.Size(96, 16);
            this.checkTrainG.TabIndex = 4;
            this.checkTrainG.Text = "GC-高铁/城际";
            this.checkTrainG.UseVisualStyleBackColor = true;
            this.checkTrainG.CheckedChanged += new System.EventHandler(this.checkTrainG_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "车次席别：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "到达车站：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "出发车站：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "车次类型：";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(839, 92);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // TrainTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 584);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ticketGridView);
            this.Name = "TrainTicket";
            this.Text = "TrainTicket";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TrainTicket_FormClosing);
            this.Load += new System.EventHandler(this.TrainTicket_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ticketGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.DataGridView ticketGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioRoundTrip;
        private System.Windows.Forms.RadioButton radioOneWay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimeFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtToStation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFromStation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioStudent;
        private System.Windows.Forms.RadioButton radioOrdinary;
        private System.Windows.Forms.DateTimePicker dateTimeTo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkTrainZ;
        private System.Windows.Forms.CheckBox checkTrainD;
        private System.Windows.Forms.CheckBox checkTrainG;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkSecondSeat;
        private System.Windows.Forms.CheckBox checkFirstSeat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnLogin;
    }
}