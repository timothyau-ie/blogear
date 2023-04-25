using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace blocal
{
    public partial class Form1 : Form
    {
        Nicebrowser nicebrowser;
        VirDirectory virdir;
        Browserkeeper browserkeeper;

        Tablehtml reftable;
        Form reffrmtable;

        string pictypes = "";

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            browserkeeper.switchpage(tabControl1.SelectedIndex);
        }

        #region inits

        public Form1()
        {
            InitializeComponent();
            
            
        }

        bool activated = false;
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (!activated)
            {

                nicebrowser = new Nicebrowser(ref webBrowser1);

                List<ToolStripMenuItem> toolmoods = new List<ToolStripMenuItem>();
                toolmoods.Add(toolmenumood0);
                toolmoods.Add(toolmenumood1);
                toolmoods.Add(toolmenumood2);
                toolmoods.Add(toolmenumood3);
                toolmoods.Add(toolmenumood4);
                toolmoods.Add(toolmenumood5);
                toolmoods.Add(toolmenumood6);
                toolmoods.Add(toolmenumood7);
                toolmoods.Add(toolmenumood8);
                toolmoods.Add(toolmenumood9);
                List<ToolStripMenuItem> toolweathers = new List<ToolStripMenuItem>();
                toolweathers.Add(toolmenuweather0);
                toolweathers.Add(toolmenuweather1);
                toolweathers.Add(toolmenuweather2);
                toolweathers.Add(toolmenuweather3);
                toolweathers.Add(toolmenuweather4);
                toolweathers.Add(toolmenuweather5);
                toolweathers.Add(toolmenuweather6);
                Menucontainer.init(condreexco, condop, cond);
                browserkeeper = new Browserkeeper(nicebrowser, tabControl1,
                    iconsmood, iconsweather, toolmood, toolweather, ref texthtml, toolmoods, toolweathers);
                current_font = 2;
                nicebrowser.execCommand("FontSize", false, current_font + 1);
                fontlist = new List<ToolStripMenuItem>();
                fontlist.Add(toolitem1);
                fontlist.Add(toolitem2);
                fontlist.Add(toolitem3);
                fontlist.Add(toolitem4);
                fontlist.Add(toolitem5);
                fontlist.Add(toolitem6);
                fontlist.Add(toolitem7);

                reftable = new Tablehtml();
                reffrmtable = new frmtable(ref reftable);

                tabControl1.Dock = DockStyle.Fill;

                string[] typearr = new string[] { "jpg", "jpe", "jpeg", "gif", "bmp", "tiff", "tif", "png" };
                string type1 = string.Join(",*.", typearr);
                string type2 = string.Join(";*.", typearr);
                type1 = "*." + type1;
                type2 = "*." + type2;
                pictypes = "Image Files (" + type1 + ")|" + type2;

                Progressshow.init(ref toolStripProgressBar1);
                virdir = new VirDirectory(treeView1);

                activated = true;
            }
        }



        #endregion

        #region tools
        private void toolsave_Click(object sender, EventArgs e)
        {
            browserkeeper.save();
            toolpreview.Visible = true;
            tmrpreview.Interval = 2000;
            tmrpreview.Enabled = true;
        }

        private void toolclose_Click(object sender, EventArgs e)
        {
            if (verify_ask_save())
                browserkeeper.close();
        }

        private void tooldate_Click(object sender, EventArgs e)
        {
            DayfromCalendar day = new DayfromCalendar();
            DialogResult dr = (new frmcalendar(ref day, browserkeeper.get_cdate(), browserkeeper.get_cdate(), null)).ShowDialog();
            if (dr == DialogResult.OK)
            {
                browserkeeper.set_cdate(day.d);
            }
        }

        private void toolmenumood(object sender, EventArgs e)
        {
            browserkeeper.toolmenuchosen((ToolStripMenuItem)sender);
        }

        private void toolmenuweather(object sender, EventArgs e)
        {
            browserkeeper.toolmenuchosen((ToolStripMenuItem)sender);
        }

        private void toolbold_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Bold", false, null);
        }

        private void toolitalic_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Italic", false, null);
        }

        private void toolunder_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Underline", false, null);
        }

        string currenttextcolor = "#FF0000";

        private void tooltextcolor_ButtonClick(object sender, EventArgs e)
        {
            nicebrowser.execCommand("ForeColor", false, currenttextcolor);
        }

        private void toolitemtextcolor_Click(object sender, EventArgs e)
        {
            currenttextcolor = dlg_hex();
            tooltextcolor_ButtonClick(null, null);
        }

        string currenthighlightcolor = "#FFFF00";

        private void toolhighlightcolor_ButtonClick(object sender, EventArgs e)
        {
            nicebrowser.execCommand("BackColor", false, currenthighlightcolor);
        }

        private void toolitemhighlightcolor_Click(object sender, EventArgs e)
        {
            currenthighlightcolor = dlg_hex();
            toolhighlightcolor_ButtonClick(null, null);
        }

        private void toolbackcolor_Click(object sender, EventArgs e)
        {
            browserkeeper.set_bgcolor("#" + dlg_hex());
        }

        private string dlg_hex()
        {
            ColorDialog colordlg = new ColorDialog();
            colordlg.ShowDialog();
            string r = String.Format("{0:x2}", colordlg.Color.R);
            string g = String.Format("{0:x2}", colordlg.Color.G);
            string b = String.Format("{0:x2}", colordlg.Color.B);
            colordlg.Dispose();
            return r + g + b;
        }

        int current_font = 2;
        List<ToolStripMenuItem> fontlist;

        private void toolfont_ButtonClick(object sender, EventArgs e)
        {
            nicebrowser.execCommand("FontSize", false, current_font + 1);
        }

        private void toolitem_Click(object sender, EventArgs e)
        {
            fontlist[current_font].Checked = false;
            for (int i = 0; i < fontlist.Count; i++)
                if (fontlist[i] == sender)
                {
                    current_font = i;
                    break;
                }
            fontlist[current_font].Checked = true;
            nicebrowser.execCommand("FontSize", false, current_font + 1);
        }

        private void toolimage_ButtonClick(object sender, EventArgs e)
        {
            toolitembrowseimage_Click(null, null);
        }

        private void toolitembrowseimage_Click(object sender, EventArgs e)
        {
            string imagepath = Application.StartupPath + "\\" + browserkeeper.path;
            while (!imagepath.EndsWith("\\"))
                imagepath = imagepath.Substring(0, imagepath.Length - 1);
            string file = browseforfile(pictypes, "Browse for Image", false, imagepath);
            if (file == "")
                return;
            file = checkimport(file, imagepath);
            file = browserkeeper.urlencoder(file);
            nicebrowser.execCommand("InsertImage", false, file);
        }

        private void toolitempasteclipboard_Click(object sender, EventArgs e)
        {
            string imagepath = Application.StartupPath + "\\" + browserkeeper.path;
            while (!imagepath.EndsWith("\\"))
                imagepath = imagepath.Substring(0, imagepath.Length - 1);
            string filename = check_filename_with_squarebrackets(imagepath, "clipboard.jpg");
            Image clipimage = Clipboard.GetImage();
            string file = imagepath + filename;
            clipimage.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);
            virdir.populatenode(browserkeeper.parentnode);
            file = browserkeeper.urlencoder(file);
            nicebrowser.execCommand("InsertImage", false, file);
        }

        private string checkimport(string file, string checkpath)
        {
            if (file == "")
                return "";
            //if not (image file is in a descendent folder of current folder or image file is in a ancestor folder)
            bool is_descendent = file.ToLower().StartsWith(checkpath.ToLower());
            bool is_ancestor = checkpath.ToLower().StartsWith(file.Substring(0, file.LastIndexOf('\\')).ToLower());
            if (!(is_descendent || is_ancestor))
            {
                string filename = file;
                while (filename.IndexOf('\\') != -1)
                    filename = filename.Substring(1);
                try
                {
                    filename = check_filename_with_squarebrackets(checkpath, filename);
                    File.Copy(file, checkpath + filename);
                }
                catch (Exception) { }
                file = checkpath + filename;
                virdir.populatenode(browserkeeper.parentnode);
            }
            return file;
        }

        private string check_filename_with_squarebrackets(string path, string file)
        {
            try
            {
                if (File.Exists(path + file))
                {
                    int count = 1;
                    int point = file.IndexOf('.');
                    file = file.Substring(0, point) + "[**]"
                        + file.Substring(point);
                    while (File.Exists(path + file.Replace("**", count.ToString())))
                        count++;
                    file = file.Replace("**", count.ToString());
                }
            }
            catch (Exception) { return "";  }
            return file;
        }

        private void enterPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = Interaction.InputBox("Web URL to an image", "Image URL", "", 100, 100);
            nicebrowser.execCommand("InsertImage", false, url);
        }

        private void toolitembgpic_Click(object sender, EventArgs e)
        {
            if (browserkeeper.have_bgpic())
            {
                browserkeeper.set_bgpic("");
            }
            else
            {
                string imagepath = Application.StartupPath + "\\" + browserkeeper.currentuser + "\\Images\\";
                string file = browseforfile("All Files|*.*", "Browse for Background Image", false, imagepath);
                if (file == "")
                    return;
                file = checkimport(file, imagepath);
                file = browserkeeper.urlencoder(file);
                browserkeeper.set_bgpic(file);
            }
        }

        private void tooltable_Click(object sender, EventArgs e)
        {
            //Tablehtml table = new Tablehtml();
            //DialogResult dr = (new frmtable(ref table)).ShowDialog();
            DialogResult dr = reffrmtable.ShowDialog();
            if (dr == DialogResult.OK)
            {
                nicebrowser.replace_with(reftable.s);
            }
        }

        private void toollink_ButtonClick(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Unlink", false, null);
            nicebrowser.execCommand("CreateLink", true, null);
        }

        private void toolitementer_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Unlink", false, null);
            nicebrowser.execCommand("CreateLink", true, null);
        }

        private void toolitemhighlight_Click(object sender, EventArgs e)
        {

            nicebrowser.execCommand("CreateLink", false, nicebrowser.get_selection_text());
        }

        private void toolitembrowse_Click(object sender, EventArgs e)
        {
            string imagepath = Application.StartupPath + "\\" + browserkeeper.path;
            while (!imagepath.EndsWith("\\"))
                imagepath = imagepath.Substring(0, imagepath.Length - 1);
            string file = browseforfile("All Files|*.*", "Browse for File", false, imagepath);
            if (file == "")
                return;
            file = checkimport(file, imagepath);
            file = browserkeeper.urlencoder(file);
            nicebrowser.execCommand("Unlink", false, null);
            nicebrowser.execCommand("CreateLink", false, file);
        }

        private void toollinkbookmark_Click(object sender, EventArgs e)
        {
            string bookmark = Interaction.InputBox("Name of bookmark? (e.g. \"Flowers\")", "Bookmark", "", 100, 100);
            if (bookmark != "")
            {
                nicebrowser.execCommand("Unlink", false, null);
                nicebrowser.execCommand("CreateLink", false, "#" + bookmark);
            }
        }

        private void toolremovelink_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Unlink", false, null);
        }

        private void toolbookmark_Click(object sender, EventArgs e)
        {
            string mark = Interaction.InputBox("Input a name for bookmark:", "Blogear", "", 100, 100);
            if (mark != "")
                nicebrowser.execCommand("CreateBookmark", false, mark);
        }

        private void toolindent_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Indent", false, null);
        }

        private void tooloutdent_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Outdent", false, null);
        }

        private void toolleft_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("JustifyLeft", false, null);
        }

        private void toolcentre_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("JustifyCenter", false, null);
        }

        private void toolright_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("JustifyRight", false, null);
        }

        private void toololist_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("InsertOrderedList", false, null);
        }

        private void toolulist_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("InsertUnorderedList", false, null);
        }

        private void toolline_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("InsertHorizontalRule", false, null);
        }

        private void toolmarquee_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("InsertMarquee", false, null);
        }

        private void toolpreview_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\" + browserkeeper.path);
        }

        private void tmrpreview_Tick(object sender, EventArgs e)
        {
            toolpreview.Visible = false;
            tmrpreview.Enabled = false;
        }

        private void toolinsert_Click(object sender, EventArgs e)
        {
            nicebrowser.replace_with(Interaction.InputBox("Code insertion (e.g. <br />):",
                    "Blogear", "", 100, 100));
        }

        private void toolundo_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Undo", false, null);
        }

        private void toolredo_Click(object sender, EventArgs e)
        {
            nicebrowser.execCommand("Redo", false, null);
        }

        #endregion

        #region tree
        TreeNode currentnode = null;



        private void treeselect()
        {
            if (currentnode == null)
                return;
            string tag = (string)currentnode.Tag;
            treeactionhub("click", null);
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                treeactionhub("delete", null);
            if (e.KeyCode == Keys.Enter)
            {
                currentnode = treeView1.SelectedNode;
                treeactionhub("click", null);
                treeactionhub("dblclick", null);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (currentnode == null)
                return;
            string tag = (string)currentnode.Tag;
            treeactionhub("dblclick", null);
        }

        private void newpage(TreeNode parent)
        {
            if (!verify_ask_save())
                return;

            string name;
            do
            {
                name = Interaction.InputBox("Input new page name (e.g. \"Biking\"):",
                    "Blogear", DateTime.Now.Year.ToString()
                        + DateTime.Now.Month.ToString("00")
                        + DateTime.Now.Day.ToString("00"), 100, 100);
            }
            while (name == "(New Page)");

            if (name == "")
                return;

            settitle(name + ".html");

            string fullpath = parent.FullPath + "\\" + name + ".html";
            if (!File.Exists(fullpath))
            {
                browserkeeper.newpage(fullpath, find_user(parent), parent);
                virdir.add_new_page(parent, name);
            }
            else
            {
                MessageBox.Show("Page name Invalid.", "Blogear");
            }
        }

        private void openpage(TreeNode node)
        {
            string path = node.FullPath;
            if (browserkeeper.path == path)
                return;
            if (verify_ask_save())
            {
                string filename = path;
                while (filename.IndexOf('\\') != -1)
                    filename = filename.Substring(1);
                if (!(browserkeeper.load(path, find_user(node), node.Parent)))
                {
                    node.Nodes.Remove(node);
                    return;
                }
                settitle(filename);
            }
        }

        private void openimage(TreeNode node)
        {
            string path = Application.StartupPath + "\\" + node.FullPath;
            if (File.Exists(path))
                (new frmimage(path, node.Text)).ShowDialog();
            else
                node.Parent.Nodes.Remove(node);
        }

        private void openfile(TreeNode node)
        {
            string path = Application.StartupPath + "\\" + node.FullPath;
            if (File.Exists(path))
                System.Diagnostics.Process.Start(path);
            else
                node.Parent.Nodes.Remove(node);
        }

        private string find_user(TreeNode node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
                if (node.Tag != null)
                    if (((string)node.Tag).StartsWith("isuser"))
                    {
                        return node.Text;
                    }
            }
            return "";
        }

        //return true if continue to load
        //return false if no load
        private bool verify_ask_save()
        {
            if (browserkeeper.ismodified())
            {
                DialogResult dr = MessageBox.Show("Do you want to save the changes?",
                    "Up to you", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    browserkeeper.save();
                    return true;
                }
                if (dr == DialogResult.No)
                    return true;
                if (dr == DialogResult.Cancel)
                    return false;
            }
            return true;
        }

        private void settitle(string s)
        {
            this.Text = "Simple Blogear - " + s;
        }



        #endregion

        private void treeactionhub(string command, TreeNode node)
        {
            if (node == null)
                node = currentnode;
            string tag = (string)(node.Tag);
            if (command == "dblclick")
            {
                if (tag == "newuser")
                {
                    Userinfo user = new Userinfo();
                    DialogResult dr = (new frmuser(ref user, "New User")).ShowDialog();
                    if (dr == DialogResult.OK)
                        virdir.add_new_user(user.name, user.icon);
                }
                else if (tag == "newfolder")
                {
                    string forbidname, askname;
                    forbidname = "(New Folder)";
                    askname = "folder";
                    string name;
                    do
                    {
                        name = Interaction.InputBox("Input new " + askname + " name:", "Blogear", "", 100, 100);
                    }
                    while (name == forbidname);
                    if (name == "")
                        return;
                    virdir.add_new_folder(node.Parent, name, tag);
                }
                else if (tag == "newpage")
                    newpage(node.Parent);
                else if (tag == "isimage")
                    openimage(node);
                else if (tag == "isfile")
                    openfile(node);
            }
            else if (command == "click")
            {
                if (tag == "ispage")
                    openpage(node);
            }
            else if (command == "delete")
            {
                bool prompted = false;
                DialogResult dr = DialogResult.Yes;
                if (browserkeeper.path.StartsWith(node.FullPath))
                {
                    dr = MessageBox.Show("The page you are editing is affected. Continue deletion?", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    prompted = true;
                    if (dr == DialogResult.No)
                        return;
                    else
                        browserkeeper.close();
                }
                if (tag.StartsWith("isuser"))
                {
                    if (!prompted)
                        dr = MessageBox.Show("You are trying to delete a user. Are you sure?", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string inipath = Application.StartupPath + "\\users.ini";
                        string userstr = File.ReadAllText(inipath, Encoding.UTF8);
                        string username = tag.Substring("isuser".Length);
                        int userptr = userstr.IndexOf(username);
                        int dellength = username.Length + ";;0%%".Length;
                        userstr = userstr.Substring(0, userptr) + userstr.Substring(userptr + dellength);
                        File.WriteAllText(inipath, userstr, Encoding.UTF8);
                        recyclefolder(Application.StartupPath + "\\" + node.FullPath);
                        treeView1.Nodes.Remove(node);
                    }
                }
                else if (tag == "isfolder")
                {
                    if (!prompted)
                        dr = MessageBox.Show("Delete selection?", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        recyclefolder(Application.StartupPath + "\\" + node.FullPath);
                        node.Parent.Nodes.Remove(node);
                    }
                }
                else if (tag == "isimage" || tag == "isfile" || tag == "ispage")
                {
                    if (!prompted)
                        dr = MessageBox.Show("Delete selection?", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        recyclefile(Application.StartupPath + "\\" + node.FullPath);
                        node.Parent.Nodes.Remove(node);
                    }
                }
            }
            else if (command == "edit")
            {
                if (browserkeeper.path.StartsWith(node.FullPath))
                {
                    DialogResult dr = MessageBox.Show("The page you are editing is affected. Continue renaming?", "Rename", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.No)
                        return;
                    else
                        browserkeeper.close();
                }

                if (tag.StartsWith("isuser"))
                {
                    string inipath = Application.StartupPath + "\\users.ini";
                    string userstr = File.ReadAllText(inipath, Encoding.UTF8);
                    int ptr = userstr.IndexOf(node.Text);

                    Userinfo user = new Userinfo(node.Text,
                        Convert.ToInt16(userstr.Substring(ptr + node.Text.Length + 2, 1)));
                    DialogResult dr = (new frmuser(ref user, "Edit User")).ShowDialog();

                    //check duplicate names
                    foreach (TreeNode tuser in treeView1.Nodes)
                        if (tuser != node && tuser.Text == user.name)
                        {
                            MessageBox.Show("Invalid name!", "Blogear");
                            return;
                        }

                    if (dr == DialogResult.OK)
                    {
                        string replaced = node.Text + ";;0";
                        string replacing = user.name + ";;" + user.icon.ToString();
                        userstr = userstr.Substring(0, ptr)
                            + replacing
                            + userstr.Substring(ptr + replaced.Length);
                        File.WriteAllText(inipath, userstr, Encoding.UTF8);

                        //rename folder
                        if (node.Text != user.name)
                        {
                            string path = Application.StartupPath + "\\";
                            string oldname = node.Text;
                            Directory.Move(path + oldname, path + ".foldertemp");
                            Directory.Move(path + ".foldertemp", path + user.name);
                        }

                        node.ImageKey = "user" + user.icon.ToString() + ".png";
                        node.SelectedImageKey = node.ImageKey;
                        node.Text = user.name;
                    }
                }
                else if (tag == "isfolder")
                {
                    string forbidname, askname, oldname;
                    forbidname = "(New Folder)";
                    askname = "folder";
                    oldname = node.Text;

                    string name;
                    do
                    {
                        name = Interaction.InputBox("Change " + askname + " name to:", "Blogear", oldname, 100, 100);
                    }
                    while (name == forbidname);
                    if (name == "")
                        return;
                    string path = Application.StartupPath + "\\" + node.Parent.FullPath + "\\";
                    Directory.Move(path + oldname, path + ".foldertemp");
                    Directory.Move(path + ".foldertemp", path + name);
                    node.Text = name;
                }
                else if (tag == "ispage" || tag == "isfile" || tag == "isimage")
                {
                    string forbidname, askname;
                    forbidname = "(New Page)";
                    if (tag == "ispage")
                        askname = "page";
                    else if (tag == "isfile")
                        askname = "file";
                    else // if (tag == "isimage")
                        askname = "image";

                    string fullname = node.Text;
                    int ptr = fullname.IndexOf('.');
                    string ext = fullname.Substring(ptr);
                    string oldname = fullname.Substring(0, ptr);

                    string name;
                    do
                    {
                        name = Interaction.InputBox("Change " + askname + " name to:", "Blogear", oldname, 100, 100);
                    }
                    while (name == forbidname);
                    if (name == "")
                        return;
                    string path = Application.StartupPath + "\\" + node.Parent.FullPath + "\\";

                    Directory.Move(path + oldname + ext, path + ".foldertemp");
                    Directory.Move(path + ".foldertemp", path + name + ext);
                    node.Text = name + ext;
                }
            }
            else if (command == "refresh")
            {
                virdir.populatenode(node);
                node.Collapse();
            }
            else if (command == "expand")
            {
                node.ExpandAll();
            }
            else if (command == "collapse")
            {
                node.Collapse(false);
            }
            else if (command == "open")
            {
                if (tag == "ispage")
                {
                    openpage(node);
                }
                else
                {
                    openimage(node);
                }
            }
            else if (command == "impimages" || command == "impfiles")
            {
                string title = "";
                string types = "All Files (*.*)|*.*";
                if (command == "impimages")
                {
                    title = "Import Image(s)";
                    types = pictypes;
                }
                else
                    title = "Import File(s)";
                string files = browseforfile(types, title, true, "");
                if (files != "")
                    virdir.add_new_files(node, files);
            }
        }

        private void recyclefile(string file)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file,
                    UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception) { }
        }

        private void recyclefolder(string path)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path,
                    UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            catch (Exception) { }
        }

        //input string ext is the exact "XML Files|*.xml|UML Files|*.uml"
        //output string if multiple, can be separated by ";;"
        public string browseforfile(string ext, string title, bool multi, string path)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = title;
            if (path == "")
                fDialog.InitialDirectory = Application.StartupPath;
            else
                fDialog.InitialDirectory = path;
            fDialog.AddExtension = true;
            fDialog.CheckFileExists = true;
            fDialog.CheckPathExists = true;
            fDialog.Multiselect = multi;
            fDialog.Filter = ext;
            if (fDialog.ShowDialog() != DialogResult.OK)
                return "";
            if (multi)
                return string.Join(";;", fDialog.FileNames);
            else
                return fDialog.FileName;
        }

        //private void enablecontrols(bool enabled)
        //{
        //    menuStrip1.Enabled = enabled;
        //    treeView1.Enabled = enabled;
        //    tabControl1.Visible = enabled;
        //}

        #region strips

        TreeNode rightclicknode = null;

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            rightclicknode = e.Node;
            if (e.Button == MouseButtons.Left)
            {
                currentnode = e.Node;
                treeselect();
            }
        }

        private void stripedit_Click(object sender, EventArgs e)
        {
            treeactionhub("edit", rightclicknode);
        }

        private void stripdel_Click(object sender, EventArgs e)
        {
            treeactionhub("delete", rightclicknode);
        }

        private void stripref_Click(object sender, EventArgs e)
        {
            treeactionhub("refresh", rightclicknode);
        }

        private void stripexp_Click(object sender, EventArgs e)
        {
            treeactionhub("expand", rightclicknode);
        }

        private void stripcol_Click(object sender, EventArgs e)
        {
            treeactionhub("collapse", rightclicknode);
        }

        private void stripopen_Click(object sender, EventArgs e)
        {
            treeactionhub("open", rightclicknode);
        }

        private void stripimpimages_Click(object sender, EventArgs e)
        {
            treeactionhub("impimages", rightclicknode);
        }

        private void stripimpfiles_Click(object sender, EventArgs e)
        {
            treeactionhub("impfiles", rightclicknode);
        }

        #endregion


        string searchstring = "";

        private void texthtml_KeyDown(object sender, KeyEventArgs e)
        {
            bool ctrl = false;
            if (e.Control == true && e.Alt == false && e.Shift == false)
                ctrl = true;
            if (ctrl)
            {
                if (e.KeyCode == Keys.A)
                {
                    texthtml.SelectionStart = 0;
                    texthtml.SelectionLength = texthtml.Text.Length;
                }
                if (e.KeyCode == Keys.F)
                {
                    searchstring = Interaction.InputBox("Find what:", "Find", searchstring, 100, 100);

                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
                    int foundindex = texthtml.SelectionStart + 1;
                    foundindex = culture.CompareInfo.IndexOf(texthtml.Text, searchstring, foundindex, System.Globalization.CompareOptions.IgnoreCase);
                    if (foundindex >= 0)
                    {
                        texthtml.SelectionStart = texthtml.Text.IndexOf("\n", foundindex) + 2;
                        texthtml.SelectionLength = 0;
                        texthtml.ScrollToCaret(); // scroll past selection
                        texthtml.SelectionStart = foundindex;
                        texthtml.SelectionLength = searchstring.Length;
                        foundindex += 1; // advance past selection
                        //				_txtControl.Invalidate();
                    }
                    else
                    {
                        MessageBox.Show("Reached the end of the document.", "Blogear");
                        foundindex = 0;
                    }
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == true)
                Hide();
            else
                Show();
            //WindowState = FormWindowState.Normal;
        }

        private void stripnotifyopen_Click(object sender, EventArgs e)
        {
            if (this.Visible == true)
                Hide();
            else
                Show();
            //WindowState = FormWindowState.Normal;
        }

        private void stripnotifyexit_Click(object sender, EventArgs e)
        {
            endprogram();
        }

        private void mainexit_Click(object sender, EventArgs e)
        {
            endprogram();
        }

        private void mainhide_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void mainkeyword_Click(object sender, EventArgs e)
        {
            frmKeyword.ShowInstance();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!verify_ask_save())
                e.Cancel = true;
            else
                virdir.save_expand_configuration();
        }

        private void endprogram()
        {
            if (verify_ask_save())
            {
                virdir.save_expand_configuration();
                Application.Exit();
            }
        }

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            virdir.load_expand_configuration();
            treeView1.MouseMove -= treeView1_MouseMove;
        }

 








    }
}