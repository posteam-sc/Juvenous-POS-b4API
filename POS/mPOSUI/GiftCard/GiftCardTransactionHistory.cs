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
    public partial class GiftCardTransactionHistory : Form
    {
        #region Initialize
        public GiftCardTransactionHistory()
        {
            InitializeComponent();
            dgvGiftTransactionList.AutoGenerateColumns = false;
        }
        #endregion

        #region Vairable
        POSEntities entity = new POSEntities();
        public int GiftCardId = 0;
        #endregion

        #region Method
        private void DataBind()
        {
          
                entity = new POSEntities();
                IQueryable<object> q = from t in entity.Transactions
                                       join ty in entity.PaymentTypes on t.PaymentTypeId equals ty.Id
                                       join u in entity.Users on t.UserId equals u.Id
                                       where (t.IsDeleted == false || t.IsDeleted == null) &&
                                       (t.PaymentTypeId == 3) && (t.GiftCardId == GiftCardId) &&
                                       (t.IsComplete == true) && (t.IsActive == true)
                                       select new
                                       {
                                           TransactionId = t.Id,
                                           Type = t.Type,
                                           PaymentMethod = ty.Name,
                                           Date = t.DateTime,
                                           Time = t.DateTime,
                                           SalePerson = u.Name,
                                           TotalAmount = t.TotalAmount,
                                           GiftCardAmount = t.GiftCardAmount,
                                           CashAmount = t.RecieveAmount

                                       };
                List<object> giftCard = new List<object>(q);

                dgvGiftTransactionList.AutoGenerateColumns = false;
                dgvGiftTransactionList.DataSource = giftCard;

             Int64 TotalGiftCardAmt = dgvGiftTransactionList.Rows.Cast<DataGridViewRow>()
                         .Sum(t =>  Convert.ToInt32(t.Cells["colGiftCardAmount"].Value));

             lblTotalGiftCardAmt.Text = TotalGiftCardAmt.ToString();

        }
     
        #endregion



        private void GiftCardTransactionHistory_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dgvGiftTransactionList.Columns["colDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
            dgvGiftTransactionList.Columns["colTime"].DefaultCellStyle.Format = "hh:mm";
       
            DataBind();
        }

        private void dgvGiftTransactionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string currentTransactionId = dgvGiftTransactionList.Rows[e.RowIndex].Cells[0].Value.ToString();
            //View Detail
            if (e.ColumnIndex == 9)
            {
                
                    TransactionDetailForm newForm = new TransactionDetailForm();
                    newForm.transactionId = currentTransactionId;
                    newForm.ShowDialog();
                
            }
        }
      
    }
}
