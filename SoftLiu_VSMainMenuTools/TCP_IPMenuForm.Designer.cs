﻿namespace SoftLiu_VSMainMenuTools
{
    partial class TCP_IPMenuForm
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
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tCPClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uDPClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlClient = new System.Windows.Forms.TabControl();
            this.tabPageTCP_Client = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonCheckCode = new System.Windows.Forms.Button();
            this.pictureBoxCheckCode = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTCPSend = new System.Windows.Forms.TextBox();
            this.textBoxTCPRecv = new System.Windows.Forms.TextBox();
            this.buttonDisconnectTCP = new System.Windows.Forms.Button();
            this.buttonConnectTcp = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.radioConnectStatus = new System.Windows.Forms.RadioButton();
            this.tabPageUDPClient = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            this.tabControlClient.SuspendLayout();
            this.tabPageTCP_Client.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheckCode)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(845, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tCPClientToolStripMenuItem,
            this.uDPClientToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // tCPClientToolStripMenuItem
            // 
            this.tCPClientToolStripMenuItem.Name = "tCPClientToolStripMenuItem";
            this.tCPClientToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.tCPClientToolStripMenuItem.Text = "TCP";
            // 
            // uDPClientToolStripMenuItem
            // 
            this.uDPClientToolStripMenuItem.Name = "uDPClientToolStripMenuItem";
            this.uDPClientToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.uDPClientToolStripMenuItem.Text = "UDP";
            // 
            // tabControlClient
            // 
            this.tabControlClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControlClient.Controls.Add(this.tabPageTCP_Client);
            this.tabControlClient.Controls.Add(this.tabPageUDPClient);
            this.tabControlClient.ItemSize = new System.Drawing.Size(67, 18);
            this.tabControlClient.Location = new System.Drawing.Point(13, 28);
            this.tabControlClient.Name = "tabControlClient";
            this.tabControlClient.SelectedIndex = 0;
            this.tabControlClient.Size = new System.Drawing.Size(820, 477);
            this.tabControlClient.TabIndex = 1;
            // 
            // tabPageTCP_Client
            // 
            this.tabPageTCP_Client.Controls.Add(this.textBox2);
            this.tabPageTCP_Client.Controls.Add(this.label3);
            this.tabPageTCP_Client.Controls.Add(this.buttonCheckCode);
            this.tabPageTCP_Client.Controls.Add(this.pictureBoxCheckCode);
            this.tabPageTCP_Client.Controls.Add(this.textBox1);
            this.tabPageTCP_Client.Controls.Add(this.textBoxPassword);
            this.tabPageTCP_Client.Controls.Add(this.textBoxUserName);
            this.tabPageTCP_Client.Controls.Add(this.label2);
            this.tabPageTCP_Client.Controls.Add(this.label1);
            this.tabPageTCP_Client.Controls.Add(this.textBoxTCPSend);
            this.tabPageTCP_Client.Controls.Add(this.textBoxTCPRecv);
            this.tabPageTCP_Client.Controls.Add(this.buttonDisconnectTCP);
            this.tabPageTCP_Client.Controls.Add(this.buttonConnectTcp);
            this.tabPageTCP_Client.Controls.Add(this.buttonSend);
            this.tabPageTCP_Client.Controls.Add(this.radioConnectStatus);
            this.tabPageTCP_Client.Location = new System.Drawing.Point(4, 22);
            this.tabPageTCP_Client.Name = "tabPageTCP_Client";
            this.tabPageTCP_Client.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTCP_Client.Size = new System.Drawing.Size(812, 451);
            this.tabPageTCP_Client.TabIndex = 0;
            this.tabPageTCP_Client.Text = "TCP Client";
            this.tabPageTCP_Client.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(7, 230);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(217, 66);
            this.textBox2.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Check Code:";
            // 
            // buttonCheckCode
            // 
            this.buttonCheckCode.Location = new System.Drawing.Point(662, 158);
            this.buttonCheckCode.Name = "buttonCheckCode";
            this.buttonCheckCode.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckCode.TabIndex = 8;
            this.buttonCheckCode.Text = "Check Code";
            this.buttonCheckCode.UseVisualStyleBackColor = true;
            this.buttonCheckCode.Click += new System.EventHandler(this.buttonCheckCode_Click);
            // 
            // pictureBoxCheckCode
            // 
            this.pictureBoxCheckCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxCheckCode.Location = new System.Drawing.Point(278, 113);
            this.pictureBoxCheckCode.Name = "pictureBoxCheckCode";
            this.pictureBoxCheckCode.Size = new System.Drawing.Size(293, 190);
            this.pictureBoxCheckCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCheckCode.TabIndex = 7;
            this.pictureBoxCheckCode.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(344, 83);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 20);
            this.textBox1.TabIndex = 6;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(344, 52);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(132, 20);
            this.textBoxPassword.TabIndex = 6;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(344, 21);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(132, 20);
            this.textBoxUserName.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "User Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxTCPSend
            // 
            this.textBoxTCPSend.Location = new System.Drawing.Point(7, 24);
            this.textBoxTCPSend.Multiline = true;
            this.textBoxTCPSend.Name = "textBoxTCPSend";
            this.textBoxTCPSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTCPSend.Size = new System.Drawing.Size(244, 171);
            this.textBoxTCPSend.TabIndex = 4;
            // 
            // textBoxTCPRecv
            // 
            this.textBoxTCPRecv.Location = new System.Drawing.Point(6, 316);
            this.textBoxTCPRecv.Multiline = true;
            this.textBoxTCPRecv.Name = "textBoxTCPRecv";
            this.textBoxTCPRecv.ReadOnly = true;
            this.textBoxTCPRecv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTCPRecv.Size = new System.Drawing.Size(800, 129);
            this.textBoxTCPRecv.TabIndex = 3;
            // 
            // buttonDisconnectTCP
            // 
            this.buttonDisconnectTCP.Location = new System.Drawing.Point(722, 90);
            this.buttonDisconnectTCP.Name = "buttonDisconnectTCP";
            this.buttonDisconnectTCP.Size = new System.Drawing.Size(75, 23);
            this.buttonDisconnectTCP.TabIndex = 2;
            this.buttonDisconnectTCP.Text = "Disconnect";
            this.buttonDisconnectTCP.UseVisualStyleBackColor = true;
            this.buttonDisconnectTCP.Click += new System.EventHandler(this.buttonDisconnectTCP_Click);
            // 
            // buttonConnectTcp
            // 
            this.buttonConnectTcp.Location = new System.Drawing.Point(722, 45);
            this.buttonConnectTcp.Name = "buttonConnectTcp";
            this.buttonConnectTcp.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectTcp.TabIndex = 2;
            this.buttonConnectTcp.Text = "Connect";
            this.buttonConnectTcp.UseVisualStyleBackColor = true;
            this.buttonConnectTcp.Click += new System.EventHandler(this.buttonConnectTcp_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(78, 201);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // radioConnectStatus
            // 
            this.radioConnectStatus.AutoSize = true;
            this.radioConnectStatus.BackColor = System.Drawing.Color.Transparent;
            this.radioConnectStatus.Location = new System.Drawing.Point(783, 11);
            this.radioConnectStatus.Name = "radioConnectStatus";
            this.radioConnectStatus.Size = new System.Drawing.Size(14, 13);
            this.radioConnectStatus.TabIndex = 0;
            this.radioConnectStatus.TabStop = true;
            this.radioConnectStatus.UseVisualStyleBackColor = false;
            this.radioConnectStatus.CheckedChanged += new System.EventHandler(this.radioConnectStatus_CheckedChanged);
            // 
            // tabPageUDPClient
            // 
            this.tabPageUDPClient.Location = new System.Drawing.Point(4, 22);
            this.tabPageUDPClient.Name = "tabPageUDPClient";
            this.tabPageUDPClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUDPClient.Size = new System.Drawing.Size(812, 451);
            this.tabPageUDPClient.TabIndex = 1;
            this.tabPageUDPClient.Text = "UDP Client";
            this.tabPageUDPClient.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(16, 10);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 522);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(845, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(697, 19);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.ForeColor = System.Drawing.Color.Lime;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripProgressBar1.RightToLeftLayout = true;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 18);
            this.toolStripProgressBar1.Value = 50;
            // 
            // TCP_IPMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 546);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControlClient);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TCP_IPMenuForm";
            this.Text = "TCP_IPMenuForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCP_IPMenuForm_FormClosing);
            this.Load += new System.EventHandler(this.TCP_IPMenuForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlClient.ResumeLayout(false);
            this.tabPageTCP_Client.ResumeLayout(false);
            this.tabPageTCP_Client.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheckCode)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tCPClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uDPClientToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlClient;
        private System.Windows.Forms.TabPage tabPageTCP_Client;
        private System.Windows.Forms.RadioButton radioConnectStatus;
        private System.Windows.Forms.TabPage tabPageUDPClient;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button buttonConnectTcp;
        private System.Windows.Forms.TextBox textBoxTCPRecv;
        private System.Windows.Forms.TextBox textBoxTCPSend;
        private System.Windows.Forms.Button buttonDisconnectTCP;
        private System.Windows.Forms.Button buttonCheckCode;
        private System.Windows.Forms.PictureBox pictureBoxCheckCode;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}