
namespace WinFormsExcel
{
    partial class WinFormsExcel
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextLog = new System.Windows.Forms.RichTextBox();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnCSharp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextLog
            // 
            this.richTextLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextLog.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.richTextLog.Location = new System.Drawing.Point(13, 189);
            this.richTextLog.Name = "richTextLog";
            this.richTextLog.ReadOnly = true;
            this.richTextLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextLog.Size = new System.Drawing.Size(854, 168);
            this.richTextLog.TabIndex = 0;
            this.richTextLog.Text = "";
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(13, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 20);
            this.btnExcel.TabIndex = 1;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnCSharp
            // 
            this.btnCSharp.Location = new System.Drawing.Point(13, 38);
            this.btnCSharp.Name = "btnCSharp";
            this.btnCSharp.Size = new System.Drawing.Size(75, 23);
            this.btnCSharp.TabIndex = 2;
            this.btnCSharp.Text = "C#";
            this.btnCSharp.UseVisualStyleBackColor = true;
            this.btnCSharp.Click += new System.EventHandler(this.btnCSharp_Click);
            // 
            // WinFormsExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 367);
            this.Controls.Add(this.btnCSharp);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.richTextLog);
            this.Name = "WinFormsExcel";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextLog;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnCSharp;
    }
}

