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
            this.labelSend = new System.Windows.Forms.Label();
            this.labelRecv = new System.Windows.Forms.Label();
            this.comboBoxTcpAddress = new System.Windows.Forms.ComboBox();
            this.labelTCPAddress = new System.Windows.Forms.Label();
            this.textBoxTCPRecv = new System.Windows.Forms.TextBox();
            this.textBoxTCPSend = new System.Windows.Forms.TextBox();
            this.textBoxTCPTips = new System.Windows.Forms.TextBox();
            this.buttonConnectTcp = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.radioConnectStatus = new System.Windows.Forms.RadioButton();
            this.tabPageUDPClient = new System.Windows.Forms.TabPage();
            this.comboBoxUdpAddress = new System.Windows.Forms.ComboBox();
            this.labelUdpAddress = new System.Windows.Forms.Label();
            this.buttonUDPSend = new System.Windows.Forms.Button();
            this.textBoxUDPReceive = new System.Windows.Forms.TextBox();
            this.textBoxUDPSendMessage = new System.Windows.Forms.TextBox();
            this.tabPageTools = new System.Windows.Forms.TabPage();
            this.richTextBoxToolsLog = new System.Windows.Forms.RichTextBox();
            this.buttonToolsGetIP = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1.SuspendLayout();
            this.tabControlClient.SuspendLayout();
            this.tabPageTCP_Client.SuspendLayout();
            this.tabPageUDPClient.SuspendLayout();
            this.tabPageTools.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(969, 24);
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
            this.tabControlClient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlClient.Controls.Add(this.tabPageTCP_Client);
            this.tabControlClient.Controls.Add(this.tabPageUDPClient);
            this.tabControlClient.Controls.Add(this.tabPageTools);
            this.tabControlClient.ItemSize = new System.Drawing.Size(67, 18);
            this.tabControlClient.Location = new System.Drawing.Point(13, 26);
            this.tabControlClient.Name = "tabControlClient";
            this.tabControlClient.SelectedIndex = 0;
            this.tabControlClient.Size = new System.Drawing.Size(945, 438);
            this.tabControlClient.TabIndex = 1;
            // 
            // tabPageTCP_Client
            // 
            this.tabPageTCP_Client.Controls.Add(this.labelSend);
            this.tabPageTCP_Client.Controls.Add(this.labelRecv);
            this.tabPageTCP_Client.Controls.Add(this.comboBoxTcpAddress);
            this.tabPageTCP_Client.Controls.Add(this.labelTCPAddress);
            this.tabPageTCP_Client.Controls.Add(this.textBoxTCPRecv);
            this.tabPageTCP_Client.Controls.Add(this.textBoxTCPSend);
            this.tabPageTCP_Client.Controls.Add(this.textBoxTCPTips);
            this.tabPageTCP_Client.Controls.Add(this.buttonConnectTcp);
            this.tabPageTCP_Client.Controls.Add(this.buttonSend);
            this.tabPageTCP_Client.Controls.Add(this.radioConnectStatus);
            this.tabPageTCP_Client.Location = new System.Drawing.Point(4, 22);
            this.tabPageTCP_Client.Name = "tabPageTCP_Client";
            this.tabPageTCP_Client.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTCP_Client.Size = new System.Drawing.Size(937, 412);
            this.tabPageTCP_Client.TabIndex = 0;
            this.tabPageTCP_Client.Text = "TCP Client";
            this.tabPageTCP_Client.UseVisualStyleBackColor = true;
            // 
            // labelSend
            // 
            this.labelSend.AutoSize = true;
            this.labelSend.Location = new System.Drawing.Point(17, 53);
            this.labelSend.Name = "labelSend";
            this.labelSend.Size = new System.Drawing.Size(71, 12);
            this.labelSend.TabIndex = 8;
            this.labelSend.Text = "Send Data：";
            // 
            // labelRecv
            // 
            this.labelRecv.AutoSize = true;
            this.labelRecv.Location = new System.Drawing.Point(380, 53);
            this.labelRecv.Name = "labelRecv";
            this.labelRecv.Size = new System.Drawing.Size(89, 12);
            this.labelRecv.TabIndex = 8;
            this.labelRecv.Text = "Receive Data：";
            // 
            // comboBoxTcpAddress
            // 
            this.comboBoxTcpAddress.FormattingEnabled = true;
            this.comboBoxTcpAddress.Location = new System.Drawing.Point(84, 10);
            this.comboBoxTcpAddress.Name = "comboBoxTcpAddress";
            this.comboBoxTcpAddress.Size = new System.Drawing.Size(109, 20);
            this.comboBoxTcpAddress.TabIndex = 7;
            this.comboBoxTcpAddress.SelectedIndexChanged += new System.EventHandler(this.comboBoxTcpAddress_SelectedIndexChanged);
            // 
            // labelTCPAddress
            // 
            this.labelTCPAddress.AutoSize = true;
            this.labelTCPAddress.Location = new System.Drawing.Point(17, 13);
            this.labelTCPAddress.Name = "labelTCPAddress";
            this.labelTCPAddress.Size = new System.Drawing.Size(65, 12);
            this.labelTCPAddress.TabIndex = 6;
            this.labelTCPAddress.Text = "主机地址：";
            // 
            // textBoxTCPRecv
            // 
            this.textBoxTCPRecv.Location = new System.Drawing.Point(383, 67);
            this.textBoxTCPRecv.Multiline = true;
            this.textBoxTCPRecv.Name = "textBoxTCPRecv";
            this.textBoxTCPRecv.ReadOnly = true;
            this.textBoxTCPRecv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTCPRecv.Size = new System.Drawing.Size(548, 220);
            this.textBoxTCPRecv.TabIndex = 4;
            // 
            // textBoxTCPSend
            // 
            this.textBoxTCPSend.Location = new System.Drawing.Point(20, 67);
            this.textBoxTCPSend.Multiline = true;
            this.textBoxTCPSend.Name = "textBoxTCPSend";
            this.textBoxTCPSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTCPSend.Size = new System.Drawing.Size(254, 222);
            this.textBoxTCPSend.TabIndex = 4;
            // 
            // textBoxTCPTips
            // 
            this.textBoxTCPTips.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTCPTips.Location = new System.Drawing.Point(6, 306);
            this.textBoxTCPTips.Multiline = true;
            this.textBoxTCPTips.Name = "textBoxTCPTips";
            this.textBoxTCPTips.ReadOnly = true;
            this.textBoxTCPTips.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTCPTips.Size = new System.Drawing.Size(925, 103);
            this.textBoxTCPTips.TabIndex = 3;
            // 
            // buttonConnectTcp
            // 
            this.buttonConnectTcp.Location = new System.Drawing.Point(357, 8);
            this.buttonConnectTcp.Name = "buttonConnectTcp";
            this.buttonConnectTcp.Size = new System.Drawing.Size(75, 21);
            this.buttonConnectTcp.TabIndex = 2;
            this.buttonConnectTcp.Text = "Connect";
            this.buttonConnectTcp.UseVisualStyleBackColor = true;
            this.buttonConnectTcp.Click += new System.EventHandler(this.buttonConnectTcp_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(280, 268);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 21);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // radioConnectStatus
            // 
            this.radioConnectStatus.AutoSize = true;
            this.radioConnectStatus.BackColor = System.Drawing.Color.Transparent;
            this.radioConnectStatus.Location = new System.Drawing.Point(447, 13);
            this.radioConnectStatus.Name = "radioConnectStatus";
            this.radioConnectStatus.Size = new System.Drawing.Size(14, 13);
            this.radioConnectStatus.TabIndex = 0;
            this.radioConnectStatus.TabStop = true;
            this.radioConnectStatus.UseVisualStyleBackColor = false;
            // 
            // tabPageUDPClient
            // 
            this.tabPageUDPClient.Controls.Add(this.comboBoxUdpAddress);
            this.tabPageUDPClient.Controls.Add(this.labelUdpAddress);
            this.tabPageUDPClient.Controls.Add(this.buttonUDPSend);
            this.tabPageUDPClient.Controls.Add(this.textBoxUDPReceive);
            this.tabPageUDPClient.Controls.Add(this.textBoxUDPSendMessage);
            this.tabPageUDPClient.Location = new System.Drawing.Point(4, 22);
            this.tabPageUDPClient.Name = "tabPageUDPClient";
            this.tabPageUDPClient.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUDPClient.Size = new System.Drawing.Size(937, 412);
            this.tabPageUDPClient.TabIndex = 1;
            this.tabPageUDPClient.Text = "UDP Client";
            this.tabPageUDPClient.UseVisualStyleBackColor = true;
            // 
            // comboBoxUdpAddress
            // 
            this.comboBoxUdpAddress.FormattingEnabled = true;
            this.comboBoxUdpAddress.Location = new System.Drawing.Point(85, 17);
            this.comboBoxUdpAddress.Name = "comboBoxUdpAddress";
            this.comboBoxUdpAddress.Size = new System.Drawing.Size(109, 20);
            this.comboBoxUdpAddress.TabIndex = 9;
            this.comboBoxUdpAddress.SelectedIndexChanged += new System.EventHandler(this.comboBoxUdpAddress_SelectedIndexChanged);
            // 
            // labelUdpAddress
            // 
            this.labelUdpAddress.AutoSize = true;
            this.labelUdpAddress.Location = new System.Drawing.Point(18, 19);
            this.labelUdpAddress.Name = "labelUdpAddress";
            this.labelUdpAddress.Size = new System.Drawing.Size(65, 12);
            this.labelUdpAddress.TabIndex = 8;
            this.labelUdpAddress.Text = "主机地址：";
            // 
            // buttonUDPSend
            // 
            this.buttonUDPSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUDPSend.Location = new System.Drawing.Point(852, 221);
            this.buttonUDPSend.Name = "buttonUDPSend";
            this.buttonUDPSend.Size = new System.Drawing.Size(78, 26);
            this.buttonUDPSend.TabIndex = 3;
            this.buttonUDPSend.Text = "发送";
            this.buttonUDPSend.UseVisualStyleBackColor = true;
            this.buttonUDPSend.Click += new System.EventHandler(this.buttonUDPSend_Click);
            // 
            // textBoxUDPReceive
            // 
            this.textBoxUDPReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUDPReceive.Location = new System.Drawing.Point(6, 265);
            this.textBoxUDPReceive.Multiline = true;
            this.textBoxUDPReceive.Name = "textBoxUDPReceive";
            this.textBoxUDPReceive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxUDPReceive.Size = new System.Drawing.Size(925, 144);
            this.textBoxUDPReceive.TabIndex = 2;
            // 
            // textBoxUDPSendMessage
            // 
            this.textBoxUDPSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUDPSendMessage.Location = new System.Drawing.Point(6, 96);
            this.textBoxUDPSendMessage.Multiline = true;
            this.textBoxUDPSendMessage.Name = "textBoxUDPSendMessage";
            this.textBoxUDPSendMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUDPSendMessage.Size = new System.Drawing.Size(925, 118);
            this.textBoxUDPSendMessage.TabIndex = 2;
            // 
            // tabPageTools
            // 
            this.tabPageTools.Controls.Add(this.richTextBoxToolsLog);
            this.tabPageTools.Controls.Add(this.buttonToolsGetIP);
            this.tabPageTools.Location = new System.Drawing.Point(4, 22);
            this.tabPageTools.Name = "tabPageTools";
            this.tabPageTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTools.Size = new System.Drawing.Size(937, 412);
            this.tabPageTools.TabIndex = 2;
            this.tabPageTools.Text = "Tools";
            this.tabPageTools.UseVisualStyleBackColor = true;
            // 
            // richTextBoxToolsLog
            // 
            this.richTextBoxToolsLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxToolsLog.Location = new System.Drawing.Point(7, 226);
            this.richTextBoxToolsLog.Name = "richTextBoxToolsLog";
            this.richTextBoxToolsLog.Size = new System.Drawing.Size(924, 183);
            this.richTextBoxToolsLog.TabIndex = 1;
            this.richTextBoxToolsLog.Text = "";
            // 
            // buttonToolsGetIP
            // 
            this.buttonToolsGetIP.Location = new System.Drawing.Point(6, 18);
            this.buttonToolsGetIP.Name = "buttonToolsGetIP";
            this.buttonToolsGetIP.Size = new System.Drawing.Size(75, 21);
            this.buttonToolsGetIP.TabIndex = 0;
            this.buttonToolsGetIP.Text = "获取IP";
            this.buttonToolsGetIP.UseVisualStyleBackColor = true;
            this.buttonToolsGetIP.Click += new System.EventHandler(this.buttonToolsGetIP_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(16, 10);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 478);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(969, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(801, 19);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.ForeColor = System.Drawing.Color.Lime;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripProgressBar1.RightToLeftLayout = true;
            this.toolStripProgressBar1.Size = new System.Drawing.Size(120, 18);
            this.toolStripProgressBar1.Value = 50;
            // 
            // TCP_IPMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 502);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControlClient);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
            this.tabPageUDPClient.ResumeLayout(false);
            this.tabPageUDPClient.PerformLayout();
            this.tabPageTools.ResumeLayout(false);
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
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button buttonConnectTcp;
        private System.Windows.Forms.TextBox textBoxTCPTips;
        private System.Windows.Forms.TextBox textBoxTCPSend;
        private System.Windows.Forms.TabPage tabPageTools;
        private System.Windows.Forms.RichTextBox richTextBoxToolsLog;
        private System.Windows.Forms.Button buttonToolsGetIP;
        private System.Windows.Forms.Label labelTCPAddress;
        private System.Windows.Forms.TextBox textBoxTCPRecv;
        private System.Windows.Forms.ComboBox comboBoxTcpAddress;
        private System.Windows.Forms.TabPage tabPageUDPClient;
        private System.Windows.Forms.ComboBox comboBoxUdpAddress;
        private System.Windows.Forms.Label labelUdpAddress;
        private System.Windows.Forms.Button buttonUDPSend;
        private System.Windows.Forms.TextBox textBoxUDPReceive;
        private System.Windows.Forms.TextBox textBoxUDPSendMessage;
        private System.Windows.Forms.Label labelRecv;
        private System.Windows.Forms.Label labelSend;
    }
}