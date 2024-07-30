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
    public partial class OutstandingSupplierDetail : Form
    {
        #region Variables

        POSEntities entity = new POSEntities();
        public int supplierId;
        int _rowindex = 0;
        #endregion

        public OutstandingSupplierDetail()
        {
            InitializeComponent();
        }

        #region Event
        private void OutstandingSupplierDetail_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            DataBind();
        }

        private void dgvOutstandingTransaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _rowindex = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                int currentMainPurchaseId = Convert.ToInt32(dgvOutstandingTransaction.Rows[e.RowIndex].Cells[0].Value);

                DataGridViewRow row = dgvOutstandingTransaction.Rows[e.RowIndex];
                //View Detail
                if (e.ColumnIndex == 7)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.PurchaseRole.ViewDetail || MemberShip.isAdmin)
                    {
                        PurchaseDetailList newform = new PurchaseDetailList();
                        newform.mainPurchaseId = currentMainPurchaseId;
                        newform.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to view purchase detail", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                ////IsPaid Check Box
                else if (e.ColumnIndex == 6)
                {
                    dgvOutstandingTransaction.CommitEdit(new DataGridViewDataErrorContexts());

                    var check = Convert.ToBoolean(dgvOutstandingTransaction.Rows[_rowindex].Cells[6].Value);
                    if (check == false)
                    {
                        dgvOutstandingTransaction.Rows[_rowindex].Cells[6].Value = true;
                    }
                    else
                    {
                        dgvOutstandingTransaction.Rows[_rowindex].Cells[6].Value = false;
                    }
                    ChkOutstanding_CheckChanged(sender, e);
                }
            }
        }

        private void btnPaidCash_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOutstandingTransaction.Rows)
            {
                int _Id = Convert.ToInt32(row.Cells[0].Value);
                long _settlementAmt = Convert.ToInt64(row.Cells[5].Value);
                bool _isPaid = Convert.ToBoolean(row.Cells[6].Value);

                if (_isPaid == true)
                {
                    APP_Data.MainPurchase _mainPurchase = entity.MainPurchases.Where(x => x.Id == _Id).FirstOrDefault();

                    _mainPurchase.IsCompletedInvoice = true;
                    _mainPurchase.SettlementAmount = _settlementAmt;
                    _mainPurchase.OldCreditAmount = 0;
                    _mainPurchase.UpdatedDate = DateTime.Now;
                    _mainPurchase.UpdatedBy = MemberShip.UserId;
                    _mainPurchase.IsCompletedPaid = true;

                    entity.Entry(_mainPurchase).State = EntityState.Modified;
                    entity.SaveChanges();

                    MessageBox.Show("Succesfully Paid!", "mPOS");
                    DataBind();
                    txtTotalOutstandingAmt.Text = "0";
                }
            }
            if (System.Windows.Forms.Application.OpenForms["chart"] != null)
            {
                chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                newForm.FormFresh();
            }
        }

        #endregion
       
        #region Method
        private void DataBind()
        {
            Supplier sup = (from s in entity.Suppliers where s.Id == supplierId select s).FirstOrDefault<Supplier>();
            
            lblName.Text = sup.Name != "" ? sup.Name : "-";
            lblPhoneNumber.Text = sup.PhoneNumber != "" ? sup.PhoneNumber : "-";
            lblContactPerson.Text = sup.ContactPerson != "" ? sup.ContactPerson : "-";
            lblAddress.Text = sup.Address != "" ? sup.Address : "-";
            lblEmail.Text = sup.Email != "" ? sup.Email : "-";

            var tranList = from trans in sup.MainPurchases
                           join p in entity.Users on trans.CreatedBy equals p.Id
                           where trans.IsCompletedPaid == false && trans.SupplierId==supplierId && (trans.IsDeleted == null || trans.IsDeleted == false) && (trans.IsCompletedInvoice==true)
                           select new
                           {
                               Id = trans.Id,
                               VouncherNo = trans.VoucherNo,
                               Date = trans.Date.Value.ToShortDateString(),
                               Time = trans.Date.Value.ToShortTimeString(),
                               CreatedBy = p.Name,
                               OldCreditAmount = trans.OldCreditAmount
                           };

            int _totalOustandingAmt = Convert.ToInt32(tranList.Sum(x => x.OldCreditAmount).Value);
            lblTotalOutstanding.Text = _totalOustandingAmt.ToString();
            List<object> _tranList = new List<object>(tranList);
            dgvOutstandingTransaction.AutoGenerateColumns = false;
            dgvOutstandingTransaction.DataSource = _tranList;
        }

        protected void ChkOutstanding_CheckChanged(object sender, EventArgs e)
        {
            txtTotalOutstandingAmt.Text = (dgvOutstandingTransaction.Rows.Cast<DataGridViewRow>()
                                                               .Where(r => Convert.ToBoolean(r.Cells[6].Value).Equals(true))
                                                               .Sum(t => Convert.ToInt32(t.Cells[5].Value))).ToString();
        }
        #endregion

    }
}
