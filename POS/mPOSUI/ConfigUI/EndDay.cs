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
    public partial class EndDay : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        int TotalAmount = 0;
        long TotalIncome = 0;
        long OpeningBalance = 0 ;

        #endregion

        #region Events

        public EndDay()
        {
            InitializeComponent();
        }

        private void EndDay_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            DailyRecord latestRecord = (from rec in entity.DailyRecords where rec.CounterId == MemberShip.CounterId && rec.IsActive == true select rec).FirstOrDefault();
            if (latestRecord == null)
            {
                MessageBox.Show("There is no record which start the day!");
                this.Dispose();
            }
            else
            {

                OpeningBalance = (long)latestRecord.OpeningBalance;

                lblOpeningBalance.Text = OpeningBalance.ToString();


                List<Transaction> transList = (from ts in entity.Transactions where ts.DateTime > latestRecord.StartDateTime && ts.CounterId == MemberShip.CounterId select ts).ToList();

                foreach (Transaction ts in transList)
                {
                    //Normal Transaction Or Debt Payment
                    if (ts.Type == TransactionType.Sale || ts.Type == TransactionType.Settlement)
                    {
                        TotalIncome += (long)ts.TotalAmount;
                    }
                    //Credit
                    else if (ts.Type == TransactionType.Credit)
                    {
                        TotalIncome += (long)ts.RecieveAmount;

                    }
                    //Refund Amount
                    else if (ts.Type == TransactionType.Refund)
                    {
                        TotalIncome -= (long)ts.TotalAmount;
                    }

                }

                lblTotalIncome.Text = TotalIncome.ToString();
                lblRequireAmount.Text = (TotalIncome + OpeningBalance).ToString();
                lblRequireOrOver.ForeColor = Color.Red;
                lblRequireAmount.ForeColor = Color.Red;

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
        }

        void calculateTotal(object sender, KeyEventArgs e)
        {
            int var10000, var5000, var1000, var500, var200, var100, var50, var20, var10, extra;
            Int32.TryParse(txt10000.Text, out var10000);
            Int32.TryParse(txt5000.Text, out var5000);
            Int32.TryParse(txt1000.Text, out var1000);
            Int32.TryParse(txt500.Text, out var500);
            Int32.TryParse(txt200.Text, out var200);
            Int32.TryParse(txt100.Text, out var100);
            Int32.TryParse(txt50.Text, out var50);
            Int32.TryParse(txt20.Text, out var20);
            Int32.TryParse(txt10.Text, out var10);
            Int32.TryParse(txtOtherAmount.Text, out extra);

            TotalAmount = (10000 * var10000) + (5000 * var5000) + (1000 * var1000) + (500 * var500) + (200 * var200) + (100 * var100) + (50 * var50) + (20 * var20) + (10 * var10) + extra;
            lblTotal.Text = TotalAmount.ToString();
            long differenceAmount = TotalAmount - (TotalIncome + OpeningBalance);
            lblRequireAmount.Text = differenceAmount.ToString();
            if (differenceAmount < 0)
            {
                lblRequireOrOver.Text = "Require Amount";
                lblRequireOrOver.ForeColor = Color.Red;
                lblRequireAmount.ForeColor = Color.Red;
            }
            else
            {
                lblRequireOrOver.Text = "Exceed Amount";
                lblRequireOrOver.ForeColor = Color.Green;
                lblRequireAmount.ForeColor = Color.Green;
            }
        }

        void textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        #endregion

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DailyRecord latestRecord = (from rec in entity.DailyRecords where rec.CounterId == MemberShip.CounterId && rec.IsActive == true select rec).FirstOrDefault();
            latestRecord.EndDateTime = DateTime.Now;
            latestRecord.Comment = txtComment.Text;
            latestRecord.ClosingBalance = TotalAmount;
            latestRecord.DifferenceAmount = TotalAmount - (TotalIncome + OpeningBalance);
            latestRecord.IsActive = false;
            entity.SaveChanges();
            MessageBox.Show("Daily Record for " + DateTime.Now.ToString("dd-MM-yyyy") + " is close now!");
            this.Dispose();
        }

    }
}
