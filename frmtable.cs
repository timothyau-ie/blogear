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
    public partial class frmtable : Form
    {
        Tablehtml ftable;

        public frmtable(ref Tablehtml table)
        {
            InitializeComponent();
            ftable = table;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool noborder = false;
            if (numericUpDown3.Value == 0)
                noborder = true;
            string s = "<table border = \"" + numericUpDown3.Value.ToString() + "\">";
            for (int i = 0; i < numericUpDown2.Value; i++)
            {
                s = s + "<tr>";
                for (int j = 0; j < numericUpDown1.Value; j++)
                {
                    s = s + "<td><div>";
                    if (noborder)
                        s = s + "%";
                    s = s + "</div></td>";
                }
                s = s + "</tr>";
            }
            s = s + "</table>";
            ftable.s = s;

            this.DialogResult = DialogResult.OK;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmuser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Cancel &&
                this.DialogResult != DialogResult.OK)
                e.Cancel = true;
        }
    }

    public class Tablehtml
    {
        public string s;
    }
}
