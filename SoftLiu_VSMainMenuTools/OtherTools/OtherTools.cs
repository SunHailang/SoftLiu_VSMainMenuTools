using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SoftLiu_VSMainMenuTools.OtherTools
{
    public partial class OtherTools : Form
    {
        private Dictionary<string, CheckBox> m_checkBoxDic = new Dictionary<string, CheckBox>();
        public OtherTools()
        {
            InitializeComponent();
        }

        private void OtherTools_Load(object sender, EventArgs e)
        {
            m_checkBoxDic.Clear();
            m_checkBoxDic.Add(".meta", checkBoxMeta);
            m_checkBoxDic.Add(".png", checkBoxPng);
            m_checkBoxDic.Add(".jpg", checkBoxJpg);
            m_checkBoxDic.Add(".cs", checkBoxCs);
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

        private void btnSure_Click(object sender, EventArgs e)
        {
            List<string> delStr = new List<string>();
            delStr.Clear();
            foreach (var item in m_checkBoxDic)
            {
                if (item.Value.Checked)
                {
                    delStr.Add(item.Key);
                }
            }
            toolStripProgressBarDelete.Value = 0;
            DirectoryInfo info = new DirectoryInfo(textBoxFilePath.Text);
            List<FileInfo> list = Delete(info, delStr);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (File.Exists(list[i].FullName))
                    {
                        list[i].Delete();
                    }
                    int value = (int)(100.0f / list.Count * i);
                    toolStripProgressBarDelete.Value = value < 0 ? 0 : value >= 100 ? 100 : value;
                }
            }
            toolStripProgressBarDelete.Value = 100;
        }

        private List<FileInfo> Delete(DirectoryInfo info, List<string> ends)
        {
            List<FileInfo> fileList = new List<FileInfo>();
            foreach (var item in info.GetDirectories())
            {
                fileList.AddRange(Delete(item, ends));
            }
            foreach (var item in info.GetFiles())
            {
                if (item.IsReadOnly)
                {
                    item.IsReadOnly = false;
                    //item.Attributes = FileAttributes.w;
                }
                string s = Path.GetExtension(item.FullName);
                if (ends.Contains(s.ToLower()))
                {
                    //item.Delete();
                    fileList.Add(item);
                }
            }
            return fileList;
        }
    }
}
