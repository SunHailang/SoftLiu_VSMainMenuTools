
namespace GitExportForm
{
    partial class GitExportForm
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
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.buttonGitStatus = new System.Windows.Forms.Button();
            this.textBoxGitPath = new System.Windows.Forms.TextBox();
            this.buttonGitPath = new System.Windows.Forms.Button();
            this.dataGridViewGitInfo = new System.Windows.Forms.DataGridView();
            this.FileRecord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonExport = new System.Windows.Forms.Button();
            this.labelGitPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSvnPath = new System.Windows.Forms.TextBox();
            this.buttonSvnPath = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGitInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInfo.Location = new System.Drawing.Point(11, 437);
            this.textBoxInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxInfo.Size = new System.Drawing.Size(819, 161);
            this.textBoxInfo.TabIndex = 0;
            // 
            // buttonGitStatus
            // 
            this.buttonGitStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGitStatus.Location = new System.Drawing.Point(755, 3);
            this.buttonGitStatus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGitStatus.Name = "buttonGitStatus";
            this.buttonGitStatus.Size = new System.Drawing.Size(75, 25);
            this.buttonGitStatus.TabIndex = 1;
            this.buttonGitStatus.Text = "获取Status";
            this.buttonGitStatus.UseVisualStyleBackColor = true;
            this.buttonGitStatus.Click += new System.EventHandler(this.buttonGitStatus_Click);
            // 
            // textBoxGitPath
            // 
            this.textBoxGitPath.Location = new System.Drawing.Point(97, 6);
            this.textBoxGitPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxGitPath.Name = "textBoxGitPath";
            this.textBoxGitPath.ReadOnly = true;
            this.textBoxGitPath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxGitPath.Size = new System.Drawing.Size(422, 21);
            this.textBoxGitPath.TabIndex = 2;
            // 
            // buttonGitPath
            // 
            this.buttonGitPath.Location = new System.Drawing.Point(526, 5);
            this.buttonGitPath.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGitPath.Name = "buttonGitPath";
            this.buttonGitPath.Size = new System.Drawing.Size(31, 21);
            this.buttonGitPath.TabIndex = 3;
            this.buttonGitPath.Text = "...";
            this.buttonGitPath.UseVisualStyleBackColor = true;
            this.buttonGitPath.Click += new System.EventHandler(this.buttonGitPath_Click);
            // 
            // dataGridViewGitInfo
            // 
            this.dataGridViewGitInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewGitInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGitInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileRecord,
            this.FilePath});
            this.dataGridViewGitInfo.Location = new System.Drawing.Point(10, 63);
            this.dataGridViewGitInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewGitInfo.Name = "dataGridViewGitInfo";
            this.dataGridViewGitInfo.RowTemplate.Height = 25;
            this.dataGridViewGitInfo.Size = new System.Drawing.Size(819, 370);
            this.dataGridViewGitInfo.TabIndex = 4;
            this.dataGridViewGitInfo.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewGitInfo_CellMouseUp);
            // 
            // FileRecord
            // 
            this.FileRecord.HeaderText = "记录";
            this.FileRecord.Name = "FileRecord";
            this.FileRecord.ReadOnly = true;
            // 
            // FilePath
            // 
            this.FilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FilePath.HeaderText = "路径";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(753, 34);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 25);
            this.buttonExport.TabIndex = 2;
            this.buttonExport.Text = "导出";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // labelGitPath
            // 
            this.labelGitPath.AutoSize = true;
            this.labelGitPath.Location = new System.Drawing.Point(8, 9);
            this.labelGitPath.Name = "labelGitPath";
            this.labelGitPath.Size = new System.Drawing.Size(83, 12);
            this.labelGitPath.TabIndex = 6;
            this.labelGitPath.Text = "GIT仓库路径：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "SVN仓库路径：";
            // 
            // textBoxSvnPath
            // 
            this.textBoxSvnPath.Location = new System.Drawing.Point(97, 34);
            this.textBoxSvnPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxSvnPath.Name = "textBoxSvnPath";
            this.textBoxSvnPath.ReadOnly = true;
            this.textBoxSvnPath.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBoxSvnPath.Size = new System.Drawing.Size(422, 21);
            this.textBoxSvnPath.TabIndex = 8;
            // 
            // buttonSvnPath
            // 
            this.buttonSvnPath.Location = new System.Drawing.Point(526, 33);
            this.buttonSvnPath.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSvnPath.Name = "buttonSvnPath";
            this.buttonSvnPath.Size = new System.Drawing.Size(31, 21);
            this.buttonSvnPath.TabIndex = 9;
            this.buttonSvnPath.Text = "...";
            this.buttonSvnPath.UseVisualStyleBackColor = true;
            this.buttonSvnPath.Click += new System.EventHandler(this.buttonSvnPath_Click);
            // 
            // GitExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 606);
            this.Controls.Add(this.buttonSvnPath);
            this.Controls.Add(this.textBoxSvnPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.labelGitPath);
            this.Controls.Add(this.buttonGitStatus);
            this.Controls.Add(this.dataGridViewGitInfo);
            this.Controls.Add(this.buttonGitPath);
            this.Controls.Add(this.textBoxGitPath);
            this.Controls.Add(this.textBoxInfo);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GitExportForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.GitExportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGitInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button buttonGitStatus;
        private System.Windows.Forms.TextBox textBoxGitPath;
        private System.Windows.Forms.Button buttonGitPath;
        private System.Windows.Forms.DataGridView dataGridViewGitInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label labelGitPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSvnPath;
        private System.Windows.Forms.Button buttonSvnPath;
    }
}

