using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace blocal
{
    public partial class frmKeyword : Form
    {
        private static frmKeyword fk = null;

        public static frmKeyword ShowInstance()
        {
            if (fk == null)
            {
                fk = new frmKeyword();
                fk.Show();
                fk.BringToFront();
                fk.FormClosed += delegate { fk = null; };
            }
            return fk;

        }


        public frmKeyword()
        {
            InitializeComponent();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            if (txt_Expression.Text.Trim() == "")
                return;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath + txt_Directory.Text);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.html", System.IO.SearchOption.AllDirectories);
            List<string> queryMatchingFiles = new List<string>();
            if (!chk_CaseSensitive.Checked)
            {
                queryMatchingFiles = (from file in fileList
                                      let fileText = GetFileText(file.FullName)
                                      where fileText.ToUpper().Contains(txt_Expression.Text.ToUpper())
                                      select file.FullName).ToList();
            }
            else
            {
                queryMatchingFiles = (from file in fileList
                                      let fileText = GetFileText(file.FullName)
                                      where fileText.Contains(txt_Expression.Text)
                                      select file.FullName).ToList();
            }
            txt_Results.Text = "";
            if (queryMatchingFiles.Count() == 0)
            {
                txt_Results.Text = "None...";
                return;
            }
            foreach (string pathname in queryMatchingFiles)
            {
                string filename = pathname.Substring(Application.StartupPath.Length);
                txt_Results.Text += filename + "\r\n";
            }
        }

        private static string GetFileText(string name)
        {
            string fileContents = string.Empty;
            if (System.IO.File.Exists(name))
                fileContents = System.IO.File.ReadAllText(name);
            return fileContents;
        }

        private void txt_Expression_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_Search_Click(null, null);
        }

        private void btn_Directory_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder";
                dlg.SelectedPath = Application.StartupPath;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txt_Directory.Text = dlg.SelectedPath.Substring(Application.StartupPath.Length);
                }
            } 
        }
    }
}
