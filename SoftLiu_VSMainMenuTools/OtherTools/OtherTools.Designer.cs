namespace SoftLiu_VSMainMenuTools.OtherTools
{
    partial class OtherTools
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
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.btnSure = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarDelete = new System.Windows.Forms.ToolStripProgressBar();
            this.checkBoxMeta = new System.Windows.Forms.CheckBox();
            this.checkBoxPng = new System.Windows.Forms.CheckBox();
            this.checkBoxJpg = new System.Windows.Forms.CheckBox();
            this.checkBoxCs = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.AllowDrop = true;
            this.textBoxFilePath.Location = new System.Drawing.Point(105, 12);
            this.textBoxFilePath.Multiline = true;
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(423, 70);
            this.textBoxFilePath.TabIndex = 0;
            this.textBoxFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragDrop);
            this.textBoxFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragEnter);
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(105, 166);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 1;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "删除文件的\r\n\r\n文件夹路径";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBarDelete});
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(575, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(595, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripProgressBarDelete
            // 
            this.toolStripProgressBarDelete.Name = "toolStripProgressBarDelete";
            this.toolStripProgressBarDelete.Size = new System.Drawing.Size(100, 16);
            // 
            // checkBoxMeta
            // 
            this.checkBoxMeta.AutoSize = true;
            this.checkBoxMeta.Location = new System.Drawing.Point(107, 106);
            this.checkBoxMeta.Name = "checkBoxMeta";
            this.checkBoxMeta.Size = new System.Drawing.Size(52, 17);
            this.checkBoxMeta.TabIndex = 4;
            this.checkBoxMeta.Text = ".meta";
            this.checkBoxMeta.UseVisualStyleBackColor = true;
            // 
            // checkBoxPng
            // 
            this.checkBoxPng.AutoSize = true;
            this.checkBoxPng.Location = new System.Drawing.Point(165, 106);
            this.checkBoxPng.Name = "checkBoxPng";
            this.checkBoxPng.Size = new System.Drawing.Size(47, 17);
            this.checkBoxPng.TabIndex = 4;
            this.checkBoxPng.Text = ".png";
            this.checkBoxPng.UseVisualStyleBackColor = true;
            // 
            // checkBoxJpg
            // 
            this.checkBoxJpg.AutoSize = true;
            this.checkBoxJpg.Location = new System.Drawing.Point(218, 106);
            this.checkBoxJpg.Name = "checkBoxJpg";
            this.checkBoxJpg.Size = new System.Drawing.Size(43, 17);
            this.checkBoxJpg.TabIndex = 4;
            this.checkBoxJpg.Text = ".jpg";
            this.checkBoxJpg.UseVisualStyleBackColor = true;
            // 
            // checkBoxCs
            // 
            this.checkBoxCs.AutoSize = true;
            this.checkBoxCs.Location = new System.Drawing.Point(267, 106);
            this.checkBoxCs.Name = "checkBoxCs";
            this.checkBoxCs.Size = new System.Drawing.Size(40, 17);
            this.checkBoxCs.TabIndex = 4;
            this.checkBoxCs.Text = ".cs";
            this.checkBoxCs.UseVisualStyleBackColor = true;
            // 
            // OtherTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 261);
            this.Controls.Add(this.checkBoxCs);
            this.Controls.Add(this.checkBoxJpg);
            this.Controls.Add(this.checkBoxPng);
            this.Controls.Add(this.checkBoxMeta);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.textBoxFilePath);
            this.Name = "OtherTools";
            this.Text = "OtherTools";
            this.Load += new System.EventHandler(this.OtherTools_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarDelete;
        private System.Windows.Forms.CheckBox checkBoxMeta;
        private System.Windows.Forms.CheckBox checkBoxPng;
        private System.Windows.Forms.CheckBox checkBoxJpg;
        private System.Windows.Forms.CheckBox checkBoxCs;
    }
}