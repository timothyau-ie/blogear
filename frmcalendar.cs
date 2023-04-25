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
    public partial class frmcalendar : Form
    {
        DayfromCalendar fday;

        public frmcalendar(ref DayfromCalendar day, DateTime starttime, DateTime endtime, DateTime[] boldedtimes)
        {
            InitializeComponent();
            //this.Width = monthCalendar1.Width;
            //this.Height = monthCalendar1.Height + 32;
            fday = day;
            monthCalendar1.SelectionStart = starttime;
            monthCalendar1.SelectionEnd = endtime;
            monthCalendar1.BoldedDates = boldedtimes;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            fday.d = monthCalendar1.SelectionStart;
            this.DialogResult = DialogResult.OK;
        }

    }

    public class DayfromCalendar
    {
        public DateTime d;
    }
}
