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
    public partial class frmuser : Form
    {
        Userinfo fuser;

        List<RadioButton> radiolist;

        public frmuser(ref Userinfo user, string title)
        {
            InitializeComponent();
            init_radios();

            this.Text = title;
            fuser = user;
            textBox2.Text = user.name;
            radiolist[user.icon].Checked = true;
        }

        private void init_radios()
        {
            radiolist = new List<RadioButton>();
            radiolist.Add(radioButton1);
            radiolist.Add(radioButton2);
            radiolist.Add(radioButton3);
            radiolist.Add(radioButton4);
            radiolist.Add(radioButton5);
            radiolist.Add(radioButton6);
            radiolist.Add(radioButton7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox2.Text == null)
            {
                MessageBox.Show("User Name Required.", "Blogear");
                return;
            }
            fuser.name = textBox2.Text;
            int i = 0;
            for (i = 0; i < radiolist.Count; i++)
                if (radiolist[i].Checked == true)
                    break;
            fuser.icon = i;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmuser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Cancel &&
                this.DialogResult != DialogResult.OK)
                e.Cancel = true;
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }
    }

    public class Userinfo
    {
        public string name;
        public int icon;

        public Userinfo(string name, int icon)
        {
            this.name = name;
            this.icon = icon;
        }

        public Userinfo()
        {
            name = "";
            icon = 0;
        }
    }
}
