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
    public partial class frmimage : Form
    {
        public frmimage(string path, string name)
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            try
            {
                pictureBox1.Image = Image.FromFile(path);
                this.Text = name + " " + pictureBox1.Width.ToString()
                    + " X " + pictureBox1.Height.ToString();
            }
            catch (Exception)
            {
                pictureBox1.Image = imageerror.Images[0];
                this.Text = "Error";
            }
            this.Width = pictureBox1.Width + 6;
            this.Height = pictureBox1.Height + 32;
            pictureBox1.Visible = true;
        }

        private void frmimage_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image.Dispose();
            this.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
                pictureBox1.Image.Dispose();
                this.Dispose();
        }

    }
}
