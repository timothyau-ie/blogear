using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using System.Threading;

namespace blocal
{
    class Nicebrowser
    {
        private mshtml.IHTMLDocument2 doc;
        private WebBrowser webBrowser1;

        public Nicebrowser(ref WebBrowser webBrowser1)
        {
            this.webBrowser1 = webBrowser1;
            this.webBrowser1.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            try
            {
                doc = (mshtml.IHTMLDocument2)webBrowser1.Document.DomDocument;
                doc.designMode = "On";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.GetType().ToString() + ", " + e.Message, "Blogear");
            }
        }


        public void execCommand(string cmdID, bool showUI, object value)
        {
            if (doc != null)
                doc.execCommand(cmdID, showUI, value);
        }

        public void replace_with(string htmlstr)
        {
            mshtml.IHTMLTxtRange txt = (mshtml.IHTMLTxtRange)doc.selection.createRange();
            txt.pasteHTML(htmlstr);
        }

        public string get_selection_html()
        {
            mshtml.IHTMLTxtRange txt = (mshtml.IHTMLTxtRange)doc.selection.createRange();
            return txt.htmlText;
        }

        public string get_selection_text()
        {
            mshtml.IHTMLTxtRange txt = (mshtml.IHTMLTxtRange)doc.selection.createRange();
            return txt.text;
        }

        public string get_html()
        {
            if (doc == null)
                return "";
            return doc.body.innerHTML;
        }

        public void set_html(string s)
        {
            doc.body.innerHTML = s;
        }

        public void refresh(string bgcolor, string bgpic)
        {
            doc.body.style.backgroundColor = bgcolor;
            doc.body.style.backgroundImage = "url(" + bgpic + ")";
        }

    }

    class Browserkeeper
    {
        Nicebrowser nb;
        TabControl tc;
        public string path;
        public TreeNode parentnode;
        TextBox texthtml;
        string blogear;
        string htmlbody;

        string cdate;
        string mdate;
        int mood;
        int weather;
        string bgcolor;
        string bgpic;

        bool modified_flag;

        List<ToolStripMenuItem> toolmoods;
        List<ToolStripMenuItem> toolweathers;
        ToolStripSplitButton buttonmood;
        ToolStripSplitButton buttonweather;

        ImageList iconsmood;
        ImageList iconsweather;

        public string currentuser = "";

        public Browserkeeper(Nicebrowser nb, TabControl tc, ImageList iconsmood, ImageList iconsweather, ToolStripSplitButton buttonmood, ToolStripSplitButton buttonweather, ref TextBox texthtml, List<ToolStripMenuItem> toolmoods, List<ToolStripMenuItem> toolweathers)
        {
            this.nb = nb;
            this.tc = tc;
            this.toolmoods = toolmoods;
            this.toolweathers = toolweathers;
            this.buttonmood = buttonmood;
            this.buttonweather = buttonweather;
            this.iconsmood = iconsmood;
            this.iconsweather = iconsweather;
            this.texthtml = texthtml;
            tc.Enabled = false;
            tc.SelectedIndex = 0;
            path = "";
        }

        public void newpage(string path, string currentuser, TreeNode parentnode)
        {
            this.currentuser = currentuser;
            
            /* record page dates */

            string file = "<!--<cdate>"
                + dt_str(DateTime.Today)
                + "</cdate><mdate></mdate><mood>9</mood><weather>6</weather>"
                + "<bgcolor></bgcolor><bgpic></bgpic>gear-->\n<div></div>";
            File.WriteAllText(Application.StartupPath + "\\" + path, file, Encoding.UTF8);
            load(path, currentuser, parentnode);
        }

        public bool load(string path, string currentuser, TreeNode parentnode)
        {
            Progressshow.start();

            //this happens earliest to check a valid file
            string file = "";
            try
            {
                file = File.ReadAllText(Application.StartupPath + "\\" + path, Encoding.UTF8);
            }
            catch (Exception)
            {
                return false;
            }

            Progressshow.value(20);

            if (file.IndexOf("gear-->") == -1)
                return false;

            this.currentuser = currentuser;

            //this must happen early, else trigger something worse...
            tc.SelectedIndex = 0;

            this.path = path;
            this.parentnode = parentnode;
            tc.Enabled = true;
            
            int separation = file.IndexOf("gear-->") + 7;
            htmlbody = file.Substring(separation);
            try
            {
                blogear = file.Substring(0, separation);
                blogear = blogear.Substring(4, blogear.Length - 4 - 7);
                XmlDocument xmldoc = new XmlDocument();
                XmlElement eblog = xmldoc.CreateElement("blogear");
                eblog.InnerXml = blogear;
                cdate = eblog["cdate"].InnerText;
                mdate = eblog["mdate"].InnerText;
                mood = Convert.ToInt16(eblog["mood"].InnerText);
                weather = Convert.ToInt16(eblog["weather"].InnerText);
                bgcolor = eblog["bgcolor"].InnerText;
                bgpic = eblog["bgpic"].InnerText;
            }
            catch (Exception) { }
            toolmenuchosen(toolmoods[mood]);
            toolmenuchosen(toolweathers[weather]);

            Progressshow.value(50);

            nb.refresh(bgcolor, bgpic);

            Progressshow.value(70);

            htmlbody = path_decode(htmlbody);

            nb.set_html(htmlbody);

            //do a refresh to prevent unnecessary prompt for saving an unmodified page
            switchpage(1);
            htmlbody = texthtml.Text;

            modified_flag = false;

            Progressshow.value(90);
            Progressshow.value(100);
            Progressshow.end();

            return true;
        }

        public string getblogear(string path, string element)
        {
            string s;
            try
            {
                string file = File.ReadAllText(path, Encoding.UTF8);
                int separation = file.IndexOf("gear-->") + 7;
                blogear = file.Substring(0, separation);
                blogear = blogear.Substring(4, blogear.Length - 4 - 7);
                XmlDocument xmldoc = new XmlDocument();
                XmlElement eblog = xmldoc.CreateElement("blogear");
                eblog.InnerXml = blogear;
                s = eblog[element].InnerText;
            }
            catch(Exception) { }
            return blogear;
        }

        private string extensionof(string file)
        {
            return file.Substring(file.IndexOf('.') + 1);
        }

        public void save()
        {
            modified_flag = false;
            mdate = dt_str(DateTime.Today);
            if (tc.SelectedIndex == 0)
                switchpage(1);
            htmlbody = texthtml.Text;

            string html = form_html();
            path_encode(html);
            File.WriteAllText(Application.StartupPath + "\\" + path, html, Encoding.UTF8);

            /* calendar */
        }

        private string path_encode(string html)
        {
            if (html == null)
                return "";
            string[] patharr = path.Split(new string[]{"\\"}, StringSplitOptions.RemoveEmptyEntries);
            //maybe Timothy(must have), Martial Arts, wing chun.html (remove)
            for (int i = patharr.Length - 2; i >= 0; i--)
            {
                string pre = Application.StartupPath + "\\";
                for (int j = 0; j <= i; j++)
                    pre = pre + patharr[j] + "\\";
                string post = "";
                for (int j = 1; j <= (patharr.Length - 2) - i; j++)
                    post = post + "../";
                html = specialReplace(html, "=\"" + urlencoder(pre), "=\"" + post); //add " at beginning to prevent swapping paths that are not embed, but user typed
                html = specialReplace(html, "=\"" + pre, "=\"" + post);
                html = html.Replace("about:", "");
            }
            
            return html;
        }

        private string path_decode(string html)
        {
            if (html == null)
                return "";

            //only capable to solve images outside of "martial arts"
            string[] patharr = path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            //maybe Timothy(must have), Martial Arts, wing chun.html (remove)
            for (int i = 0; i <= patharr.Length - 2;  i++)
            {
                string pre = Application.StartupPath + "\\";
                for (int j = 0; j <= i; j++)
                    pre = pre + patharr[j] + "\\";
                string post = "";
                for (int j = 1; j <= (patharr.Length -2) - i; j++)
                    post = post + "../";
                if (post != "")
                    html = specialReplace(html, "=\"" + post, "=\"" + pre);
            }

            string cutpath = path;
            while (!cutpath.EndsWith("\\"))
                cutpath = cutpath.Substring(0, cutpath.Length - 1);
            //to solve those as neighbours or children folders
            string path2 = "";
            string pathlists = recursive_find_images(parentnode, cutpath, path2);
            string[] pathlistarr = pathlists.Split(new string[]{";;"}, StringSplitOptions.RemoveEmptyEntries);
            foreach(string pathlisting in pathlistarr)
            {
                html = specialReplace(html, quotes(pathlisting), quotes(Application.StartupPath + "\\" + cutpath + pathlisting));
            }

            return html;
        }

        private string specialReplace(string content, string oldValue, string newValue)
        {
            content = content.Replace(oldValue.Replace('/', '\\'), newValue);
            content = content.Replace(oldValue.Replace('\\', '/'), newValue);
            return content;
        }

        private string quotes(string s)
        {
            return "\"" + s + "\"";
        }

        private string recursive_find_images(TreeNode parentnode, string path, string path2)
        {
            string strreturn = "";
            if (parentnode == null)
                return "";
            foreach (TreeNode childnode in parentnode.Nodes)
            {
                string tag = (string)childnode.Tag;
                if (tag == "isfolder")
                    strreturn += recursive_find_images(childnode, path, path2 + childnode.Text + "\\") + ";;";
                else if (tag != "newfolder" && tag != "newpage")
                    strreturn += (path2 + childnode.Text + ";;");
            }
            return strreturn;
        }

        public string form_html()
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement eblog = xmldoc.CreateElement("blogear");

            XmlElement etemp = xmldoc.CreateElement("cdate");
            etemp.InnerText = cdate;
            eblog.AppendChild(etemp);
            etemp = xmldoc.CreateElement("mdate");
            etemp.InnerText = mdate;
            eblog.AppendChild(etemp);
            etemp = xmldoc.CreateElement("mood");
            etemp.InnerText = mood.ToString();
            eblog.AppendChild(etemp);
            etemp = xmldoc.CreateElement("weather");
            etemp.InnerText = weather.ToString();
            eblog.AppendChild(etemp);
            etemp = xmldoc.CreateElement("bgcolor");
            etemp.InnerText = bgcolor;
            eblog.AppendChild(etemp);
            etemp = xmldoc.CreateElement("bgpic");
            etemp.InnerText = bgpic;
            eblog.AppendChild(etemp);

            string htmlhead = "<html><head>";

            htmlhead = htmlhead + bgtext();

            htmlhead = htmlhead + "</head><body>";

            string htmlend = "</body></html>";

            string file = "<!--" + eblog.InnerXml + "gear-->" + htmlhead + htmlbody + htmlend;

            return file;
        }

        private string bgtext()
        {
            string htmlhead = "";
            if (bgcolor != "" || bgpic != "")
            {
                htmlhead = htmlhead + "<style type='text/css'> body {";
                if (bgcolor != "")
                    htmlhead = htmlhead + "background-color: " + bgcolor + ";";
                if (bgpic != "")
                    htmlhead = htmlhead + "background-image:url('" + bgpic + "');";
                htmlhead = htmlhead + "}</style>";
            }

            return htmlhead;
        }

        public void switchpage(int selindex)
        {
            if (selindex == 0)
            {
                nb.set_html(path_decode(texthtml.Text));
            }
            else
            {
                texthtml.Text = path_encode(nb.get_html());
            }
        }

        public void toolmenuchosen(ToolStripMenuItem tool)
        {
            for (int i = 0; i < toolmoods.Count; i++)
            {
                modified_flag = true;

                toolmoods[i].Checked = false;
                if (toolmoods[i] == tool)
                {
                    toolmoods[i].Checked = true;
                    mood = i;
                    buttonmood.Image = iconsmood.Images[i];
                }
            }
            for (int i = 0; i < toolweathers.Count; i++)
            {
                toolweathers[i].Checked = false;
                if (toolweathers[i] == tool)
                {
                    toolweathers[i].Checked = true;
                    weather = i;
                    buttonweather.Image = iconsweather.Images[i];
                }
            }
        }

        public void set_bgcolor(string bgcolor)
        {
            modified_flag = true;
            this.bgcolor = bgcolor;
            nb.refresh(bgcolor, bgpic);
        }

        public void set_bgpic(string bgpic)
        {
            modified_flag = true;
            this.bgpic = urlencoder(bgpic);
            nb.refresh(bgcolor, bgpic);
        }

        public void set_cdate(DateTime dtcdate)
        {
            modified_flag = true;
            cdate = dt_str(dtcdate);
        }

        public DateTime get_cdate()
        {
            return str_dt(cdate);
        }

        public DateTime str_dt(string date)
        {
            string[] arr = date.Split(new string[] { "/" }, StringSplitOptions.None);
            int day = Convert.ToInt32(arr[0]);
            int month = Convert.ToInt32(arr[1]);
            int year = Convert.ToInt32(arr[2]);
            DateTime dt = new DateTime(year, month, day);
            return dt;
        }

        public string dt_str(DateTime dtdate)
        {
            return dtdate.Day.ToString() + "/"
                + dtdate.Month.ToString() + "/"
                + dtdate.Year.ToString();
        }

        public bool ismodified()
        {
            if (nb.get_html() == null)
                return false;
            if (modified_flag)
                return true;
            if (tc.SelectedIndex == 0)
                switchpage(1);
            return !(htmlbody == texthtml.Text);
        }

        public bool have_bgpic()
        {
            return !(bgpic == "" || bgpic == "file:///");
        }

        public string urlencoder(string url)
        {
            url = System.Web.HttpUtility.UrlPathEncode(url);
            url = url.Replace('\\', '/');
            url = "file:///" + url;
            return url;
        }

        public void close()
        {
            this.path = "";
            this.buttonweather.Image = iconsweather.Images[6];
            this.buttonmood.Image = iconsmood.Images[9];
            this.nb.set_html("");
            tc.Enabled = false;
        }
    }
}
