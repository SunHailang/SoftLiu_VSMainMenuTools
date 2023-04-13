using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitExportForm
{
    public partial class GitExportForm : Form
    {
        public GitExportForm()
        {
            InitializeComponent();
            
        }

        private Configuration configuration = null;

        private void GitExportForm_Load(object sender, EventArgs e)
        {
            this.buttonExport.Visible = m_fileStatusDict.Count > 0;
            configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            this.textBoxGitPath.Text = GetConfiguration("GitPath");
            this.textBoxSvnPath.Text = GetConfiguration("SvnPath");
        }

        private void buttonGitPath_Click(object sender, EventArgs e)
        {
            // 打开文件夹路径
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选文件夹...";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                this.textBoxGitPath.Text = dialog.SelectedPath;
                SaveConfiguration("GitPath", dialog.SelectedPath);
            }
        }
        private string GetConfiguration(string key)
        {
            KeyValueConfigurationCollection collection = configuration.AppSettings.Settings;
            string[] keys = collection.AllKeys;
            if (keys != null && keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if(keys[i] == key)
                    {
                        return collection[key].Value;
                    }
                }
            }
            collection.Add(new KeyValueConfigurationElement(key, ""));
            configuration.Save();
            return collection[key].Value;
        }
        private void SaveConfiguration(string key, string value)
        {
            KeyValueConfigurationCollection collection = configuration.AppSettings.Settings;
            string[] keys = collection.AllKeys;
            if (keys != null && keys.Length > 0)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keys[i] == key)
                    {
                        collection[key].Value = value;
                        configuration.Save();
                        return;
                    }
                }
            }
            collection.Add(new KeyValueConfigurationElement(key, value));
            configuration.Save();            
        }
        private void buttonSvnPath_Click(object sender, EventArgs e)
        {
            // 打开文件夹路径
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选文件夹...";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                this.textBoxSvnPath.Text = dialog.SelectedPath;
                SaveConfiguration("SvnPath", dialog.SelectedPath);
            }
        }
        private void buttonGitStatus_Click(object sender, EventArgs e)
        {
            // 判断git仓库路径
            string workingDir = this.textBoxGitPath.Text;
            if (!Directory.Exists(workingDir) || !Directory.Exists(Path.Combine(workingDir, ".git")))
            {
                MessageBox.Show("Git仓库地址选择错误，请重新选择。", "路径错误", MessageBoxButtons.OK);
                return;
            }

            string strInput = $"git status";//Console.ReadLine();
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            // 工作目录
            p.StartInfo.WorkingDirectory = workingDir;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(strInput + "&exit");

            p.StandardInput.AutoFlush = true;

            dataGridViewGitInfo.Rows.Clear();

            //获取输出信息
            string strOuput = p.StandardOutput.ReadToEnd();

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();

            ShowTextBoxInfo(strOuput);

            ShowDataGridView(strOuput);

            this.buttonExport.Visible = m_fileStatusDict.Count > 0;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string exportPath = this.textBoxSvnPath.Text;
            if (!Directory.Exists(exportPath))
            {
                MessageBox.Show("Svn仓库地址选择错误，请重新选择。", "路径错误", MessageBoxButtons.OK);
                return;
            }
            m_fileExportList.Clear();
            foreach (var item in m_fileStatusDict)
            {
                if (item.Value == null || !item.Value.IsSelect || string.IsNullOrEmpty(item.Value.FilePath)) continue;
                m_fileExportList.Add(item.Value);
            }
            if (m_fileExportList.Count <= 0)
            {
                MessageBox.Show("没有选择可导出的文件，请重新选择。", "选择错误", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("是否确定导出所选择的文件？", "导出文件", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string gitPath = this.textBoxGitPath.Text;
                for (int i = 0; i < m_fileExportList.Count; i++)
                {
                    FileStatusData data = m_fileExportList[i];
                    string sourcePath = Path.Combine(gitPath, data.FilePath);
                    string targetPath = Path.Combine(exportPath, data.FilePath);
                    if (data.RecordType != FileRecordType.DeleteType)
                    {
                        if (!File.Exists(sourcePath)) continue;

                        string dir = Path.GetDirectoryName(targetPath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        File.Copy(sourcePath, targetPath, true);
                    }
                    else
                    {
                        // 删除文件
                        if (File.Exists(targetPath))
                        {
                            File.Delete(targetPath);
                        }
                    }
                }
            }
        }

        private Dictionary<int, FileStatusData> m_fileStatusDict = new Dictionary<int, FileStatusData>();
        private List<FileStatusData> m_fileExportList = new List<FileStatusData>();
        private void ShowTextBoxInfo(string strOuput)
        {
            strOuput = strOuput.Replace("\r\n", Environment.NewLine)
                               .Replace("\n", Environment.NewLine);
            this.textBoxInfo.Text = strOuput;
        }
        private void ShowDataGridView(string strOuput)
        {
            strOuput = strOuput.Trim('\n');
            int startIndex = strOuput.IndexOf("\n\t");
            int endIndex = strOuput.LastIndexOf("\n\n");
            int untrackIndex = strOuput.LastIndexOf("Untracked files:");
            int strOutputLength = strOuput.Length;
            string addStrOutput = "";
            if (untrackIndex > 0)
            {
                addStrOutput = strOuput.Substring(untrackIndex, endIndex - untrackIndex);
                int addStartIndex = addStrOutput.IndexOf("\n\t");
                addStrOutput = addStrOutput.Substring(addStartIndex);

                strOuput = strOuput.Substring(startIndex + 2, untrackIndex - startIndex - 4);
            }
            else
            {
                strOuput = strOuput.Substring(startIndex + 2, endIndex - startIndex - 2);
            }

            //strOuput = strOuput.Replace("\n\t", Environment.NewLine);
            //addStrOutput = addStrOutput.Replace("\n\t", Environment.NewLine);
            m_fileStatusDict.Clear();
            dataGridViewGitInfo.ClearSelection();
            dataGridViewGitInfo.AutoGenerateColumns = false;

            if (!string.IsNullOrEmpty(addStrOutput))
            {
                string[] addSplits = addStrOutput.Split('\n', '\t');
                for (int i = 0; i < addSplits.Length; i++)
                {
                    AddDataGirdViewRow(true, addSplits[i]);
                }
            }

            string[] splits = strOuput.Split('\n', '\t');
            for (int i = 0; i < splits.Length; i++)
            {
                AddDataGirdViewRow(false, splits[i]);
            }
            if (dataGridViewGitInfo.Columns.Contains("IsSelect"))
            {
                return;
            }
            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "IsSelect";
            checkBoxCol.HeaderText = "";
            checkBoxCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //checkBoxCol.DefaultCellStyle.BackColor = Color.Yellow;
            dataGridViewGitInfo.Columns.Add(checkBoxCol);
        }

        private void AddDataGirdViewRow(bool isNew, string split)
        {
            if (string.IsNullOrEmpty(split)) return;
            string[] record = split.Split(':');
            if (record == null || (record.Length < 2 && !isNew)) return;
            string fileRecord = "";
            string filePath = "";
            if (!isNew)
            {
                fileRecord = record[0].Trim();
                filePath = record[1].Trim();
            }
            else
            {
                fileRecord = "newed";
                filePath = record[0].Trim();
            }
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileRecord)) return;
            FileRecordType type = GetFileRecord(fileRecord);
            if (type == FileRecordType.None) return;
            int row = dataGridViewGitInfo.Rows.Add();
            dataGridViewGitInfo.Rows[row].Cells[0].Value = fileRecord;
            dataGridViewGitInfo.Rows[row].Cells[1].Value = filePath;
            FileStatusData fileStatusData = new FileStatusData()
            {
                FilePath = filePath,
                IsSelect = false,
                RecordType = type
            };
            m_fileStatusDict[row] = fileStatusData;
        }

        private FileRecordType GetFileRecord(string record)
        {
            switch (record)
            {
                case "modified":
                    return FileRecordType.ModifyType;
                case "newed":
                    return FileRecordType.NewType;
                case "deleted":
                    return FileRecordType.DeleteType;
                default:
                    break;
            }
            return FileRecordType.None;
        }

       


        private void dataGridViewGitInfo_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            if (rowIndex >= 0 && columnIndex >= 0)
            {
                DataGridViewColumn columnData = dataGridViewGitInfo.Columns[columnIndex];
                switch (columnData.Name)
                {
                    case "IsSelect":
                        {
                            DataGridViewRow rowData = dataGridViewGitInfo.Rows[rowIndex];
                            DataGridViewCell checkBoxData = rowData.Cells[columnIndex];
                            bool select = Convert.ToBoolean(checkBoxData.EditedFormattedValue);
                            rowData.DefaultCellStyle.BackColor = select ? Color.Green : SystemColors.HighlightText;

                            if (m_fileStatusDict.TryGetValue(rowIndex, out FileStatusData data) && data != null)
                            {
                                data.IsSelect = select;
                            }
                            break;
                        }
                }
            }
        }
    }
}
