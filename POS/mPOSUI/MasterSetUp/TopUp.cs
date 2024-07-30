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
    public partial class TopUp : Form
    {
        #region Variable

        public int GiftCardId{ get; set; }

        private POSEntities entity = new POSEntities();

        GiftCard giftCardOdbj = new GiftCard();

        private ToolTip tp = new ToolTip();

        #endregion

        #region Event

        public TopUp()
        {
            InitializeComponent();
        }

        private void TopUp_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
             giftCardOdbj = (from g in entity.GiftCards where g.Id == GiftCardId select g).FirstOrDefault();

             txtCardNo.Text = giftCardOdbj.CardNumber;
             
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            tp.RemoveAll();
            tp.IsBalloon = true;
            tp.ToolTipIcon = ToolTipIcon.Error;
            tp.ToolTipTitle = "Error";
            //Validation
            if (txtAmount.Text.Trim() == string.Empty)
            {
                tp.SetToolTip(txtAmount, "Error");
                tp.Show("Please fill up amount!", txtAmount);
                hasError = true;
            }
            if (!hasError)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to save? ", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result.Equals(DialogResult.OK))
                {
                    giftCardOdbj.Amount += Convert.ToInt32(txtAmount.Text);
                    entity.Entry(giftCardOdbj).State = System.Data.EntityState.Modified;
                    entity.SaveChanges();
                    MessageBox.Show("Successfully Save!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void TopUp_MouseMove(object sender, MouseEventArgs e)
        {
            tp.Hide(txtAmount);
        }

        #endregion    

        

    }
}
