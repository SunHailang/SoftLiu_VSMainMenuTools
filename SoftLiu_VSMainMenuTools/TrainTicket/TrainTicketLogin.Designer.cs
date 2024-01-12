namespace SoftLiu_VSMainMenuTools.TrainTicket
{
    partial class TrainTicketLogin
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
            this.components = new System.ComponentModel.Container();
            this.pictureQR = new System.Windows.Forms.PictureBox();
            this.TimerQueryQR = new System.Windows.Forms.Timer(this.components);
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            this.tabControlLogin = new System.Windows.Forms.TabControl();
            this.tabPageUser = new System.Windows.Forms.TabPage();
            this.tabPageQR = new System.Windows.Forms.TabPage();
            this.btnQRRefresh = new System.Windows.Forms.Button();
            this.btnUserLogin = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBoxLogin = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureQR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.tabControlLogin.SuspendLayout();
            this.tabPageUser.SuspendLayout();
            this.tabPageQR.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureQR
            // 
            this.pictureQR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureQR.Location = new System.Drawing.Point(10, 10);
            this.pictureQR.Name = "pictureQR";
            this.pictureQR.Size = new System.Drawing.Size(200, 200);
            this.pictureQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureQR.TabIndex = 0;
            this.pictureQR.TabStop = false;
            // 
            // TimerQueryQR
            // 
            this.TimerQueryQR.Interval = 1000;
            this.TimerQueryQR.Tick += new System.EventHandler(this.TimerQueryQR_Tick);
            // 
            // pictureLogo
            // 
            this.pictureLogo.Location = new System.Drawing.Point(12, 12);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new System.Drawing.Size(200, 50);
            this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureLogo.TabIndex = 1;
            this.pictureLogo.TabStop = false;
            // 
            // tabControlLogin
            // 
            this.tabControlLogin.Controls.Add(this.tabPageUser);
            this.tabControlLogin.Controls.Add(this.tabPageQR);
            this.tabControlLogin.Location = new System.Drawing.Point(218, 12);
            this.tabControlLogin.Name = "tabControlLogin";
            this.tabControlLogin.SelectedIndex = 0;
            this.tabControlLogin.Size = new System.Drawing.Size(230, 285);
            this.tabControlLogin.TabIndex = 2;
            this.tabControlLogin.SelectedIndexChanged += new System.EventHandler(this.tabControlLogin_SelectedIndexChanged);
            // 
            // tabPageUser
            // 
            this.tabPageUser.Controls.Add(this.label2);
            this.tabPageUser.Controls.Add(this.label1);
            this.tabPageUser.Controls.Add(this.textBox2);
            this.tabPageUser.Controls.Add(this.textBox1);
            this.tabPageUser.Controls.Add(this.btnUserLogin);
            this.tabPageUser.Location = new System.Drawing.Point(4, 22);
            this.tabPageUser.Name = "tabPageUser";
            this.tabPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUser.Size = new System.Drawing.Size(222, 224);
            this.tabPageUser.TabIndex = 0;
            this.tabPageUser.Text = "账号登录";
            this.tabPageUser.UseVisualStyleBackColor = true;
            // 
            // tabPageQR
            // 
            this.tabPageQR.Controls.Add(this.label3);
            this.tabPageQR.Controls.Add(this.btnQRRefresh);
            this.tabPageQR.Controls.Add(this.pictureQR);
            this.tabPageQR.Location = new System.Drawing.Point(4, 22);
            this.tabPageQR.Name = "tabPageQR";
            this.tabPageQR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageQR.Size = new System.Drawing.Size(222, 259);
            this.tabPageQR.TabIndex = 1;
            this.tabPageQR.Text = "扫码登录";
            this.tabPageQR.UseVisualStyleBackColor = true;
            // 
            // btnQRRefresh
            // 
            this.btnQRRefresh.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnQRRefresh.Location = new System.Drawing.Point(10, 10);
            this.btnQRRefresh.Name = "btnQRRefresh";
            this.btnQRRefresh.Size = new System.Drawing.Size(200, 200);
            this.btnQRRefresh.TabIndex = 1;
            this.btnQRRefresh.Text = "刷新";
            this.btnQRRefresh.UseVisualStyleBackColor = false;
            this.btnQRRefresh.Visible = false;
            this.btnQRRefresh.Click += new System.EventHandler(this.btnQRRefresh_Click);
            // 
            // btnUserLogin
            // 
            this.btnUserLogin.BackColor = System.Drawing.Color.Orange;
            this.btnUserLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserLogin.Location = new System.Drawing.Point(21, 167);
            this.btnUserLogin.Name = "btnUserLogin";
            this.btnUserLogin.Size = new System.Drawing.Size(180, 42);
            this.btnUserLogin.TabIndex = 0;
            this.btnUserLogin.Text = "立即登录";
            this.btnUserLogin.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(21, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(180, 21);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(21, 105);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(180, 21);
            this.textBox2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "用户名/邮箱/手机号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "密码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "打开12306手机APP扫描二维码";
            // 
            // richTextBoxLogin
            // 
            this.richTextBoxLogin.Location = new System.Drawing.Point(12, 85);
            this.richTextBoxLogin.Name = "richTextBoxLogin";
            this.richTextBoxLogin.ReadOnly = true;
            this.richTextBoxLogin.Size = new System.Drawing.Size(200, 212);
            this.richTextBoxLogin.TabIndex = 3;
            this.richTextBoxLogin.Text = "";
            // 
            // TrainTicketLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 309);
            this.Controls.Add(this.richTextBoxLogin);
            this.Controls.Add(this.tabControlLogin);
            this.Controls.Add(this.pictureLogo);
            this.Name = "TrainTicketLogin";
            this.Text = "TrainTicketLogin";
            this.Load += new System.EventHandler(this.TrainTicketLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureQR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.tabControlLogin.ResumeLayout(false);
            this.tabPageUser.ResumeLayout(false);
            this.tabPageUser.PerformLayout();
            this.tabPageQR.ResumeLayout(false);
            this.tabPageQR.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureQR;
        private System.Windows.Forms.Timer TimerQueryQR;
        private System.Windows.Forms.PictureBox pictureLogo;
        private System.Windows.Forms.TabControl tabControlLogin;
        private System.Windows.Forms.TabPage tabPageUser;
        private System.Windows.Forms.TabPage tabPageQR;
        private System.Windows.Forms.Button btnQRRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnUserLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBoxLogin;
    }
}