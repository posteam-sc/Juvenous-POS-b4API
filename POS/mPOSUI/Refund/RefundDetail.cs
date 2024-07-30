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
    public partial class RefundDetail : Form
    {
        #region Variable

        private POSEntities entity = new POSEntities();
        public string transactionId;
        public bool IsRefund;

        #endregion
        #region Event
        public RefundDetail()
        {
            InitializeComponent();
            dgvRefundDetail.AutoGenerateColumns = false;
        }

        private void RefundDetail_Load(object sender, EventArgs e)
        {
            dgvRefundDetail.AutoGenerateColumns = false;
            Localization.Localize_FormControls(this);
            
            if (!IsRefund)
            {
                Transaction refundTransactionDetail = (from t in entity.Transactions where t.Id == transactionId select t).FirstOrDefault();
                lblSalePerson.Text = (refundTransactionDetail.User == null) ? "-" : refundTransactionDetail.User.Name;
                lblDate.Text = refundTransactionDetail.DateTime.Value.ToString("dd-MM-yyyy");
                lblTime.Text = refundTransactionDetail.DateTime.Value.ToString("hh:mm");
                lblMainTransaction.Text = refundTransactionDetail.ParentId.ToString();
                dgvRefundDetail.DataSource = refundTransactionDetail.TransactionDetails.ToList();
                lblCash.Text = refundTransactionDetail.RecieveAmount.ToString();
               // lblChangeGiven.Text = (refundTransactionDetail.RecieveAmount - refundTransactionDetail.TotalAmount).ToString();
                //to show main transaction Discount amount
                //int ItemDis = 0;
                //Transaction trans = (from t in entity.Transactions where t.Id == refundTransactionDetail.ParentId select t).FirstOrDefault();
                //foreach (TransactionDetail td in trans.TransactionDetails)
                //{
                //    ItemDis += Convert.ToInt32(((td.UnitPrice) * (td.DiscountRate / 100)) * td.Qty);
                //}
                int DiscountAmount = 0;
                DiscountAmount = (int)refundTransactionDetail.DiscountAmount;// -ItemDis;
                lblChangeGiven.Text = DiscountAmount.ToString();
                lblTotal.Text = refundTransactionDetail.TotalAmount.ToString();
            }
                //to test
            else
            {
                Transaction refundTransactionDetail = (from t in entity.Transactions where t.ParentId == transactionId && t.Type == TransactionType.Refund select t).FirstOrDefault();
                lblSalePerson.Text = (refundTransactionDetail.User == null) ? "-" : refundTransactionDetail.User.Name;
                lblDate.Text = refundTransactionDetail.DateTime.Value.ToString("dd-MM-yyyy");
                lblTime.Text = refundTransactionDetail.DateTime.Value.ToString("hh:mm");
                lblMainTransaction.Text = refundTransactionDetail.ParentId.ToString();
                dgvRefundDetail.DataSource = refundTransactionDetail.TransactionDetails.ToList();
                lblCash.Text = refundTransactionDetail.RecieveAmount.ToString();
                lblChangeGiven.Text = (refundTransactionDetail.RecieveAmount - refundTransactionDetail.TotalAmount).ToString();
                lblTotal.Text = refundTransactionDetail.TotalAmount.ToString(); 
            }
        }

        private void dgvRefundDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvRefundDetail.Rows)
            {
                TransactionDetail transactionDetailObj = (TransactionDetail)row.DataBoundItem;
                row.Cells[0].Value = transactionDetailObj.Product.ProductCode;
                row.Cells[1].Value = transactionDetailObj.Product.Name;
                row.Cells[2].Value = transactionDetailObj.Qty;
                row.Cells[3].Value = transactionDetailObj.UnitPrice;
                row.Cells[4].Value = transactionDetailObj.DiscountRate + "%";
                row.Cells[5].Value = transactionDetailObj.TotalAmount;
                row.Cells[6].Value = transactionDetailObj.Transaction.Type;
            }
        }

        #endregion

    }
}
