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
            this.SuspendLayout();
            // 
            // webSocketBtnSend
            // 
            this.webSocketBtnSend.Location = new System.Drawing.Point(127, 191);
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
            this.webSocketTextBoxSend.Location = new System.Drawing.Point(12, 46);
            this.webSocketTextBoxSend.Multiline = true;
            this.webSocketTextBoxSend.Name = "webSocketTextBoxSend";
            this.webSocketTextBoxSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.webSocketTextBoxSend.Size = new System.Drawing.Size(340, 139);
            this.webSocketTextBoxSend.TabIndex = 1;
            // 
            // textBoxError
            // 
            this.textBoxError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxError.Location = new System.Drawing.Point(12, 242);
            this.textBoxError.Multiline = true;
            this.textBoxError.Name = "textBoxError";
            this.textBoxError.ReadOnly = true;
            this.textBoxError.Size = new System.Drawing.Size(827, 158);
            this.textBoxError.TabIndex = 2;
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceive.Location = new System.Drawing.Point(509, 46);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.ReadOnly = true;
            this.textBoxReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReceive.Size = new System.Drawing.Size(330, 139);
            this.textBoxReceive.TabIndex = 1;
            // 
            // labelSend
            // 
            this.labelSend.AutoSize = true;
            this.labelSend.Location = new System.Drawing.Point(15, 27);
            this.labelSend.Name = "labelSend";
            this.labelSend.Size = new System.Drawing.Size(49, 13);
            this.labelSend.TabIndex = 3;
            this.labelSend.Text = "发送框：";
            // 
            // labelRecv
            // 
            this.labelRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRecv.AutoSize = true;
            this.labelRecv.Location = new System.Drawing.Point(511, 28);
            this.labelRecv.Name = "labelRecv";
            this.labelRecv.Size = new System.Drawing.Size(49, 13);
            this.labelRecv.TabIndex = 3;
            this.labelRecv.Text = "接收框：";
            // 
            // labelLog
            // 
            this.labelLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(15, 226);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(46, 13);
            this.labelLog.TabIndex = 4;
            this.labelLog.Text = "Logcat：";
            // 
            // radioWebSocketState
            // 
            this.radioWebSocketState.AutoSize = true;
            this.radioWebSocketState.Location = new System.Drawing.Point(765, 12);
            this.radioWebSocketState.Name = "radioWebSocketState";
            this.radioWebSocketState.Size = new System.Drawing.Size(49, 17);
            this.radioWebSocketState.TabIndex = 5;
            this.radioWebSocketState.TabStop = true;
            this.radioWebSocketState.Text = "状态";
            this.radioWebSocketState.UseVisualStyleBackColor = true;
            // 
            // buttonClean
            // 
            this.buttonClean.Location = new System.Drawing.Point(788, 215);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(51, 24);
            this.buttonClean.TabIndex = 0;
            this.buttonClean.Text = "清除";
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // btnCleanRecv
            // 
            this.btnCleanRecv.Location = new System.Drawing.Point(509, 191);
            this.btnCleanRecv.Name = "btnCleanRecv";
            this.btnCleanRecv.Size = new System.Drawing.Size(51, 24);
            this.btnCleanRecv.TabIndex = 0;
            this.btnCleanRecv.Text = "清除";
            this.btnCleanRecv.UseVisualStyleBackColor = true;
            this.btnCleanRecv.Click += new System.EventHandler(this.btnCleanRecv_Click);
            // 
            // WebSocketClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 412);
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
            this.Name = "WebSocketClient";
            this.Text = "WebSocketClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WebSocketClient_FormClosed);
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
    }
}