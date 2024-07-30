using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Windows.Forms;
using POS.APP_Data;

namespace POS
{
    public partial class TransactionList : Form
    {
        #region Variables

        private POSEntities entity = new POSEntities();
        int Qty = 0;

        List<Stock_Transaction> productList = new List<Stock_Transaction>();

        public int index = 0;
        private Boolean IsStart = false;
        public string note;
        #endregion

        #region Event

        public TransactionList()
        {
            InitializeComponent();
        }

        private void TransactionList_Load(object sender, EventArgs e)
        {
            dgvTransactionList.AutoGenerateColumns = false;
            Localization.Localize_FormControls(this);

            Utility.BindShop(cboshoplist);
            cboshoplist.Text = SettingController.DefaultShop.ShopName;
            Utility.ShopComBo_EnableOrNot(cboshoplist);
            IsStart = true;
            if (SettingController.TicketSale )
            {
                dgvTransactionList.Columns[13].Visible = true;
            }
            LoadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDate.Checked)
            {
                gbDate.Enabled = true;
                gbId.Enabled = false;
                txtId.Clear();
            }
            else
            {
                gbDate.Enabled = false;
                gbId.Enabled = true;
            }
            LoadData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();

        }

        private void dgvTransactionList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                string currentTransactionId = dgvTransactionList.Rows[e.RowIndex].Cells[0].Value.ToString();

                List<int> type = new List<int> { 3, 4, 5, 6 };
                List<APP_Data.TransactionDetail> _isConsignmentPaidTranList = entity.TransactionDetails.Where(x => x.TransactionId == currentTransactionId && x.IsDeleted == false && x.IsConsignmentPaid == true).ToList();
                //Refund
                if (e.ColumnIndex == 8)
                {
                    Transaction tObj = (Transaction)dgvTransactionList.Rows[e.RowIndex].DataBoundItem;
                    if (type.Contains(Convert.ToInt32(tObj.PaymentTypeId)))
                    {
                        MessageBox.Show("Non Refundable!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    else
                    {
                        if (tObj.Type != "Settlement")
                        {
                            if (_isConsignmentPaidTranList.Count > 0)
                            {
                                MessageBox.Show("This transaction already  made  Consignment Settlement. It cannot be refund!");
                            }
                            else
                            {
                                RefundTransaction newForm = new RefundTransaction();
                                newForm.transactionId = currentTransactionId;
                                newForm.IsCash = true;
                                newForm.ShowDialog();
                            }

                        }
                        else
                        {
                            colRefund.ReadOnly = true;

                        }
                    }
                }

                //View Detail
                else if (e.ColumnIndex == 9)
                {
                    var tranType = (from t in entity.Transactions where t.Id == currentTransactionId select t.Type).FirstOrDefault();
                    if (tranType == "Settlement")
                    {
                        colViewDetail.ReadOnly = true;
                    }
                    else
                    {
                        TransactionDetailForm newForm = new TransactionDetailForm();
                        newForm.transactionId = currentTransactionId;
                        newForm.shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                        newForm.ShowDialog();
                    }
                }
                //Delete the record and add delete log
                else if (e.ColumnIndex == 10)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Transaction.EditOrDelete || MemberShip.isAdmin)
                    {
                        #region Delete
                        Transaction ts = entity.Transactions.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        var isPurchasePackakgeInvoiceData = entity.PackageUsedHistories.Any(x => x.PackagePurchasedInvoice.TransactionDetail.TransactionId == currentTransactionId && x.IsDelete == false);

                        if (ts.Type == "Settlement")
                        {
                            colDelete.ReadOnly = true;
                        }
                        else
                        {
                            APP_Data.Transaction ts2 = entity.Transactions.Where(x => x.ParentId == currentTransactionId && x.IsDeleted == false).FirstOrDefault();

                            if (ts2 != null)
                            {
                                MessageBox.Show("This transaction already made  refund. It cannot be deleted!");

                            }
                            else if (_isConsignmentPaidTranList.Count > 0)
                            {
                                MessageBox.Show("This transaction already made  Consignment Settlement. It cannot be deleted!");
                            }
                            else if (isPurchasePackakgeInvoiceData)
                            {
                                MessageBox.Show("This transaction already used by Offset. It cannot be deleted!");
                                return;
                            }

                            else
                            {

                                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                if (result.Equals(DialogResult.OK))
                                {


                                    ts.IsDeleted = true;
                                    ts.UpdatedDate = DateTime.Now;
                                    // add gift card amount
                                    if (ts.GiftCardId != null && ts.GiftCard != null)
                                    {
                                        ts.GiftCard.Amount = ts.GiftCard.Amount + Convert.ToInt32(ts.GiftCardAmount);
                                    }
                                    foreach (TransactionDetail detail in ts.TransactionDetails)
                                    {
                                        //detail.IsDeleted = false;
                                        detail.IsDeleted = true;
                                        detail.Product.Qty = detail.Product.Qty + detail.Qty;


                                        //save in stocktransaction

                                        Stock_Transaction st = new Stock_Transaction();
                                        st.ProductId = detail.Product.Id;
                                        Qty -= Convert.ToInt32(detail.Qty);
                                        st.Sale = Qty;
                                        productList.Add(st);
                                        Qty = 0;

                                        if (detail.Product.IsWrapper == true)
                                        {
                                            List<SPDetail> wList = detail.Product.SPDetails.Where(x => x.TransactionDetailID == detail.Id).ToList();
                                            if (wList.Count > 0)
                                            {
                                                foreach (SPDetail w in wList)
                                                {
                                                    Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();
                                                    wpObj.Qty = wpObj.Qty + (w.ChildQty);

                                                    Stock_Transaction stwp = new Stock_Transaction();
                                                    stwp.ProductId = w.ChildProductID;
                                                    Qty -= Convert.ToInt32(w.ChildQty);
                                                    stwp.Sale = Qty;
                                                    productList.Add(stwp);

                                                    Qty = 0;
                                                }
                                            }
                                        }

                                        //For Purchase 
                                        #region Purchase Delete

                                        if (detail.Product.IsWrapper == true)
                                        {//ZP Get purchase detail with child product Id and order by purchase date ascending
                                            List<SPDetail> splist = detail.Product.SPDetails.Where(x => x.TransactionDetailID == detail.Id).ToList();
                                            foreach (SPDetail w in splist)
                                            {
                                                //  int qty = Convert.ToInt32(w.Qty);
                                                Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();

                                                List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == wpObj.Id).ToList();
                                                if (puInTranDetail.Count > 0)
                                                {
                                                    foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                                    {
                                                        PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                                        if (pud != null)
                                                        {
                                                            pud.CurrentQy = pud.CurrentQy + p.Qty;
                                                        }
                                                        entity.Entry(pud).State = EntityState.Modified;
                                                        entity.SaveChanges();

                                                        //entity.PurchaseDetailInTransactions.Remove(p);
                                                        //entity.SaveChanges();

                                                        p.Qty = 0;
                                                        entity.Entry(p).State = EntityState.Modified;

                                                        entity.PurchaseDetailInTransactions.Remove(p);
                                                        entity.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == detail.ProductId).ToList();
                                            if (puInTranDetail.Count > 0)
                                            {
                                                foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                                {
                                                    PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                                    if (pud != null)
                                                    {
                                                        pud.CurrentQy = pud.CurrentQy + p.Qty;
                                                    }
                                                    entity.Entry(pud).State = EntityState.Modified;
                                                    entity.SaveChanges();

                                                    //entity.PurchaseDetailInTransactions.Remove(p);
                                                    //entity.SaveChanges();

                                                    p.Qty = 0;
                                                    entity.Entry(p).State = EntityState.Modified;

                                                    entity.PurchaseDetailInTransactions.Remove(p);
                                                    entity.SaveChanges();
                                                }
                                            }
                                        }




                                        #endregion

                                        //For Ticket
                                        Ticket ti = entity.Tickets.Where(x => x.TransactionDetailId == detail.Id).FirstOrDefault();
                                        if (ti != null)
                                        {
                                            ti.isDelete = true;
                                            ti.DeletedDate = DateTime.Now;
                                            int uid = MemberShip.UserId;
                                            var cUser = entity.Users.Find(uid);
                                            ti.UserName = cUser.Name;
                                            entity.Entry(ti).State = EntityState.Modified;
                                            entity.SaveChanges();
                                        }



                                    }

                                    string date = dgvTransactionList.Rows[e.RowIndex].Cells[3].Value.ToString();
                                    DateTime _Trandate = Utility.Convert_Date(date);
                                    //save in stock transaction
                                    Save_SaleQty_ToStockTransaction(productList, _Trandate);
                                    productList.Clear();
                                    DeleteLog dl = new DeleteLog();
                                    dl.DeletedDate = DateTime.Now;
                                    dl.CounterId = MemberShip.CounterId;
                                    dl.UserId = MemberShip.UserId;
                                    dl.IsParent = true;
                                    dl.TransactionId = ts.Id;

                                    entity.DeleteLogs.Add(dl);

                                    entity.SaveChanges();

                                    LoadData();

                                    if (System.Windows.Forms.Application.OpenForms["chart"] != null)
                                    {
                                        chart newForm = (chart)System.Windows.Forms.Application.OpenForms["chart"];
                                        newForm.FormFresh();
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete transaction information", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                //modify delete and copy (zp)
                else if (e.ColumnIndex == 11)
                {
                    //Role Management
                    RoleManagementController controller = new RoleManagementController();
                    controller.Load(MemberShip.UserRoleId);
                    if (controller.Transaction.DeleteAndCopy || MemberShip.isAdmin)
                    {
                        #region Delete And Copy
                        Transaction ts = entity.Transactions.Where(x => x.Id == currentTransactionId).FirstOrDefault();
                        var isPurchasePackakgeInvoiceDataDeleteCopy = entity.PackageUsedHistories.Any(x => x.PackagePurchasedInvoice.TransactionDetail.TransactionId == currentTransactionId && x.IsDelete == false);

                        if (ts.Type == "Settlement")
                        {

                            colDeleteAndCopy.ReadOnly = true;

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
                                MessageBox.Show("This transaction already make refund. So it can't be delete!");
                            }
                            else if (_isConsignmentPaidTranList.Count > 0)
                            {
                                MessageBox.Show("This transaction already made  Consignment Settlement. It cannot be deleted!");
                            }
                            else if (isPurchasePackakgeInvoiceDataDeleteCopy)
                            {
                                MessageBox.Show("This transaction already used by Offset. It cannot be deleted!");
                                return;
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                if (result.Equals(DialogResult.OK))
                                {

                                    ts.IsDeleted = true;
                                    ts.UpdatedDate = DateTime.Now;
                                    // add gift card amount
                                    if (ts.GiftCardId != null && ts.GiftCard != null)
                                    {
                                        ts.GiftCard.Amount = ts.GiftCard.Amount + Convert.ToInt32(ts.GiftCardAmount);
                                    }
                                    foreach (TransactionDetail detail in ts.TransactionDetails)
                                    {
                                        //detail.IsDeleted = false;
                                        detail.IsDeleted = true;
                                        detail.Product.Qty = detail.Product.Qty + detail.Qty;


                                        //save in stocktransaction

                                        Stock_Transaction st = new Stock_Transaction();
                                        st.ProductId = detail.Product.Id;
                                        Qty -= Convert.ToInt32(detail.Qty);
                                        st.Sale = Qty;
                                        productList.Add(st);
                                        Qty = 0;


                                        if (detail.Product.IsWrapper == true)
                                        {
                                            List<SPDetail> wList = detail.Product.SPDetails.Where(x => x.TransactionDetailID == detail.Id).ToList();
                                            if (wList.Count > 0)
                                            {
                                                foreach (SPDetail w in wList)
                                                {
                                                    Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();
                                                    wpObj.Qty = wpObj.Qty + (w.ChildQty);

                                                    Stock_Transaction stwp = new Stock_Transaction();
                                                    stwp.ProductId = w.ChildProductID;
                                                    Qty -= Convert.ToInt32(w.ChildQty);
                                                    stwp.Sale = Qty;
                                                    productList.Add(stwp);

                                                    Qty = 0;
                                                }
                                            }
                                        }

                                        //For Purchase 
                                        #region Purchase Delete

                                        if (detail.Product.IsWrapper == true)
                                        {//ZP Get purchase detail with child product Id and order by purchase date ascending
                                            List<SPDetail> splist = detail.Product.SPDetails.Where(x => x.TransactionDetailID == detail.Id).ToList();
                                            foreach (SPDetail w in splist)
                                            {
                                                //  int qty = Convert.ToInt32(w.Qty);
                                                Product wpObj = (from p in entity.Products where p.Id == w.ChildProductID select p).FirstOrDefault();

                                                List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == wpObj.Id).ToList();
                                                if (puInTranDetail.Count > 0)
                                                {
                                                    foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                                    {
                                                        PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                                        if (pud != null)
                                                        {
                                                            pud.CurrentQy = pud.CurrentQy + p.Qty;
                                                        }
                                                        entity.Entry(pud).State = EntityState.Modified;
                                                        entity.SaveChanges();

                                                        //entity.PurchaseDetailInTransactions.Remove(p);
                                                        //entity.SaveChanges();

                                                        p.Qty = 0;
                                                        entity.Entry(p).State = EntityState.Modified;

                                                        entity.PurchaseDetailInTransactions.Remove(p);
                                                        entity.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            List<APP_Data.PurchaseDetailInTransaction> puInTranDetail = entity.PurchaseDetailInTransactions.Where(x => x.TransactionDetailId == detail.Id && x.ProductId == detail.ProductId).ToList();
                                            if (puInTranDetail.Count > 0)
                                            {
                                                foreach (PurchaseDetailInTransaction p in puInTranDetail)
                                                {
                                                    PurchaseDetail pud = entity.PurchaseDetails.Where(x => x.Id == p.PurchaseDetailId).FirstOrDefault();
                                                    if (pud != null)
                                                    {
                                                        pud.CurrentQy = pud.CurrentQy + p.Qty;
                                                    }
                                                    entity.Entry(pud).State = EntityState.Modified;
                                                    entity.SaveChanges();

                                                    //entity.PurchaseDetailInTransactions.Remove(p);
                                                    //entity.SaveChanges();

                                                    p.Qty = 0;
                                                    entity.Entry(p).State = EntityState.Modified;

                                                    entity.PurchaseDetailInTransactions.Remove(p);
                                                    entity.SaveChanges();
                                                }
                                            }
                                        }




                                        #endregion
                                    }

                                    string date = dgvTransactionList.Rows[e.RowIndex].Cells[3].Value.ToString();
                                    DateTime _Trandate = Utility.Convert_Date(date);
                                    //save in stock transaction
                                    Save_SaleQty_ToStockTransaction(productList, _Trandate);
                                    productList.Clear();
                                    DeleteLog dl = new DeleteLog();
                                    dl.DeletedDate = DateTime.Now;
                                    dl.CounterId = MemberShip.CounterId;
                                    dl.UserId = MemberShip.UserId;
                                    dl.IsParent = true;
                                    dl.TransactionId = ts.Id;

                                    entity.DeleteLogs.Add(dl);

                                    entity.SaveChanges();
                                    if (System.Windows.Forms.Application.OpenForms["Sales"] != null)
                                    {
                                        Sales openedForm = (Sales)System.Windows.Forms.Application.OpenForms["Sales"];
                                        openedForm.DeleteCopy(currentTransactionId);
                                        this.Dispose();
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("You are not allowed to delete transaction information", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (e.ColumnIndex == 12)
                {
                    string Note = (from p in entity.Transactions where p.Id == currentTransactionId select p.Note).FirstOrDefault();
                    AddNote note = new AddNote();
                    note.editnote = Note;
                    note.status = "EDIT";
                    note.tranid = currentTransactionId;
                    note.ShowDialog();
                }

                else if (e.ColumnIndex == 13)
                {
                    var tranType = (from t in entity.Transactions where t.Id == currentTransactionId select t.Type).FirstOrDefault();
                    ViewTicket newForm = new ViewTicket();
                    newForm.transactionId = currentTransactionId;
                    newForm.ShowDialog();

                }
                //Cancellation
                else if (e.ColumnIndex == colCancel.Index)
                {                  

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

        private void dgvTransactionList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvTransactionList.Rows)
            {
                Transaction currentt = (Transaction)row.DataBoundItem;
                row.Cells[0].Value = currentt.Id;
                row.Cells[1].Value = currentt.Type;
                row.Cells[2].Value = currentt.PaymentType.Name;
                row.Cells[3].Value = currentt.DateTime.Value.ToString("dd-MM-yyyy");
                row.Cells[4].Value = currentt.DateTime.Value.ToString("hh:mm");
                row.Cells[5].Value = currentt.User.Name;



                var refundList = (from t in entity.Transactions where t.Type == TransactionType.Refund && t.ParentId == currentt.Id && t.IsDeleted == false select t).ToList();
                int refundAmt = Convert.ToInt32(refundList.Sum(x => x.TotalAmount));
                var DiscountAmt = Convert.ToInt32(refundList.Sum(x => x.DiscountAmount));
                int currentRefundAmt = refundAmt - DiscountAmt;
                if (currentt.PaymentType.Name != "FOC")
                {
                    row.Cells[6].Value = currentt.TotalAmount - currentRefundAmt;
                }
                else
                {
                    row.Cells[6].Value = 0;
                }

                row.Cells[7].Value = currentRefundAmt;
                if (BOOrPOS.IsBackOffice == true)
                {
                    if (dgvTransactionList.Columns[11].Visible != false)
                    {
                        dgvTransactionList.Columns[11].Visible = false;
                    }
                }
                else
                {
                    if (dgvTransactionList.Columns[11].Visible != false)
                    {
                        dgvTransactionList.Columns[11].Visible = true;
                    }
                }

            }
        }

        private void dgvTransactionList_CustomCellFormatting()
        {
            //Role Management
            RoleManagementController controller = new RoleManagementController();
            controller.Load(MemberShip.UserRoleId);
            // Transaction Delete
            if (!MemberShip.isAdmin && !controller.Transaction.EditOrDelete)
            {
                dgvTransactionList.Columns["colDelete"].Visible = false;
            }
            // Transaction Delete And Copy
            if (!MemberShip.isAdmin && !controller.Transaction.DeleteAndCopy)
            {
                dgvTransactionList.Columns["colDeleteAndCopy"].Visible = false;
            }
            if (!MemberShip.isAdmin && !controller.Transaction.Cancel)
            {
                dgvTransactionList.Columns["colCancel"].Visible = false;
            }
        }

        private void cboshoplist_selectedvaluechanged(object sender, EventArgs e)
        {

            LoadData();
        }
        #endregion

        #region Function
        #region for saving Sale Qty in Stock Transaction table
        private void Save_SaleQty_ToStockTransaction(List<Stock_Transaction> productList, DateTime _tranDate)
        {
            int _year, _month;

            _year = _tranDate.Year;
            _month = _tranDate.Month;
            Utility.Sale_Run_Process(_year, _month, productList);
        }
        #endregion

        public void LoadData()
        {

            if (IsStart == true)
            {


                entity = new POSEntities();
                dgvTransactionList_CustomCellFormatting();

                int shopid = Convert.ToInt32(cboshoplist.SelectedValue);
                string shortcode = (from p in entity.Shops where p.Id == shopid select p.ShortCode).FirstOrDefault();

                bool optionvisible = Utility.TransactionDelRefHide(shopid);
                dgvTransactionList.Columns[8].Visible = optionvisible;
                dgvTransactionList.Columns[10].Visible = optionvisible;
                dgvTransactionList.Columns[11].Visible = optionvisible;

                List<string> type = new List<string>();
                type.Add(TransactionType.Sale);

                if (rdbDate.Checked)
                {
                    DateTime fromDate = dtpFrom.Value.Date;
                    DateTime toDate = dtpTo.Value.Date;

                    // type.Add(TransactionType.Settlement);
                    // type.Add(TransactionType.Prepaid);


                    //List<Transaction> transList = (from t in entity.Transactions where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true && t.IsActive == true && t.Type == TransactionType.Sale && t.IsDeleted==false select t).ToList<Transaction>();
                    List<Transaction> transList = (from t in entity.Transactions
                                                   where EntityFunctions.TruncateTime((DateTime)t.DateTime) >= fromDate && EntityFunctions.TruncateTime((DateTime)t.DateTime) <= toDate && t.IsComplete == true
                                                       && t.IsActive == true && type.Contains(t.Type) && (t.IsDeleted == false || t.IsDeleted == null)
                                                       && t.Id.Substring(2, 2) == shortcode
                                                   select t).ToList<Transaction>();
                    if (transList.Count > 0)
                    {
                        dgvTransactionList.DataSource = transList.Where(x => x.IsDeleted != true).ToList();
                    }
                    else
                    {
                        dgvTransactionList.DataSource = "";
                        //MessageBox.Show("Item not found!", "Cannot find");
                    }

                }
                else
                {
                    string Id = txtId.Text;
                    if (Id.Trim() != string.Empty)
                    {
                        List<Transaction> transList = (from t in entity.Transactions where t.Id == Id && type.Contains(t.Type) && t.Id.Substring(2, 2) == shortcode select t).ToList().Where(x => x.IsDeleted != true).ToList();
                        if (transList.Count > 0)
                        {
                            dgvTransactionList.DataSource = transList;
                        }
                        else
                        {
                            dgvTransactionList.DataSource = "";
                            MessageBox.Show("Item not found!", "Cannot find");
                        }
                    }
                    else
                    {
                        dgvTransactionList.DataSource = "";
                    }
                }
            }

        }

        #endregion

    }
}

