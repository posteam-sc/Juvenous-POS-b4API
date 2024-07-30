using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{    
    public partial class StartDay : Form
    {
        #region Variables

        private long TotalAmount = 0;

        private POSEntities entity = new POSEntities();

        #endregion

        public StartDay()
        {
            InitializeComponent();
        }

        private void StartDay_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            DailyRecord latestRecord = (from rec in entity.DailyRecords where rec.CounterId == MemberShip.CounterId && rec.IsActive == true select rec).FirstOrDefault();
            if (latestRecord != null)
            {
                MessageBox.Show("Closing balance haven't put for the last day, please add this first before opening a new day!");
                this.Dispose();
            }

            txt10000.KeyUp += calculateTotal;
            txt5000.KeyUp += calculateTotal;
            txt1000.KeyUp += calculateTotal;
            txt500.KeyUp += calculateTotal;
            txt200.KeyUp += calculateTotal;
            txt100.KeyUp += calculateTotal;
            txt50.KeyUp += calculateTotal;
            txt20.KeyUp += calculateTotal;
            txt10.KeyUp += calculateTotal;
            txtOtherAmount.KeyUp += calculateTotal;

            //Allow only integer
            txt10000.KeyPress += textbox_KeyPress;
            txt5000.KeyPress += textbox_KeyPress;
            txt1000.KeyPress += textbox_KeyPress;
            txt500.KeyPress += textbox_KeyPress;
            txt200.KeyPress += textbox_KeyPress;
            txt100.KeyPress += textbox_KeyPress;
            txt50.KeyPress += textbox_KeyPress;
            txt20.KeyPress += textbox_KeyPress;
            txt10.KeyPress += textbox_KeyPress;
            txtOtherAmount.KeyPress += textbox_KeyPress;
        }

        void calculateTotal(object sender, KeyEventArgs e)
        {
            int var10000, var5000, var1000, var500, var200, var100, var50, var20, var10,extra;
            Int32.TryParse(txt10000.Text,out var10000);
            Int32.TryParse(txt5000.Text, out var5000);
            Int32.TryParse(txt1000.Text, out var1000);
            Int32.TryParse(txt500.Text, out var500);
            Int32.TryParse(txt200.Text, out var200);
            Int32.TryParse(txt100.Text, out var100);
            Int32.TryParse(txt50.Text, out var50);
            Int32.TryParse(txt20.Text, out var20);
            Int32.TryParse(txt10.Text, out var10);
            Int32.TryParse(txtOtherAmount.Text, out extra);

            TotalAmount = (10000 * var10000) + (5000 * var5000) + (1000 * var1000) + (500 * var500) + (200 * var200) + (100 * var100) + (50 * var50) + (20 * var20) + (10 * var10)+ extra;
            lblTotal.Text = TotalAmount.ToString();

        }

        void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DailyRecord dr = new DailyRecord();
            dr.OpeningBalance = TotalAmount;
            dr.StartDateTime = DateTime.Now;
            dr.IsActive = true;
            dr.CounterId = MemberShip.CounterId;
            entity.DailyRecords.Add(dr);
            entity.SaveChanges();
            MessageBox.Show("The new day is begin");

            if (System.Windows.Forms.Application.OpenForms["Sales"] == null)
            {
                Sales form = new Sales();
                form.WindowState = FormWindowState.Maximized;
                form.MdiParent = ((MDIParent)this.ParentForm);
                form.Show();
            }

            this.Dispose();
        }

     
    }
}
