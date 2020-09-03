namespace SoftLiu_VSMainMenuTools
{
    partial class SplashLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashLoader));
            this.progressBarSplash = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBarSplash
            // 
            resources.ApplyResources(this.progressBarSplash, "progressBarSplash");
            this.progressBarSplash.Name = "progressBarSplash";
            this.progressBarSplash.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarSplash.BindingContextChanged += new System.EventHandler(this.progressBarSplash_BindingContextChanged);
            this.progressBarSplash.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.progressBarSplash_ChangeUICues);
            // 
            // SplashLoader
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBarSplash);
            this.Cursor = System.Windows.Forms.Cursors.PanNE;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashLoader";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplashLoader_FormClosing);
            this.Load += new System.EventHandler(this.SplashLoader_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBarSplash;
    }
}