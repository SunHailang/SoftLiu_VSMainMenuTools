using System;

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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.AllowDrop = true;
            this.textBoxFilePath.Location = new System.Drawing.Point(105, 12);
            this.textBoxFilePath.Multiline = true;
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(423, 33);
            this.textBoxFilePath.TabIndex = 0;
            this.textBoxFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragDrop);
            this.textBoxFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragEnter);
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(453, 51);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 21);
            this.btnSure.TabIndex = 1;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 36);
            this.label1.TabIndex = 2;
            this.label1.Text = $"删除文件的{Environment.NewLine}文件夹路径";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBarDelete});
            this.statusStrip1.Location = new System.Drawing.Point(0, 219);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(575, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(458, 17);
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
            this.checkBoxMeta.Location = new System.Drawing.Point(106, 51);
            this.checkBoxMeta.Name = "checkBoxMeta";
            this.checkBoxMeta.Size = new System.Drawing.Size(54, 16);
            this.checkBoxMeta.TabIndex = 4;
            this.checkBoxMeta.Text = ".meta";
            this.checkBoxMeta.UseVisualStyleBackColor = true;
            // 
            // checkBoxPng
            // 
            this.checkBoxPng.AutoSize = true;
            this.checkBoxPng.Location = new System.Drawing.Point(164, 51);
            this.checkBoxPng.Name = "checkBoxPng";
            this.checkBoxPng.Size = new System.Drawing.Size(48, 16);
            this.checkBoxPng.TabIndex = 4;
            this.checkBoxPng.Text = ".png";
            this.checkBoxPng.UseVisualStyleBackColor = true;
            // 
            // checkBoxJpg
            // 
            this.checkBoxJpg.AutoSize = true;
            this.checkBoxJpg.Location = new System.Drawing.Point(217, 51);
            this.checkBoxJpg.Name = "checkBoxJpg";
            this.checkBoxJpg.Size = new System.Drawing.Size(48, 16);
            this.checkBoxJpg.TabIndex = 4;
            this.checkBoxJpg.Text = ".jpg";
            this.checkBoxJpg.UseVisualStyleBackColor = true;
            // 
            // checkBoxCs
            // 
            this.checkBoxCs.AutoSize = true;
            this.checkBoxCs.Location = new System.Drawing.Point(266, 51);
            this.checkBoxCs.Name = "checkBoxCs";
            this.checkBoxCs.Size = new System.Drawing.Size(42, 16);
            this.checkBoxCs.TabIndex = 4;
            this.checkBoxCs.Text = ".cs";
            this.checkBoxCs.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Location = new System.Drawing.Point(105, 115);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(423, 48);
            this.textBox1.TabIndex = 5;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBox1_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "需要导出文件";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(453, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OtherTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 241);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}