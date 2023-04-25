using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using System.IO;

namespace blocal
{
    class VirDirectory
    {
        TreeView treeview;
        

        public VirDirectory(TreeView treeview)
        {
            this.treeview = treeview;
            string userstr = "";
            try
            {
                userstr = File.ReadAllText(Application.StartupPath + "\\users.ini", Encoding.UTF8);
            }
            catch(Exception)
            {
                userstr = "";
            }
            string[] arrusers = userstr.Split(new string[] { "%%" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string usericon in arrusers)
            {
                string[] arr = usericon.Split(new string[]{ ";;" }, StringSplitOptions.None);
                try
                {
                    string user = arr[0];
                    int icon = Convert.ToInt16(arr[1]);
                    if (Directory.Exists(Application.StartupPath + "\\" + user))
                        inituser(user, icon);
                }
                catch (Exception) { }
            }

            TreeNode new_user = addusernode(treeview, "(New User)", -1, false, null, "newuser");
            new_user.Tag = "newuser";
            treeview.CollapseAll();
        }

        private void inituser(string name, int icon)
        {
            TreeNode tuser = addusernode(treeview, name, icon, false, Menucontainer.dreexco, "isuser" + name);
            populateuser(tuser);
        }

        public void populateuser(TreeNode tuser)
        {
            tuser.Nodes.Clear();

            Progressshow.start();

            //recursive file directories
            Progressshow.value(40);
            populatenode(tuser);
            Progressshow.value(90);
            Progressshow.value(100);
            Progressshow.end();
        }

        public void populatenode(TreeNode parent)
        {
            if (parent == null)
            {
                MessageBox.Show("Internal Error, Category folder", "Blogear");
                return;
            }
            parent.Nodes.Clear();
            string dir = Application.StartupPath + "\\" + parent.FullPath;
            foreach (string subdir in Directory.GetDirectories(dir))
            {
                TreeNode son = addnode(parent, subdirname(subdir), false, "isfolder");
                populatenode(son);
            }

            TreeNode tfolder = addnode(parent, "", false, "newfolder");

            string imgexts = "|jpg|jpeg|jpe|gif|bmp|tiff|tif|png|";
            imgexts = imgexts + imgexts.ToUpper();
            foreach (string file in Directory.GetFiles(dir))
            {
               string ext = extensionof(file);
               if (ext == "html")
                    addnode(parent, subdirname(file), false, "ispage");
            }

            TreeNode tnewpage = addnode(parent, "", false, "newpage");

            foreach (string file in Directory.GetFiles(dir))
            {
                string ext = extensionof(file);
                if (imgexts.IndexOf("|" + ext + "|") != -1)
                    addnode(parent, subdirname(file), false, "isimage");
            }

            foreach (string file in Directory.GetFiles(dir))
            {
                string ext = extensionof(file);
                if (imgexts.IndexOf("|" + ext + "|") == -1)
                    addnode(parent, subdirname(file), false, "isfile");
            }
            
        }

        private string subdirname(string subdir)
        {
            while (subdir.IndexOf('\\') != -1)
                subdir = subdir.Substring(subdir.IndexOf('\\') + 1);
            return subdir;
        }

        private string extensionof(string file)
        {
            while (file.IndexOf('.') != -1)
                file = file.Substring(file.IndexOf('.') + 1);
            return file;
        }

        public void add_new_user(string name, int icon)
        {
            //add
            TreeNode tuser = addusernode(treeview, name, icon, true, Menucontainer.dreexco, "isuser" + name);
            if (tuser != null)
            {
                //ini file
                FileInfo f = new FileInfo(Application.StartupPath + "\\users.ini");
                StreamWriter sw = f.AppendText();
                sw.Write(name + ";;" + icon.ToString() + "%%");
                sw.Flush();
                sw.Close();

                string root = Application.StartupPath + "\\" + name + "\\";
                makepath(root);

                populateuser(tuser);

                tuser.Expand();
            }
            else
            {
                MessageBox.Show("User name exists!", "Blogear");
            }
        }

        public void add_new_folder(TreeNode currentnode, string name, string tag)
        {
            string trypath = currentnode.FullPath + "\\" + name;
            if (makepath(trypath))
            {
                populatenode(currentnode);
            }
            else
            {
                MessageBox.Show("Folder name Invalid.", "Blogear");
            }
        }

        public bool add_new_page(TreeNode currentnode, string name)
        {
            try
            {
                populatenode(currentnode);
                return true;
            }
            catch(Exception)
            {
                MessageBox.Show("Page error.", "Blogear");
                return false;
            }
        }

        public void add_new_files(TreeNode currentnode, string files)
        {
            string dest = Application.StartupPath + "\\" + currentnode.FullPath + "\\";
            string[] arr = files.Split(new string[] { ";;" }, StringSplitOptions.None);
            foreach (string file in arr)
            {
                string filename = file;
                while (filename.IndexOf('\\') != -1)
                    filename = filename.Substring(1);
                try
                {
                    if (File.Exists(dest + filename))
                    {
                        int count = 1;
                        int point = filename.IndexOf('.');
                        filename = filename.Substring(0, point) + "[**]"
                            + filename.Substring(point);
                        while (File.Exists(dest + filename.Replace("**", count.ToString())))
                            count++;
                        filename = filename.Replace("**", count.ToString());
                    }
                    File.Copy(file, dest + filename);
                }
                catch (Exception) { }
            }   
            populatenode(currentnode);
        }

        private bool makepath(string trypath)
        {
            if (!Directory.Exists(trypath))
            {
                DirectoryInfo dir1 = new DirectoryInfo(trypath);
                dir1.Create();
                return true;
            }
            else
            {
                return false;
            }
        }

        private TreeNode addnode(TreeNode parent, string text, bool insert, string tag)
        {
            string icon = "";
            if (tag == "newpage")
            {
                icon = "page_add.png";
                text = "(New Page)";
            }
            if (tag == "newfolder")
            {
                icon = "folder_add.png";
                text = "(New Folder)";
            }
            if (tag == "isfolder") icon = "folder.png";
            if (tag == "isimage") icon = "picture.png";
            if (tag == "isfile") icon = "cog.png";
            if (tag == "ispage") icon = "page.png";
            
            ContextMenuStrip conmen = null;
            string strdreexco = "isfolder";
            string strdop = "isimage;;ispage";
            string strd = "isfile";
            if (strdreexco.IndexOf(tag) != -1)
                conmen = Menucontainer.dreexco;
            if (strdop.IndexOf(tag) != -1)
                conmen = Menucontainer.dop;
            if (strd.IndexOf(tag) != -1)
                conmen = Menucontainer.d;
            foreach (TreeNode sibling in parent.Nodes)
                if (sibling.Text == text)
                    return null;
            TreeNode son;
            if (insert)
                son = parent.Nodes.Insert(parent.Nodes.Count - 1, text);
            else
                son = parent.Nodes.Add(text);
            son.Name = (string)tag;
            if (conmen != null)
                son.ContextMenuStrip = conmen;
            son.ImageKey = icon;
            son.SelectedImageKey = son.ImageKey;
            son.Tag = tag;
            return son;
        }

        private TreeNode addusernode(TreeView parent, string text, int icon, bool insert, ContextMenuStrip conmen, string tag)
        {
            foreach (TreeNode sibling in parent.Nodes)
                if (sibling.Text == text)
                    return null;
            TreeNode son;
            if (!insert)
                son = treeview.Nodes.Add(text);
            else
                son = treeview.Nodes.Insert(treeview.Nodes.Count - 1, text);
            string s = "_add";
            if (icon != -1)
                s = icon.ToString().Trim();
            son.ImageKey = "user" + s + ".png";
            son.SelectedImageKey = son.ImageKey;
            if (conmen != null)
                son.ContextMenuStrip = conmen;
            son.Tag = tag;
            son.Name = (string)tag;
            return son;
        }

        public void save_expand_configuration()
        {
            XmlDocument xdoc = new XmlDocument();
            XmlNode xroot = xdoc.CreateElement("ExpandConfig");
            xdoc.AppendChild(xroot);
            //collect the list of treenodes expanded, and save into xml in the same hierarchy
            foreach (TreeNode tnode in treeview.Nodes)
                fill_up_expand_config_xml(xroot, tnode);

            xdoc.Save("config.ini");
        }

        private void fill_up_expand_config_xml(XmlNode xparent, TreeNode tnode)
        {
             if (tnode.Nodes.Count > 0 && tnode.IsExpanded)
             {
                 XmlNode xnode = null;
                 try
                 {
                     xnode = xparent.OwnerDocument.CreateElement(tnode.Name + tnode.Text);
                 }
                 catch (Exception) { return; }
                 xparent.AppendChild(xnode);
                 foreach (TreeNode tchild in tnode.Nodes)
                     fill_up_expand_config_xml(xnode, tchild);
             }
        }

        public void load_expand_configuration()
        {
            XmlDocument xdoc = new XmlDocument();
            XmlNode xroot;
            try
            {
                xdoc.Load("config.ini");
                xroot = xdoc.SelectSingleNode("ExpandConfig");
            }
            catch (Exception) { return; }
            foreach (TreeNode tnode in treeview.Nodes)
                if (tnode.Nodes.Count > 0)
                {
                    XmlNode xnode = null;
                    try
                    {
                        xnode = xroot.SelectSingleNode(tnode.Name + tnode.Text);
                    }
                    catch (Exception) { }
                    if (xnode != null)
                        expand_treenodes_by_xml(xnode, tnode);
                }
        }

        private void expand_treenodes_by_xml(XmlNode xnode, TreeNode tnode)
        {
            tnode.Expand();
            foreach (TreeNode tchild in tnode.Nodes)
                if (tchild.Nodes.Count > 0)
                {
                    XmlNode xchild = null;
                    try
                    {
                        xchild = xnode.SelectSingleNode(tchild.Name + tchild.Text);
                    }
                    catch (Exception) { }
                    if (xchild != null)
                        expand_treenodes_by_xml(xchild, tchild);
                }
        }
    }

    class Menucontainer
    {
        static public ContextMenuStrip dreexco;
        static public ContextMenuStrip dop;
        static public ContextMenuStrip d;

        static public void init(ContextMenuStrip dr, ContextMenuStrip p, ContextMenuStrip del)
        {
            dreexco = dr;
            dop = p;
            d = del;
        }
    }

    class Progressshow
    {
        static ToolStripProgressBar progressbar;
        static public void init(ref ToolStripProgressBar p)
        {
            progressbar = p;
        }

        static public void start()
        {
            progressbar.Value = 0;
            progressbar.Visible = true;
        }

        static public void end()
        {
            progressbar.Visible = false;
        }

        static public void value(int i)
        {
            progressbar.Value = i;
        }
    }
}
