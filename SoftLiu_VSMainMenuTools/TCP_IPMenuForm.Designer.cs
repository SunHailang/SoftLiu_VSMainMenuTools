namespace SoftLiu_VSMainMenuTools
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
            this.buttonConnectTcp = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.radioConnectStatus = new System.Windows.Forms.RadioButton();
            this.tabPageUDPClient = new System.Windows.Forms.TabPage();
            this.tabControlServer = new System.Windows.Forms.TabControl();
            this.tabPageTCP_Server = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabPageUDP_Server = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            this.tabControlClient.SuspendLayout();
            this.tabPageTCP_Client.SuspendLayout();
            this.tabControlServer.SuspendLayout();
            this.tabPageTCP_Server.SuspendLayout();
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
            this.tabControlClient.Size = new System.Drawing.Size(378, 477);
            this.tabControlClient.TabIndex = 1;
            // 
            // tabPageTCP_Client
            // 
            this.tabPageTCP_Client.Controls.Add(this.buttonConnectTcp);
            this.tabPageTCP_Client.Controls.Add(this.buttonSend);
            this.tabPageTCP_Client.Controls.Add(this.radioConnectStatus);
            this.tabPageTCP_Client.Location = new System.Drawing.Point(4, 22);
            this.tabPageTCP_Client.Name = "tabPageTCP_Client";
            this.tabPageTCP_Client.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTCP_Client.Size = new System.Drawing.Size(370, 451);
            this.tabPageTCP_Client.TabIndex = 0;
            this.tabPageTCP_Client.Text = "TCP Client";
            this.tabPageTCP_Client.UseVisualStyleBackColor = true;
            // 
            // buttonConnectTcp
            // 
            this.buttonConnectTcp.Location = new System.Drawing.Point(121, 176);
            this.buttonConnectTcp.Name = "buttonConnectTcp";
            this.buttonConnectTcp.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectTcp.TabIndex = 2;
            this.buttonConnectTcp.Text = "Connect";
            this.buttonConnectTcp.UseVisualStyleBackColor = true;
            this.buttonConnectTcp.Click += new System.EventHandler(this.buttonConnectTcp_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(121, 228);
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
            this.radioConnectStatus.Location = new System.Drawing.Point(350, 6);
            this.radioConnectStatus.Name = "radioConnectStatus";
            this.radioConnectStatus.Size = new System.Drawing.Size(14, 13);
            this.radioConnectStatus.TabIndex = 0;
            this.radioConnectStatus.TabStop = true;
            this.radioConnectStatus.UseVisualStyleBackColor = false;
            // 
            // tabPageUDPClient
            // 
            this.tabPageUDPClient.Location = new System.Drawing.Point(4, 22);
            this.tabPageUDPClient.Name = "tabPageUDPClient";
            this.tabPageUDPClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUDPClient.Size = new System.Drawing.Size(370, 451);
            this.tabPageUDPClient.TabIndex = 1;
            this.tabPageUDPClient.Text = "UDP Client";
            this.tabPageUDPClient.UseVisualStyleBackColor = true;
            // 
            // tabControlServer
            // 
            this.tabControlServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlServer.Controls.Add(this.tabPageTCP_Server);
            this.tabControlServer.Controls.Add(this.tabPageUDP_Server);
            this.tabControlServer.Location = new System.Drawing.Point(408, 28);
            this.tabControlServer.Name = "tabControlServer";
            this.tabControlServer.SelectedIndex = 0;
            this.tabControlServer.Size = new System.Drawing.Size(425, 477);
            this.tabControlServer.TabIndex = 2;
            // 
            // tabPageTCP_Server
            // 
            this.tabPageTCP_Server.Controls.Add(this.textBox1);
            this.tabPageTCP_Server.Location = new System.Drawing.Point(4, 22);
            this.tabPageTCP_Server.Name = "tabPageTCP_Server";
            this.tabPageTCP_Server.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTCP_Server.Size = new System.Drawing.Size(417, 451);
            this.tabPageTCP_Server.TabIndex = 0;
            this.tabPageTCP_Server.Text = "TCP Server";
            this.tabPageTCP_Server.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(22, 76);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(346, 290);
            this.textBox1.TabIndex = 0;
            // 
            // tabPageUDP_Server
            // 
            this.tabPageUDP_Server.Location = new System.Drawing.Point(4, 22);
            this.tabPageUDP_Server.Name = "tabPageUDP_Server";
            this.tabPageUDP_Server.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUDP_Server.Size = new System.Drawing.Size(417, 451);
            this.tabPageUDP_Server.TabIndex = 1;
            this.tabPageUDP_Server.Text = "UDP Server";
            this.tabPageUDP_Server.UseVisualStyleBackColor = true;
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
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(728, 19);
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
            this.Controls.Add(this.tabControlServer);
            this.Controls.Add(this.tabControlClient);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TCP_IPMenuForm";
            this.Text = "TCP_IPMenuForm";
            this.Load += new System.EventHandler(this.TCP_IPMenuForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlClient.ResumeLayout(false);
            this.tabPageTCP_Client.ResumeLayout(false);
            this.tabPageTCP_Client.PerformLayout();
            this.tabControlServer.ResumeLayout(false);
            this.tabPageTCP_Server.ResumeLayout(false);
            this.tabPageTCP_Server.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControlServer;
        private System.Windows.Forms.TabPage tabPageTCP_Server;
        private System.Windows.Forms.TabPage tabPageUDP_Server;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button buttonConnectTcp;
        private System.Windows.Forms.TextBox textBox1;
    }
}