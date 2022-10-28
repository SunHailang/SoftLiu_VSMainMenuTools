using System;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.UGUI.Tools
{
    public partial class ReaderPDFTools : Form
    {
        public ReaderPDFTools()
        {
            InitializeComponent();
        }

        private void ReaderPDFTools_Load(object sender, EventArgs e)
        {
            //PdfDocument pdf = new PdfDocument();
            //pdf.LoadFromFile("");
        }

        private void ReaderPDFTools_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instance.BackClose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "PDF文件(*.pdf)|*.pdf";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxFilePath.Text = dialog.FileName;
            }
        }
    }
}
