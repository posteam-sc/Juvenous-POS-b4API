using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;
using System.Data.Objects;

namespace POS
{
    public partial class PurchaseListBySupplier : Form
    {
        #region Variable

        public int supplierId;
        public string supplierName;
        POSEntities entity = new POSEntities();
        public static PurchaseListBySupplier plist;
        bool Approved = false;
        DateTime _fromDate = new DateTime();
        DateTime _toDate = new DateTime();
        bool _startProcess = false;

        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();
        #endregion

        #region Event

        public PurchaseListBySupplier()
        {
            InitializeComponent();
            CenterToScreen();
            plist = this;
        }


        private void PurchaseListBySupplier_Load(object sender, EventArgs e)
        {
            Localization.Localize_FormControls(this);
            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd-MMM-yyyy";

            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd-MMM-yyyy";


            Default_Date();
            Radio_Status();
            BindSupplier();

            dgvMainPurchaseList.AutoGenerateColumns = false;
            lblSupplierName.Text = supplierName;
            if (supplierName != null)
            {
                lblsName.Visible = false;
                lblSupplierName.Visible = false;
                cboSupplierName.Visible = false;
                groupBox1.Text += " for " + supplierName;
            }
            _startProcess = true;
            DataBind();
        }

        private void dgvMainPurchaseList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int currentMainPurchaseId = Convert.ToInt32(dgvMainPurchaseList.Rows[e.RowIndex].Cells[0].Value);
                string vouncherNo = dgvMainPurchaseList.Rows[e.RowIndex].Cells[2].Value.ToString();

                MainPurchase _mainPurchase = (from p in entity.MainPurchases where p.Id == currentMainPurchaseId select p).FirstOrDefault();
                //Edit
                if (e.ColumnIndex == 9)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.PurchaseRole.EditOrDelete || MemberShip.isAdmin)
                    {
                        if (_mainPurchase.IsCompletedInvoice == true)
                        {
                            MessageBox.Show("You have already approved Invoice No. " + vouncherNo + " You cannot edit it anymore.");
                            return;
                        }
                        else
                        {
                            int _totalAmt = Convert.ToInt32(dgvMainPurchaseList.Rows[e.RowIndex].Cells[4].Value);
                            int _cashAmt = Convert.ToInt32(dgvMainPurchaseList.Rows[e.RowIndex].Cells[5].Value);

                            int CrAmt = _totalAmt - _cashAmt;
                            PurchaseInput newform = new PurchaseInput();
                            newform.mainPurchaseId = currentMainPurchaseId;
                            newform.CrAmt = CrAmt;
                            newform.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to edit purchase.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }


                }
                //View Detail
                if (e.ColumnIndex == 10)
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
                //Approved
                else if (e.ColumnIndex == 11)
                {
                    if (_mainPurchase.IsCompletedInvoice == true)
                    {
                        MessageBox.Show("This Invoice No. " + vouncherNo + " is already Approved!", "Information");
                        return;
                    }
                    else
                    {
                        RoleManagementController controller = new RoleManagementController();
                        controller.Load(MemberShip.UserRoleId);
                        if (controller.PurchaseRole.Approved || MemberShip.isAdmin)
                        {
                            DialogResult result1 = MessageBox.Show("Please note that you cannot edit  purchase order invoice anymore after you clicked Approved. ", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            if (result1.Equals(DialogResult.OK))
                            {


                                List<APP_Data.PurchaseDetail> _purchaseDetail = entity.PurchaseDetails.Where(x => x.MainPurchaseId == currentMainPurchaseId && x.IsDeleted == false).ToList();

                                foreach (var p in _purchaseDetail)
                                {
                                    //increate qty inproduct  after approve
                                    APP_Data.Product pdObj = entity.Products.Where(x => x.Id == p.ProductId).FirstOrDefault();
                                    if (pdObj != null)
                                    {
                                        pdObj.Qty = pdObj.Qty + p.Qty;
                                        entity.Entry(pdObj).State = EntityState.Modified;
                                        entity.SaveChanges();


                                        //save in stocktransaction

                                        Stock_Transaction st = new Stock_Transaction();
                                        st.ProductId = p.ProductId;
                                        st.Purchase = p.Qty;
                                        productList.Add(st);
                                    }
                                }


                                _mainPurchase.IsCompletedInvoice = true;
                                _mainPurchase.UpdatedDate = DateTime.Now;
                                _mainPurchase.UpdatedBy = MemberShip.UserId;

                                entity.Entry(_mainPurchase).State = EntityState.Modified;
                                entity.SaveChanges();


                                //save in stocktransaction
                                Save_PurQty_ToStockTransaction(productList, Convert.ToDateTime(_mainPurchase.Date));
                                productList.Clear();

                                MessageBox.Show("Successfully Approved Invoice no. " + vouncherNo);
                                if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                                {
                                    chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                                    newForm.FormFresh();
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("You are not allowed to approve purchase.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                else if (e.ColumnIndex == 12)
                {
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.PurchaseRole.EditOrDelete || MemberShip.isAdmin)
                    {
                        DialogResult result1 = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result1.Equals(DialogResult.OK))
                        {
                            //////////////int OldcurrentMainPurchaseId = 0;
                            //////////////if (dgvMainPurchaseList.Rows.Count > 1)
                            //////////////{
                            //////////////    OldcurrentMainPurchaseId = Convert.ToInt32(dgvMainPurchaseList.Rows[e.RowIndex - 1].Cells[0].Value);
                            //////////////}

                            List<APP_Data.PurchaseDetail> purdlist = entity.PurchaseDetails.Where(x => x.MainPurchaseId == currentMainPurchaseId).ToList();

                            foreach (PurchaseDetail pd in purdlist)
                            {
                                List<APP_Data.PurchaseDetailInTransaction> pdilist = entity.PurchaseDetailInTransactions.Where(x => x.PurchaseDetailId == pd.Id).ToList();

                                if (pdilist.Count > 0)
                                {
                                    MessageBox.Show("You cannot delete this record !", "Delete");
                                    return;
                                }
                            }

                            APP_Data.MainPurchase mainPur = entity.MainPurchases.Where(x => x.Id == currentMainPurchaseId).FirstOrDefault();

                            foreach (PurchaseDetail pd in purdlist)
                            {
                                pd.IsDeleted = true;
                                pd.DeletedDate = DateTime.Now;
                                pd.DeletedUser = MemberShip.UserId;
                                entity.Entry(pd).State = EntityState.Modified;
                                entity.SaveChanges();



                                if (mainPur.IsCompletedInvoice == true)
                                {
                                    APP_Data.Product pdObj = entity.Products.Where(x => x.Id == pd.ProductId).FirstOrDefault();
                                    if (pdObj != null)
                                    {

                                        pdObj.Qty = pdObj.Qty - pd.Qty;
                                        entity.Entry(pdObj).State = EntityState.Modified;
                                        entity.SaveChanges();


                                        //save in stocktransaction

                                        Stock_Transaction st = new Stock_Transaction();
                                        st.ProductId = pd.ProductId;
                                        Qty -= Convert.ToInt32(pd.Qty);
                                        st.Purchase = Qty;
                                        productList.Add(st);
                                        Qty = 0;

                                    }
                                }
                            }


                            //Main Purchase Delete


                            string voucherNo = mainPur.VoucherNo.ToString();
                            mainPur.IsDeleted = true;
                            mainPur.IsActive = false;

                            entity.Entry(mainPur).State = EntityState.Modified;
                            entity.SaveChanges();

                            //save in stocktransaction
                            Save_PurQty_ToStockTransaction(productList, Convert.ToDateTime(mainPur.Date));
                            productList.Clear();

                            //Delete Log for  Main purchase

                            List<PurchaseDeleteLog> deleloglist = entity.PurchaseDeleteLogs.Where(x => x.MainPurchaseId == mainPur.Id && x.IsParent == false).ToList();
                            foreach (PurchaseDeleteLog pdDel in deleloglist)
                            {
                                entity.PurchaseDeleteLogs.Remove(pdDel);
                                entity.SaveChanges();
                            }
                            APP_Data.PurchaseDeleteLog delObj = new APP_Data.PurchaseDeleteLog();
                            delObj.CounterId = MemberShip.CounterId;
                            delObj.UserId = MemberShip.UserId;
                            delObj.MainPurchaseId = currentMainPurchaseId;
                            delObj.DeletedDate = DateTime.Now;
                            delObj.IsParent = true;
                            delObj.VoucherNo = voucherNo;
                            entity.PurchaseDeleteLogs.Add(delObj);
                            entity.SaveChanges();
                            if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                            {
                                chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                                newForm.FormFresh();
                            }
                        }
                        if (true)
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete purchase", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                DataBind();
            }

        }

        private void cboSupplierName_SelectedValueChanged(object sender, EventArgs e)
        {
            Radio_Status();
            Date_Assign();
            DataBind();
        }

        private void rdoPending_CheckedChanged(object sender, EventArgs e)
        {
            //After Approved Don't allow to delete and edit
            //dgvMainPurchaseList.Columns[12].Visible = dgvMainPurchaseList.Columns[9].Visible =  !rdoApproved.Checked;
            Radio_Status();
            Date_Assign();
            DataBind();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            Date_Assign();
            DataBind();
        }

        #endregion

        #region Function

        #region for saving Purchase Qty in Stock Transaction table
        private void Save_PurQty_ToStockTransaction(List<Stock_Transaction> productList, DateTime _tranDate)
        {
            int _year, _month;

            _year = _tranDate.Year;
            _month = _tranDate.Month;
            Utility.Purchase_Run_Process(_year, _month, productList);
        }
        #endregion

        private void Default_Date()
        {
            dtTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            Date_Assign();
        }

        private void Date_Assign()
        {
            _fromDate = dtFrom.Value;
            _toDate = dtTo.Value;
        }

        private void Radio_Status()
        {
            if (rdoPending.Checked == true)
            {
                Approved = false;
            }
            else
            {
                Approved = true;
            }
        }

        public void BindSupplier()
        {
            List<Supplier> supplierList = new List<Supplier>();
            Supplier supplierObj = new Supplier();
            supplierObj.Id = 0;
            supplierObj.Name = "Select All";
            supplierList.Add(supplierObj);
            supplierList.AddRange(entity.Suppliers.ToList());
            cboSupplierName.DataSource = supplierList;
            cboSupplierName.DisplayMember = "Name";
            cboSupplierName.ValueMember = "Id";
            cboSupplierName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSupplierName.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public void DataBind()
        {
            // 
            if (_startProcess == true)
            {
                dgvMainPurchaseList.Columns[1].DefaultCellStyle.Format = "dd-MM-yyyy";
                if (supplierId != 0)
                {
                    if (cboSupplierName.SelectedIndex == 0)
                    {
                        BindSupplier();
                        cboSupplierName.SelectedValue = supplierId;
                    }

                }

                if (Approved == true)
                {
                    dgvMainPurchaseList.Columns[11].Visible = false;
                }
                else
                {
                    dgvMainPurchaseList.Columns[11].Visible = true;
                }

                if (cboSupplierName.SelectedIndex > 0)
                {
                    supplierId = Convert.ToInt32(cboSupplierName.SelectedValue);
                    dgvMainPurchaseList.AutoGenerateColumns = false;
                    dgvMainPurchaseList.DataSource = (from mp in entity.MainPurchases
                                                      where mp.SupplierId == supplierId && mp.IsDeleted == false
                                                      && mp.IsCompletedInvoice == Approved
                                                      && mp.IsPurchase == true
                                                     && (EntityFunctions.TruncateTime(mp.Date.Value) >= _fromDate)
                                                   && (EntityFunctions.TruncateTime(mp.Date.Value) <= _toDate)
                                                      orderby mp.Date descending
                                                      select new
                                                      {
                                                          Id = mp.Id,
                                                          Date = mp.Date,
                                                          VoucherNo = mp.VoucherNo,
                                                          SupplierName = mp.Supplier.Name,
                                                          TotalAmount = mp.TotalAmount,
                                                          Cash = mp.Cash,
                                                          //OldCreditAmount = mp.OldCreditAmount,
                                                          DiscountAmount = mp.DiscountAmount,
                                                          //  OldCreditAmount = mp.TotalAmount > mp.Cash ?  mp.TotalAmount - mp.DiscountAmount - mp.Cash :  0 ,
                                                          OldCreditAmount = mp.OldCreditAmount,
                                                          SettlementAmount = mp.SettlementAmount,
                                                          SupplierId = mp.SupplierId
                                                      }).ToList();
                }
                else
                {
                    dgvMainPurchaseList.AutoGenerateColumns = false;
                    lblSupplierName.Visible = false;
                    //lblsName.Visible = false;
                    dgvMainPurchaseList.DataSource = (from mp in entity.MainPurchases
                                                      where mp.IsDeleted == false
                                                        && mp.IsCompletedInvoice == Approved
                                                         && mp.IsCompletedInvoice == Approved
                                                          && mp.IsPurchase == true
                                                        && (EntityFunctions.TruncateTime(mp.Date.Value) >= _fromDate)
                                                   && (EntityFunctions.TruncateTime(mp.Date.Value) <= _toDate)
                                                      orderby mp.Date descending
                                                      select new
                                                      {
                                                          Id = mp.Id,
                                                          Date = mp.Date,
                                                          VoucherNo = mp.VoucherNo,
                                                          SupplierName = mp.Supplier.Name,
                                                          TotalAmount = mp.TotalAmount,
                                                          Cash = mp.Cash,
                                                          //OldCreditAmount = mp.OldCreditAmount,
                                                          DiscountAmount = mp.DiscountAmount,
                                                          //  OldCreditAmount = mp.TotalAmount - mp.DiscountAmount - mp.Cash,
                                                          //  OldCreditAmount = mp.TotalAmount > mp.Cash ? mp.TotalAmount - mp.DiscountAmount - mp.Cash : 0,
                                                          OldCreditAmount = mp.OldCreditAmount,
                                                          SettlementAmount = mp.SettlementAmount,
                                                          SupplierId = mp.SupplierId
                                                      }).ToList();
                }
                supplierId = 0;
                Outstanding_CreditAmount();
            }

        }

        private void Outstanding_CreditAmount()
        {
            if (cboSupplierName.SelectedIndex > 0)
            {
                int supplierId = Convert.ToInt32(cboSupplierName.SelectedValue);

                // Int64 SettlementAmt = dgvMainPurchaseList.Rows.Cast<DataGridViewRow>()
                //.Where(r => Convert.ToInt32(r.Cells["colSupplierId"].Value) == supplierId)
                //.Sum(t => Convert.ToInt32(t.Cells["colSettlementAmount"].Value));


                Int64 OutstandingCreditAmount = dgvMainPurchaseList.Rows.Cast<DataGridViewRow>()
                         .Where(r => Convert.ToInt32(r.Cells["colSupplierId"].Value) == supplierId)
                         .Sum(t => Convert.ToInt32(t.Cells["colOldCreditAmount"].Value));


                // txtOutstandingCreditAmt.Text = (OutstandingCreditAmount - SettlementAmt).ToString();
                txtOutstandingCreditAmt.Text = OutstandingCreditAmount.ToString();

            }
            else
            {
                //Int64 SettlementAmt = dgvMainPurchaseList.Rows.Cast<DataGridViewRow>()
                //      .Sum(t => Convert.ToInt32(t.Cells["colSettlementAmount"].Value));


                Int64 OutstandingCreditAmount = dgvMainPurchaseList.Rows.Cast<DataGridViewRow>()
                   .Sum(t => Convert.ToInt32(t.Cells["colOldCreditAmount"].Value));

                //txtOutstandingCreditAmt.Text = (OutstandingCreditAmount - SettlementAmt).ToString();
                txtOutstandingCreditAmt.Text = OutstandingCreditAmount.ToString();

            }
        }

        #endregion

        private void PurchaseListBySupplier_FormClosed(object sender, FormClosedEventArgs e)
        {
            chart newform = new chart();
            newform.FormFresh();
        }


    }
}
