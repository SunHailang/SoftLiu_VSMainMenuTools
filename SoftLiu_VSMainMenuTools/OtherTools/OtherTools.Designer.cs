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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.AllowDrop = true;
            this.textBoxFilePath.Location = new System.Drawing.Point(55, 48);
            this.textBoxFilePath.Multiline = true;
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(194, 70);
            this.textBoxFilePath.TabIndex = 0;
            this.textBoxFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragDrop);
            this.textBoxFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxFilePath_DragEnter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(55, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OtherTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 267);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxFilePath);
            this.Name = "OtherTools";
            this.Text = "OtherTools";
            this.Load += new System.EventHandler(this.OtherTools_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Button button1;
    }
}