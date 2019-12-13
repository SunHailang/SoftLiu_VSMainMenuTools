using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.OtherTools
{
    public partial class OtherTools : Form
    {
        public OtherTools()
        {
            InitializeComponent();
        }

        private void OtherTools_Load(object sender, EventArgs e)
        {

        }

        private void textBoxFilePath_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();//获得路径
            this.textBoxFilePath.Text = path;
        }

        private void textBoxFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All; //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Multiselect = true;//该值确定是否可以选择多个文件
            //dialog.Title = "请选择文件夹";
            //dialog.Filter = "所有文件(*.*)|*.*";
            //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    if (this.textBoxFilePath.Text.CompareTo(dialog.FileName) != 0)
            //    {
            //        this.textBoxFilePath.Text = dialog.FileName;
            //    }
            //}
            DirectoryInfo info = new DirectoryInfo(textBoxFilePath.Text);
            Delete(info, ".meta");
        }

        private void Delete(DirectoryInfo info, string ends)
        {
            foreach (var item in info.GetDirectories())
            {
                Delete(item, ends);
            }
            foreach (var item in info.GetFiles())
            {
                if (item.IsReadOnly)
                {
                    item.IsReadOnly = false;
                    //item.Attributes = FileAttributes.w;
                }
                if (item.FullName.EndsWith(ends))
                {
                    item.Delete();
                }                
            }
        }
    }
}
