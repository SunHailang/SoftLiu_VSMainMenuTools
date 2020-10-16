namespace SoftLiu_VSMainMenuTools.SocketClient.WebSocketData
{
    partial class WebSocketClient
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
            this.webSocketBtnSend = new System.Windows.Forms.Button();
            this.webSocketTextBoxSend = new System.Windows.Forms.TextBox();
            this.textBoxError = new System.Windows.Forms.TextBox();
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.labelSend = new System.Windows.Forms.Label();
            this.labelRecv = new System.Windows.Forms.Label();
            this.labelLog = new System.Windows.Forms.Label();
            this.radioWebSocketState = new System.Windows.Forms.RadioButton();
            this.buttonClean = new System.Windows.Forms.Button();
            this.btnCleanRecv = new System.Windows.Forms.Button();
            this.buttonCloseWebSocket = new System.Windows.Forms.Button();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.labelServer = new System.Windows.Forms.Label();
            this.buttonConnServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webSocketBtnSend
            // 
            this.webSocketBtnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.webSocketBtnSend.Location = new System.Drawing.Point(160, 243);
            this.webSocketBtnSend.Name = "webSocketBtnSend";
            this.webSocketBtnSend.Size = new System.Drawing.Size(79, 26);
            this.webSocketBtnSend.TabIndex = 0;
            this.webSocketBtnSend.Text = "发送";
            this.webSocketBtnSend.UseVisualStyleBackColor = true;
            this.webSocketBtnSend.Click += new System.EventHandler(this.webSocketBtnSend_Click);
            // 
            // webSocketTextBoxSend
            // 
            this.webSocketTextBoxSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.webSocketTextBoxSend.Location = new System.Drawing.Point(12, 63);
            this.webSocketTextBoxSend.Multiline = true;
            this.webSocketTextBoxSend.Name = "webSocketTextBoxSend";
            this.webSocketTextBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.webSocketTextBoxSend.Size = new System.Drawing.Size(380, 171);
            this.webSocketTextBoxSend.TabIndex = 1;
            // 
            // textBoxError
            // 
            this.textBoxError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxError.Location = new System.Drawing.Point(12, 294);
            this.textBoxError.Multiline = true;
            this.textBoxError.Name = "textBoxError";
            this.textBoxError.ReadOnly = true;
            this.textBoxError.Size = new System.Drawing.Size(797, 158);
            this.textBoxError.TabIndex = 2;
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceive.Location = new System.Drawing.Point(440, 63);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ReadOnly = true;
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReceive.Size = new System.Drawing.Size(369, 171);
            this.textBoxReceive.TabIndex = 1;
            // 
            // labelSend
            // 
            this.labelSend.AutoSize = true;
            this.labelSend.Location = new System.Drawing.Point(12, 44);
            this.labelSend.Name = "labelSend";
            this.labelSend.Size = new System.Drawing.Size(49, 13);
            this.labelSend.TabIndex = 3;
            this.labelSend.Text = "发送框：";
            // 
            // labelRecv
            // 
            this.labelRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRecv.AutoSize = true;
            this.labelRecv.Location = new System.Drawing.Point(440, 45);
            this.labelRecv.Name = "labelRecv";
            this.labelRecv.Size = new System.Drawing.Size(49, 13);
            this.labelRecv.TabIndex = 3;
            this.labelRecv.Text = "接收框：";
            // 
            // labelLog
            // 
            this.labelLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(12, 278);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(49, 13);
            this.labelLog.TabIndex = 4;
            this.labelLog.Text = "日志框：";
            // 
            // radioWebSocketState
            // 
            this.radioWebSocketState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioWebSocketState.AutoSize = true;
            this.radioWebSocketState.Location = new System.Drawing.Point(680, 12);
            this.radioWebSocketState.Name = "radioWebSocketState";
            this.radioWebSocketState.Size = new System.Drawing.Size(49, 17);
            this.radioWebSocketState.TabIndex = 5;
            this.radioWebSocketState.TabStop = true;
            this.radioWebSocketState.Text = "状态";
            this.radioWebSocketState.UseVisualStyleBackColor = true;
            // 
            // buttonClean
            // 
            this.buttonClean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClean.Location = new System.Drawing.Point(758, 267);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(51, 24);
            this.buttonClean.TabIndex = 0;
            this.buttonClean.Text = "清除";
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // btnCleanRecv
            // 
            this.btnCleanRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanRecv.Location = new System.Drawing.Point(443, 244);
            this.btnCleanRecv.Name = "btnCleanRecv";
            this.btnCleanRecv.Size = new System.Drawing.Size(51, 24);
            this.btnCleanRecv.TabIndex = 0;
            this.btnCleanRecv.Text = "清除";
            this.btnCleanRecv.UseVisualStyleBackColor = true;
            this.btnCleanRecv.Click += new System.EventHandler(this.btnCleanRecv_Click);
            // 
            // buttonCloseWebSocket
            // 
            this.buttonCloseWebSocket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCloseWebSocket.Location = new System.Drawing.Point(734, 9);
            this.buttonCloseWebSocket.Name = "buttonCloseWebSocket";
            this.buttonCloseWebSocket.Size = new System.Drawing.Size(75, 23);
            this.buttonCloseWebSocket.TabIndex = 6;
            this.buttonCloseWebSocket.Text = "断开连接";
            this.buttonCloseWebSocket.UseVisualStyleBackColor = true;
            this.buttonCloseWebSocket.Click += new System.EventHandler(this.buttonCloseWebSocket_Click);
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.FormattingEnabled = true;
            this.comboBoxServer.Items.AddRange(new object[] {
            "wss://cs-s-1000106500.gamebean.net/echo"});
            this.comboBoxServer.Location = new System.Drawing.Point(73, 9);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(155, 21);
            this.comboBoxServer.TabIndex = 7;
            // 
            // labelServer
            // 
            this.labelServer.AutoSize = true;
            this.labelServer.Location = new System.Drawing.Point(12, 12);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(49, 13);
            this.labelServer.TabIndex = 8;
            this.labelServer.Text = "服务器：";
            // 
            // buttonConnServer
            // 
            this.buttonConnServer.Location = new System.Drawing.Point(236, 7);
            this.buttonConnServer.Name = "buttonConnServer";
            this.buttonConnServer.Size = new System.Drawing.Size(75, 23);
            this.buttonConnServer.TabIndex = 9;
            this.buttonConnServer.Text = "连接";
            this.buttonConnServer.UseVisualStyleBackColor = true;
            this.buttonConnServer.Click += new System.EventHandler(this.buttonConnServer_Click);
            // 
            // WebSocketClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 464);
            this.Controls.Add(this.buttonConnServer);
            this.Controls.Add(this.labelServer);
            this.Controls.Add(this.comboBoxServer);
            this.Controls.Add(this.buttonCloseWebSocket);
            this.Controls.Add(this.radioWebSocketState);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.labelRecv);
            this.Controls.Add(this.labelSend);
            this.Controls.Add(this.textBoxError);
            this.Controls.Add(this.textBoxReceive);
            this.Controls.Add(this.webSocketTextBoxSend);
            this.Controls.Add(this.btnCleanRecv);
            this.Controls.Add(this.buttonClean);
            this.Controls.Add(this.webSocketBtnSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "WebSocketClient";
            this.Text = "WebSocketClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WebSocketClient_FormClosing);
            this.Load += new System.EventHandler(this.WebSocketClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button webSocketBtnSend;
        private System.Windows.Forms.TextBox webSocketTextBoxSend;
        private System.Windows.Forms.TextBox textBoxError;
        private System.Windows.Forms.TextBox textBoxReceive;
        private System.Windows.Forms.Label labelSend;
        private System.Windows.Forms.Label labelRecv;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.RadioButton radioWebSocketState;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Button btnCleanRecv;
        private System.Windows.Forms.Button buttonCloseWebSocket;
        private System.Windows.Forms.ComboBox comboBoxServer;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Button buttonConnServer;
    }
}