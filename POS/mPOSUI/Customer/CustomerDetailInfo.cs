using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class CustomerDetailInfo : Form
    {
        #region Variables
        POSEntities entity = new POSEntities();
        public int customerId;
        #endregion

        #region Event
        public CustomerDetailInfo()
        {
            InitializeComponent();
        }

        private void CustomerDetailInfo_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            Customer cust = (from c in entity.Customers where c.Id == customerId select c).FirstOrDefault<Customer>();

            lblName.Text = cust.Title + " " + cust.Name;

            lblMCId.Text = cust.VIPMemberId != null ? cust.VIPMemberId : "-";


            lblMType.Text = (from m in entity.MemberTypes where m.Id == cust.MemberTypeID select m.Name).FirstOrDefault();


            if (lblMType.Text == null || lblMType.Text == "")
            {
                lblMType.Text = "-";
            }

            lblPhoneNumber.Text = cust.PhoneNumber != "" ? cust.PhoneNumber : "-";

            lblNrc.Text = cust.NRC != "" ? cust.NRC : "-";

            lblAddress.Text = cust.Address != "" ? cust.Address : "-";

            lblEmail.Text = cust.Email != "" ? cust.Email : "-";

            lblGender.Text = cust.Gender != "" ? cust.Gender : "-";
            lblDrug.Text = cust.DrugAllergy != "" ? cust.DrugAllergy : "-";
            lblEmerPh.Text = cust.EmergencyContactPhone != "" ? cust.EmergencyContactPhone : "-";
            lblContactName.Text = cust.EmergencyContactName != "" ? cust.EmergencyContactName : "-";
            lblMartialStatus.Text = cust.Maritalstatus != "" ? cust.Maritalstatus : "-";
            lblMainConcern.Text = cust.MainConcern != "" ? cust.MainConcern : "-";
            lblMedicalHistory.Text = cust.MedicalHistory != "" ? cust.MedicalHistory : "-";

            lblRS.Text = cust.Relationship != "" ? cust.Relationship : "-";
            lblBirthday.Text = cust.Birthday != null ? Convert.ToDateTime(cust.Birthday).ToString("dd-MM-yyyy") : "-";
            lblCity.Text = cust.City != null ? cust.City.CityName : "-";



            //TTN
            if (cust.ReferredID != null)
            {
                lblReferrer.Text = (from c in entity.Customers where c.Id == cust.ReferredID select c.Name).FirstOrDefault();
            }
            else
            {
                lblReferrer.Text = "-";

            }
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
           
            if (!MemberShip.isAdmin && !controller.Transaction.Cancel)
            {
                dgvTransactionList.Columns["colCancel"].Visible = false;
            }

            dgvTransactionList.AutoGenerateColumns = false;
            List<Transaction> transList = cust.Transactions.Where(trans => (trans.IsDeleted == false || trans.IsDeleted == null) && (trans.IsComplete == true)).ToList();
            dgvTransactionList.DataSource = transList;
            //  lbltamtspentholder.Text = transList.Where(a => a.Type != "Settlement" && a.Type != "Prepaid").Sum(a => a.TotalAmount).ToString();

            lbltamtspentholder.Text = transList.Sum(a => a.RecieveAmount).ToString() ;



        }

        private void dgvNormalTransaction_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTransactionList.Rows)
            {
                Transaction ts = (Transaction)row.DataBoundItem;
                row.Cells[0].Value = ts.Shop.ShopName;
                row.Cells[1].Value = ts.Id;
                row.Cells[2].Value = ts.DateTime.Value.Date.ToString("dd-MM-yyyy");
                //row.Cells[2].Value = ts.DateTime.Value.TimeOfDay.Hours.ToString() + ts.DateTime.Value.TimeOfDay.Minutes.ToString();
                row.Cells[3].Value = ts.DateTime.Value.TimeOfDay.Hours.ToString() + ":" + ts.DateTime.Value.TimeOfDay.Minutes.ToString() + ":" + ts.DateTime.Value.Second.ToString();
                row.Cells[4].Value = ts.PaymentType.Name;
                row.Cells[5].Value = ts.TotalAmount;
                row.Cells[6].Value = ts.Type == "Settlement" ? ts.Type + "  (" + ts.TranVouNos + ")" : ts.Type;
                if (ts.Type == "Settlement")
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#BDEDFF");

                }
                row.Cells[7].Value = ts.User.Name;
            }
        }
        #endregion

        private void dgvNormalTransaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string currentTransactionId = dgvTransactionList.Rows[e.RowIndex].Cells[1].Value.ToString();
                var type = (from p in entity.Transactions where p.Id == currentTransactionId select p.Type).FirstOrDefault();
                if (e.ColumnIndex == colViewDetail.Index)
                {
                    if (type == "Settlement")
                    {
                        this.dgvTransactionList.Rows[e.RowIndex].Cells[8].ReadOnly = true;

                        return;
                    }
                    TransactionDetailForm newForm = new TransactionDetailForm();
                    newForm.transactionId = currentTransactionId;
                    newForm.shopid = SettingController.DefaultShop.Id;
                    newForm.ShowDialog();
                }
                else if(e.ColumnIndex == colCancel.Index)
                {
                    string transType = dgvTransactionList.Rows[e.RowIndex].Cells[colType.Index].Value.ToString();
                    if (transType == "Settlement" || transType == "Prepaid")
                    {
                        colCancel.ReadOnly = true;
                        return;
                    }

                    List<APP_Data.TransactionDetail> IsConsignmentPaidTranList = entity.TransactionDetails.Where(x => x.TransactionId == currentTransactionId && x.IsDeleted == false && x.IsConsignmentPaid == true).ToList();
                    if (IsConsignmentPaidTranList.Count > 0)
                    {
                        MessageBox.Show("This transaction already paid Consignment. It cannot be cancelled!");
                        return;
                    }

                    Transaction ts = entity.Transactions.FirstOrDefault(x => x.Id == currentTransactionId && x.IsCancelled == false);
                    if (ts == null)
                    {
                        MessageBox.Show("This transaction is already cancelled.");
                        return;
                    }
                    else
                    {
                        List<Transaction> rlist = new List<Transaction>();
                        if (ts.Transaction1.Count > 0)
                        {
                            rlist = ts.Transaction1.Where(x => x.IsDeleted == false).ToList();
                        }
                        if (rlist.Count > 0)
                        {
                            MessageBox.Show("This transaction is already refunded.", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        //  }
                        bool isPackage = entity.PackagePurchasedInvoices.Any(x => x.TransactionDetail.TransactionId == ts.Id && x.IsDelete == false);
                        if (!isPackage)
                        {
                            MessageBox.Show("This transaction does not contain package product", "Cancellation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        CancelTransaction cancelForm = new CancelTransaction(ts.Id);
                        cancelForm.Show();

                    }
                }
            }
        }

        
    }
}
